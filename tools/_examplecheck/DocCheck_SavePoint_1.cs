namespace DocCheck_SavePoint
{
using Elintria;
using Elintria.Standard;

public class PaidSave : SavePoint
{
    public int cost = 10;

    public override bool CanInteract(Interactor who, out string reason)
    {
        Wallet w = who != null ? who.GetComponent<Wallet>() : null;
        reason = "Costs " + cost;
        return w != null && w.money >= cost;
    }

    public override void Interact(Interactor who)
    {
        who.GetComponent<Wallet>().Spend(cost);
        base.Interact(who);
    }
}
}
