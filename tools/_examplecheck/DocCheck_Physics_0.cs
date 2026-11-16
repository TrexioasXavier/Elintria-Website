namespace DocCheck_Physics
{
using Elintria;

public class GroundCheck : ElintriaScript
{
    public float footOffset = 0.1f;
    public float checkDistance = 0.3f;
    public bool grounded;
    public float slope;

    public override void FixedUpdate()
    {
        Vector3 from = transform.position + Vector3.up * footOffset;
        grounded = Physics.Raycast(from, Vector3.down, out RaycastHit hit, footOffset + checkDistance);
        slope = grounded ? Vector3.Angle(hit.normal, Vector3.up) : 0f;
    }
}
}
