namespace DocCheck_Joint
{
using Elintria;

public class StrainGauge : ElintriaScript
{
    public override void Update()
    {
        HingeJoint j = GetComponent<HingeJoint>();
        if (j == null) return;
        if (j.currentForce > 0.8f * j.breakForce) Debug.Log("Creaking...");
    }
}
}
