namespace DocCheck_UIRuntime
{
using Elintria;

public class DesignedPause : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        bool show = !UIRuntime.IsVisible("Pause_Panel");
        UIRuntime.SetGroupVisible("Pause_", show);
        Time.timeScale = show ? 0f : 1f;
    }
}
}
