namespace DocCheck_NetworkMode
{
using Elintria;
using Elintria.Netcode;

public class ServerLogic : ElintriaScript
{
    public override void Update()
    {
        NetworkMode m = NetworkManager.Mode;
        if (m == NetworkMode.Host || m == NetworkMode.Server) { /* spawn enemies, run AI */ }
    }
}
}
