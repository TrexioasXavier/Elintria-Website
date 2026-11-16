namespace DocCheck_CollapsingFloor
{
using Elintria;
using Elintria.Standard;

public class FallenCount : ElintriaScript
{
    public override void Update()
    {
        int n = 0;
        foreach (CollapsingFloor f in FindObjectsOfType<CollapsingFloor>()) if (f.fallen) ++n;
        ScreenGUI.Text(20f, 280f, n + " tiles fallen");
    }
}
}
