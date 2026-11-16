namespace DocCheck_WaitWhile
{
using System.Collections;
using Elintria;

public class HoldWhilePaused : ElintriaScript
{
    public IEnumerator Sequence()
    {
        Debug.Log("Step 1");
        yield return new WaitWhile(() => Time.timeScale == 0f);   // the game is paused
        Debug.Log("Step 2");
    }

    public override void Start() { StartCoroutine(Sequence()); }
}
}
