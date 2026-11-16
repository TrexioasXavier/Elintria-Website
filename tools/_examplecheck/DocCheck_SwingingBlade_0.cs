namespace DocCheck_SwingingBlade
{
using Elintria;
using Elintria.Standard;

public class BladeRow : ElintriaScript
{
    public override void Start()
    {
        int i = 0;
        foreach (SwingingBlade b in FindObjectsOfType<SwingingBlade>()) b.phase = (i++) * 0.6f;
    }
}
}
