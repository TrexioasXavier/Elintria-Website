namespace DocCheck_Quaternion
{
using Elintria;

public class SwingDoor : ElintriaScript
{
    public float openAngle = 100f;
    public float speed = 2f;
    public bool open;

    Quaternion closedRot, openRot;
    float t;

    public override void Start()
    {
        closedRot = transform.rotation;
        openRot = closedRot * Quaternion.Euler(0f, openAngle, 0f);   // turn about the door's own up
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) open = !open;
        t = Mathf.MoveTowards(t, open ? 1f : 0f, speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(closedRot, openRot, Mathf.SmoothStep(0f, 1f, t));
    }
}
}
