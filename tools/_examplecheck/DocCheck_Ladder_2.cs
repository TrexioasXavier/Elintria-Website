namespace DocCheck_Ladder
{
using Elintria;
using Elintria.Standard;

public class AnyoneClimbs : ElintriaScript
{
    public override void Start()
    {
        GetComponent<Ladder>().requiredTag = "";   // any Character Controller
    }
}
}
