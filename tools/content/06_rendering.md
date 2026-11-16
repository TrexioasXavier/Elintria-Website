# Light
summary: A light source: directional sun, point bulb, spot cone or area panel, with colour, intensity and shadows.
see: LightType, Color, RenderEnvironment

The `Light` component lights the scene. Change its colour, intensity, range and type from code for day-night cycles, flickering torches, alarms and muzzle flashes. Lights, like cameras, shine down their object's -Z axis.

## Members
type: Directional, Point, Spot or Area.
intensity: How bright it is. 1 is normal; higher values for strong lights.
range: How far a point or spot light reaches, in units.
spotAngle: The full angle of a spot light's cone, in degrees.
shadows: Whether the light casts shadows.
color: The light's colour.

## Example: A flickering torch
file: Scripts/Torch.cs
```csharp
using Elintria;

public class Torch : ElintriaScript
{
    public float baseIntensity = 1.6f;
    public float flicker = 0.4f;
    Light lamp;

    public override void Start() { lamp = GetComponent<Light>(); }

    public override void Update()
    {
        float n = Mathf.PerlinNoise(Time.time * 6f, 0.7f);
        lamp.intensity = baseIntensity + (n - 0.5f) * 2f * flicker;
        lamp.color = Color.Lerp(new Color(1f, 0.55f, 0.2f), new Color(1f, 0.75f, 0.35f), n);
    }
}
```

## Example: Day and night with a directional light
file: Scripts/DayNight.cs
Turns the sun about the X axis and fades its intensity and colour with the time of day.
```csharp
using Elintria;

public class DayNight : ElintriaScript
{
    public float dayLengthSeconds = 120f;
    Light sun;

    public override void Start()
    {
        sun = GetComponent<Light>();
        sun.type = LightType.Directional;
        sun.shadows = true;
    }

    public override void Update()
    {
        float t = (Time.time / dayLengthSeconds) % 1f;            // 0..1 through the day
        float angle = t * 360f - 90f;                               // sunrise at t = 0
        transform.rotation = Quaternion.Euler(angle, 30f, 0f);
        float up = Mathf.Clamp01(Mathf.Sin(t * Mathf.PI * 2f));     // 0 at night, 1 at noon
        sun.intensity = Mathf.Lerp(0.05f, 1.3f, up);
        sun.color = Color.Lerp(new Color(1f, 0.5f, 0.3f), Color.white, up);
    }
}
```

## Example: An alarm with a spot light
file: Scripts/AlarmLight.cs
```csharp
using Elintria;

public class AlarmLight : ElintriaScript
{
    public bool alarm;
    Light spot;

    public override void Start()
    {
        spot = GetComponent<Light>();
        spot.type = LightType.Spot;
        spot.spotAngle = 40f;
        spot.range = 15f;
    }

    public override void Update()
    {
        spot.intensity = alarm ? 3f : 0f;
        if (!alarm) return;
        transform.Rotate(0f, 360f * Time.deltaTime, 0f, Space.World);
        spot.color = Color.red;
    }
}
```

# LightType
summary: The kind of light: Directional, Point, Spot or Area.
see: Light

## Members
Directional: Parallel rays from far away, like the sun; position does not matter, only rotation.
Point: Shines in all directions from a point, fading out by `range`.
Spot: A cone of light along -Z, `spotAngle` wide.
Area: Light from a rectangle, for soft panel lights.

## Example: Switching type at run time
file: Scripts/LightMode.cs
```csharp
using Elintria;

public class LightMode : ElintriaScript
{
    public override void Update()
    {
        Light l = GetComponent<Light>();
        if (Input.GetKeyDown(KeyCode.Alpha1)) l.type = LightType.Point;
        if (Input.GetKeyDown(KeyCode.Alpha2)) l.type = LightType.Spot;
        if (Input.GetKeyDown(KeyCode.Alpha3)) l.type = LightType.Directional;
    }
}
```

## Example: Finding the sun
file: Scripts/FindSun.cs
```csharp
using Elintria;

public class FindSun : ElintriaScript
{
    public override void Start()
    {
        foreach (Light l in FindObjectsOfType<Light>())
            if (l.type == LightType.Directional) { Debug.Log("The sun is " + l.gameObject.name); break; }
    }
}
```

## Example: Making a lamp from code
file: Scripts/MakeLamp.cs
```csharp
using Elintria;

public class MakeLamp : ElintriaScript
{
    public override void Start()
    {
        GameObject g = new GameObject("Lamp");
        g.transform.position = transform.position + Vector3.up * 3f;
        Light l = g.AddComponent<Light>();
        l.type = LightType.Point;
        l.range = 8f;
        l.intensity = 2f;
        l.color = new Color(1f, 0.9f, 0.7f);
    }
}
```

# RuntimeMesh
summary: Geometry built by a script, such as terrain, procedural buildings or custom shapes, that the engine draws.
see: ObjectLook, GpuMesh, GameObject

Fill a `RuntimeMesh` with vertices, normals, UVs and triangles, then attach it to an object with `RuntimeMesh.Assign`. The object then draws your geometry with its material, like any model. Triangles are wound anticlockwise when seen from the front.

## Members
RuntimeMesh: Makes an empty mesh.
vertexCount: How many vertices the mesh has.
indexCount: How many triangle indices it has (three per triangle).
SetVertices: Sets the vertex positions, in the object's local space.
@v: One entry per vertex.
SetNormals: Sets one normal per vertex, for lighting.
SetUVs: Sets one texture coordinate per vertex (0..1).
SetTriangles: Sets the triangles: every three indices make one, wound anticlockwise seen from the front.
@idx: Vertex indices, three per triangle.
AssignedTo: The id of the runtime mesh drawn by `go`, or 0.

## Example: A quad from scratch
file: Scripts/MakeQuad.cs
```csharp
using Elintria;

public class MakeQuad : ElintriaScript
{
    public override void Start()
    {
        RuntimeMesh m = new RuntimeMesh();
        m.SetVertices(new[] { new Vector3(-1, 0, -1), new Vector3(1, 0, -1), new Vector3(1, 0, 1), new Vector3(-1, 0, 1) });
        m.SetNormals(new[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up });
        m.SetUVs(new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) });
        m.SetTriangles(new[] { 0, 2, 1, 0, 3, 2 });   // anticlockwise seen from above
        RuntimeMesh.Assign(gameObject, m);
    }
}
```

