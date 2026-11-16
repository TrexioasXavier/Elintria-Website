namespace DocCheck_NetworkSession
{
using Elintria;

public class SessionChat : ElintriaScript
{
    public override void Update()
    {
        if (!NetworkSession.IsRunning)
        {
            if (Input.GetKeyDown(KeyCode.H)) NetworkSession.StartServer(7800, 8, "Chat Room");
            if (Input.GetKeyDown(KeyCode.J)) NetworkSession.ConnectClient("127.0.0.1", 7800);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Return)) NetworkSession.SendToAll("Hello from " + NetworkSession.LocalPeerId);
        while (NetworkSession.PopReceived(out string text, out uint from)) Debug.Log(from + ": " + text);
    }
}
}
