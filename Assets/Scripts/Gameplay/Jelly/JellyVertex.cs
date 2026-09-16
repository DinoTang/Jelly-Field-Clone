using UnityEngine;
class JellyVertex
{
    public int ID;
    public Vector3 Position;
    public Vector3 Velocity;
    public Vector3 Force;

    public JellyVertex(int id, Vector3 position)
    {
        this.ID = id;
        this.Position = position;
        this.Velocity = Vector3.zero;
        this.Force = Vector3.zero;
    }

    public void Shake(Vector3 target, float mass, float stiffness, float damping, float maxStretch)
    {
        this.Force = (target - this.Position) * stiffness;

        this.Velocity += this.Force / Mathf.Max(mass, 0.0001f);

        this.Velocity *= damping;

        this.Position += this.Velocity;
        /*
         * Giới hạn độ dãn tối đa
         */
        Vector3 offset = this.Position - target;

        if (offset.magnitude > maxStretch)
        {
            this.Position = target + offset.normalized * maxStretch;
            /*
             * giảm velocity khi bị clamp
             * tránh bị bật quá mạnh
             */
            this.Velocity *= 0.3f;
        }

        if ((target - this.Position).sqrMagnitude < 0.000001f
        )
        {
            this.Position = target;

            this.Velocity = Vector3.zero;

        }
    }

    public void Reset(Vector3 position)
    {
        this.Position = position;

        this.Velocity = Vector3.zero;

        this.Force = Vector3.zero;
    }
}