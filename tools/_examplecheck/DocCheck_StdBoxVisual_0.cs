namespace DocCheck_StdBoxVisual
{
using Elintria;
using Elintria.Standard;

public class SpawnOrb : ElintriaScript
{
    public override void Start()
    {
        StdBoxVisual.Make("Orb", transform.position + Vector3.up, new Vector3(0.3f, 0.3f, 0.3f), new Color(0.4f, 0.8f, 1f, 1f), true);
    }
}
}