## Example: A heightmap grid
file: Scripts/NoiseHills.cs
Builds a grid of vertices raised by noise, a common start for procedural terrain.
```csharp
using Elintria;

public class NoiseHills : ElintriaScript
{
    public int size = 64;
    public float scale = 0.08f;
    public float height = 6f;
    RuntimeMesh mesh;

    public override void Start()
    {
        int n = size + 1;
        Vector3[] v = new Vector3[n * n];
        Vector2[] uv = new Vector2[n * n];
        for (int z = 0; z < n; z++)
            for (int x = 0; x < n; x++)
            {
                v[z * n + x] = new Vector3(x, Mathf.PerlinNoise(x * scale, z * scale) * height, z);
                uv[z * n + x] = new Vector2(x / (float)size, z / (float)size);
            }
        int[] tri = new int[size * size * 6];
        int t = 0;
        for (int z = 0; z < size; z++)
            for (int x = 0; x < size; x++)
            {
                int i = z * n + x;
                tri[t++] = i; tri[t++] = i + n; tri[t++] = i + 1;
                tri[t++] = i + 1; tri[t++] = i + n; tri[t++] = i + n + 1;
            }
        mesh = new RuntimeMesh();
        mesh.SetVertices(v);
        mesh.SetUVs(uv);
        mesh.SetTriangles(tri);
        RuntimeMesh.Assign(gameObject, mesh);
    }

    public override void OnDestroy() { if (mesh != null) mesh.Release(); }
}
```

## Example: Replacing geometry at run time
file: Scripts/Pulsing.cs
```csharp
using Elintria;

public class Pulsing : ElintriaScript
{
    RuntimeMesh mesh;
    Vector3[] baseVerts = { new Vector3(0, 1, 0), new Vector3(-1, -1, 0), new Vector3(1, -1, 0) };

    public override void Start()
    {
        mesh = new RuntimeMesh();
        mesh.SetTriangles(new[] { 0, 1, 2 });
        RuntimeMesh.Assign(gameObject, mesh);
    }

    public override void Update()
    {
        float s = 1f + 0.2f * Mathf.Sin(Time.time * 3f);
        Vector3[] v = new Vector3[3];
        for (int i = 0; i < 3; i++) v[i] = baseVerts[i] * s;
        mesh.SetVertices(v);
    }

    public override void OnDestroy() { RuntimeMesh.Assign(gameObject, null); mesh.Release(); }
}
```

# ObjectLook
summary: Read and change how an object is drawn: its primitive or model, and its materials.
see: RuntimeMesh, GameObject

`ObjectLook` tells you what an object looks like, and lets you change it: swap its model, or give it different materials. Destruction scripts use it to build pieces that look like the original.

## Members
SetMaterials: Gives `go` these materials, one per sub-mesh ("Assets/Materials/Red.elmat"). Returns false when it cannot.
@materials: Material paths, one per sub-mesh.
SetModel: Draws `go` with the model file at `path` ("" for none).

## Example: Swapping a damaged model
file: Scripts/DamagedLook.cs
```csharp
using Elintria;

public class DamagedLook : ElintriaScript
{
    public string damagedModel = "Assets/Models/car_wrecked.fbx";
    public float health = 100f;

    public void ApplyDamage(float amount)
    {
        health -= amount;
        if (health <= 0f && ObjectLook.ModelPath(gameObject) != damagedModel)
            ObjectLook.SetModel(gameObject, damagedModel);
    }
}
```

## Example: Team colours with materials
file: Scripts/TeamPaint.cs
```csharp
using Elintria;

public class TeamPaint : ElintriaScript
{
    public int team;

    public override void Start()
    {
        string mat = team == 0 ? "Assets/Materials/Red.elmat" : "Assets/Materials/Blue.elmat";
        ObjectLook.SetMaterials(gameObject, new[] { mat });
    }
}
```

## Example: Describing objects
file: Scripts/LookReport.cs
```csharp
using Elintria;

public class LookReport : ElintriaScript
{
    public override void Start()
    {
        string shape = ObjectLook.Primitive(gameObject);
        string model = ObjectLook.ModelPath(gameObject);
        Debug.Log(name + " is a " + shape + (model != "" ? " (" + model + ")" : "") +
                  " with " + ObjectLook.Materials(gameObject).Length + " material(s)");
    }
}
```

# SpriteRenderer
summary: Draws an image on a flat rectangle, for 2D characters, items and backgrounds.
see: Tilemap, Color

A `SpriteRenderer` draws an image in its object's XY plane. Swap `spritePath` to animate or change the picture, use `flipX` to face left or right, tint it with `color`, and sort overlapping sprites with `sortingOrder`.

## Members
flipX: Mirrors the image left to right.
flipY: Mirrors the image top to bottom.

## Example: A walking 2D character with flipping
file: Scripts/Walker2D.cs
```csharp
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
```

## Example: Flip-book animation
file: Scripts/SpriteAnim.cs
Changing `spritePath` shows another image; cycling through frames animates.
```csharp
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
```

## Example: Damage flash and draw order
file: Scripts/HurtFlash.cs
```csharp
using System.Collections;
using Elintria;

public class HurtFlash : ElintriaScript
{
    public void ApplyDamage(float amount) { StartCoroutine(Flash()); }

    IEnumerator Flash()
    {
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        int order = s.sortingOrder;
        s.sortingOrder = order + 10;     // draw on top while flashing
        s.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        s.color = Color.white;
        s.sortingOrder = order;
    }
}
```

# ParticleEmitter
summary: A particle system: sparks, smoke, fire, dust and magic effects, controlled from code.
see: ParticleShape, ParticleRenderMode, ParticleBlend

A `ParticleEmitter` spawns and animates many small sprites. Set it up in the Inspector, then from code start and stop it, emit bursts, and change its settings. Every setting can be read and written by name with `Set` and `Get`, and the common ones are also properties.

## Members
isPlaying: Whether it is playing.
isPaused: Whether it is paused.
isEmitting: Whether it is emitting new particles now.
particleCount: How many particles are alive.
time: How far into its cycle it is, in seconds.
Play: Starts emitting (and simulating).
Pause: Freezes the particles where they are.
Clear: Removes every particle alive now.
duration: How long one cycle lasts, in seconds.
looping: Whether it starts again after `duration`.
playOnAwake: Whether it starts by itself when play begins.
startLifetime: How long each particle lives, in seconds.
startSpeed: How fast particles leave the emitter.
startSize: Each particle's starting size.
startColor: Each particle's starting colour.
gravityModifier: How much gravity pulls particles (0 none, 1 full).
rateOverTime: Particles emitted per second.
maxParticles: The most particles alive at once.
simulationSpeed: Plays the effect faster or slower.
shape: The shape particles are emitted from.
shapeRadius: The size of the emission shape.
renderMode: How each particle faces the camera.
blendMode: How particles mix with what is behind them.
Get: A setting's value by its key, as text.
@key: A setting's key, such as "startSpeed".
@value: The value as text, such as "5" or "1,0.5,0,1".
@clear: Also removes the particles alive now.
@count: How many particles to emit at once.
@seconds: How far to run the simulation.

