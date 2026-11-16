namespace DocCheck_WaitUntil
{
using System.Collections;
using Elintria;

public class WaitForBoss : ElintriaScript
{
    public override void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        GameObject boss = null;
        yield return new WaitUntil(() => (boss = GameObject.FindWithTag("Boss")) != null);
        Debug.Log("Boss spawned: " + boss.name);
    }
}
}
