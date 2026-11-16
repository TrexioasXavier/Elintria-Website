namespace DocCheck_Overlay2D
{
using Elintria;

public class Snow2D : ElintriaScript
{
    const int N = 200;
    float[] q = new float[N * 14];
    Vector2[] flakes = new Vector2[N];
    int tex;

    public override void Start()
    {
        tex = GpuTexture.Create(1, 1, new byte[] { 255, 255, 255, 255 }, TextureFlags.Linear);
        for (int i = 0; i < N; i++) flakes[i] = new Vector2(Random.Range(0f, 1280f), Random.Range(0f, 720f));
    }

    public override void Update()
    {
        for (int i = 0; i < N; i++)
        {
            flakes[i].y = (flakes[i].y + 40f * Time.deltaTime) % GameWindow.height;
            int o = i * 14;
            q[o] = tex; q[o + 1] = flakes[i].x; q[o + 2] = flakes[i].y; q[o + 3] = 3; q[o + 4] = 3;
            q[o + 5] = 0; q[o + 6] = 0; q[o + 7] = 1; q[o + 8] = 1;
            q[o + 9] = 1; q[o + 10] = 1; q[o + 11] = 1; q[o + 12] = 0.8f; q[o + 13] = 0;
        }
        Overlay2D.Submit(q, N);
    }
}
}
