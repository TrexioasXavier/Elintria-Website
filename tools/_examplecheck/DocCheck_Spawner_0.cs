namespace DocCheck_Spawner
{
using Elintria;
using Elintria.Standard;

public class DeerSetup : ElintriaScript
{
    public GameObject deerTemplate;

    public override void Start()
    {
        Spawner s = GetComponent<Spawner>();
        s.template = deerTemplate;
        s.maxAlive = 5;
        s.interval = 20f;
        s.radius = 25f;
        s.randomYaw = true;
        s.spawnOnStart = 3;
        s.StartSpawning();
    }
}
}
