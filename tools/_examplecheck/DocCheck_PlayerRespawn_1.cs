namespace DocCheck_PlayerRespawn
{
using Elintria;
using Elintria.Standard;

public class Campfire : Interactable
{
    public override string Prompt(Interactor who) => "Rest";

    public override void Interact(Interactor who)
    {
        PlayerRespawn pr = who.GetComponent<PlayerRespawn>();
        if (pr != null) pr.SetCheckpoint(transform.position + transform.forward, transform.eulerAngles.y);
        who.Say("Rested");
    }
}
}
