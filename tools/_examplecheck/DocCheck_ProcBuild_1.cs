namespace DocCheck_ProcBuild
{
using Elintria;
using Elintria.Procedural;

public class RockField : ElintriaScript
{
    public override void Start()
    {
        var rocks = ProcBuild.Scatter("Rock", transform.position, 60f, 60f, 5f, 12, 0.4f);
        Debug.Log("Placed " + rocks.Count + " rocks");
    }
}
}
