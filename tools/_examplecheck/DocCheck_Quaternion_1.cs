namespace DocCheck_Quaternion
{
using Elintria;

public class Shotgun : ElintriaScript
{
    public int pellets = 7;
    public float spreadDegrees = 20f;

    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        for (int i = 0; i < pellets; i++)
        {
            float a = Mathf.Lerp(-spreadDegrees, spreadDegrees, i / (float)(pellets - 1));
            Vector3 dir = Quaternion.AngleAxis(a, transform.up) * transform.forward;
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 40f))
                Debug.DrawLine(transform.position, hit.point, Color.yellow, 0.5f);
        }
    }
}
}
