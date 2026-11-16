namespace DocCheck_Vector4
{
using Elintria;

public class WeaponTuning : ElintriaScript
{
    // x damage, y fire rate, z range, w spread
    public Vector4 stats = new Vector4(12f, 8f, 60f, 1.5f);

    public override void Start()
    {
        Debug.Log("Damage " + stats.x + ", rate " + stats.y + "/s, range " + stats.z + " m, spread " + stats.w + " deg");
    }
}
}
