namespace DocCheck_TopDownController
{
using Elintria;
using Elintria.Standard;

public class ClickMarker : ElintriaScript
{
    public GameObject marker;

    public override void Update()
    {
        TopDownController td = GetComponent<TopDownController>();
        td.clickToMove = true;
        if (marker != null) { marker.SetActive(td.hasTarget); marker.transform.position = td.target; }
    }
}
}
