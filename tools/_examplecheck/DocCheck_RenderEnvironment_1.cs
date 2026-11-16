namespace DocCheck_RenderEnvironment
{
using System.Collections;
using Elintria;

public class Blackout : ElintriaScript
{
    public void Trigger() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        for (int i = 0; i < 4; i++)   // flicker
        {
            RenderEnvironment.SetPower(0.2f, 0f);
            yield return new WaitForSeconds(0.1f);
            RenderEnvironment.SetPower(1f, 1f);
            yield return new WaitForSeconds(0.15f);
        }
        RenderEnvironment.SetPower(0.15f, 0f);
        yield return new WaitForSeconds(10f);
        RenderEnvironment.SetPower(1f, 1f);
    }
}
}
