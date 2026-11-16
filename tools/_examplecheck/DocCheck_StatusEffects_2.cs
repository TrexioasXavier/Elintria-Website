namespace DocCheck_StatusEffects
{
using Elintria;
using Elintria.Standard;

public class BurnTint : ElintriaScript
{
    public override void Update()
    {
        StatusEffects fx = GetComponent<StatusEffects>();
        if (fx != null && fx.burning) ScreenGUI.Fill(new Rect(0f, 0f, GameWindow.width, GameWindow.height), new Color(1f, 0.4f, 0f, 0.15f));
    }
}
}
