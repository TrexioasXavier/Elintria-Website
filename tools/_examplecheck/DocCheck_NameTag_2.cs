namespace DocCheck_NameTag
{
using Elintria;
using Elintria.Standard;

public class TitledName : ElintriaScript
{
    public string title = "the Brave";

    public override void Start()
    {
        NameTag t = GetComponent<NameTag>();
        t.displayName = gameObject.name + " " + title;
    }
}
}
