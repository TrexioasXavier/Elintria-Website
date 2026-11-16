namespace DocCheck_NetworkVariable
{
using Elintria;
using Elintria.Netcode;

public class TeamScore : NetworkBehaviour
{
    public NetworkVariable<int> red = new NetworkVariable<int>();
    public NetworkVariable<int> blue = new NetworkVariable<int>();

    public void Score(bool redTeam)
    {
        if (!IsServer) return;
        if (redTeam) red.Value++; else blue.Value++;
    }

    public override void Update() { ScreenGUI.Text(20, 20, "Red " + red + " - " + blue + " Blue"); }
}
}
