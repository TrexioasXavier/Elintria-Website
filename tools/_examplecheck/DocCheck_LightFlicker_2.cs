namespace DocCheck_LightFlicker
{
using Elintria;
using Elintria.Standard;

public class FuseBox : Interactable
{
    public LightFlicker broken;

    public override string Prompt(Interactor who) => "Replace fuse";

    public override void Interact(Interactor who)
    {
        if (broken != null) broken.enabled = false;
    }
}
}
