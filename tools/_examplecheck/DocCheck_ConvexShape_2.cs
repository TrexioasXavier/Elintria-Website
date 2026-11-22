namespace DocCheck_ConvexShape
{
using Elintria;

public class BallDropper : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.B)) return;
        GameObject ball = GameObject.Create("Ball");
        ball.transform.position = transform.position;
        ConvexCollider c = ball.AddComponent<ConvexCollider>();
        c.shape = ConvexShape.Sphere;
        c.size = new Vector3(0.5f, 0.5f, 0.5f);
        c.bounciness = 0.6f;
        ball.AddComponent<Rigidbody>();
    }
}
}
