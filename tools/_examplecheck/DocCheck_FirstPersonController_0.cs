namespace DocCheck_FirstPersonController
{
using Elintria;
using Elintria.Standard;

public class Wading : ElintriaScript
{
    public float waterTop = 0.4f;

    public override void Update()
    {
        FirstPersonController fp = GetComponent<FirstPersonController>();
        bool wet = transform.position.y < waterTop;
        fp.walkSpeed = wet ? 2f : 4f;
        fp.sprintSpeed = wet ? 3f : 7f;
    }
}
}