## Example: A burst on impact
file: Scripts/ImpactBurst.cs
```csharp
using Elintria;

public class ImpactBurst : ElintriaScript
{
    public GameObject effect;   // has a ParticleEmitter, not looping

    public override void OnCollisionEnter(Collision collision)
    {
        ParticleEmitter p = effect.GetComponent<ParticleEmitter>();
        effect.transform.position = collision.GetContact(0).point;
        p.startColor = Color.yellow;
        p.Emit(Mathf.Clamp((int)collision.relativeVelocity.magnitude * 4, 5, 60));
    }
}
```

## Example: Engine smoke that follows the throttle
file: Scripts/ExhaustSmoke.cs
```csharp
using Elintria;

public class ExhaustSmoke : ElintriaScript
{
    ParticleEmitter smoke;

    public override void Start()
    {
        smoke = GetComponent<ParticleEmitter>();
        smoke.texture = "smoke";
        smoke.blendMode = ParticleBlend.AlphaBlended;
        smoke.looping = true;
        smoke.Play();
    }

    public override void Update()
    {
        float throttle = Mathf.Abs(Input.GetAxisVertical());
        smoke.rateOverTime = Mathf.Lerp(4f, 40f, throttle);
        smoke.startSpeed = Mathf.Lerp(0.5f, 3f, throttle);
    }
}
```

## Example: Building a campfire from code
file: Scripts/Campfire.cs
Uses properties for the common settings and `Set` for one by key.
```csharp
using Elintria;

public class Campfire : ElintriaScript
{
    public override void Start()
    {
        ParticleEmitter fire = GetComponent<ParticleEmitter>();
        fire.shape = ParticleShape.Cone;
        fire.shapeRadius = 0.3f;
        fire.texture = "flare";
        fire.blendMode = ParticleBlend.Additive;
        fire.startColor = new Color(1f, 0.5f, 0.1f, 1f);
        fire.gravityModifier = -0.2f;              // rises
        fire.Set("startLifetime", "0.6,1.2");      // a random range, by key
        fire.Simulate(2f);                          // start already burning
        fire.Play();
        Debug.Log("Particles now: " + fire.particleCount);
    }
}
```

# ParticleShape
summary: The shape a ParticleEmitter spawns particles from.
see: ParticleEmitter

## Members
Sphere: From inside a sphere, outward in every direction.
Hemisphere: From a half-sphere, upward.
Cone: In a cone along the emitter's forward axis (fire, sprays).
Box: From inside a box.
Circle: From a flat circle.
Edge: From a line.
Point: From a single point.

## Example: Explosion sphere
file: Scripts/ExplosionShape.cs
```csharp
using Elintria;

public class ExplosionShape : ElintriaScript
{
    public override void Start()
    {
        ParticleEmitter p = GetComponent<ParticleEmitter>();
        p.shape = ParticleShape.Sphere;
        p.shapeRadius = 0.5f;
        p.Emit(80);
    }
}
```

## Example: A rain box over the player
file: Scripts/RainBox.cs
```csharp
using Elintria;

public class RainBox : ElintriaScript
{
    public GameObject player;

    public override void Start()
    {
        ParticleEmitter p = GetComponent<ParticleEmitter>();
        p.shape = ParticleShape.Box;
        p.texture = "drop";
        p.rateOverTime = 400f;
        p.gravityModifier = 2f;
    }

    public override void Update()
    {
        if (player != null) transform.position = player.transform.position + Vector3.up * 12f;
    }
}
```

## Example: Choosing the shape from an int
file: Scripts/ShapePicker.cs
```csharp
using Elintria;

public class ShapePicker : ElintriaScript
{
    public int shapeIndex = 2;   // 0 Sphere .. 6 Point

    public override void Start()
    {
        GetComponent<ParticleEmitter>().shape = (ParticleShape)Mathf.Clamp(shapeIndex, 0, 6);
    }
}
```

# ParticleRenderMode
summary: How each particle is drawn: facing the camera, stretched along its motion, or lying flat.
see: ParticleEmitter

## Members
Billboard: Each particle faces the camera (the usual choice).
Stretched: Stretched along its velocity, for sparks and rain streaks.
Horizontal: Lies flat, facing up (ripples, ground dust).
Vertical: Stands upright, facing the camera horizontally.

## Example: Sparks stretched along motion
file: Scripts/SparkStreaks.cs
```csharp
using Elintria;

public class SparkStreaks : ElintriaScript
{
    public override void Start()
    {
        ParticleEmitter p = GetComponent<ParticleEmitter>();
        p.renderMode = ParticleRenderMode.Stretched;
        p.texture = "spark";
        p.blendMode = ParticleBlend.Additive;
    }
}
```

## Example: Ground ripples
file: Scripts/Ripples.cs
```csharp
using Elintria;

public class Ripples : ElintriaScript
{
    public override void Start()
    {
        ParticleEmitter p = GetComponent<ParticleEmitter>();
        p.renderMode = ParticleRenderMode.Horizontal;
        p.texture = "ring";
        p.startSpeed = 0f;
    }
}
```

## Example: Upright grass tufts
file: Scripts/Tufts.cs
```csharp
using Elintria;

public class Tufts : ElintriaScript
{
    public override void Start() { GetComponent<ParticleEmitter>().renderMode = ParticleRenderMode.Vertical; }
}
```

# ParticleBlend
summary: How particles mix with the scene behind them.
see: ParticleEmitter

## Members
AlphaBlended: Normal transparency, for smoke and dust.
Additive: Brightens what is behind, for fire, sparks, glows and magic.
Multiply: Darkens what is behind, for soot and shadows.

## Example: Glowing magic
file: Scripts/MagicGlow.cs
```csharp
using Elintria;

public class MagicGlow : ElintriaScript
{
    public override void Start()
    {
        ParticleEmitter p = GetComponent<ParticleEmitter>();
        p.blendMode = ParticleBlend.Additive;
        p.startColor = new Color(0.4f, 0.6f, 1f, 1f);
        p.texture = "glow";
    }
}
```

