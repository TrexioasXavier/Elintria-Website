namespace DocCheck_CollisionFlags
{
using Elintria;

public class HeadBump : ElintriaScript
{
    public float vy = 8f;

    public override void Update()
    {
        CollisionFlags f = GetComponent<CharacterController>().Move(Vector3.up * vy * Time.deltaTime);
        if ((f & CollisionFlags.Above) != 0) { vy = 0f; Debug.Log("Bonk"); }
    }
}
}
