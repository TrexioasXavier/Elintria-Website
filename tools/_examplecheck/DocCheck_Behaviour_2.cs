namespace DocCheck_Behaviour
{
using Elintria;

public class ToggleBehaviour : ElintriaScript
{
    public KeyCode key = KeyCode.T;
    public GameObject target;

    public override void Update()
    {
        if (!Input.GetKeyDown(key) || target == null) return;
        foreach (ElintriaScript s in target.GetComponents<ElintriaScript>())
            s.enabled = !s.enabled;
    }
}
}
