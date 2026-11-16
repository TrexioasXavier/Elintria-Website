namespace DocCheck_StdCamera
{
using Elintria;
using Elintria.Standard;

public class CameraCheck : ElintriaScript
{
    public override void Start()
    {
        GameObject cam = StdCamera.Find();
        Debug.Log(cam != null ? "Using camera object " + cam.name : "No camera object: the script camera will be used");
    }
}
}
