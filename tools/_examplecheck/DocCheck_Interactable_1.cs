namespace DocCheck_Interactable
{
using Elintria;
using Elintria.Standard;

public class PoweredSwitch : Interactable
{
    public bool powered;

    public override bool CanInteract(Interactor who, out string reason)
    {
        reason = powered ? "" : "No power";
        return powered && interactable;
    }

    public override void Interact(Interactor who) { Debug.Log("Switch flipped"); base.Interact(who); }
}
}
