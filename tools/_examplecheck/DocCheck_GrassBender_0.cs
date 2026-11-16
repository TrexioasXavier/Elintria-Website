namespace DocCheck_GrassBender
{
using Elintria;
using Elintria.Standard;

public class BenderSetup : ElintriaScript
{
    public GameObject player;
    public GameObject car;

    public override void Start()
    {
        GrassBender p = player.AddComponent<GrassBender>();
        p.slot = 0; p.radius = 0.7f;
        GrassBender c = car.AddComponent<GrassBender>();
        c.slot = 1; c.radius = 2f; c.offset = new Vector3(0f, 0f, -0.5f);
    }
}
}
