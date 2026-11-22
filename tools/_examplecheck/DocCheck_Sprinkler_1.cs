namespace DocCheck_Sprinkler
{
using Elintria;
using Elintria.Standard;

public class SprinklerUpgrade : Interactable
{
    public override void Interact(Interactor who)
    {
        Wallet w = who.GetComponent<Wallet>();
        if (w != null && w.Spend(50)) { GetComponent<Sprinkler>().radius += 2f; StdToast.Show("Sprinkler upgraded"); }
    }
}
}
