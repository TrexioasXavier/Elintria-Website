namespace DocCheck_Destructible
{
using System.Collections;
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class ChainBreak : ElintriaScript
{
    public List<GameObject> statues = new List<GameObject>();

    public void Go() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        foreach (GameObject s in statues)
        {
            Destructible d = s.GetComponent<Destructible>();
            if (d != null && !d.broken) d.Break();
            yield return new WaitForSeconds(0.25f);
        }
    }
}
}
