namespace DocCheck_ItemStack
{
using Elintria;
using Elintria.Standard;

public class StackDemo : ElintriaScript
{
    public override void Start()
    {
        ItemStack s = new ItemStack("arrow", 24);
        Debug.Log(s + " empty? " + s.IsEmpty);
    }
}
}
