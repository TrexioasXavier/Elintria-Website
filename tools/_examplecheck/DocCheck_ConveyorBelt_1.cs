namespace DocCheck_ConveyorBelt
{
using Elintria;
using Elintria.Standard;

public class BeltReverser : ElintriaScript
{
    public float every = 4f;
    private float m_next;

    public override void Update()
    {
        if (Time.time < m_next) return;
        m_next = Time.time + every;
        GetComponent<ConveyorBelt>().speed *= -1f;
    }
}
}
