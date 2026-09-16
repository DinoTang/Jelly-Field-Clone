using UnityEngine;

public class JellyPieceJiggle : JellyPieceAbstract
{
    [Header("Jelly Physics")]
    [SerializeField] private float intensity = 0.55f;
    [SerializeField] private float mass = 1.2f;
    [SerializeField] private float stiffness = 0.35f;
    [SerializeField] private float damping = 0.82f;
    [Header("Jelly Limit")]
    [SerializeField] private float maxStretch = 0.25f;
    [SerializeField] private float followLag = 0.08f;

    private Mesh originalMesh;
    private Mesh meshClone;

    private JellyVertex[] jellyVertices;
    private Vector3[] vertexArray;
    private Bounds localBounds;
    private Vector3 previousModelPosition;

    protected override void Awake()
    {
        /*
         * Mesh gốc.
         */
        this.originalMesh = this.jellyPieceCtrl.JellyPieceModel.MeshFilter.sharedMesh;

        /*
         * Clone mesh để deform runtime.
         *
         * Không deform asset gốc.
         */
        this.meshClone = Instantiate(this.originalMesh);

        this.meshClone.name = this.originalMesh.name + "_JiggleRuntime";

        this.jellyPieceCtrl.JellyPieceModel.MeshFilter.mesh = this.meshClone;

        /*
         * Tạo một JellyVertex cho mỗi vertex.
         *
         * Position được lưu ở WORLD SPACE.
         */

        this.jellyVertices =
            new JellyVertex[this.meshClone.vertexCount];

        for (int i = 0; i < this.meshClone.vertexCount; i++)
        {
            Vector3 worldPosition =
                this.jellyPieceCtrl.JellyPieceModel.transform.TransformPoint(
                    this.meshClone.vertices[i]
                );

            this.jellyVertices[i] =
                new JellyVertex(
                    i,
                    worldPosition
                );
        }

        this.vertexArray = new Vector3[this.meshClone.vertexCount];

        this.localBounds = this.originalMesh.bounds;

        this.previousModelPosition = this.jellyPieceCtrl.JellyPieceModel.transform.position;
    }

    private void FixedUpdate()
    {
        if (this.meshClone == null ||
            this.originalMesh == null ||
            this.jellyVertices == null)
            return;

        Vector3[] originalVertices = this.originalMesh.vertices;

        Vector3 currentModelPosition = this.jellyPieceCtrl.JellyPieceModel.transform.position;

        float movementX = currentModelPosition.x - this.previousModelPosition.x;

        this.previousModelPosition = currentModelPosition;

        /*
         * Mỗi frame physics:
         *
         * 1. Lấy vị trí target của vertex
         * 2. JellyVertex spring về target
         * 3. Convert WORLD → LOCAL
         * 4. Ghi lại mesh
         */

        for (int i = 0; i < this.jellyVertices.Length; i++)
        {
            Vector3 target =
                this.GetDeformedTarget(
                    originalVertices[i],
                    movementX
                );

            this.jellyVertices[i].Shake(
                target,
                this.mass,
                this.stiffness,
                this.damping,
                maxStretch
            );

            Vector3 localPosition =
                this.jellyPieceCtrl.JellyPieceModel.transform.InverseTransformPoint(
                    this.jellyVertices[i].Position
                );

            float deformationWeight =
                this.GetDeformationWeight(
                    originalVertices[i]
                );

            this.vertexArray[i] =
                Vector3.Lerp(
                    originalVertices[i],
                    localPosition,
                    deformationWeight
                );
        }

        this.meshClone.vertices =
            this.vertexArray;

        this.meshClone.RecalculateBounds();
        this.meshClone.RecalculateNormals();
    }

