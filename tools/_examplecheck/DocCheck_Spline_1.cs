namespace DocCheck_Spline
{
using Elintria;

public class RaceProgress : ElintriaScript
{
    public GameObject track;

    public float Progress()
    {
        Spline s = Spline.Get(track);
        return s == null ? 0f : s.NearestT(transform.position);   // 0..1 round the lap
    }

    public override void Update() { ScreenGUI.Text(20, 20, "Lap progress " + Mathf.RoundToInt(Progress() * 100f) + "%"); }
}
}
