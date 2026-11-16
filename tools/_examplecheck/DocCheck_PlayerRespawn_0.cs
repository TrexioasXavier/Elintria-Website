namespace DocCheck_PlayerRespawn
{
using Elintria;
using Elintria.Standard;

public class DropOnDeath : ElintriaScript
{
    public void OnRespawn()
    {
        Inventory inv = GetComponent<Inventory>();
        if (inv != null) inv.Remove("coin", inv.Count("coin") / 2);
        Debug.Log("Deaths so far: " + GetComponent<PlayerRespawn>().deaths);
    }
}
}
