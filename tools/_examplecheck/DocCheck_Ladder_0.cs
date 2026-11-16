namespace DocCheck_Ladder
{
using Elintria;
using Elintria.Standard;

public class AddLadder : ElintriaScript
{
    public override void Start()
    {
        Ladder l = AddComponent<Ladder>();
        l.height = 6f;
        l.climbSpeed = 3f;
    }
}
}
