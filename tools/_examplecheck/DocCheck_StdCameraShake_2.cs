namespace DocCheck_StdCameraShake
{
using Elintria;
using Elintria.Standard;

public class HardLanding : ElintriaScript
{
    private float m_fall;

    public override void Update()
    {
        FirstPersonController fp = GetComponent<FirstPersonController>();
        if (!fp.isGrounded) m_fall = Mathf.Min(m_fall, fp.velocity.y);
        else { if (m_fall < -12f) StdCameraShake.Shake(0.15f, 0.3f); m_fall = 0f; }
    }
}
}
