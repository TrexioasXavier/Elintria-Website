namespace DocCheck_BoxCollider
{
using Elintria;

public class CrouchBox : ElintriaScript
{
    BoxCollider box;

    public override void Start() { box = GetComponent<BoxCollider>(); }

    public override void Update()
    {
        bool crouch = Input.GetKey(KeyCode.C);
        float h = crouch ? 1f : 2f;
        box.size = new Vector3(0.6f, h, 0.6f);
        box.center = new Vector3(0f, h * 0.5f, 0f);
    }
}
}
