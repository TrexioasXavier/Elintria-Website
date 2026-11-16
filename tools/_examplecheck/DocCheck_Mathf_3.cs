namespace DocCheck_Mathf
{
using Elintria;

public class ZoomCamera : ElintriaScript
{
    public float minDistance = 2f;
    public float maxDistance = 20f;
    float targetDistance = 8f, distance = 8f, velocity;

    public override void Update()
    {
        targetDistance = Mathf.Clamp(targetDistance - Input.mouseScrollDelta, minDistance, maxDistance);
        distance = Mathf.SmoothDamp(distance, targetDistance, ref velocity, 0.2f);
        transform.localPosition = new Vector3(0f, distance * 0.4f, -distance);
    }
}
}
