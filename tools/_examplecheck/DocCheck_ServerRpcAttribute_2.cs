namespace DocCheck_ServerRpcAttribute
{
using Elintria;
using Elintria.Netcode;

public class NetShop : NetworkBehaviour
{
    public NetworkVariable<int> gold = new NetworkVariable<int>(100);

    [ServerRpc] void Buy(string item, int price)
    {
        if (gold.Value < price) return;   // refuse
        gold.Value -= price;
        CallClient(OwnerId, nameof(Bought), item);
    }

    [ClientRpc] void Bought(string item) { Debug.Log("You bought " + item); }
}
}
