namespace DocCheck_Wallet
{
using Elintria;
using Elintria.Standard;

public class Interest : ElintriaScript
{
    private float m_next = 60f;

    public override void Update()
    {
        if (Time.time < m_next) return;
        m_next = Time.time + 60f;
        Wallet w = GetComponent<Wallet>();
        w.Add(w.money / 20);
    }
}
}
