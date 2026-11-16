namespace DocCheck_ObjectLook
{
using Elintria;

public class LookReport : ElintriaScript
{
    public override void Start()
    {
        string shape = ObjectLook.Primitive(gameObject);
        string model = ObjectLook.ModelPath(gameObject);
        Debug.Log(name + " is a " + shape + (model != "" ? " (" + model + ")" : "") +
                  " with " + ObjectLook.Materials(gameObject).Length + " material(s)");
    }
}
}
