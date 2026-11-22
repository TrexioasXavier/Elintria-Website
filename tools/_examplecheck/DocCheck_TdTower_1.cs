namespace DocCheck_TdTower
{
using Elintria;
using Elintria.Standard;

public class TowerLevels : ElintriaScript
{
    public override void Update()
    {
        foreach (TdTower t in FindObjectsOfType<TdTower>())
        {
            Vector2 s;
            if (StdScreen.WorldToScreen(t.transform.position + Vector3.up * 2.2f, out s)) ScreenGUI.Text(s.x - 6f, s.y, "L" + t.level, null, Color.white);
        }
    }
}
}
