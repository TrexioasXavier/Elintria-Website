namespace DocCheck_WaitForSecondsRealtime
{
using System.Diagnostics;
using Elintria;

public class HitStop : ElintriaScript
{
    readonly Stopwatch frozenFor = new Stopwatch();
    float length;

    public void Freeze(float seconds)
    {
        length = seconds;
        Time.timeScale = 0f;
        frozenFor.Restart();
    }

    public override void Update()
    {
        if (frozenFor.IsRunning && frozenFor.Elapsed.TotalSeconds >= length)
        {
            frozenFor.Reset();
            Time.timeScale = 1f;
        }
    }
}
}
