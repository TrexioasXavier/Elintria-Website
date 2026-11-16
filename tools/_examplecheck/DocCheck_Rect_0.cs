namespace DocCheck_Rect
{
using Elintria;

public class LevelSelect : ElintriaScript
{
    public int levels = 12;

    public override void Update()
    {
        for (int i = 0; i < levels; i++)
        {
            Rect r = new Rect(40 + (i % 4) * 130, 80 + (i / 4) * 70, 120, 60);
            if (ScreenGUI.Button(r, "Level " + (i + 1)))
                SceneManager.LoadScene("Level" + (i + 1));
        }
    }
}
}
