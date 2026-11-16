namespace DocCheck_HeatSource
{
using Elintria;
using Elintria.Standard;

public class LightFire : Interactable
{
    public override string Prompt(Interactor who) => "Light the fire";

    public override void Interact(Interactor who)
    {
        HeatSource h = GetComponent<HeatSource>();
        h.enabled = true;
        h.warmth = 15f;
    }
}
}
