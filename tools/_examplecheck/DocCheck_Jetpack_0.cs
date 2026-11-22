namespace DocCheck_Jetpack
{
using Elintria;
using Elintria.Standard;

public class FuelCanister : Interactable
{
    public override void Interact(Interactor who)
    {
        Jetpack j = who.GetComponent<Jetpack>();
        if (j == null) return;
        j.fuel = 1f;
        Destroy(gameObject);
    }
}
}
