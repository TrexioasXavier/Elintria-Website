namespace DocCheck_NetworkManagerComponent
{
using Elintria;
using Elintria.Netcode;

public class ServerBoot : ElintriaScript
{
    public override void Start()
    {
        bool dedicated = System.Environment.CommandLine.Contains("-server");
        if (dedicated && !NetworkManager.IsRunning)
            GetComponent<NetworkManagerComponent>().StartServer();
    }
}
}
