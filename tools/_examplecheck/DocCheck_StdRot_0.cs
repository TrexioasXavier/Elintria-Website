namespace DocCheck_StdRot
{
using Elintria;
using Elintria.Standard;

public class AlignToGround : ElintriaScript
{
    public override void Update()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 3f))
            transform.rotation = StdRot.FromTo(transform.up, hit.normal) * transform.rotation;
    }
}
}