## Example: Dark soot
file: Scripts/Soot.cs
```csharp
using Elintria;

public class Soot : ElintriaScript
{
    public override void Start() { GetComponent<ParticleEmitter>().blendMode = ParticleBlend.Multiply; }
}
```

## Example: Day/night blend swap
file: Scripts/FireflyBlend.cs
Additive particles glow well at night but wash out by day.
```csharp
using Elintria;

public class FireflyBlend : ElintriaScript
{
    public bool night;

    public override void Update()
    {
        GetComponent<ParticleEmitter>().blendMode = night ? ParticleBlend.Additive : ParticleBlend.AlphaBlended;
    }
}
```

# GpuMesh
summary: Low-level geometry uploaded straight to the GPU, for drawing many copies with MeshInstance.
see: MeshInstance, GpuTexture, RuntimeMesh

`GpuMesh` is the fast, low-level path for scripts that draw a lot of geometry themselves, such as voxel worlds and custom renderers. Upload positions (and optionally UVs, colours and normals) once with `SetData`, then place copies with [[MeshInstance]]. No CPU copy is kept. For geometry attached to an ordinary GameObject, [[RuntimeMesh]] is simpler.

## Members
GpuMesh: Makes an empty GPU mesh.
SetData: Uploads the geometry. `positions` and `normals` hold 3 floats per vertex, `uvs` 2; `rgba` packs a colour per vertex as 0xAABBGGRR. UVs, colours and normals may be null.
@positions: x, y, z for each vertex.
@uvs: u, v for each vertex, or null.
@rgba: One packed colour per vertex, or null.
@normals: x, y, z for each vertex, or null.
@vertexCount: How many vertices.
@indices: Triangle indices, three per triangle.
@indexCount: How many indices to use.
Release: Frees the GPU memory. Instances using the mesh stop drawing.

## Example: A coloured triangle
file: Scripts/GpuTriangle.cs
```csharp
using Elintria;

public class GpuTriangle : ElintriaScript
{
    GpuMesh mesh;
    MeshInstance inst;

    public override void Start()
    {
        mesh = new GpuMesh();
        float[] pos = { 0, 2, 0,  -1, 0, 0,  1, 0, 0 };
        uint[] col = { 0xFF0000FF, 0xFF00FF00, 0xFFFF0000 };   // red, green, blue (0xAABBGGRR)
        mesh.SetData(pos, null, col, null, 3, new[] { 0, 1, 2 }, 3);
        inst = new MeshInstance(mesh);
        inst.SetPosition(0f, 1f, 5f);
        inst.SetMaterial(0, 1f, 1f, 1f, 1f, MaterialFlags.DoubleSided);
    }

    public override void OnDestroy() { inst.Destroy(); mesh.Release(); }
}
```

## Example: A field of cubes from one mesh
file: Scripts/CubeField.cs
One upload, many instances: much cheaper than many GameObjects.
```csharp
using System.Collections.Generic;
using Elintria;

public class CubeField : ElintriaScript
{
    readonly List<MeshInstance> cubes = new List<MeshInstance>();
    GpuMesh box;

    public override void Start()
    {
        box = BuildBox();
        for (int x = 0; x < 30; x++)
            for (int z = 0; z < 30; z++)
            {
                var i = new MeshInstance(box);
                i.SetPosition(x * 1.5f, Mathf.PerlinNoise(x * 0.2f, z * 0.2f) * 3f, z * 1.5f);
                i.SetColor(0.3f, 0.6f + x / 90f, 0.4f, 1f);
                cubes.Add(i);
            }
    }

    static GpuMesh BuildBox()
    {
        // 8 corners, 12 triangles, wound anticlockwise from outside.
        float[] p = { -.5f,-.5f,-.5f, .5f,-.5f,-.5f, .5f,.5f,-.5f, -.5f,.5f,-.5f, -.5f,-.5f,.5f, .5f,-.5f,.5f, .5f,.5f,.5f, -.5f,.5f,.5f };
        int[] t = { 0,2,1, 0,3,2, 4,5,6, 4,6,7, 0,1,5, 0,5,4, 3,6,2, 3,7,6, 0,4,7, 0,7,3, 1,2,6, 1,6,5 };
        var m = new GpuMesh();
        m.SetData(p, null, null, null, 8, t, t.Length);
        return m;
    }

    public override void OnDestroy() { foreach (var c in cubes) c.Destroy(); box.Release(); }
}
```

## Example: Rebuilding a chunk
file: Scripts/ChunkMesh.cs
Calling `SetData` again replaces the geometry, e.g. after a voxel edit.
```csharp
using Elintria;

public class ChunkMesh : ElintriaScript
{
    GpuMesh mesh = new GpuMesh();
    MeshInstance inst;

    public void Rebuild(float[] positions, int[] indices)
    {
        mesh.SetData(positions, null, null, null, positions.Length / 3, indices, indices.Length);
        if (inst == null) { inst = new MeshInstance(mesh); inst.SetPosition(0f, 0f, 0f); }
    }

    public override void OnDestroy() { if (inst != null) inst.Destroy(); mesh.Release(); }
}
```

# GpuTexture
summary: Create, load, update and destroy textures by id, including render textures for cameras.
see: GpuMesh, MeshInstance, RenderCamera, ScreenGUI

Textures are handled by integer id. Use `Load` to read an image file, `Create` to build one from your own pixels (RGBA bytes), `Update` to change its pixels, and `CreateRender` to make a texture a [[RenderCamera]] draws into. The ids work with [[MeshInstance.SetMaterial]] and [[ScreenGUI.Image]].

## Members
Create: Makes a `w` x `h` texture from RGBA bytes (4 per pixel, top row first). Returns its id.
@rgba: The pixels, 4 bytes each.
@flags: Filtering, wrapping and mipmaps.
Load: Loads an image file (PNG, JPG, BMP, TGA). Returns its id (0 on failure) and fills in its size.
@width: Filled in with the image width.
@height: Filled in with the image height.
Update: Replaces the pixels of texture `id` (same size).
Destroy: Frees texture `id`.
CreateRender: Makes a `w` x `h` texture a camera can draw into: a security monitor, a mirror, a minimap. Black until first drawn.

