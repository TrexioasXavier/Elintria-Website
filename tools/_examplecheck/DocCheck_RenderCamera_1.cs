namespace DocCheck_RenderCamera
{
using System.Collections.Generic;
using Elintria;

public class MonitorBank : ElintriaScript
{
    public List<GameObject> cameras = new List<GameObject>();
    readonly List<int> textures = new List<int>();
    int next;

    public override void Start() { foreach (var c in cameras) textures.Add(GpuTexture.CreateRender(256, 144)); }

    public override void Update()
    {
        if (cameras.Count == 0) return;
        next = (next + 1) % cameras.Count;
        Transform t = cameras[next].transform;
        RenderCamera.Render(textures[next], t.position, -t.forward, t.up, 70f, 0.1f, 60f);
        for (int i = 0; i < textures.Count; i++)
            ScreenGUI.Image(new Rect(20 + i * 270, ScreenGUI.screenHeight - 170, 256, 144), textures[i], Color.white);
    }
}
}
