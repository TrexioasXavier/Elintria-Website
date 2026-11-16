namespace DocCheck_MantleAbility
{
using Elintria;
using Elintria.Standard;

public class MantleOnE : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) GetComponent<MantleAbility>().TryMantle();
    }
}
}
