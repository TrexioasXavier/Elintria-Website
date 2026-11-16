namespace DocCheck_WaitForSeconds
{
using System.Collections;
using Elintria;

public class Countdown : ElintriaScript
{
    public int from = 3;

    public override void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        for (int i = from; i > 0; i--)
        {
            Debug.Log(i + "...");
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("GO!");
    }
}
}
