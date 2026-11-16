namespace DocCheck_SerializeFieldAttribute
{
using Elintria;

public class Jumper : ElintriaScript
{
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private int maxJumps = 2;

    int jumpsLeft;

    public override void Start() { jumpsLeft = maxJumps; }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0)
        {
            jumpsLeft--;
            Debug.Log("Jump to " + jumpHeight + " m, " + jumpsLeft + " left");
        }
    }
}
}
