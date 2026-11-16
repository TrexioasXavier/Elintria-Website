namespace DocCheck_Projectile
{
using Elintria;
using Elintria.Standard;

public class FireballTemplate : ElintriaScript
{
    public override void Start()
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "Fireball Template";
        ball.transform.localScale = Vector3.one * 0.4f;
        Projectile p = ball.AddComponent<Projectile>();
        p.speed = 18f;
        p.damage = 35f;
        p.explosionRadius = 2f;
        p.lifetime = 4f;
        ball.SetActive(false);
        GetComponent<ProjectileWeapon>().projectileTemplate = ball;
    }
}
}
