namespace DocCheck_StdThrowKinds
{
using Elintria;
using Elintria.Standard;

public class ThrowableCache : Interactable
{
    public override void Interact(Interactor who)
    {
        Inventory inv = who.inventory;
        foreach (string id in StdThrowKinds.ids) inv.Add(id, 1);
        StdToast.Show("Got one of each throwable");
    }
}
}
