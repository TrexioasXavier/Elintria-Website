namespace DocCheck_ElintriaScript
{
using Elintria;

public class FloatingPickup : ElintriaScript
{
    public float spinSpeed = 90f;     // degrees a second
    public float bobHeight = 0.25f;   // metres
    public float bobSpeed = 2f;       // cycles a second

    Vector3 startPos;

    public override void Start()
    {
        startPos = transform.position;
    }

    public override void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
        float offset = Mathf.Sin(Time.time * bobSpeed * Mathf.PI * 2f) * bobHeight;
        transform.position = startPos + Vector3.up * offset;
    }
}
}