## Example: Showing a loaded image on screen
file: Scripts/Logo.cs
```csharp
using Elintria;

public class Logo : ElintriaScript
{
    int tex, w, h;

    public override void Start() { tex = GpuTexture.Load(Project.Resolve("Assets/UI/logo.png"), TextureFlags.Linear, out w, out h); }

    public override void Update()
    {
        if (tex != 0) ScreenGUI.Image(new Rect(20, 20, w * 0.5f, h * 0.5f), tex, Color.white);
    }

    public override void OnDestroy() { if (tex != 0) GpuTexture.Destroy(tex); }
}
```

## Example: A texture you paint from code
file: Scripts/PaintCanvas.cs
```csharp
using Elintria;

public class PaintCanvas : ElintriaScript
{
    const int Size = 128;
    byte[] pixels = new byte[Size * Size * 4];
    int tex;

    public override void Start() { tex = GpuTexture.Create(Size, Size, pixels, TextureFlags.Nearest); }

    public override void Update()
    {
        Rect area = new Rect(20, 20, Size * 3, Size * 3);
        Vector2 m = Input.mousePosition;
        if (Input.GetMouseButton(0) && area.Contains(m))
        {
            int x = (int)((m.x - area.x) / 3), y = (int)((m.y - area.y) / 3);
            int i = (y * Size + x) * 4;
            pixels[i] = 255; pixels[i + 1] = 200; pixels[i + 2] = 40; pixels[i + 3] = 255;
            GpuTexture.Update(tex, pixels);
        }
        ScreenGUI.Image(area, tex, Color.white);
    }
}
```

## Example: A security monitor
file: Scripts/SecurityMonitor.cs
A render texture shows what a second camera sees.
```csharp
using Elintria;

public class SecurityMonitor : ElintriaScript
{
    public GameObject cameraSpot;
    int screen;

    public override void Start() { screen = GpuTexture.CreateRender(320, 180); }

    public override void Update()
    {
        Transform c = cameraSpot.transform;
        RenderCamera.Render(screen, c.position, c.forward, Vector3.up, 60f, 0.1f, 80f);
        ScreenGUI.Image(new Rect(ScreenGUI.screenWidth - 340, 20, 320, 180), screen, Color.white);
    }

    public override void OnDestroy() { GpuTexture.Destroy(screen); }
}
```

# MeshInstance
summary: One placed, coloured and textured copy of a GpuMesh.
see: GpuMesh, GpuTexture, MaterialFlags

A `MeshInstance` draws a [[GpuMesh]] at a position (or with a full matrix) using a texture, a colour and [[MaterialFlags]]. Many instances can share one mesh. Destroy instances you no longer need.

## Members
MeshInstance: Makes an instance, optionally of `mesh`.
@mesh: The mesh to draw.
SetMesh: Changes which mesh it draws.
SetMeshId: Changes the mesh by its id.
@meshId: A GpuMesh id.
SetMatrix: Places the instance with a full 4x4 matrix (16 floats, column by column).
@m16: The matrix's 16 numbers.
SetPosition: Places the instance at (x, y, z) with no rotation or scale.
SetMaterial: Sets the texture (0 for none), the tint colour and the drawing flags.
@textureId: A GpuTexture id, or 0.
@r: Red tint.
@g: Green tint.
@b: Blue tint.
@a: Alpha.
SetColor: Changes just the tint colour.
visible: Whether it is drawn.
Destroy: Removes the instance.

## Example: A spinning, textured crate
file: Scripts/SpinCrate.cs
```csharp
using Elintria;

public class SpinCrate : ElintriaScript
{
    public GameObject meshSource;   // a script that built a GpuMesh exposes its id
    MeshInstance inst;
    float[] m = new float[16];

    public void Init(GpuMesh mesh, int texture)
    {
        inst = new MeshInstance(mesh);
        inst.SetMaterial(texture, 1f, 1f, 1f, 1f, MaterialFlags.None);
    }

    public override void Update()
    {
        if (inst == null) return;
        Matrix4x4 trs = Matrix4x4.TRS(transform.position, Quaternion.Euler(0f, Time.time * 90f, 0f), Vector3.one);
        for (int i = 0; i < 16; i++) m[i] = trs[i];
        inst.SetMatrix(m);
    }
}
```

## Example: Hiding far instances
file: Scripts/InstanceLod.cs
```csharp
using System.Collections.Generic;
using Elintria;

public class InstanceLod : ElintriaScript
{
    public float drawDistance = 60f;
    public readonly List<MeshInstance> instances = new List<MeshInstance>();
    public readonly List<Vector3> positions = new List<Vector3>();

    public override void Update()
    {
        for (int i = 0; i < instances.Count; i++)
            instances[i].visible = Vector3.Distance(positions[i], transform.position) < drawDistance;
    }
}
```

## Example: A see-through ghost
file: Scripts/GhostPreview.cs
```csharp
using Elintria;

public class GhostPreview : ElintriaScript
{
    MeshInstance ghost;

    public void Show(GpuMesh mesh, Vector3 at, bool valid)
    {
        if (ghost == null) ghost = new MeshInstance(mesh);
        ghost.SetPosition(at.x, at.y, at.z);
        ghost.SetMaterial(0, valid ? 0.3f : 1f, valid ? 1f : 0.3f, 0.3f, 0.4f, MaterialFlags.Transparent | MaterialFlags.NoDepthWrite);
    }

    public override void OnDestroy() { if (ghost != null) ghost.Destroy(); }
}
```

# MaterialFlags
summary: How a MeshInstance is drawn: cut-out, transparent, double-sided, unlit by fog, additive and more.
see: MeshInstance

Combine flags with `|` and pass them to [[MeshInstance.SetMaterial]].

## Members
None: An ordinary solid surface.
DoubleSided: Draws both sides of each triangle.
NoFog: Ignores the scene's fog.
Additive: Adds its colour to what is behind (glows).
NoDepthWrite: Does not hide what is drawn after it (for transparent effects).

## Example: Glass
file: Scripts/GlassFlags.cs
```csharp
using Elintria;

public class GlassFlags : ElintriaScript
{
    public static void MakeGlass(MeshInstance i)
    {
        i.SetMaterial(0, 0.7f, 0.9f, 1f, 0.3f, MaterialFlags.Transparent | MaterialFlags.DoubleSided);
    }
}
```

## Example: Leaves with cut-out alpha
file: Scripts/LeafFlags.cs
```csharp
using Elintria;

public class LeafFlags : ElintriaScript
{
    public static void MakeLeaves(MeshInstance i, int leafTexture)
    {
        i.SetMaterial(leafTexture, 1f, 1f, 1f, 1f, MaterialFlags.AlphaTest | MaterialFlags.DoubleSided);
    }
}
```

