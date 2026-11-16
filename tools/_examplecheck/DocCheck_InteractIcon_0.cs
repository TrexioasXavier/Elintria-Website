namespace DocCheck_InteractIcon
{
using Elintria;
using Elintria.Standard;

public class IconReach : ElintriaScript
{
    public override void Start() { GetComponent<InteractIcon>().range = 2.5f; }
}
}
