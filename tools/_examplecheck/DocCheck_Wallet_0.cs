namespace DocCheck_Wallet
{
using Elintria;
using Elintria.Standard;

public class Toll : Interactable
{
    public int price = 5;

    public override string Prompt(Interactor who) => "Pay " + price + " to pass";

    public override void Interact(Interactor who)
    {
        Wallet w = who.GetComponent<Wallet>();
        if (w != null && w.Spend(price)) GetComponent<Door>().Open();
        else who.Say("Not enough money");
    }
}
}
