namespace DocCheck_TdBuildSpot
{
using Elintria;
using Elintria.Standard;

public class FreeSpots : ElintriaScript
{
    public override void Update()
    {
        int free = 0;
        foreach (TdBuildSpot s in FindObjectsOfType<TdBuildSpot>()) if (!s.occupied) ++free;
        ScreenGUI.Text(20f, 80f, free + " spots free", null, Color.white);
    }
}
}
