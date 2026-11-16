namespace DocCheck_WaveSpawner
{
using Elintria;
using Elintria.Standard;

public class WaveBanner : ElintriaScript
{
    public override void Update()
    {
        WaveSpawner w = GetComponent<WaveSpawner>();
        string text = w.finished ? "VICTORY" : w.between ? "Next wave in " + Mathf.CeilToInt(w.countdown) : "Wave " + w.wave + "  -  " + (w.aliveCount + w.toSpawn) + " left";
        ScreenGUI.Label(new Rect(0, 20, ScreenGUI.screenWidth, 40), text, GuiFont.Get("", 26, true), Color.white, TextAnchor.MiddleCenter);
        if (w.between && Input.GetKeyDown(KeyCode.N)) w.NextWaveNow();
    }
}
}
