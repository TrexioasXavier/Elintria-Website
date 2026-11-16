namespace DocCheck_Animator
{
using Elintria;

public class LocomotionAnim : ElintriaScript
{
    Animator anim;
    CharacterController cc;

    public override void Start()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    public override void Update()
    {
        Vector3 v = cc.velocity; v.y = 0f;
        anim.SetFloat("Speed", v.magnitude, 0.1f, Time.deltaTime);
        anim.SetBool("Grounded", cc.isGrounded);
        if (cc.isGrounded && Input.GetKeyDown(KeyCode.Space)) anim.SetTrigger("Jump");
    }
}
}
