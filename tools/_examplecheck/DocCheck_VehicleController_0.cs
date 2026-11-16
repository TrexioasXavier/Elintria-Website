namespace DocCheck_VehicleController
{
using Elintria;
using Elintria.Standard;

public class Speedometer : ElintriaScript
{
    public override void Update()
    {
        VehicleController car = GetComponent<VehicleController>();
        if (!car.driving) return;
        ScreenGUI.Label(new Rect(ScreenGUI.screenWidth - 220, ScreenGUI.screenHeight - 70, 200, 50),
                        Mathf.RoundToInt(Mathf.Abs(car.speedKmh)) + " km/h" + (car.drifting ? "  DRIFT!" : ""),
                        GuiFont.Get("", 28, true), Color.white, TextAnchor.MiddleRight);
    }
}
}
