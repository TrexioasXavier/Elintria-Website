namespace DocCheck_StdEvent
{
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class Button3D : ElintriaScript
{
    public List<GameObject> onPressTargets = new List<GameObject>();
    public string onPressMessage = "Open";

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            StdEvent.Fire(onPressTargets, onPressMessage, other.gameObject);
    }
}
}
