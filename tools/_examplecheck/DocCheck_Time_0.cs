namespace DocCheck_Time
{
using Elintria;

public class Conveyor : ElintriaScript
{
    public float metresPerSecond = 2f;
    public GameObject cargo;

    public override void Update()
    {
        if (cargo != null)
            cargo.transform.position += Vector3.right * metresPerSecond * Time.deltaTime;
    }
}
}
