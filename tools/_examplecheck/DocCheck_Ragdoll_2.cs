namespace DocCheck_Ragdoll
{
using Elintria;
using Elintria.Standard;

public class RagdollCam : ElintriaScript
{
    public override void LateUpdate()
    {
        Ragdoll r = GetComponent<Ragdoll>();
        if (!r.active) return;
        Vector3 hips = r.hipsPosition;
        ScriptCamera.Set(hips + new Vector3(0f, 3f, -4f), (hips - (hips + new Vector3(0f, 3f, -4f))).normalized, Vector3.up, 60f, 0.1f, 200f);
    }
}
}
