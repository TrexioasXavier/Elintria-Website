namespace DocCheck_HudMarker
{
using Elintria;
using Elintria.Standard;

public class ChestMarkers : ElintriaScript
{
    public override void Start()
    {
        foreach (LootChest c in FindObjectsOfType<LootChest>())
        {
            HudMarker m = c.gameObject.AddComponent<HudMarker>();
            m.label = "Chest";
            m.maxDistance = 25f;
        }
    }
}
}
