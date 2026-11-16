namespace DocCheck_Pushable
{
using Elintria;
using Elintria.Standard;

public class HeavyBlock : ElintriaScript
{
    public override void Start() { GetComponent<Pushable>().pushSpeed = 0.6f; }
}
}
