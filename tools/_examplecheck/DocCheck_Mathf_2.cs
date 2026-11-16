namespace DocCheck_Mathf
{
using Elintria;

public class CompassNeedle : ElintriaScript
{
    public GameObject north;   // a marker far to the north
    float yaw;

    public override void Update()
    {
        if (north == null) return;
        Vector3 to = north.transform.position - transform.position;
        float wanted = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg;          // degrees about +y
        yaw = Mathf.MoveTowardsAngle(yaw, wanted, 180f * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        if (Mathf.Abs(Mathf.DeltaAngle(yaw, wanted)) < 1f) Debug.Log("Pointing north");
    }
}
}
