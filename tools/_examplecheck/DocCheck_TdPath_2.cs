namespace DocCheck_TdPath
{
using Elintria;
using Elintria.Standard;

public class DangerLine : ElintriaScript
{
    public override void Update()
    {
        TdPath p = GetComponent<TdPath>();
        foreach (TdEnemy e in TdEnemy.all)
            if (e != null && e.alive && e.distance > p.length - 10f)
                ScreenGUI.Text(20f, 60f, "Enemies near the base!", null, new Color(1f, 0.3f, 0.3f, 1f));
    }
}
}
