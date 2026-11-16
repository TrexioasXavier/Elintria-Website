namespace DocCheck_SerializeFieldAttribute
{
using Elintria;

public class Door2 : ElintriaScript
{
    public bool locked;                       // in the Inspector, public
    [SerializeField] float openSpeed = 90f;   // in the Inspector, private
    float angle;                              // not in the Inspector

    public override void Update()
    {
        if (locked) return;
        angle = Mathf.MoveTowards(angle, 90f, openSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0f, angle, 0f);
    }
}
}
