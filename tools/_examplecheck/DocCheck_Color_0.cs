namespace DocCheck_Color
{
using Elintria;

public class DangerLight : ElintriaScript
{
    public GameObject player;
    public float range = 20f;

    public override void Update()
    {
        Light l = GetComponent<Light>();
        if (l == null || player == null) return;
        float near = 1f - Mathf.Clamp01(Vector3.Distance(player.transform.position, transform.position) / range);
        l.color = Color.Lerp(Color.green, Color.red, near);
    }
}
}
