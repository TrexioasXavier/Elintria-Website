namespace DocCheck_CropInfo
{
using Elintria;
using Elintria.Standard;

public class Greenhouse : ElintriaScript
{
    public override void Start()
    {
        foreach (FarmPlot p in FindObjectsOfType<FarmPlot>())
            if (Vector3.Distance(p.transform.position, transform.position) < 6f)
                p.crops = "carrot:hours=18,yield=3-5|tomato:hours=30,yield=3-4,regrow=12";
    }
}
}
