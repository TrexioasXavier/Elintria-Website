namespace DocCheck_DoubleJumpAbility
{
using Elintria;
using Elintria.Standard;

public class TripleJump : ElintriaScript
{
    public override void Start() { GetComponent<DoubleJumpAbility>().extraJumps = 2; }
}
}
