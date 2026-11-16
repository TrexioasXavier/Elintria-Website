namespace DocCheck_LayerMask
{
using Elintria;

public class MaskBuilder : ElintriaScript
{
    public override void Start()
    {
        int mask = LayerMask.GetMask("Default", "Enemies");
        Debug.Log("Mask value " + mask);
    }
}
}
