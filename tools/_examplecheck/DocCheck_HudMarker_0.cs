namespace DocCheck_HudMarker
{
using Elintria;
using Elintria.Standard;

public class ExitMarker : ElintriaScript
{
    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        bool hasKey = p != null && p.GetComponent<Inventory>().Has("exit_key");
        GetComponent<HudMarker>().enabled = hasKey;
    }
}
}
