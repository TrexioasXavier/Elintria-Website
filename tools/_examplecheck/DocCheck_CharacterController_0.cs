namespace DocCheck_CharacterController
{
using Elintria;

public class PlayerMotor : ElintriaScript
{
    public float speed = 5f;
    public float jumpHeight = 1.4f;
    public float gravity = -20f;

    CharacterController cc;
    float vy;

    public override void Start()
    {
        cc = GetComponent<CharacterController>();
        cc.useGravity = false;   // this script handles gravity
    }

    public override void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisHorizontal(), 0f, Input.GetAxisVertical());
        Vector3 move = transform.TransformDirection(input.normalized) * speed;

        if (cc.isGrounded)
        {
            vy = -1f;   // keep pressed against the ground
            if (Input.GetKeyDown(KeyCode.Space)) vy = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }
        vy += gravity * Time.deltaTime;
        move.y = vy;

        CollisionFlags hit = cc.Move(move * Time.deltaTime);
        if ((hit & CollisionFlags.Above) != 0 && vy > 0f) vy = 0f;   // bumped the ceiling
    }
}
}
