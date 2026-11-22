namespace DocCheck_Turret
{
using Elintria;
using Elintria.Standard;

public class TurretAlarm : ElintriaScript
{
    public override void Update()
    {
        foreach (Turret t in FindObjectsOfType<Turret>())
            if (t.target != null && t.target.CompareTag("Player"))
                ScreenGUI.Text(GameWindow.width * 0.5f - 40f, 60f, "DETECTED", null, new Color(1f, 0.2f, 0.2f, 1f));
    }
}
}
