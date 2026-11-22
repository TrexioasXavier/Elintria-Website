namespace DocCheck_NoiseEvent
{
using Elintria;
using Elintria.Standard;

public class Footsteps : ElintriaScript
{
    public float stepEvery = 0.45f;
    float timer;

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer < stepEvery) return;
        timer = 0f;
        bool running = Input.GetKey(KeyCode.LeftShift);
        StdNoise.Make(transform.position, running ? 12f : 4f, gameObject);
    }
}
}
