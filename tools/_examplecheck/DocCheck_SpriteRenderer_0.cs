namespace DocCheck_SpriteRenderer
{
using Elintria;

public class Walker2D : ElintriaScript
{
    public float speed = 4f;
    SpriteRenderer sprite;

    public override void Start() { sprite = GetComponent<SpriteRenderer>(); }

    public override void Update()
    {
        float h = Input.GetAxisHorizontal();
        transform.position += new Vector3(h * speed * Time.deltaTime, 0f, 0f);
        if (h > 0.01f) sprite.flipX = false;
        else if (h < -0.01f) sprite.flipX = true;
    }
}
}
