namespace DocCheck_Interactable
{
using Elintria;
using Elintria.Standard;

public class ShrineSetup : ElintriaScript
{
    public GameObject bridge;

    public override void Start()
    {
        Interactable shrine = gameObject.AddComponent<Interactable>();
        shrine.prompt = "Pray";
        shrine.onInteractTargets.Add(bridge);
        shrine.onInteractMessage = "SetActive:true";   // the hidden bridge appears
    }
}
}
