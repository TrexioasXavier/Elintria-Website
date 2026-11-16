namespace DocCheck_Behaviour
{
using System.Collections;
using Elintria;

public class Stun : ElintriaScript
{
    public ElintriaScript movementScript;   // drag the movement script's object... or find it in Start

    public void Apply(float seconds)
    {
        StartCoroutine(Run(seconds));
    }

    IEnumerator Run(float seconds)
    {
        if (movementScript != null) movementScript.enabled = false;
        yield return new WaitForSeconds(seconds);
        if (movementScript != null) movementScript.enabled = true;
    }
}
}
