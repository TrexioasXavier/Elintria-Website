namespace DocCheck_TopDownController
{
using Elintria;
using Elintria.Standard;

public class IsoSetup : ElintriaScript
{
    public override void Start()
    {
        TopDownController td = GetComponent<TopDownController>();
        td.cameraYaw = 45f;
        td.cameraHeight = 14f;
        td.cameraDistance = 14f;
        td.fieldOfView = 35f;
    }
}
}
