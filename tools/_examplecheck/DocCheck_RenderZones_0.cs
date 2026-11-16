namespace DocCheck_RenderZones
{
using Elintria;

public class LevelZones : ElintriaScript
{
    public override void Start()
    {
        RenderZones.Set(new[]
        {
            new Bounds(new Vector3(0, 3, 0), new Vector3(20, 6, 20)),     // 0: hall
            new Bounds(new Vector3(0, 3, 20), new Vector3(4, 6, 20)),     // 1: corridor
            new Bounds(new Vector3(0, 3, 40), new Vector3(20, 6, 20)),    // 2: lab
        });
        RenderZones.SetNeighbours(0, 1);
        RenderZones.SetNeighbours(1, 0, 2);
        RenderZones.SetNeighbours(2, 1);
    }

    public override void OnDestroy() { RenderZones.Clear(); }
}
}
