namespace DocCheck_WorldHealthBar
{
using Elintria;
using Elintria.Standard;

public class AllBars : ElintriaScript
{
    public override void Start()
    {
        foreach (BasicEnemy e in FindObjectsOfType<BasicEnemy>())
            if (e.GetComponent<WorldHealthBar>() == null) e.gameObject.AddComponent<WorldHealthBar>();
    }
}
}
