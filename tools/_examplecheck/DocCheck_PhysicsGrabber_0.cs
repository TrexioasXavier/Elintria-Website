namespace DocCheck_PhysicsGrabber
{
using Elintria;
using Elintria.Standard;

public class StrongArms : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id == "exo_arms") GetComponent<PhysicsGrabber>().maxMass = 200f;
    }
}
}
