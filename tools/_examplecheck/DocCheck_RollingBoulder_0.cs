namespace DocCheck_RollingBoulder
{
using Elintria;
using Elintria.Standard;

public class BoulderPlate : ElintriaScript
{
    public RollingBoulder boulder;

    public void Pressed() { if (boulder != null) boulder.Roll(); }
}
}
