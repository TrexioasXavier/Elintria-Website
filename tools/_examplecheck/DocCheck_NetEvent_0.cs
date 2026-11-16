namespace DocCheck_NetEvent
{
using Elintria;
using Elintria.Netcode;

public class EventPump : ElintriaScript
{
    public INetTransport transport;

    public override void Update()
    {
        if (transport == null) return;
        while (transport.Poll(out NetEvent e))
        {
            switch (e.kind)
            {
                case NetEventKind.Connected: Debug.Log("peer " + e.peer + " connected"); break;
                case NetEventKind.Disconnected: Debug.Log("peer " + e.peer + " left"); break;
                case NetEventKind.Data: Debug.Log("peer " + e.peer + ": " + e.data); break;
                case NetEventKind.Error: Debug.LogError(e.data); break;
            }
        }
    }
}
}
