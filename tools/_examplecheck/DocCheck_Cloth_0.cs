namespace DocCheck_Cloth
{
using Elintria;
using Elintria.Standard;

public class WindyFlag : ElintriaScript
{
    public override void Start()
    {
        Cloth c = GetComponent<Cloth>();
        c.pinMode = "Left";
        c.width = 2f;
        c.height = 1.2f;
        c.wind = new Vector3(6f, 0f, 1f);
        c.turbulence = 0.5f;
    }

    public override void Update()
    {
        GetComponent<Cloth>().wind = new Vector3(6f + Mathf.Sin(Time.time * 0.7f) * 3f, 0f, Mathf.Cos(Time.time * 0.4f));
    }
}
}
