namespace DocCheck_StdCamera
{
using Elintria;
using Elintria.Standard;

public class TopDownCam : ElintriaScript
{
    public float height = 18f;

    public override void LateUpdate()
    {
        StdCamera.Place(StdCamera.Find(), transform.position + new Vector3(0f, height, -height * 0.4f),
                        new Vector3(0f, -1f, 0.4f).normalized, 55f);
    }
}
}
