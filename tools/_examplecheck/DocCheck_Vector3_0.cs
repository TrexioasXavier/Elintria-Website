namespace DocCheck_Vector3
{
using Elintria;

public class Homing : ElintriaScript
{
    public GameObject target;
    public float speed = 5f;

    public override void Update()
    {
        if (target == null) return;
        Vector3 goal = target.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, goal) < 0.05f)
            Debug.Log("Reached " + target.name);
    }
}
}
