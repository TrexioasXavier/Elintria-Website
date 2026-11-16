namespace DocCheck_DayNightCycle
{
using Elintria;
using Elintria.Standard;

public class Bed : Interactable
{
    public override string Prompt(Interactor who) => "Sleep until morning";

    public override void Interact(Interactor who)
    {
        DayNightCycle d = DayNightCycle.current;
        if (d != null) d.hour = 6.5f;
        StdToast.Show("You wake up rested");
    }
}
}
