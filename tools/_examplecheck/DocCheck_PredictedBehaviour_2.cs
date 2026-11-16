namespace DocCheck_PredictedBehaviour
{
using Elintria;
using Elintria.Netcode;

public class NetStats : ElintriaScript
{
    public override void Start() { NetworkManager.SimulatedLatencyMs = 120; }

    public override void Update()
    {
        NetPlayer p = GetComponent<NetPlayer>();
        if (p != null && p.IsOwner)
            ScreenGUI.Text(20, 40, "pending " + p.PendingInputs + "  mispredictions " + p.Mispredictions + "  rtt " + NetworkManager.RttMs.ToString("0"));
    }
}
}
