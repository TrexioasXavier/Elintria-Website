namespace DocCheck_Component
{
using Elintria;

public static class ComponentUtil
{
    public static string Describe(Component c)
    {
        if (c == null) return "(none)";
        return c.GetType().Name;
    }
}

public class Inspectorish : ElintriaScript
{
    public override void Start()
    {
        Debug.Log(ComponentUtil.Describe(GetComponent<Rigidbody>()));
        Debug.Log(ComponentUtil.Describe(GetComponent<Inspectorish>()));
    }
}
}
