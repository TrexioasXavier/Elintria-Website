namespace DocCheck_DialogueNpc
{
using Elintria;
using Elintria.Standard;

public class Ambush : ElintriaScript
{
    private bool m_done;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (m_done || p == null || Vector3.Distance(p.transform.position, transform.position) > 3f) return;
        m_done = true;
        GetComponent<DialogueNpc>().Begin();
    }
}
}
