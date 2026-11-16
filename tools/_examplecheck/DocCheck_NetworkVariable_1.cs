namespace DocCheck_NetworkVariable
{
using Elintria;
using Elintria.Netcode;

public class MatchTimer : NetworkBehaviour
{
    public NetworkVariable<int> secondsLeft = new NetworkVariable<int>(300);
    float acc;

    public override void Update()
    {
        if (IsServer && secondsLeft > 0)
        {
            acc += Time.deltaTime;
            if (acc >= 1f) { acc -= 1f; secondsLeft.Value -= 1; }
        }
        ScreenGUI.Text(ScreenGUI.screenWidth / 2 - 30, 10, (secondsLeft / 60) + ":" + (secondsLeft % 60).ToString("00"));
    }
}
}
