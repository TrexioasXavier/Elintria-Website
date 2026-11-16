namespace DocCheck_Ladder
{
using Elintria;
using Elintria.Standard;

public class QuickClimb : ElintriaScript
{
    public override void Update()
    {
        Ladder l = GetComponent<Ladder>();
        l.climbSpeed = Input.GetKey(KeyCode.LeftShift) ? 4.5f : 2.5f;
    }
}
}
