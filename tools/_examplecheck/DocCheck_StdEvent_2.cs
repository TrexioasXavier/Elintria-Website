namespace DocCheck_StdEvent
{
using Elintria;

public class WelcomeSign : ElintriaScript
{
    // TriggerZone: onEnterTargets = [this sign], onEnterMessage = "Greet"
    public void Greet(GameObject visitor)
    {
        Debug.Log("Welcome, " + (visitor != null ? visitor.name : "stranger") + "!");
    }
}
}
