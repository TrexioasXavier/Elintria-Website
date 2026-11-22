namespace DocCheck_Jetpack
{
using Elintria;
using Elintria.Standard;

public class FuelWarning : ElintriaScript
{
    public override void Update()
    {
        Jetpack j = GetComponent<Jetpack>();
        if (j.thrusting && j.fuel < 0.2f)
            ScreenGUI.Text(GameWindow.width * 0.5f - 40f, GameWindow.height - 110f, "LOW FUEL", null, new Color(1f, 0.3f, 0.2f, 1f));
    }
}
}
