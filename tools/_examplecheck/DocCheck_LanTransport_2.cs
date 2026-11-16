namespace DocCheck_LanTransport
{
using Elintria;
using Elintria.Netcode;

public class LanJoin : ElintriaScript
{
    public string address = "192.168.1.50";

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            NetworkManager.StartClient(new LanTransport(), new NetworkConfig { address = address, port = 7777, name = "Guest" });
    }
}
}
