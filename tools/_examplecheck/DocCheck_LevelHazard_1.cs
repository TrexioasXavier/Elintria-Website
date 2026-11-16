namespace DocCheck_LevelHazard
{
using Elintria;
using Elintria.Standard;

public class TrapDifficulty : ElintriaScript
{
    public int depth = 1;

    public override void Start()
    {
        foreach (LevelHazard h in FindObjectsOfType<LevelHazard>())
        {
            h.damage *= 1f + depth * 0.25f;
            h.offTime = Mathf.Max(0.6f, h.offTime - depth * 0.2f);
        }
    }
}
}
