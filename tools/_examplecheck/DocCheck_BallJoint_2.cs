namespace DocCheck_BallJoint
{
using Elintria;

public class SwingMeter : ElintriaScript
{
    public override void Update()
    {
        BallJoint b = GetComponent<BallJoint>();
        if (b != null && b.swing > 45f) Debug.Log("Big swing: " + b.swing.ToString("0") + " degrees");
    }
}
}