    private Vector3 GetDeformedTarget(
        Vector3 originalVertex,
        float movementX)
    {
        Vector3 localBottomLeft =
            new Vector3(
                this.localBounds.min.x,
                this.localBounds.center.y,
                this.localBounds.min.z
            );

        Vector3 localBottomRight =
            new Vector3(
                this.localBounds.max.x,
                this.localBounds.center.y,
                this.localBounds.min.z
            );

        float distanceToGrip =
            Mathf.Min(
                Vector3.Distance(originalVertex, localBottomLeft),
                Vector3.Distance(originalVertex, localBottomRight)
            );

        float gripRadius =
            Mathf.Max(
                this.localBounds.size.x,
                this.localBounds.size.z
            ) * 0.75f;

        float gripInfluence =
            gripRadius <= 0.0001f
                ? 1f
                : 1f - Mathf.Clamp01(distanceToGrip / gripRadius);

        float height =
            Mathf.InverseLerp(
                this.localBounds.min.z,
                this.localBounds.max.z,
                originalVertex.z
            );

        float upperBodyLag =
            Mathf.SmoothStep(0f, 1f, height) *
            (1f - gripInfluence * 0.5f);

        Vector3 target =
            this.jellyPieceCtrl.JellyPieceModel.transform.TransformPoint(
                originalVertex
            );

        float lagOffsetX =
            movementX /
            Mathf.Max(Time.fixedDeltaTime, 0.0001f) *
            this.followLag *
            upperBodyLag;

        lagOffsetX =
            Mathf.Clamp(
                lagOffsetX,
                -this.maxStretch,
                this.maxStretch
            );

        target.x -=
            lagOffsetX;

        return target;
    }

    private float GetDeformationWeight(Vector3 originalVertex)
    {
        float height =
            Mathf.InverseLerp(
                this.localBounds.min.z,
                this.localBounds.max.z,
                originalVertex.z
            );

        float bottomWeight =
            1f - Mathf.SmoothStep(0f, 1f, height);

        return Mathf.Lerp(
            this.intensity,
            1f,
            bottomWeight
        );
    }

    public void ResetJiggle()
    {
        if (this.originalMesh == null ||
            this.meshClone == null ||
            this.jellyVertices == null)
            return;

        Vector3[] originalVertices =
            this.originalMesh.vertices;

        for (int i = 0; i < this.jellyVertices.Length; i++)
        {
            Vector3 worldPosition =
                this.jellyPieceCtrl.JellyPieceModel.transform.TransformPoint(
                    originalVertices[i]
                );

            this.jellyVertices[i].Reset(
                worldPosition
            );

            this.vertexArray[i] =
                originalVertices[i];
        }

        this.meshClone.vertices =
            this.vertexArray;

        this.previousModelPosition =
            this.jellyPieceCtrl.JellyPieceModel.transform.position;

        this.meshClone.RecalculateBounds();
        this.meshClone.RecalculateNormals();
    }

    protected override void OnDestroy()
    {
        if (this.meshClone != null)
            Destroy(this.meshClone);
    }

    private class JellyVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 Force;

        public JellyVertex(
            int id,
            Vector3 position)
        {
            this.ID = id;
            this.Position = position;
            this.Velocity = Vector3.zero;
            this.Force = Vector3.zero;
        }

        public void Shake(
    Vector3 target,
    float mass,
    float stiffness,
    float damping,
    float maxStretch
)
        {
            this.Force =
                (target - this.Position) *
                stiffness;

            this.Velocity +=
                this.Force /
                Mathf.Max(mass, 0.0001f);

            this.Velocity *=
                damping;

            this.Position +=
                this.Velocity;


            /*
             * Giới hạn độ dãn tối đa
             */
            Vector3 offset =
                this.Position - target;


            if (offset.magnitude > maxStretch)
            {
                this.Position =
                    target +
                    offset.normalized *
                    maxStretch;


                /*
                 * giảm velocity khi bị clamp
                 * tránh bị bật quá mạnh
                 */
                this.Velocity *= 0.3f;
            }


            if (
                (target - this.Position).sqrMagnitude <
                0.000001f
            )
            {
                this.Position =
                    target;

                this.Velocity =
                    Vector3.zero;
            }
        }

        public void Reset(Vector3 position)
        {
            this.Position =
                position;

            this.Velocity =
                Vector3.zero;

            this.Force =
                Vector3.zero;
        }
    }
}