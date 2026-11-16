namespace DocCheck_SpriteRenderer
{
using System.Collections.Generic;
using Elintria;

public class SpriteAnim : ElintriaScript
{
    public List<string> frames = new List<string>();   // "Assets/Sprites/run_0.png" ...
    public float fps = 10f;

    public override void Update()
    {
        if (frames.Count == 0) return;
        int f = (int)(Time.time * fps) % frames.Count;
        GetComponent<SpriteRenderer>().spritePath = frames[f];
    }
}
}
