namespace DocCheck_Companion
{
using Elintria;
using Elintria.Standard;

public class Pacifist : ElintriaScript
{
    public override void Start()
    {
        Companion c = GetComponent<Companion>();
        c.fights = false;
        c.followDistance = 4f;
    }
}
}
