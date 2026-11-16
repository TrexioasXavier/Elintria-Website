namespace DocCheck_InteractIcon
{
using Elintria;
using Elintria.Standard;

public class IconColour : ElintriaScript
{
    public override void Start() { GetComponent<InteractIcon>().color = new Color(1f, 0.9f, 0.4f, 1f); }
}
}
