namespace DocCheck_NavAgent
{
using Elintria;

public class ClickToMove : ElintriaScript
{
    public GameObject cameraObject;

    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Transform cam = cameraObject.transform;
        if (Physics.Raycast(cam.position, -cam.forward, out RaycastHit hit, 200f))
            NavAgent.Of(gameObject).SetDestination(hit.point);
    }
}
}
