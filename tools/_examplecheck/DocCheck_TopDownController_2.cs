namespace DocCheck_TopDownController
{
using Elintria;
using Elintria.Standard;

public class MouseShooter : ElintriaScript
{
    public override void Update()
    {
        TopDownController td = GetComponent<TopDownController>();
        if (!Input.GetMouseButtonDown(0)) return;
        Vector3 dir = td.mouseWorld - transform.position;
        dir.y = 0f;
        Debug.Log("Fire toward " + dir.normalized);
    }
}
}