## Example: A held item drawn over everything
file: Scripts/ViewModelFlags.cs
```csharp
using Elintria;

public class ViewModelFlags : ElintriaScript
{
    public static void MakeViewModel(MeshInstance i, int tex)
    {
        i.SetMaterial(tex, 1f, 1f, 1f, 1f, MaterialFlags.Overlay | MaterialFlags.NoRenderTexture);
    }
}
```

# TextureFlags
summary: Filtering, wrapping and mipmap options for GpuTexture.
see: GpuTexture

## Members
Nearest: Sharp, blocky filtering: pixel art.
Linear: Smooth filtering.
Clamp: Edges don't repeat (instead of tiling).
Mipmaps: Builds smaller copies for distant, minified drawing.

## Example: Pixel art
file: Scripts/PixelArt.cs
```csharp
using Elintria;

public class PixelArt : ElintriaScript
{
    public override void Start()
    {
        int tex = GpuTexture.Load(Project.Resolve("Assets/Sprites/hero.png"), TextureFlags.Nearest | TextureFlags.Clamp, out int w, out int h);
        Debug.Log("Loaded " + w + "x" + h);
    }
}
```

## Example: A tiling ground texture
file: Scripts/GroundTex.cs
```csharp
using Elintria;

public class GroundTex : ElintriaScript
{
    public override void Start()
    {
        GpuTexture.Load(Project.Resolve("Assets/Textures/grass.png"), TextureFlags.Linear | TextureFlags.Mipmaps, out _, out _);
    }
}
```

## Example: A UI image without repeats
file: Scripts/UiTex.cs
```csharp
using Elintria;

public class UiTex : ElintriaScript
{
    public override void Start()
    {
        GpuTexture.Load(Project.Resolve("Assets/UI/panel.png"), TextureFlags.Linear | TextureFlags.Clamp, out _, out _);
    }
}
```

# RenderCamera
summary: Draws the scene from any point into a render texture: monitors, mirrors, portals, minimaps.
see: GpuTexture, ScriptCamera

Each call to `Render` asks for one extra scene pass this frame, drawn into a texture made with [[GpuTexture.CreateRender]]. Keep render textures small and update them in turn rather than all of them every frame.

## Members
ShowShadowsOnly: A flag that also draws Shadows Only renderers (for example a first-person body).
Render: Draws the scene from `position` looking along `forward` into `texture`.
@texture: A texture from GpuTexture.CreateRender.
@forward: Which way the camera looks.

## Example: A top-down minimap
file: Scripts/Minimap.cs
```csharp
using Elintria;

public class Minimap : ElintriaScript
{
    public GameObject player;
    int tex;

    public override void Start() { tex = GpuTexture.CreateRender(256, 256); }

    public override void Update()
    {
        Vector3 p = player.transform.position;
        RenderCamera.Render(tex, p + Vector3.up * 40f, Vector3.down, Vector3.forward, 50f, 1f, 100f);
        ScreenGUI.Image(new Rect(ScreenGUI.screenWidth - 220, 20, 200, 200), tex, Color.white);
    }
}
```

## Example: Cycling several monitors
file: Scripts/MonitorBank.cs
Only one camera is rendered each frame, which keeps the cost down.
```csharp
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
```

## Example: A rear-view mirror
file: Scripts/RearMirror.cs
```csharp
using Elintria;

public class RearMirror : ElintriaScript
{
    int tex;

    public override void Start() { tex = GpuTexture.CreateRender(300, 90); }

    public override void Update()
    {
        Vector3 eye = transform.position + Vector3.up * 1.2f;
        RenderCamera.Render(tex, eye, -transform.forward, Vector3.up, 50f, 0.3f, 120f, RenderCamera.ShowShadowsOnly);
        ScreenGUI.Image(new Rect(ScreenGUI.screenWidth / 2 - 150, 10, 300, 90), tex, Color.white);
    }
}
```

# ScriptCamera
summary: A camera driven entirely by your script, overriding the scene's Camera object.
see: RenderCamera, StdCamera

While set, the script camera replaces the scene's Camera in the editor's Game view and in the built game. Call `Set` every frame with where it is and where it looks. Call `Clear` to hand the view back to the scene camera.

## Members
Set: Puts the view at `position` looking along `forward`, with `up` as up and the given field of view and clip distances.
Clear: Stops overriding: the scene's Camera object is used again.

## Example: A free-fly camera
file: Scripts/FreeFly.cs
```csharp
using Elintria;

public class FreeFly : ElintriaScript
{
    public float speed = 10f;
    Vector3 pos;
    float yaw, pitch;

    public override void Start() { pos = transform.position; }

    public override void Update()
    {
        if (Input.GetMouseButton(1)) { yaw += Input.mouseDelta.x * 0.2f; pitch = Mathf.Clamp(pitch - Input.mouseDelta.y * 0.2f, -89f, 89f); }
        Quaternion q = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 fwd = q * Vector3.forward;
        pos += (fwd * Input.GetAxisVertical() + (q * Vector3.right) * Input.GetAxisHorizontal()) * speed * Time.deltaTime;
        ScriptCamera.Set(pos, fwd, Vector3.up, 70f, 0.1f, 500f);
    }

    public override void OnDisable() { ScriptCamera.Clear(); }
}
```

## Example: A cutscene camera dolly
file: Scripts/Dolly.cs
```csharp
using Elintria;

public class Dolly : ElintriaScript
{
    public GameObject from;
    public GameObject to;
    public GameObject lookAt;
    public float seconds = 5f;
    float t;

    public override void Update()
    {
        t += Time.deltaTime / seconds;
        if (t >= 1f) { ScriptCamera.Clear(); enabled = false; return; }
        Vector3 p = Vector3.Lerp(from.transform.position, to.transform.position, Mathf.SmoothStep(0f, 1f, t));
        ScriptCamera.Set(p, (lookAt.transform.position - p).normalized, Vector3.up, 45f, 0.1f, 300f);
    }
}
```

## Example: Screen shake
file: Scripts/ShakeCam.cs
```csharp
using Elintria;

public class ShakeCam : ElintriaScript
{
    public float trauma;

    public override void LateUpdate()
    {
        trauma = Mathf.Max(0f, trauma - Time.deltaTime);
        Vector3 shake = Random.insideUnitSphere * trauma * 0.3f;
        ScriptCamera.Set(transform.position + shake, -transform.forward, Vector3.up, 60f, 0.1f, 400f);
    }

    public void AddTrauma(float amount) { trauma = Mathf.Min(1f, trauma + amount); }
}
```

