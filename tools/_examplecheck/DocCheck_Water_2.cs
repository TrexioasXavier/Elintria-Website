namespace DocCheck_Water
{
using Elintria;

public class Splash : ElintriaScript
{
    public GameObject splashEffect;
    bool wasUnder;

    public override void Update()
    {
        bool under = Water.IsUnderwater(transform.position);
        if (under && !wasUnder && Water.HeightAt(transform.position, out float h))
        {
            GameObject s = Instantiate(splashEffect, new Vector3(transform.position.x, h, transform.position.z), Quaternion.identity);
            s.SetActive(true);
            Destroy(s, 2f);
        }
        wasUnder = under;
    }
}
}
