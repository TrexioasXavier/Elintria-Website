namespace DocCheck_LevelLightAnimator
{
using Elintria;

public class LevelAlarm : ElintriaScript
{
    public void Raise(GeneratedLevel level)
    {
        foreach (GameObject g in level.lights)
        {
            LevelLightAnimator a = g.GetComponent<LevelLightAnimator>();
            if (a == null) a = g.AddComponent<LevelLightAnimator>();
            a.mode = 2;
            a.rate = 2f;
        }
    }
}
}
