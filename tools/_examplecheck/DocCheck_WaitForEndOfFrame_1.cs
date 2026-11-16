namespace DocCheck_WaitForEndOfFrame
{
using System.Collections;
using Elintria;

public class FinalPosition : ElintriaScript
{
    public GameObject watched;

    public override void Start() { StartCoroutine(Watch()); }

    IEnumerator Watch()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (watched != null) Debug.DrawRay(watched.transform.position, Vector3.up, Color.magenta);
        }
    }
}
}
