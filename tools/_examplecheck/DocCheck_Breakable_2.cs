namespace DocCheck_Breakable
{
using Elintria;
using Elintria.Standard;

public class KickCrate : Interactable
{
    public override string Prompt(Interactor who) => "Kick";

    public override void Interact(Interactor who) { GetComponent<Breakable>().Break(); }
}
}
