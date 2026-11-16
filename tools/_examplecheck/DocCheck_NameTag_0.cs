namespace DocCheck_NameTag
{
using Elintria;
using Elintria.Standard;

public class TagFromDialogue : ElintriaScript
{
    public override void Start()
    {
        NameTag t = GetComponent<NameTag>();
        DialogueNpc d = GetComponent<DialogueNpc>();
        if (d != null) t.displayName = d.npcName;
    }
}
}
