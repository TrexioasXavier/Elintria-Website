namespace DocCheck_Coroutine
{
using System.Collections;
using Elintria;

public class Flash : ElintriaScript
{
    public GameObject lampObject;
    Coroutine current;

    public void Pulse()
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        Light lamp = lampObject.GetComponent<Light>();
        for (float t = 0f; t < 1f; t += Time.deltaTime * 2f)
        {
            lamp.intensity = Mathf.Lerp(5f, 1f, t);
            yield return null;
        }
        current = null;
    }
}
}
