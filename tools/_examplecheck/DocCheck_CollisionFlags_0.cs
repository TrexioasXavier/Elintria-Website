namespace DocCheck_CollisionFlags
{
using Elintria;

public class WallSlide : ElintriaScript
{
    float vy;

    public override void Update()
    {
        CharacterController cc = GetComponent<CharacterController>();
        vy -= 20f * Time.deltaTime;
        CollisionFlags f = cc.Move(new Vector3(Input.GetAxisHorizontal() * 5f, vy, 0f) * Time.deltaTime);
        if ((f & CollisionFlags.Sides) != 0 && vy < -2f) vy = -2f;   // slide slowly down walls
        if ((f & CollisionFlags.Below) != 0) vy = 0f;
    }
}
}
