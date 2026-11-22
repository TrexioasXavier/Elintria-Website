namespace DocCheck_ThrowableUser
{
using Elintria;
using Elintria.Standard;

public class WheelThrow : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetMouseButtonDown(2)) GetComponent<ThrowableUser>().Throw();
    }
}
}
