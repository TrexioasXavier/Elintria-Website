namespace DocCheck_BallisticFlight
{
using Elintria;
using Elintria.Standard;

public class HumanCannon : Interactable
{
    public float power = 25f;

    public override void Interact(Interactor who)
    {
        Vector3 v = (transform.forward + Vector3.up).normalized * power;
        BallisticFlight.Launch(who.gameObject, v, 0.1f, 8f);
    }
}
}
