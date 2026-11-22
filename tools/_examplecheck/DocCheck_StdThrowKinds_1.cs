namespace DocCheck_StdThrowKinds
{
using Elintria;
using Elintria.Standard;

public class ThrowableCheck : ElintriaScript
{
    public void OnUseItem(string id)
    {
        int k = StdThrowKinds.Of(id);
        if (k >= 0) GetComponent<ThrowableUser>().selected = k;    // using it from the bag puts it in hand
    }
}
}
