namespace DocCheck_StdEvents
{
using Elintria;
using Elintria.Standard;

public class KillCounter : ElintriaScript
{
    public int kills;

    public override void Start()
    {
        StdEvents.Died += (h, killer) => { if (killer == gameObject) ++kills; };
    }
}
}
