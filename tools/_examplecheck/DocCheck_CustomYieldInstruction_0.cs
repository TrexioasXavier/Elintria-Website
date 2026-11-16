namespace DocCheck_CustomYieldInstruction
{
using System.Collections;
using Elintria;

public class WaitForKey : CustomYieldInstruction
{
    readonly KeyCode key;
    public WaitForKey(KeyCode key) { this.key = key; }
    public override bool keepWaiting => !Input.GetKeyDown(key);
}

public class Dialogue : ElintriaScript
{
    public override void Start() { StartCoroutine(Talk()); }

    IEnumerator Talk()
    {
        Debug.Log("Hello, traveller.");
        yield return new WaitForKey(KeyCode.E);
        Debug.Log("The road north is dangerous.");
        yield return new WaitForKey(KeyCode.E);
        Debug.Log("Good luck.");
    }
}
}
