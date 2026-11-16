namespace DocCheck_Audio
{
using Elintria;

public class VolumeSetting : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Insert)) Audio.masterVolume = Mathf.Min(1f, Audio.masterVolume + 0.1f);
        if (Input.GetKeyDown(KeyCode.Delete)) Audio.masterVolume = Mathf.Max(0f, Audio.masterVolume - 0.1f);
        ScreenGUI.Text(20, 20, "Volume " + Mathf.RoundToInt(Audio.masterVolume * 100f) + "%");
    }
}
}
