namespace DocCheck_LayerMask
{
using Elintria;

public class LayerTest : ElintriaScript
{
    public static bool InMask(GameObject g, LayerMask m) => (m.value & (1 << g.layer)) != 0;
}
}
