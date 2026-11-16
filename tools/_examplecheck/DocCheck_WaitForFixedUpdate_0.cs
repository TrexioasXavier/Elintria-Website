namespace DocCheck_WaitForFixedUpdate
{
using System.Collections;
using Elintria;

public class Thruster : ElintriaScript
{
    public float force = 30f;
    public int steps = 25;

    public void Burn() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        for (int i = 0; i < steps; i++)
        {
            rb.AddForce(transform.up * force);
            yield return new WaitForFixedUpdate();
        }
    }
}
}
