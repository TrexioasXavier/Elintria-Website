namespace DocCheck_CollapsingFloor
{
using Elintria;
using Elintria.Standard;

public class CrumblingBridge : ElintriaScript
{
    public override void Start()
    {
        foreach (CollapsingFloor f in FindObjectsOfType<CollapsingFloor>()) { f.delay = 0.4f; f.respawnTime = 0f; }
    }
}
}
