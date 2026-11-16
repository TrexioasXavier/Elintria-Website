namespace DocCheck_SaveableAttribute
{
using Elintria;
using Elintria.Standard;

public class PaintedDoor : ElintriaScript
{
    [Saveable] public Color paint = Color.white;
    [Saveable] public Vector3 lastPainter;
}
}
