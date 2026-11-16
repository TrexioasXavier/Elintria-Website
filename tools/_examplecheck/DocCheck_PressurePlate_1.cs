namespace DocCheck_PressurePlate
{
using Elintria;
using Elintria.Standard;

public class CratePlate : ElintriaScript
{
    public override void Start()
    {
        PressurePlate p = GetComponent<PressurePlate>();
        p.requiredTag = "Crate";
        p.requiredCount = 2;
        p.latch = true;
    }

    public override void Update()
    {
        PressurePlate p = GetComponent<PressurePlate>();
        ScreenGUI.Text(20, 20, "Crates on the plate: " + p.count + "/" + p.requiredCount + (p.pressed ? "  SOLVED" : ""));
    }
}
}
