namespace DocCheck_IComponentProvider
{
using Elintria;

public class StatsUser : ElintriaScript
{
    public override void Start()
    {
        Stats s = GetComponent<Stats>();   // created by StatsProvider the first time
        Debug.Log(name + " strength " + s.strength);
    }
}
}
