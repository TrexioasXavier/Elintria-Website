namespace DocCheck_Joint
{
using Elintria;

public class CutLoose : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.X)) return;
        SpringJoint j = GetComponent<SpringJoint>();
        if (j != null) j.Break();
    }
}
}
