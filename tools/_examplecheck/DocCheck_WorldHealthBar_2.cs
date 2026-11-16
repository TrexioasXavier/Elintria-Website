namespace DocCheck_WorldHealthBar
{
using Elintria;
using Elintria.Standard;

public class FriendlyBar : ElintriaScript
{
    public override void Start() { GetComponent<WorldHealthBar>().color = new Color(0.3f, 0.9f, 0.4f, 1f); }
}
}
