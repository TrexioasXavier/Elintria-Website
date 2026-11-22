namespace DocCheck_BuildingPlacer
{
using Elintria;
using Elintria.Standard;

public class NoBuildZone : ElintriaScript
{
    public float radius = 25f;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        BuildingPlacer b = p != null ? p.GetComponent<BuildingPlacer>() : null;
        if (b != null && b.building && Vector3.Distance(p.transform.position, transform.position) < radius)
        {
            b.SetBuilding(false);
            StdToast.Show("No building in town");
        }
    }
}
}
