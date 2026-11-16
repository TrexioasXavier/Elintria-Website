namespace DocCheck_NetworkManager
{
using Elintria;
using Elintria.Netcode;

public class CrateDropper : ElintriaScript
{
    public GameObject crateTemplate;   // inactive, with a NetworkObject

    public override void Start() { NetworkManager.RegisterTemplate(crateTemplate); }

    public override void Update()
    {
        if (!NetworkManager.IsServer || !Input.GetKeyDown(KeyCode.C)) return;
        Vector3 at = new Vector3(Random.Range(-5f, 5f), 10f, Random.Range(-5f, 5f));
        NetworkObject crate = NetworkManager.Spawn(crateTemplate.name, at, Quaternion.identity);
        Debug.Log("Spawned crate #" + crate.NetId);
    }
}
}
