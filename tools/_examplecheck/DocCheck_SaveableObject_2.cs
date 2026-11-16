namespace DocCheck_SaveableObject
{
using Elintria;
using Elintria.Standard;

public class StaticSave : ElintriaScript
{
    public override void Start()
    {
        SaveableObject s = GetComponent<SaveableObject>();
        s.saveTransform = false;
        s.saveActive = true;
    }
}
}
