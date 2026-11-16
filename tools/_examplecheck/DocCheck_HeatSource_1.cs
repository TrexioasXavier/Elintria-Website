namespace DocCheck_HeatSource
{
using Elintria;
using Elintria.Standard;

public class WarmthHud : ElintriaScript
{
    public override void Update()
    {
        float w = HeatSource.WarmthAt(transform.position);
        if (w > 1f) ScreenGUI.Text(20f, 160f, "Warm (+" + Mathf.Round(w) + ")");
    }
}
}
