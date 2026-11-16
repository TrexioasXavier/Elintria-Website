namespace DocCheck_Component
{
using Elintria;

public class Highlighter : ElintriaScript
{
    public static void Flash(Component anyComponentOnTheObject)
    {
        if (anyComponentOnTheObject is ComponentBase engine && engine.IsAlive)
            engine.gameObject.SendMessage("OnHighlight");
        else if (anyComponentOnTheObject is ElintriaScript script && script)
            script.gameObject.SendMessage("OnHighlight");
    }
}
}
