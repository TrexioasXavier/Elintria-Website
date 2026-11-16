namespace DocCheck_Time
{
using Elintria;

public class SlowMo : ElintriaScript
{
    public float slowScale = 0.25f;

    public override void Update()
    {
        Time.timeScale = Input.GetKey(KeyCode.Q) ? slowScale : 1f;

        float fps = Time.deltaTime > 0f ? Time.timeScale / Time.deltaTime : 0f;
        ScreenGUI.Text(10, 10, "FPS " + Mathf.RoundToInt(fps) + "   frame " + Time.frameCount +
                               "   played " + Time.time.ToString("0.0") + " s");
    }
}
}
