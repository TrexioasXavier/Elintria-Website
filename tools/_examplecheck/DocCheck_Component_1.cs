namespace DocCheck_Component
{
using Elintria;

public class ComponentLister : ElintriaScript
{
    public GameObject target;

    public override void Start()
    {
        GameObject g = target != null ? target : gameObject;
        foreach (Component c in g.GetComponents<Component>())
            Debug.Log(g.name + " has " + c.GetType().Name);
    }
}
}
