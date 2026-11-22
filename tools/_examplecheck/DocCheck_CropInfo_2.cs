namespace DocCheck_CropInfo
{
using Elintria;
using Elintria.Standard;

public class SeedPrices : ElintriaScript
{
    public override void Start()
    {
        string stock = "";
        foreach (CropInfo c in CropInfo.Parse(GetComponent<FarmPlot>().crops))
            stock += (stock.Length > 0 ? "|" : "") + c.seed + ":" + Mathf.Max(1, Mathf.RoundToInt(c.hours / 12f));
        Debug.Log(stock);
    }
}
}
