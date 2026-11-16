namespace DocCheck_Pickup
{
using Elintria;
using Elintria.Standard;

public class QuestReward : ElintriaScript
{
    public GameObject rewardPickup;

    public void Complete(GameObject player)
    {
        Pickup p = rewardPickup.GetComponent<Pickup>();
        if (!p.Give(player.GetComponent<Inventory>())) Debug.Log("Bag full: the reward stays on the ground");
    }
}
}
