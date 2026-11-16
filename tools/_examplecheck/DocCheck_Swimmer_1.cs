namespace DocCheck_Swimmer
{
using Elintria;
using Elintria.Standard;

public class Muffle : ElintriaScript
{
    public override void Update()
    {
        bool under = GetComponent<Swimmer>().underwater;
        if (under) ScreenGUI.Fill(new Rect(0f, 0f, GameWindow.width, GameWindow.height), new Color(0.1f, 0.3f, 0.5f, 0.3f));
    }
}
}
