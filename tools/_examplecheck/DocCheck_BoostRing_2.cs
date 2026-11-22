namespace DocCheck_BoostRing
{
using Elintria;
using Elintria.Standard;

public class UpRing : ElintriaScript
{
    public override void Start()
    {
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);   // facing up
        GetComponent<BoostRing>().boostSpeed = 18f;
    }
}
}
