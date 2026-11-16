namespace DocCheck_Coroutine
{
using System.Collections;
using Elintria;

public class Cutscene : ElintriaScript
{
    public GameObject door;

    public override void Start() { StartCoroutine(Play()); }

    IEnumerator Play()
    {
        yield return StartCoroutine(MoveTo(door, door.transform.position + Vector3.up * 3f, 2f));
        Debug.Log("Door is open");
        yield return new WaitForSeconds(1f);
        Debug.Log("Cutscene over");
    }

    IEnumerator MoveTo(GameObject g, Vector3 to, float seconds)
    {
        Vector3 from = g.transform.position;
        for (float t = 0f; t < 1f; t += Time.deltaTime / seconds)
        {
            g.transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
        g.transform.position = to;
    }
}
}
