namespace DocCheck_CraftingBench
{
using Elintria;
using Elintria.Standard;

public class CloseOnHit : ElintriaScript
{
    public CraftingBench bench;

    public void OnDamaged(float amount) { if (bench != null && bench.open) bench.Close(); }
}
}
