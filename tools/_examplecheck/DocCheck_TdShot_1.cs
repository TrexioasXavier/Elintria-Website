namespace DocCheck_TdShot
{
using Elintria;
using Elintria.Standard;

public class FastShots : ElintriaScript
{
    public override void Update()
    {
        foreach (TdShot s in FindObjectsOfType<TdShot>()) s.speed = Mathf.Max(s.speed, 40f);
    }
}
}
