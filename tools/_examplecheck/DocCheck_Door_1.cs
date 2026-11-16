namespace DocCheck_Door
{
using Elintria;
using Elintria.Standard;

public class VaultSetup : ElintriaScript
{
    public override void Start()
    {
        Door vault = GetComponent<Door>();
        vault.locked = true;
        vault.keyItemId = "keycard_red";
        vault.consumeKey = false;
        vault.slideOffset = new Vector3(2.2f, 0f, 0f);   // slides sideways
        vault.speed = 1f;
        vault.autoCloseDelay = 8f;
    }
}
}
