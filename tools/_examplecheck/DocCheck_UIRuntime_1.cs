namespace DocCheck_UIRuntime
{
using Elintria;

public class DesignedMenu : ElintriaScript
{
    public override void Update()
    {
        if (UIRuntime.WasClicked("PlayButton")) { UIRuntime.SetGroupVisible("Menu_", false); SceneManager.LoadScene("Level1"); }
        if (UIRuntime.WasClicked("QuitButton")) GameWindow.Quit();
    }
}
}
