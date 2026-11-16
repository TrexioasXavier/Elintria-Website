namespace DocCheck_Companion
{
using Elintria;
using Elintria.Standard;

public class WaitHere : ElintriaScript
{
    public Companion companion;

    public void OnTriggered()
    {
        if (!companion.waiting) companion.Toggle();
    }
}
}
