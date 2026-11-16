namespace DocCheck_HeatSource
{
using Elintria;
using Elintria.Standard;

public class BurningDown : ElintriaScript
{
    public float burnMinutes = 5f;

    public override void Update()
    {
        HeatSource h = GetComponent<HeatSource>();
        h.warmth = Mathf.Max(0f, h.warmth - 15f / (burnMinutes * 60f) * Time.deltaTime);
    }
}
}
