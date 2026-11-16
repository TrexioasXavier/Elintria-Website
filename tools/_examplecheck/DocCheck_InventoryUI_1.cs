namespace DocCheck_InventoryUI
{
using Elintria;
using Elintria.Standard;

public class PauseInBag : ElintriaScript
{
    public override void Update()
    {
        Time.timeScale = GetComponent<InventoryUI>().open ? 0f : 1f;
    }
}
}
