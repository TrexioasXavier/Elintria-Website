namespace DocCheck_Random
{
using Elintria;

public class LootDrop : ElintriaScript
{
    public override void OnDestroy()
    {
        float roll = Random.value;
        string item = roll < 0.6f ? "coin" : roll < 0.9f ? "potion" : "rare gem";
        int amount = item == "coin" ? Random.Range(1, 6) : 1;   // 1..5
        Debug.Log(name + " dropped " + amount + " x " + item);
    }
}
}
