namespace DocCheck_SavePoint
{
using Elintria;
using Elintria.Standard;

public class SlotPerPoint : ElintriaScript
{
    public override void Start()
    {
        int i = 1;
        foreach (SavePoint p in FindObjectsOfType<SavePoint>()) p.slot = i++;
    }
}
}
