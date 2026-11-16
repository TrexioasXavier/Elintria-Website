namespace DocCheck_LevelObjective
{
using Elintria;
using Elintria.Standard;

public class HackPanel : Interactable
{
    public LevelObjective objective;
    bool done;

    public override string Prompt(Interactor who) => done ? "Hacked" : "Hack";

    public override void Interact(Interactor who)
    {
        if (done) return;
        done = true;
        objective.Progress();
    }
}
}
