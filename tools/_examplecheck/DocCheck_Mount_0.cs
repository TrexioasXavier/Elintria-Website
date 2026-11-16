namespace DocCheck_Mount
{
using Elintria;
using Elintria.Standard;

public class Whistle : ElintriaScript
{
    public Mount horse;

    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Y) || horse.rider != null) return;
        horse.transform.position = transform.position - transform.forward * 3f;
        StdToast.Show("Your horse arrives");
    }
}
}
