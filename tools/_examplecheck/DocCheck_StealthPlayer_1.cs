namespace DocCheck_StealthPlayer
{
using Elintria;
using Elintria.Standard;

public class QuietBoots : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id != "soft_boots") return;
        StealthPlayer s = GetComponent<StealthPlayer>();
        s.walkNoise = 2f;
        s.runNoise = 6f;
    }
}
}
