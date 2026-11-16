namespace DocCheck_TerrainCollider
{
using Elintria;

public class CaveEntrance : ElintriaScript
{
    public GameObject terrain;

    public override void OnTriggerEnter(Collider other) { Physics.IgnoreTerrain(other.gameObject, terrain, true); }
    public override void OnTriggerExit(Collider other) { Physics.IgnoreTerrain(other.gameObject, terrain, false); }
}
}
