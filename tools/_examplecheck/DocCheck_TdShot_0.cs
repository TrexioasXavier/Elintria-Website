namespace DocCheck_TdShot
{
using Elintria;
using Elintria.Standard;

public class ShotCounter : ElintriaScript
{
    public override void Update()
    {
        int n = 0;
        foreach (TdShot s in FindObjectsOfType<TdShot>()) ++n;
        ScreenGUI.Text(20f, 100f, n + " shots flying", null, Color.white);
    }
}
}
