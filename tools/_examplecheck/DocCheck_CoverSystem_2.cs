namespace DocCheck_CoverSystem
{
using Elintria;
using Elintria.Standard;

public class CoverPrompt : ElintriaScript
{
    public override void Update()
    {
        CoverSystem c = GetComponent<CoverSystem>();
        if (c.inCover) ScreenGUI.Text(20f, 220f, c.peeking ? "Peeking" : "In cover - right mouse to peek");
    }
}
}
