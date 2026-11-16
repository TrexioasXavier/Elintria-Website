namespace DocCheck_FootIK
{
using Elintria;
using Elintria.Standard;

public class FootDebug : ElintriaScript
{
    public override void Update()
    {
        FootIK ik = GetComponent<FootIK>();
        if (ik.leftHit) Debug.DrawRay(ik.leftGround, Vector3.up * 0.3f, Color.green);
        if (ik.rightHit) Debug.DrawRay(ik.rightGround, Vector3.up * 0.3f, Color.cyan);
        ScreenGUI.Text(20, 20, "Hips lowered " + ik.pelvisOffset.ToString("0.00") + " m");
    }
}
}
