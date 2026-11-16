namespace DocCheck_SavePoint
{
using Elintria;
using Elintria.Standard;

public class HealOnly : ElintriaScript
{
    public override void Start() { GetComponent<SavePoint>().heal = true; }
}
}
