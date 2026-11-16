namespace DocCheck_NetEventKind
{
using Elintria;
using Elintria.Netcode;

public class ConnLog : ElintriaScript
{
    public static void Log(NetEvent e)
    {
        if (e.kind == NetEventKind.Connected || e.kind == NetEventKind.Disconnected)
            Debug.Log(e.kind + ": peer " + e.peer);
    }
}
}
