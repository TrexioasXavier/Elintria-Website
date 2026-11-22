namespace DocCheck_CompassBar
{
using Elintria;
using Elintria.Standard;

public class TreasureSense : ElintriaScript
{
    public GameObject treasure;

    public override void Update()
    {
        if (treasure != null && treasure.IsAlive) CompassBar.AddPoint(treasure.transform.position, new Color(1f, 0.85f, 0.2f, 1f), "Treasure");
    }
}
}
