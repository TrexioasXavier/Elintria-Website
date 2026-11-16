namespace DocCheck_SaveableObject
{
using Elintria;
using Elintria.Standard;

public class MarkCrates : ElintriaScript
{
    public override void Start()
    {
        foreach (Breakable b in FindObjectsOfType<Breakable>())
            if (b.GetComponent<SaveableObject>() == null) b.gameObject.AddComponent<SaveableObject>();
    }
}
}
