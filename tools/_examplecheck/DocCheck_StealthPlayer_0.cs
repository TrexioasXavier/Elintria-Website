namespace DocCheck_StealthPlayer
{
using Elintria;
using Elintria.Standard;

public class ShadowHint : ElintriaScript
{
    public override void Update()
    {
        StealthPlayer s = GetComponent<StealthPlayer>();
        if (s.lightLevel < 0.2f) ScreenGUI.Text(20f, 100f, "In shadow", null, new Color(0.6f, 0.7f, 1f, 1f));
    }
}
}
