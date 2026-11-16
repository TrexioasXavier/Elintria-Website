namespace DocCheck_Interactor
{
using Elintria;
using Elintria.Standard;

public class Hints : ElintriaScript
{
    public override void OnTriggerEnter(Collider other)
    {
        Interactor who = other.gameObject.GetComponent<Interactor>();
        if (who != null) who.Say("It's cold in here...", 2.5f);
    }
}
}
