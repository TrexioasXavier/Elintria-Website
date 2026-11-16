namespace DocCheck_WaitForFixedUpdate
{
using System.Collections;
using Elintria;

public class Dash : ElintriaScript
{
    public float speed = 18f;
    public float duration = 0.2f;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Shift)) StartCoroutine(Go());
    }

    IEnumerator Go()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float end = Time.time + duration;
        while (Time.time < end)
        {
            rb.velocity = transform.forward * speed;
            yield return new WaitForFixedUpdate();
        }
    }
}
}