# RenderEnvironment
summary: Scene-wide look: sky colour or skybox, fog, height fog, daylight, power outages and brightness.
see: Light, Quality

`RenderEnvironment` changes the whole scene's atmosphere from code: the sky, distance fog, height fog in valleys, the daylight level for voxel-lit worlds, a blackout that dims powered lamps and screens, and an overall brightness setting.

## Members
SetSky: Sets a flat sky colour.
SetSkybox: Uses a panoramic sky: an .elmat whose texture is a latitude-longitude panorama, optionally tinted.
@material: A project-relative .elmat path.
@tint: A colour to multiply the sky by.
SetFog: Fog from distance `start` to `end` in colour (r, g, b). An `end` at or before `start` turns fog off.
@start: Where fog begins.
@end: Where fog is complete.
SetHeightFog: Fog that thickens below world height `top`, so things `depth` units below it are all but hidden.
@top: The height where the fog begins.
@depth: How far below `top` it becomes opaque.
SetDaylight: Scales sky light in voxel-lit materials (1 at noon).
@d: The daylight level, 0..1.
SetPower: A blackout: scales the ambient light and the glow of powered materials. 1, 1 is normal.
@ambient: The ambient light scale.
@emission: The emission scale of powered materials.
SetBrightness: A brightness (gamma) setting for players: 1 normal, above 1 brighter shadows.
@gamma: 0.2 to 5.
Stat: A renderer statistic: 0 instances drawn last frame, 1 triangles, 2 meshes, 3 instances, 4 GPU bytes.
@what: Which statistic.

## Example: Foggy swamp
file: Scripts/Swamp.cs
```csharp
using Elintria;

public class Swamp : ElintriaScript
{
    public override void Start()
    {
        RenderEnvironment.SetSky(0.45f, 0.5f, 0.45f);
        RenderEnvironment.SetFog(0.4f, 0.45f, 0.4f, 5f, 60f);
        RenderEnvironment.SetHeightFog(0.35f, 0.4f, 0.35f, 1.5f, 3f);
    }
}
```

## Example: A blackout event
file: Scripts/Blackout.cs
```csharp
using System.Collections;
using Elintria;

public class Blackout : ElintriaScript
{
    public void Trigger() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        for (int i = 0; i < 4; i++)   // flicker
        {
            RenderEnvironment.SetPower(0.2f, 0f);
            yield return new WaitForSeconds(0.1f);
            RenderEnvironment.SetPower(1f, 1f);
            yield return new WaitForSeconds(0.15f);
        }
        RenderEnvironment.SetPower(0.15f, 0f);
        yield return new WaitForSeconds(10f);
        RenderEnvironment.SetPower(1f, 1f);
    }
}
```

## Example: A brightness slider and stats
file: Scripts/VideoOptions.cs
```csharp
using Elintria;

public class VideoOptions : ElintriaScript
{
    float gamma = 1f;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp)) gamma = Mathf.Min(2f, gamma + 0.1f);
        if (Input.GetKeyDown(KeyCode.PageDown)) gamma = Mathf.Max(0.5f, gamma - 0.1f);
        RenderEnvironment.SetBrightness(gamma);
        ScreenGUI.Text(20, 20, "Brightness " + gamma.ToString("0.0") + "   triangles " + RenderEnvironment.Stat(1));
    }
}
```

# RenderZones
summary: Draw zones: skip drawing whole areas of a level the camera cannot see.
see: Bounds, RenderEnvironment

Split a level into boxes (rooms, floors, buildings). When the camera is inside a zone, only that zone, the zones you list as its neighbours, and any forced zones are drawn. Everything else keeps running but is not drawn, which can save a lot of time in large interiors. Zones are cleared when play stops.

## Members
Set: Defines up to 64 zone boxes, in priority order.
@zones: The boxes.
Clear: Removes all zones: everything is drawn.
SetNeighbours: The zones visible from `zone` besides itself.
@zone: A zone index.
@seen: The zone indices visible from it.
Force: Zones drawn wherever the camera is, as a bit mask (bit i = zone i).
@mask: The bit mask.
ZoneAt: The zone containing `p`, or -1.
HiddenLastFrame: How many objects and lights the zones skipped last frame.

## Example: Two rooms and a corridor
file: Scripts/LevelZones.cs
```csharp
using Elintria;

public class LevelZones : ElintriaScript
{
    public override void Start()
    {
        RenderZones.Set(new[]
        {
            new Bounds(new Vector3(0, 3, 0), new Vector3(20, 6, 20)),     // 0: hall
            new Bounds(new Vector3(0, 3, 20), new Vector3(4, 6, 20)),     // 1: corridor
            new Bounds(new Vector3(0, 3, 40), new Vector3(20, 6, 20)),    // 2: lab
        });
        RenderZones.SetNeighbours(0, 1);
        RenderZones.SetNeighbours(1, 0, 2);
        RenderZones.SetNeighbours(2, 1);
    }

    public override void OnDestroy() { RenderZones.Clear(); }
}
```

## Example: Always showing the sky island
file: Scripts/ForcedZone.cs
```csharp
using Elintria;

public class ForcedZone : ElintriaScript
{
    public int skyIslandZone = 3;

    public override void Start() { RenderZones.Force(1UL << skyIslandZone); }
}
```

## Example: Debug read-out
file: Scripts/ZoneDebug.cs
```csharp
using Elintria;

public class ZoneDebug : ElintriaScript
{
    public override void Update()
    {
        ScreenGUI.Text(20, 20, "Zone " + RenderZones.ZoneAt(transform.position) + ", hidden " + RenderZones.HiddenLastFrame);
    }
}
```

# Overlay2D
summary: Draws a list of textured 2D quads over the game image each frame, fast.
see: ScreenGUI, GpuTexture

`Overlay2D.Submit` draws many sprites in one call: each quad is 14 floats (texture id, x, y, w, h, u0, v0, u1, v1, r, g, b, a, angle in degrees) in game-image pixels from the top-left. Each call replaces the previous list. For ordinary UI, [[ScreenGUI]] is easier.

## Members
Submit: Draws `count` quads from `quads` (14 floats each) this frame.
@quads: The quad data.
@count: How many quads.

## Example: A particle-like 2D effect
file: Scripts/Snow2D.cs
```csharp
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
```

