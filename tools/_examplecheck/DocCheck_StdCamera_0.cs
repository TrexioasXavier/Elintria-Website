namespace DocCheck_StdCamera
{
using Elintria;
using Elintria.Standard;

public class FixedView : ElintriaScript
{
    public GameObject lookAt;

    public override void LateUpdate()
    {
        Vector3 dir = (lookAt.transform.position - transform.position).normalized;
        StdCamera.Place(StdCamera.Find(), transform.position, dir, 50f);
    }
}
}
