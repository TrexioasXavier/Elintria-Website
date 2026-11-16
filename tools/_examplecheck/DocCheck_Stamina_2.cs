namespace DocCheck_Stamina
{
using Elintria;
using Elintria.Standard;

public class SwingCost : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GetComponent<Stamina>().Use(12f)) StdToast.Show("Too tired");
    }
}
}
