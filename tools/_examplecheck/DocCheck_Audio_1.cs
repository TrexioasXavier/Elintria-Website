namespace DocCheck_Audio
{
using Elintria;

public class PauseSounds : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Audio.paused = !Audio.paused;
            Time.timeScale = Audio.paused ? 0f : 1f;
        }
    }
}
}
