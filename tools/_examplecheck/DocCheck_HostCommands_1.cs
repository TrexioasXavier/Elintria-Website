namespace DocCheck_HostCommands
{
using Elintria;

public class Doorbell : ElintriaScript
{
    public void Ring() { HostCommands.Post("house.bell " + name); }
}

public class House : ElintriaScript
{
    public override void Update()
    {
        string c = HostCommands.Take("house.bell");
        if (c != null) Debug.Log("Someone rang: " + c.Substring(11));
    }
}
}
