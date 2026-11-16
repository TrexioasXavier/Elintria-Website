namespace DocCheck_DialogueNpc
{
using Elintria;
using Elintria.Standard;

public class Greeting : ElintriaScript
{
    public override void Start()
    {
        DialogueNpc d = GetComponent<DialogueNpc>();
        d.script = "# start ?met=1|Welcome back!|> Bye -> end" +
                   "|# start|Ah, a new face. [set:met=1]|> Hello -> end";
    }
}
}
