namespace DocCheck_Climber
{
using Elintria;
using Elintria.Standard;

public class ClimbGear : ElintriaScript
{
    public override void Update()
    {
        Equipment e = GetComponent<Equipment>();
        GetComponent<Climber>().climbSpeed = e != null && e.hands == "climbing_gloves" ? 3.5f : 2.2f;
    }
}
}
