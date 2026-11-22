namespace DocCheck_BoostRing
{
using Elintria;
using Elintria.Standard;

public class RingScore : ElintriaScript
{
    public int score;
    public void RingPassed(GameObject who) { score += 100; StdToast.Show("+100"); }
}
}
