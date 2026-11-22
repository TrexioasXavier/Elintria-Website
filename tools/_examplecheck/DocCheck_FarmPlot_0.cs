namespace DocCheck_FarmPlot
{
using Elintria;
using Elintria.Standard;

public class Fertiliser : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id != "fertiliser") return;
        foreach (FarmPlot p in FindObjectsOfType<FarmPlot>())
            if (p.planted && Vector3.Distance(p.transform.position, transform.position) < 2.5f) { p.water = 99f; p.Grow(12f); }
        GetComponent<Inventory>().Remove("fertiliser", 1);
    }
}
}
