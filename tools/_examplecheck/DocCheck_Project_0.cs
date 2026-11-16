namespace DocCheck_Project
{
using System.IO;
using Elintria;

public class ScreenshotTaker : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F12)) return;
        string dir = Project.Resolve("Screenshots");
        Directory.CreateDirectory(dir);
        string file = Path.Combine(dir, "shot_" + Time.frameCount + ".bmp");
        GameWindow.Capture(file);
        Debug.Log("Saved " + file);
    }
}
}
