namespace DocCheck_Climber
{
using Elintria;
using Elintria.Standard;

public class ClimbHint : ElintriaScript
{
    public override void Update()
    {
        if (GetComponent<Climber>().climbing) ScreenGUI.Text(20f, 140f, "W/S climb   A/D across   Space jump off");
    }
}
}
