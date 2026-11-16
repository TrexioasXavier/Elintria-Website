namespace DocCheck_LookAtIK
{
using Elintria;
using Elintria.Standard;

public class IkOff : ElintriaScript
{
    public void CutsceneStarted()
    {
        LookAtIK ik = GetComponent<LookAtIK>();
        ik.Release();
        ik.enabled = false;
    }
}
}
