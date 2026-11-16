namespace DocCheck_StdCameraShake
{
using Elintria;
using Elintria.Standard;

public class Earthquake : ElintriaScript
{
    public void Begin() { StdCameraShake.Shake(0.4f, 6f, 10f); }
}
}
