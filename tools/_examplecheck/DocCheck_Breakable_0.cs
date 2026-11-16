namespace DocCheck_Breakable
{
using Elintria;
using Elintria.Standard;

public class SecretCrate : ElintriaScript
{
    public GameObject secretDoor;

    public override void Update()
    {
        if (GetComponent<Breakable>().broken && secretDoor != null) secretDoor.SetActive(false);
    }
}
}
