namespace DocCheck_WorldHealthBar
{
using Elintria;
using Elintria.Standard;

public class BossBar : ElintriaScript
{
    public override void Start()
    {
        WorldHealthBar b = GetComponent<WorldHealthBar>();
        b.hideWhenFull = false;
        b.width = 140f;
        b.height = 4f;
    }
}
}
