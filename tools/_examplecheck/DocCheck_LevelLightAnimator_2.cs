namespace DocCheck_LevelLightAnimator
{
using Elintria;

public class Blackout : ElintriaScript
{
    public void Fail(GeneratedLevel level)
    {
        foreach (GameObject g in level.lights)
        {
            LevelLightAnimator a = g.GetComponent<LevelLightAnimator>();
            if (a != null) a.enabled = false;
            Light l = g.GetComponent<Light>();
            if (l != null) l.intensity *= 0.1f;
        }
    }
}
}
