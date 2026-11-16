namespace DocCheck_Inventory
{
using Elintria;
using Elintria.Standard;

public class StartingKit : ElintriaScript
{
    public override void Start()
    {
        Inventory inv = GetComponent<Inventory>();
        int leftOver = inv.Add("arrow", 150);   // stacks of maxStack each
        if (leftOver > 0) Debug.Log(leftOver + " arrows didn't fit");
        inv.Select(0);
        Debug.Log("Carrying: " + inv.Describe() + "; in hand: " + inv.Selected);
    }

    public void OnInventoryChanged() { Debug.Log("Inventory changed (version " + GetComponent<Inventory>().version + ")"); }
}
}
