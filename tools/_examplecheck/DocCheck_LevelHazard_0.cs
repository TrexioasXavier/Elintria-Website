namespace DocCheck_LevelHazard
{
using Elintria;
using Elintria.Standard;

public class PowerSwitch : ElintriaScript
{
    public void CutPower()
    {
        foreach (LevelHazard h in FindObjectsOfType<LevelHazard>())
            if (h.kind == 1 || h.kind == 2) h.enabled = false;     // fire and lasers need power
    }
}
}
