namespace DocCheck_Inventory
{
using Elintria;
using Elintria.Standard;

public class Workbench : Interactable
{
    public override string Prompt(Interactor who) => "Craft Torch (2 wood, 1 cloth)";

    public override bool CanInteract(Interactor who, out string reason)
    {
        Inventory inv = who.inventory;
        bool ok = inv.Has("wood", 2) && inv.Has("cloth", 1) && inv.HasRoomFor("torch");
        reason = ok ? "" : "Missing materials";
        return ok;
    }

    public override void Interact(Interactor who)
    {
        who.inventory.Remove("wood", 2);
        who.inventory.Remove("cloth", 1);
        who.inventory.Add("torch");
        who.Say("Crafted a torch");
    }
}
}
