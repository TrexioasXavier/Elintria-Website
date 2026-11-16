namespace DocCheck_StdVars
{
using Elintria;
using Elintria.Standard;

public class WorthyGate : ElintriaScript
{
    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (StdVars.Check("level>=5&has:sigil", p)) GetComponent<Door>().Unlock();
    }
}
}
