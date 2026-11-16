namespace DocCheck_StdVars
{
using Elintria;
using Elintria.Standard;

public class RememberedLever : ElintriaScript
{
    public override void Start()
    {
        if (StdVars.Is("bridge_down", "1")) GetComponent<Lever>().Toggle();
    }

    public void Pulled() { StdVars.Set("bridge_down", "1"); }
}
}
