namespace DocCheck_RenderZones
{
using Elintria;

public class ForcedZone : ElintriaScript
{
    public int skyIslandZone = 3;

    public override void Start() { RenderZones.Force(1UL << skyIslandZone); }
}
}
