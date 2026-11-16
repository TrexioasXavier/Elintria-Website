namespace DocCheck_DoubleJumpAbility
{
using Elintria;
using Elintria.Standard;

public class JumpBoots : ElintriaScript
{
    public override void Update()
    {
        Equipment e = GetComponent<Equipment>();
        GetComponent<DoubleJumpAbility>().enabled = e != null && e.feet == "jump_boots";
    }
}
}
