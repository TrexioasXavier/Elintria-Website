namespace DocCheck_SceneManager
{
using Elintria;

public class MainMenu : ElintriaScript
{
    public override void Update()
    {
        float p = SceneManager.loadProgress;
        if (p >= 0f)
        {
            ScreenGUI.Bar(new Rect(100, 400, 400, 16), p, Color.cyan, new Color(0f, 0f, 0f, 0.5f));
            return;
        }
        if (ScreenGUI.Button(new Rect(100, 300, 200, 48), "New Game")) SceneManager.LoadScene("Level1", "difficulty=normal");
        if (ScreenGUI.Button(new Rect(100, 360, 200, 48), "Quit")) GameWindow.Quit();
    }
}
}
