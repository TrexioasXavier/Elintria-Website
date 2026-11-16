namespace DocCheck_LayerMask
{
using Elintria;

public class MaskedRay : ElintriaScript
{
    public LayerMask hitMask = -1;   // everything

    public override void Update()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 30f, hitMask);
    }
}
}
