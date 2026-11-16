namespace DocCheck_Interactor
{
using Elintria;
using Elintria.Standard;

public class UseKey : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetMouseButtonDown(2)) GetComponent<Interactor>().InteractNow();   // middle mouse also uses
    }
}
}
