namespace DocCheck_SaveableObject
{
using Elintria;
using Elintria.Standard;

public class PlayerId : ElintriaScript
{
    public override void Awake()
    {
        SaveableObject s = GetComponent<SaveableObject>();
        if (s != null) s.id = "player";
    }
}
}
