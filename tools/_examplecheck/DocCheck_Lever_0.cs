namespace DocCheck_Lever
{
using Elintria;
using Elintria.Standard;

public class LightLeverSetup : ElintriaScript
{
    public GameObject lamps;   // a parent of every lamp

    public override void Start()
    {
        Lever l = GetComponent<Lever>();
        l.onOnTargets.Add(lamps);  l.onOnMessage = "SetActive:true";
        l.onOffTargets.Add(lamps); l.onOffMessage = "SetActive:false";
    }
}
}
