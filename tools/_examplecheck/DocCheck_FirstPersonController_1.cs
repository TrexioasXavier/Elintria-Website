namespace DocCheck_FirstPersonController
{
using Elintria;
using Elintria.Standard;

public class LookAtDoor : ElintriaScript
{
    public GameObject door;

    public void Play()
    {
        FirstPersonController fp = GetComponent<FirstPersonController>();
        Vector3 to = door.transform.position - fp.eyePosition;
        fp.input.overridden = true;                 // the keys and mouse do nothing for now
        fp.yaw = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg;
        fp.pitch = 0f;
    }

    public void Stop() => GetComponent<FirstPersonController>().input.overridden = false;
}
}
