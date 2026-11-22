namespace DocCheck_StdExtensions
{
using Elintria;
using Elintria.Standard;

public class SelfSafeTrigger : ElintriaScript
{
    public GameObject owner;

    public override void OnTriggerEnter(Collider other)
    {
        // A collider anywhere on the owner (an arm, a weapon) does not count.
        if (owner != null && other.transform.IsChildOf(owner.transform)) return;
        Debug.Log("Hit " + other.gameObject.name);
    }
}
}
