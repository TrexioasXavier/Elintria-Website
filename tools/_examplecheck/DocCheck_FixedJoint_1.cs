namespace DocCheck_FixedJoint
{
using Elintria;

public class WeakShelf : ElintriaScript
{
    public override void Start()
    {
        gameObject.AddComponent<FixedJoint>().breakForce = 400f;
    }
}
}
