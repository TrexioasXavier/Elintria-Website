namespace DocCheck_Interactable
{
using Elintria;
using Elintria.Standard;

public class TalkingNpc : Interactable
{
    public string[] lines = { "Hello there.", "The mine is north of here.", "Be careful." };
    int line;

    public override string Prompt(Interactor who) => "Talk";

    public override void Interact(Interactor who)
    {
        who.Say(lines[line], 3f);
        line = (line + 1) % lines.Length;
        base.Interact(who);   // still fire the Inspector event
    }
}
}
