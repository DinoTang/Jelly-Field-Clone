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
    private JellyPieceModel jellyPieceModel;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private Mesh originalMesh;
    private Mesh meshClone;

    private JellyVertex[] jellyVertices;
    private Vector3[] vertexArray;

    protected override void Awake()
    {
        this.jellyPieceCtrl =
            GetComponentInParent<JellyPieceCtrl>();

        if (this.jellyPieceCtrl == null)
            return;

        this.jellyPieceModel =
            this.jellyPieceCtrl.GetComponentInChildren<JellyPieceModel>();

        if (this.jellyPieceModel == null)
            return;

        this.meshFilter =
            this.jellyPieceModel.GetComponent<MeshFilter>();

        this.meshRenderer =
            this.jellyPieceModel.GetComponent<MeshRenderer>();

        if (this.meshFilter == null)
            return;

        if (this.meshFilter.sharedMesh == null)
            return;

        /*
         * Mesh gốc.
         */
        this.originalMesh =
            this.meshFilter.sharedMesh;

        /*
         * Clone mesh để deform runtime.
         *
         * Không deform asset gốc.
         */
        this.meshClone =
            Instantiate(this.originalMesh);

        this.meshClone.name =
            this.originalMesh.name + "_JiggleRuntime";

        this.meshFilter.mesh =
            this.meshClone;

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
                this.jellyPieceModel.transform.TransformPoint(
                    this.meshClone.vertices[i]
                );

            this.jellyVertices[i] =
                new JellyVertex(
                    i,
                    worldPosition
                );
        }

        this.vertexArray =
            new Vector3[this.meshClone.vertexCount];
    }

    private void FixedUpdate()
    {
        if (this.meshClone == null ||
            this.originalMesh == null ||
            this.jellyVertices == null)
            return;

        Vector3[] originalVertices =
            this.originalMesh.vertices;

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
                this.jellyPieceModel.transform.TransformPoint(
                    originalVertices[i]
                );

            float heightWeight =
                this.GetHeightWeight(
                    target
                );

            this.jellyVertices[i].Shake(
                target,
                this.mass,
                this.stiffness,
                this.damping,
                maxStretch
            );

            Vector3 localPosition =
                this.jellyPieceModel.transform.InverseTransformPoint(
                    this.jellyVertices[i].Position
                );

            /*
             * Intensity quyết định mức độ vertex
             * được phép giữ lại quán tính.
             *
             * heightWeight:
             * đáy thấp
             * đầu cao
             */
            this.vertexArray[i] =
                Vector3.Lerp(
                    originalVertices[i],
                    localPosition,
                    heightWeight * this.intensity
                );
        }

        this.meshClone.vertices =
            this.vertexArray;

        this.meshClone.RecalculateBounds();
        this.meshClone.RecalculateNormals();
    }

    private float GetHeightWeight(Vector3 worldPosition)
    {
        if (this.meshRenderer == null)
            return this.intensity;

        Bounds bounds =
            this.meshRenderer.bounds;

        if (bounds.size.y <= 0.0001f)
            return this.intensity;

        float height =
            Mathf.InverseLerp(
                bounds.min.y,
                bounds.max.y,
                worldPosition.y
            );

        /*
         * Bottom ít deform.
         * Top deform nhiều.
         */
        return Mathf.SmoothStep(
            0f,
            1f,
            height
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
                this.jellyPieceModel.transform.TransformPoint(
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