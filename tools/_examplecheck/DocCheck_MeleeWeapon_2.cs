namespace DocCheck_MeleeWeapon
{
using Elintria;
using Elintria.Standard;

public class SpinAttack : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Q)) return;
        MeleeWeapon m = GetComponent<MeleeWeapon>();
        float oldArc = m.arc;
        m.arc = 360f;
        Debug.Log("Spin hit " + m.Strike() + " enemies");
        m.arc = oldArc;
    }
}
}
