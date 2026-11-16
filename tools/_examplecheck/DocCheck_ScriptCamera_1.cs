namespace DocCheck_ScriptCamera
{
using Elintria;

public class Dolly : ElintriaScript
{
    public GameObject from;
    public GameObject to;
    public GameObject lookAt;
    public float seconds = 5f;
    float t;

    public override void Update()
    {
        t += Time.deltaTime / seconds;
        if (t >= 1f) { ScriptCamera.Clear(); enabled = false; return; }
        Vector3 p = Vector3.Lerp(from.transform.position, to.transform.position, Mathf.SmoothStep(0f, 1f, t));
        ScriptCamera.Set(p, (lookAt.transform.position - p).normalized, Vector3.up, 45f, 0.1f, 300f);
    }
}
}
