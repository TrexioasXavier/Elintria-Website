namespace DocCheck_TdShot
{
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class CriticalShots : ElintriaScript
{
    private readonly HashSet<TdShot> m_seen = new HashSet<TdShot>();

    public override void Update()
    {
        foreach (TdShot s in FindObjectsOfType<TdShot>())
            if (m_seen.Add(s) && Random.value < 0.1f) s.damage *= 3f;
    }
}
}
