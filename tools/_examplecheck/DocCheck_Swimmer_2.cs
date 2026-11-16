namespace DocCheck_Swimmer
{
using Elintria;
using Elintria.Standard;

public class FastSwimmer : ElintriaScript
{
    public override void Start() { GetComponent<Swimmer>().swimSpeed = 6f; }
}
}
