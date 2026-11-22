namespace DocCheck_CropInfo
{
using Elintria;
using Elintria.Standard;

public class CropList : ElintriaScript
{
    public override void Start()
    {
        foreach (CropInfo c in CropInfo.Parse(GetComponent<FarmPlot>().crops))
            Debug.Log(c.id + ": " + c.hours + " hours, " + c.yieldMin + "-" + c.yieldMax + (c.regrow > 0f ? ", grows back" : ""));
    }
}
}
