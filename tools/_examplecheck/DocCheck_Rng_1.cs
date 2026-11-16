namespace DocCheck_Rng
{
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class LootRoller : ElintriaScript
{
    public override void Start()
    {
        var rng = new Rng(42);
        string[] items = { "coin", "potion", "sword", "dragon egg" };
        float[] weights = { 70f, 20f, 9f, 1f };
        for (int i = 0; i < 5; i++) Debug.Log("Drop: " + items[rng.WeightedIndex(weights)]);

        var deck = new List<string> { "A", "K", "Q", "J", "10" };
        rng.Shuffle(deck);
        Debug.Log("Deck: " + string.Join(" ", deck) + ", lucky: " + rng.Chance(0.1f) + ", pick: " + rng.Pick(deck));
    }
}
}
