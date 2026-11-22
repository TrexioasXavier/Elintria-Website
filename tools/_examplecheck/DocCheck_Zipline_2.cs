namespace DocCheck_Zipline
{
using Elintria;
using Elintria.Standard;

public class DownhillOnly : ElintriaScript
{
    public override void Start()
    {
        Zipline z = GetComponent<Zipline>();
        z.bothWays = false;     // always toward the lower end
        z.speed = 20f;
    }
}
}
