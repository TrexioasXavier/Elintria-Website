namespace DocCheck_Time
{
using System.Diagnostics;
using Elintria;

public class PauseMenu : ElintriaScript
{
    bool paused;
    readonly Stopwatch clock = Stopwatch.StartNew();

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
            Time.timeScale = paused ? 0f : 1f;
            GameWindow.SetEscapeQuits(!paused);
        }
        if (paused)
        {
            float pulse = 0.5f + 0.5f * Mathf.Sin((float)clock.Elapsed.TotalSeconds * 4f);
            ScreenGUI.Label(new Rect(20, 20, 300, 40), "PAUSED", null, new Color(1f, 1f, 1f, pulse));
        }
    }
}
}