## Example: A rotating crosshair
file: Scripts/SpinCrosshair.cs
```csharp
using Elintria;

public class SpinCrosshair : ElintriaScript
{
    int tex;
    readonly float[] q = new float[14];

    public override void Start() { tex = GpuTexture.Load(Project.Resolve("Assets/UI/crosshair.png"), TextureFlags.Linear, out _, out _); }

    public override void Update()
    {
        float s = 32f;
        float[] v = { tex, GameWindow.width / 2f - s / 2f, GameWindow.height / 2f - s / 2f, s, s, 0, 0, 1, 1, 1, 1, 1, 1, Time.time * 90f };
        v.CopyTo(q, 0);
        Overlay2D.Submit(q, 1);
    }
}
```

## Example: Drawing a sprite-sheet frame
file: Scripts/SheetFrame.cs
UVs pick one cell of a 4x4 sheet.
```csharp
using Elintria;

public class SheetFrame : ElintriaScript
{
    public int sheet;   // texture id set elsewhere

    public override void Update()
    {
        int frame = (int)(Time.time * 8f) % 16;
        float u = (frame % 4) / 4f, v = (frame / 4) / 4f;
        float[] q = { sheet, 100, 100, 64, 64, u, v, u + 0.25f, v + 0.25f, 1, 1, 1, 1, 0 };
        Overlay2D.Submit(q, 1);
    }
}
```

# Lines3D
summary: Draws a list of coloured 3D lines each frame, in the game itself.
see: Debug

`Lines3D.Submit` draws world-space line segments as part of the game, which suits laser beams, trajectory previews and wireframes. Each line is 10 floats: x0, y0, z0, x1, y1, z1, r, g, b, a. Each call replaces the previous list.

## Members
Submit: Draws `count` lines (10 floats each) at `width` pixels this frame.
@lines: The line data.
@count: How many lines.
@width: Line width in pixels.

## Example: A grenade arc preview
file: Scripts/ArcPreview.cs
```csharp
using Elintria;

public class ArcPreview : ElintriaScript
{
    public float throwSpeed = 12f;
    const int Steps = 30;
    readonly float[] l = new float[Steps * 10];

    public override void Update()
    {
        Vector3 p = transform.position, v = transform.forward * throwSpeed;
        for (int i = 0; i < Steps; i++)
        {
            Vector3 n = p + v * 0.05f;
            v += Physics.gravity * 0.05f;
            int o = i * 10;
            l[o] = p.x; l[o + 1] = p.y; l[o + 2] = p.z; l[o + 3] = n.x; l[o + 4] = n.y; l[o + 5] = n.z;
            l[o + 6] = 1; l[o + 7] = 1; l[o + 8] = 0.2f; l[o + 9] = 1;
            p = n;
        }
        Lines3D.Submit(l, Steps, 2f);
    }
}
```

## Example: A laser beam
file: Scripts/LaserBeam.cs
```csharp
using Elintria;

public class LaserBeam : ElintriaScript
{
    public override void Update()
    {
        Vector3 a = transform.position;
        Vector3 b = Physics.Raycast(a, transform.forward, out RaycastHit hit, 50f) ? hit.point : a + transform.forward * 50f;
        Lines3D.Submit(new[] { a.x, a.y, a.z, b.x, b.y, b.z, 1f, 0.1f, 0.1f, 1f }, 1, 3f);
    }
}
```

## Example: A wireframe box
file: Scripts/WireBox.cs
```csharp
using System.Collections.Generic;
using Elintria;

public class WireBox : ElintriaScript
{
    public Vector3 size = Vector3.one;

    public override void Update()
    {
        Vector3 h = size * 0.5f, c = transform.position;
        var lines = new List<float>();
        int[,] e = { {0,1},{1,3},{3,2},{2,0},{4,5},{5,7},{7,6},{6,4},{0,4},{1,5},{2,6},{3,7} };
        Vector3 Corner(int i) => c + new Vector3((i & 1) != 0 ? h.x : -h.x, (i & 2) != 0 ? h.y : -h.y, (i & 4) != 0 ? h.z : -h.z);
        for (int i = 0; i < 12; i++)
        {
            Vector3 a = Corner(e[i, 0]), b = Corner(e[i, 1]);
            lines.AddRange(new[] { a.x, a.y, a.z, b.x, b.y, b.z, 0f, 1f, 0.5f, 1f });
        }
        Lines3D.Submit(lines.ToArray(), 12, 1.5f);
    }
}
```

# Quality
summary: Switch graphics quality levels and change graphics settings while the game runs.
see: RenderEnvironment

Every quality level from **Project Settings > Quality** is built into the game. A game starts on the default level. Use `SetLevel` for an options menu, or change individual settings such as anti-aliasing, shadows and frame-rate caps. Writing a setting changes the level now in force until you switch level again.

## Members
shadowDistance: How far from the camera shadows are drawn.
GetLevel: The level in force.
SetLevel: Switches to level `index`.
IncreaseLevel: Switches to the next higher level.
DecreaseLevel: Switches to the next lower level.

## Example: A graphics options menu
file: Scripts/GraphicsMenu.cs
```csharp
using Elintria;

public class GraphicsMenu : ElintriaScript
{
    public override void Update()
    {
        string[] names = Quality.names;
        for (int i = 0; i < names.Length; i++)
            if (ScreenGUI.Button(new Rect(40, 60 + i * 44, 200, 38), (i == Quality.level ? "> " : "") + names[i]))
                Quality.SetLevel(i);
    }
}
```

## Example: Adaptive quality by frame rate
file: Scripts/AutoQuality.cs
```csharp
using Elintria;

public class AutoQuality : ElintriaScript
{
    float slowFor;

    public override void Update()
    {
        if (Time.deltaTime > 1f / 40f) slowFor += Time.deltaTime; else slowFor = 0f;
        if (slowFor > 3f && Quality.level > 0)
        {
            Quality.DecreaseLevel();
            Debug.Log("Dropped to " + Quality.levelName);
            slowFor = 0f;
        }
    }
}
```

## Example: Individual settings
file: Scripts/CustomQuality.cs
```csharp
using Elintria;

public class CustomQuality : ElintriaScript
{
    public override void Start()
    {
        Quality.antiAliasing = 4;
        Quality.shadows = 2;               // hard and soft
        Quality.shadowDistance = 80f;
        Quality.renderScale = 0.9f;
        Quality.vSyncCount = 0;
        Quality.targetFrameRate = 144;
    }
}
```
