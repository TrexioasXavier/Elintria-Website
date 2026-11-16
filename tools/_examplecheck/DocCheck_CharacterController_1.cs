namespace DocCheck_CharacterController
{
using Elintria;

public class Wanderer : ElintriaScript
{
    public float speed = 1.5f;
    Vector3 dir = Vector3.forward;
    float nextTurn;

    public override void Update()
    {
        if (Time.time > nextTurn)
        {
            dir = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) * Vector3.forward;
            nextTurn = Time.time + Random.Range(2f, 5f);
        }
        CharacterController cc = GetComponent<CharacterController>();
        cc.SimpleMove(dir * speed);
        if (cc.velocity.sqrMagnitude > 0.01f) transform.rotation = Quaternion.LookRotation(dir);
    }
}
}
