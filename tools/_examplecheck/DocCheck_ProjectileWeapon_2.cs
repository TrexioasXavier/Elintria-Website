namespace DocCheck_ProjectileWeapon
{
using Elintria;
using Elintria.Standard;

public class ArrowCam : ElintriaScript
{
    public override void LateUpdate()
    {
        GameObject p = GetComponent<ProjectileWeapon>().lastProjectile;
        if (p != null && p.IsAlive && Input.GetKey(KeyCode.V))
            ScriptCamera.Set(p.transform.position - p.transform.forward * 2f + Vector3.up * 0.5f, p.transform.forward, Vector3.up, 60f, 0.1f, 300f);
        else ScriptCamera.Clear();
    }
}
}
