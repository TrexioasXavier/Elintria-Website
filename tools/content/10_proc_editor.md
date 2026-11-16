# Rng
summary: A seeded random number generator: the same seed always gives the same sequence.
see: Random, Noise, Poisson

`Rng` is a fast, repeatable random number generator (PCG). Make one per thing that needs its own reproducible randomness, such as one per level, per chunk or per dungeon, so results don't depend on what else used randomness first. It does ranges, chances, weighted picks, shuffles, points in circles and spheres, and normally distributed numbers.

## Members
Range: A random number from `min` (included) to `max` (excluded).
Gaussian: A normally distributed number around `mean`, spread by `sd` (Box-Muller).
Rng: Makes a generator from a seed (and optionally a stream, for several independent sequences from one seed).
@stream: Picks one of many independent sequences for the same seed.
NextUInt: The next raw 32-bit number.
Next01: A float from 0 (included) to 1 (excluded).
NextDouble: A double from 0 to 1.
Range(float min, float max): A float from `min` (included) to `max` (excluded).
Range(int min, int max): An int from `min` up to but not including `max`.
Chance: True with probability `p` (0..1).
@p: The probability.
Pick: A random item of `list`.
Shuffle: Shuffles `list` in place.
InsideUnitCircle: A random point inside a circle of radius 1.
OnUnitSphere: A random direction (a point on a sphere of radius 1).
@mean: The average value.
@sd: The standard deviation (how spread out values are).

## Example: The same level from the same seed
file: Scripts/SeededLevel.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class SeededLevel : ElintriaScript
{
    public int seed = 1234;
    public GameObject crateTemplate;

    public override void Start()
    {
        var rng = new Rng(seed);
        int crates = rng.Range(5, 12);
        for (int i = 0; i < crates; i++)
        {
            Vector2 p = rng.InsideUnitCircle() * 20f;
            GameObject c = Instantiate(crateTemplate, new Vector3(p.x, 0.5f, p.y), Quaternion.Euler(0f, rng.Range(0f, 360f), 0f));
            c.SetActive(true);
        }
    }
}
```

## Example: Weighted loot and shuffled decks
file: Scripts/LootRoller.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class LootRoller : ElintriaScript
{
    public override void Start()
    {
        var rng = new Rng(42);
        string[] items = { "coin", "potion", "sword", "dragon egg" };
        float[] weights = { 70f, 20f, 9f, 1f };
        for (int i = 0; i < 5; i++) Debug.Log("Drop: " + items[rng.WeightedIndex(weights)]);

        var deck = new List<string> { "A", "K", "Q", "J", "10" };
        rng.Shuffle(deck);
        Debug.Log("Deck: " + string.Join(" ", deck) + ", lucky: " + rng.Chance(0.1f) + ", pick: " + rng.Pick(deck));
    }
}
```

## Example: Natural variation with Gaussian
file: Scripts/TreeHeights.cs
Most values cluster around the mean, with a few outliers.
```csharp
using Elintria;
using Elintria.Procedural;

public class TreeHeights : ElintriaScript
{
    public override void Start()
    {
        var rng = new Rng(7, 3);   // seed 7, stream 3: separate from other uses of seed 7
        foreach (Transform tree in transform)
            tree.localScale = Vector3.one * Mathf.Max(0.3f, rng.Gaussian(1f, 0.2f));
    }
}
```

# Noise
summary: Coherent noise for terrain, textures and variation: Perlin, simplex, value, Worley, fBm, ridged, billow, warped.
see: Rng, Poisson, Mathf

Noise functions return smooth, repeatable values from coordinates. They are the basis of procedural terrain, clouds, caves and organic textures. `Noise.Seed` changes the pattern for every function. `HeightMap` builds a whole fBm grid in one call.

## Members
Seed: The seed every noise function uses; changing it changes the pattern.
Perlin: Perlin noise, about -1..1, in 2D or 3D. Use z = time to animate 2D noise.
PerlinTileable: Perlin noise that repeats every `periodX` by `periodY`: seamless textures.
@periodX: The repeat width (whole number).
@periodY: The repeat height (whole number).
@z: The third coordinate.
Simplex: Simplex noise, about -1..1, with fewer grid-aligned artefacts than Perlin.
Value: Value noise, 0..1: blocky-smooth, good for pixel-art variation.
Worley: Cellular noise: the distance to the nearest feature point (and, with `f2`, the second nearest). Good for cells, stones and scales.
@f2: Filled in with the distance to the second-nearest point.
Fbm: Fractal noise: several octaves of Perlin added together, about -1..1. The usual choice for terrain.
Ridged: Ridged noise, 0..1: sharp mountain ridges and cracks.
Billow: Billowy noise, 0..1: puffy clouds and rolling hills.
Warped: fBm of a point pushed around by more noise: swirls, marble, coastlines.
@strength: How far points are pushed.
HeightMap: A `w` by `h` grid of fBm heights, 0..1.
@w: The width in samples.
@h: The height in samples.

## Example: Terrain heights from noise
file: Scripts/NoiseTerrain.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class NoiseTerrain : ElintriaScript
{
    public int seed = 3;

    public override void Start()
    {
        Terrain t = Terrain.activeTerrain;
        if (t == null) return;
        TerrainData d = t.terrainData;
        int n = d.heightmapResolution;
        Noise.Seed = seed;
        float[,] h = new float[n, n];
        for (int y = 0; y < n; y++)
            for (int x = 0; x < n; x++)
            {
                float hills = Noise.Fbm(x * 0.01f, y * 0.01f) * 0.5f + 0.5f;
                float ridges = Noise.Ridged(x * 0.005f, y * 0.005f);
                h[y, x] = hills * 0.4f + ridges * ridges * 0.4f;
            }
        d.SetHeights(0, 0, h);
    }
}
```

## Example: A seamless cloud texture
file: Scripts/CloudTexture.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class CloudTexture : ElintriaScript
{
    public override void Start()
    {
        const int S = 128;
        byte[] px = new byte[S * S * 4];
        for (int y = 0; y < S; y++)
            for (int x = 0; x < S; x++)
            {
                float v = Noise.PerlinTileable(x / 16f, y / 16f, S / 16, S / 16) * 0.5f + 0.5f;
                byte b = (byte)(Mathf.Clamp01(v) * 255f);
                int i = (y * S + x) * 4;
                px[i] = b; px[i + 1] = b; px[i + 2] = b; px[i + 3] = 255;
            }
        int tex = GpuTexture.Create(S, S, px, TextureFlags.Linear | TextureFlags.Mipmaps);
        Debug.Log("Cloud texture " + tex);
    }
}
```

## Example: Animated and cellular effects
file: Scripts/LavaGlow.cs
3D Perlin with time as z animates a light; Worley makes a cell pattern for placing stones.
```csharp
using Elintria;
using Elintria.Procedural;

public class LavaGlow : ElintriaScript
{
    public override void Update()
    {
        Light l = GetComponent<Light>();
        if (l != null) l.intensity = 2f + Noise.Perlin(0.3f, 0.7f, Time.time * 0.8f) * 1.5f;
    }

    public static bool IsCellEdge(float x, float z)
    {
        float f1 = Noise.Worley(x * 0.5f, z * 0.5f, out float f2);
        return f2 - f1 < 0.08f;   // close to two cells at once: a crack between stones
    }
}
```

# Poisson
summary: Evenly spread random points: no two closer than a set distance.
see: ProcBuild, Rng

Poisson-disk sampling gives points that look random but never clump, which is ideal for trees, rocks, pickups and spawn points.

## Members
Sample: Points in a `width` by `height` area, none closer than `radius`.
@k: How many tries per point before giving up (more: denser, slower).

## Example: A natural-looking forest
file: Scripts/Forest.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class Forest : ElintriaScript
{
    public GameObject treeTemplate;
    public float size = 80f;
    public float spacing = 4f;

    public override void Start()
    {
        foreach (Vector2 p in Poisson.Sample(size, size, spacing, 99))
        {
            Vector3 at = transform.position + new Vector3(p.x - size / 2f, 0f, p.y - size / 2f);
            Instantiate(treeTemplate, at, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f)).SetActive(true);
        }
    }
}
```

## Example: Spawn points that keep players apart
file: Scripts/SpreadSpawns.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class SpreadSpawns : ElintriaScript
{
    public List<Vector3> Make(int seed)
    {
        var result = new List<Vector3>();
        foreach (Vector2 p in Poisson.Sample(60f, 60f, 15f, seed))
            result.Add(new Vector3(p.x - 30f, 1f, p.y - 30f));
        return result;
    }
}
```

## Example: Density by count
file: Scripts/PoissonCount.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class PoissonCount : ElintriaScript
{
    public override void Start()
    {
        foreach (float r in new[] { 2f, 4f, 8f })
            Debug.Log("radius " + r + ": " + Poisson.Sample(50f, 50f, r, 1).Count + " points");
    }
}
```

# Cell
summary: A grid cell position: integer x and y.
see: Grid, Room

## Members
x: The column.
y: The row.
Cell: Makes a cell position.
Equals: True when both are the same cell.

## Example: Neighbours of a cell
file: Scripts/CellNeighbours.cs
```csharp
using System.Collections.Generic;
using Elintria.Procedural;

public static class CellNeighbours
{
    public static IEnumerable<Cell> Of(Cell c)
    {
        yield return new Cell(c.x + 1, c.y);
        yield return new Cell(c.x - 1, c.y);
        yield return new Cell(c.x, c.y + 1);
        yield return new Cell(c.x, c.y - 1);
    }
}
```

## Example: Cells as dictionary keys
file: Scripts/CellVisits.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class CellVisits : ElintriaScript
{
    readonly Dictionary<Cell, int> visits = new Dictionary<Cell, int>();

    public override void Update()
    {
        var c = new Cell(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z));
        visits[c] = visits.TryGetValue(c, out int n) ? n + 1 : 1;
    }
}
```

## Example: Converting to a world position
file: Scripts/CellToWorld.cs
```csharp
using Elintria;
using Elintria.Procedural;

public static class CellToWorld
{
    public static Vector3 Center(Cell c, float cellSize) => new Vector3((c.x + 0.5f) * cellSize, 0f, (c.y + 0.5f) * cellSize);
}
```

# Room
summary: A rectangle of cells: a dungeon room.
see: Dungeon, Cell

## Members
x: The left column.
y: The top row.
w: The width in cells.
h: The height in cells.
Room: Makes a room.
Center: The room's middle cell.
Overlaps: True when this room and `o` overlap, allowing `margin` cells between them.
@margin: Extra space required between rooms.
Contains: True when cell `c` is inside the room.

## Example: Placing the boss in the biggest room
file: Scripts/BossRoom.cs
```csharp
using System.Linq;
using Elintria;
using Elintria.Procedural;

public class BossRoom : ElintriaScript
{
    public static Room Biggest(DungeonResult d) => d.rooms.OrderByDescending(r => r.w * r.h).First();
}
```

## Example: Rooms without overlap
file: Scripts/RoomPacker.cs
```csharp
using System.Collections.Generic;
using Elintria.Procedural;

public static class RoomPacker
{
    public static List<Room> Pack(int count, int seed)
    {
        var rng = new Rng(seed);
        var rooms = new List<Room>();
        for (int tries = 0; tries < 500 && rooms.Count < count; tries++)
        {
            var r = new Room(rng.Range(0, 50), rng.Range(0, 50), rng.Range(4, 10), rng.Range(4, 10));
            if (!rooms.Exists(o => o.Overlaps(r, 1))) rooms.Add(r);
        }
        return rooms;
    }
}
```

## Example: Which room is the player in?
file: Scripts/RoomOfPlayer.cs
```csharp
using Elintria;
using Elintria.Procedural;

public static class RoomOfPlayer
{
    public static int Index(DungeonResult d, Vector3 playerPos, float cellSize)
    {
        var c = new Cell((int)(playerPos.x / cellSize), (int)(playerPos.z / cellSize));
        return d.rooms.FindIndex(r => r.Contains(c));
    }
}
```

# Grid
summary: A 2D grid of cell values (Empty, Floor, Wall, Door, Corridor) with flood fill and distances.
see: Dungeon, Cell, ProcBuild

The generators in [[Dungeon]] produce `Grid`s. Each cell holds one of the constants. `Reachable` floods from a cell, `Distances` measures steps from it, and `AddWalls` puts walls around open areas.

## Members
Empty: Nothing (0).
Floor: Walkable floor (1).
Wall: A wall (2).
Door: A doorway (3).
Corridor: A corridor (4).
Width: How many columns.
Height: How many rows.
Grid: Makes a `w` by `h` grid filled with `fill`.
@fill: The starting value of every cell.
In: True when (x, y) is inside the grid.
this[int x, int y]: The value of cell (x, y).
Count: How many cells hold `v`.
Walkable: True for Floor, Door and Corridor values.
@start: The cell to start from.

## Example: Making sure the exit is reachable
file: Scripts/CheckReachable.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class CheckReachable : ElintriaScript
{
    public override void Start()
    {
        Grid caves = Dungeon.Caves(60, 40, 5);
        Cell start = new Cell(30, 20);
        for (int x = 0; x < caves.Width; x++) if (Grid.Walkable(caves[x, 20])) { start = new Cell(x, 20); break; }
        var reachable = caves.Reachable(start);
        Debug.Log(reachable.Count + " of " + caves.Count(Grid.Floor) + " floor cells reachable");
    }
}
```

## Example: Hand-made grid with walls
file: Scripts/ArenaGrid.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class ArenaGrid : ElintriaScript
{
    public override void Start()
    {
        var g = new Grid(20, 20);
        for (int y = 4; y < 16; y++) for (int x = 4; x < 16; x++) g[x, y] = Grid.Floor;
        g.AddWalls();
        Debug.Log(g.ToString());   // an ASCII picture of the grid
    }
}
```

## Example: Danger grows with distance
file: Scripts/DistanceDifficulty.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class DistanceDifficulty : ElintriaScript
{
    public override void Start()
    {
        DungeonResult d = Dungeon.Rooms(64, 64, 11);
        int[,] dist = d.grid.Distances(d.start);
        foreach (Room r in d.rooms)
        {
            int steps = dist[r.Center.x, r.Center.y];
            Debug.Log("Room at " + r.Center + ": " + (steps < 20 ? "easy" : steps < 50 ? "medium" : "hard"));
        }
    }
}
```

# DungeonResult
summary: What a room dungeon generator made: the grid, the rooms, the doors, and a start and end.
see: Dungeon, Grid, Room

## Members
grid: The cells.
rooms: The rooms.
doors: The doorways.

## Example: Player at the start, exit at the end
file: Scripts/StartEnd.cs
`end` is the walkable cell furthest from `start`, so it suits the exit.
```csharp
using Elintria;
using Elintria.Procedural;

public class StartEnd : ElintriaScript
{
    public GameObject player;
    public GameObject exit;
    public float cell = 2f;

    public override void Start()
    {
        DungeonResult d = Dungeon.Rooms(48, 48, 5);
        player.transform.position = new Vector3(d.start.x * cell, 1f, d.start.y * cell);
        exit.transform.position = new Vector3(d.end.x * cell, 0f, d.end.y * cell);
    }
}
```

## Example: Doors from the result
file: Scripts/DoorPlacer.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class DoorPlacer : ElintriaScript
{
    public GameObject doorTemplate;

    public override void Start()
    {
        DungeonResult d = Dungeon.Bsp(64, 64, 2);
        foreach (Cell c in d.doors)
            Instantiate(doorTemplate, new Vector3(c.x * 2f, 0f, c.y * 2f), Quaternion.identity).SetActive(true);
    }
}
```

## Example: A text map for debugging
file: Scripts/DungeonPrint.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class DungeonPrint : ElintriaScript
{
    public override void Start()
    {
        DungeonResult d = Dungeon.Rooms(40, 24, 8, 6);
        Debug.Log(d.rooms.Count + " rooms, " + d.doors.Count + " doors\n" + d.grid);
    }
}
```

# Dungeon
summary: Dungeon, cave, tunnel and maze generators that produce Grids.
see: Grid, DungeonResult, ProcBuild

## Members
Rooms: Rooms scattered without overlap, joined by corridors along a minimum spanning tree plus a few extra loops, with doors.
@roomCount: How many rooms to try for.
@minSize: The smallest room side.
@maxSize: The largest room side.
@extraLoops: How many extra corridors to add (0..1), so not every route is a dead end.
Bsp: Splits the area again and again, puts a room in each leaf, and joins sibling rooms.
@minLeaf: The smallest area to split.
@minRoom: The smallest room.
Caves: Caves by cellular automata: random fill, smoothed, with the largest open area kept and the rest joined to it.
@fill: How much starts solid (0..1).
@steps: How many smoothing passes.
DrunkardWalk: Winding tunnels carved by a random walk until `floorFraction` of the grid is open.
@floorFraction: How much of the grid to open up.
Maze: A perfect maze (exactly one route between any two cells) on a (2w+1) by (2h+1) grid.
@cellsX: Maze cells across.
@cellsY: Maze cells down.

## Example: Building a room dungeon
file: Scripts/RoomDungeon.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class RoomDungeon : ElintriaScript
{
    public int seed = 1;

    public override void Start()
    {
        DungeonResult d = Dungeon.Rooms(64, 64, seed, 12, 5, 11, 0.2f);
        var templates = new Dictionary<int, string> { { Grid.Floor, "FloorTile" }, { Grid.Corridor, "FloorTile" }, { Grid.Wall, "WallBlock" }, { Grid.Door, "DoorFrame" } };
        ProcBuild.BuildGrid(d.grid, 2f, Vector3.zero, templates, transform);
    }
}
```

## Example: Caves for a mining level
file: Scripts/MiningCaves.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class MiningCaves : ElintriaScript
{
    public override void Start()
    {
        Grid g = Dungeon.Caves(80, 80, 21, 0.47f, 5);
        int ore = 0;
        var rng = new Rng(21);
        for (int y = 0; y < g.Height; y++)
            for (int x = 0; x < g.Width; x++)
                if (g[x, y] == Grid.Wall && rng.Chance(0.02f)) ore++;
        Debug.Log("Cave with " + g.Count(Grid.Floor) + " floor cells and " + ore + " ore veins");
    }
}
```

## Example: A maze puzzle
file: Scripts/MazePuzzle.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class MazePuzzle : ElintriaScript
{
    public override void Start()
    {
        Grid maze = Dungeon.Maze(12, 12, Random.Range(0, 100000));
        ProcBuild.BuildGrid(maze, 1.5f, transform.position, new Dictionary<int, string> { { Grid.Wall, "HedgeBlock" } }, transform);
    }
}
```

# Wfc
summary: Wave Function Collapse: fill a grid with tiles that fit their neighbours by rules.
see: Grid, ProcBuild

Describe each tile's four edges as *sockets* with `AddTile`, or learn the rules from an example grid with `FromExample`. Optionally fix some cells with `Set`, then call `Run` to get a grid of tile indices in which every pair of neighbours fits. It is good for towns, pipe networks, terrain patches and dungeon layouts.

## Members
TileCount: How many tiles there are (rotations included).
TileName: The name of tile `i`.
TileIndex: The index of the tile called `name`.
AddTile: Adds a tile with edge sockets north, east, south and west. Two tiles may touch where the facing sockets are equal. `rotations` also adds its turned copies.
@north: The north edge's socket name.
@east: The east edge's socket name.
@south: The south edge's socket name.
@west: The west edge's socket name.
@weight: How often it should appear compared with others.
@rotations: Also add its 90, 180 and 270 degree copies.
FromExample: Learns tiles and rules from an example grid: tiles may touch as they touch in the example, weighted by how often each appears.
@sample: The example grid of tile values.
Set: Fixes cell (x, y) to a tile before running (an entrance, a river source).
@tile: The tile's name.
ClearFixed: Removes every fixed cell.
Allowed: Whether tile `b` may sit in direction `d` (0 N, 1 E, 2 S, 3 W) of tile `a`.
@d: The direction: 0 north, 1 east, 2 south, 3 west.
Run: A `width` by `height` grid of tile indices, or null if no arrangement was found in `retries` tries.
@retries: How many times to try before giving up.

## Example: Roads and grass
file: Scripts/RoadTiles.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class RoadTiles : ElintriaScript
{
    public override void Start()
    {
        var wfc = new Wfc()
            .AddTile("grass", "g", "g", "g", "g", 4f)
            .AddTile("road_straight", "r", "g", "r", "g", 1f, true)
            .AddTile("road_corner", "r", "r", "g", "g", 0.5f, true)
            .AddTile("road_cross", "r", "r", "r", "r", 0.2f);
        int[,] map = wfc.Run(16, 16, 42);
        if (map == null) { Debug.LogWarning("No layout found"); return; }
        for (int y = 0; y < 16; y++)
            for (int x = 0; x < 16; x++)
                if (wfc.TileName(map[x, y]).StartsWith("road")) Debug.DrawRay(new Vector3(x, 0f, y), Vector3.up, Color.gray, 10f);
    }
}
```

## Example: Learning from an example
file: Scripts/LearnedIslands.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class LearnedIslands : ElintriaScript
{
    public override void Start()
    {
        int[,] sample =
        {
            { 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 0 },
            { 0, 1, 2, 1, 0 },
            { 0, 1, 1, 1, 0 },
            { 0, 0, 0, 0, 0 },
        };   // 0 water, 1 sand, 2 forest
        int[,] world = Wfc.FromExample(sample).Run(40, 40, 7);
        Debug.Log(world != null ? "Generated islands" : "Try another seed");
    }
}
```

## Example: Fixing the entrance
file: Scripts/FixedEntrance.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class FixedEntrance : ElintriaScript
{
    public override void Start()
    {
        var wfc = new Wfc()
            .AddTile("wall", "w", "w", "w", "w")
            .AddTile("hall", "h", "h", "h", "h")
            .AddTile("door", "h", "w", "w", "w", 0.3f, true);
        wfc.Set(0, 8, "door");     // the entrance always here
        int[,] layout = wfc.Run(16, 16, 3, 20);
        wfc.ClearFixed();
        Debug.Log(layout == null ? "failed" : "entrance tile: " + wfc.TileName(layout[0, 8]));
    }
}
```

# LSystem
summary: L-systems: grow plants, coral and fractals by rewriting strings and drawing them with a turtle.
see: ProcBuild

An L-system starts from an *axiom* string and replaces characters by *rules* several times. The result is drawn with turtle commands: `F` (forward, drawing), `f` (forward), `+` and `-` (turn), and `[` and `]` (save and restore position, for branches).

## Members
Expand: `axiom` rewritten `iterations` times by `rules`.
Turtle: The line segments a turtle draws from `commands`.
@commands: The expanded string.
@step: How far F moves.
@angleDeg: How far + and - turn, in degrees.
@startAngleDeg: The starting direction (90 is straight up).

## Example: A fractal plant drawn with lines
file: Scripts/FractalPlant.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class FractalPlant : ElintriaScript
{
    List<(Vector2 a, Vector2 b)> segs;

    public override void Start()
    {
        var rules = new Dictionary<char, string> { { 'X', "F+[[X]-X]-F[-FX]+X" }, { 'F', "FF" } };
        string s = LSystem.Expand("X", rules, 5);
        segs = LSystem.Turtle(s, 0.05f, 25f);
    }

    public override void Update()
    {
        foreach (var seg in segs)
            Debug.DrawLine(transform.position + (Vector3)seg.a, transform.position + (Vector3)seg.b, Color.green);
    }
}
```

## Example: A Koch snowflake
file: Scripts/Snowflake.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class Snowflake : ElintriaScript
{
    public override void Start()
    {
        string s = LSystem.Expand("F--F--F", new Dictionary<char, string> { { 'F', "F+F--F+F" } }, 3);
        var lines = LSystem.Turtle(s, 0.2f, 60f, 0f);
        Debug.Log("Snowflake with " + lines.Count + " segments");
    }
}
```

## Example: Trees as 3D lines
file: Scripts/LineTree.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class LineTree : ElintriaScript
{
    float[] data;
    int count;

    public override void Start()
    {
        var segs = LSystem.Turtle(LSystem.Expand("F", new Dictionary<char, string> { { 'F', "F[+F]F[-F]F" } }, 3), 0.1f, 25.7f);
        count = segs.Count;
        data = new float[count * 10];
        for (int i = 0; i < count; i++)
        {
            Vector3 a = transform.position + (Vector3)segs[i].a, b = transform.position + (Vector3)segs[i].b;
            float[] l = { a.x, a.y, a.z, b.x, b.y, b.z, 0.4f, 0.8f, 0.3f, 1f };
            l.CopyTo(data, i * 10);
        }
    }

    public override void Update() { Lines3D.Submit(data, count, 2f); }
}
```

# ProcBuild
summary: Turn generated grids and points into scene objects by copying template objects.
see: Grid, Dungeon, Poisson

The generators produce data, and `ProcBuild` turns it into a scene. Keep one inactive *template* object per kind of piece (a floor tile, a wall, a tree) and these functions copy it where it belongs.

## Members
BuildGrid: Copies the template for each cell's value, `cellSize` apart from `origin` in X and Z. Cells whose value has no template stay empty. Returns the copies.
@grid: The grid to build.
@origin: Where cell (0, 0) goes.
Scatter: Copies `template` at evenly spread points over a `width` by `depth` area, with a random turn and size.
@depth: The area's depth.
@spacing: The smallest distance between copies.
@scaleJitter: How much each copy's size may vary (0..1).
BuildDungeon: A whole room dungeon in one call: floor and wall copies for every cell. Returns the number of rooms, and fills in a start point.
@rooms: How many rooms to try for.
@floorTemplate: The name of the floor template object.
@wallTemplate: The name of the wall template object.

## Example: A dungeon in one call
file: Scripts/OneCallDungeon.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class OneCallDungeon : ElintriaScript
{
    public GameObject player;

    public override void Start()
    {
        int rooms = ProcBuild.BuildDungeon(Random.Range(0, 99999), 48, 48, 10, 2f, "FloorTile", "WallBlock", Vector3.zero, out Vector3 start);
        player.transform.position = start + Vector3.up;
        Debug.Log(rooms + " rooms built; the player starts at " + start);
    }
}
```

## Example: Scattering rocks
file: Scripts/RockField.cs
```csharp
using Elintria;
using Elintria.Procedural;

public class RockField : ElintriaScript
{
    public override void Start()
    {
        var rocks = ProcBuild.Scatter("Rock", transform.position, 60f, 60f, 5f, 12, 0.4f);
        Debug.Log("Placed " + rocks.Count + " rocks");
    }
}
```

## Example: A maze from hedges
file: Scripts/HedgeMaze.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class HedgeMaze : ElintriaScript
{
    public override void Start()
    {
        Grid maze = Dungeon.Maze(10, 10, 5);
        List<GameObject> hedges = ProcBuild.BuildGrid(maze, 2f, transform.position, new Dictionary<int, string> { { Grid.Wall, "Hedge" } }, transform);
        foreach (GameObject h in hedges) h.transform.localScale = new Vector3(2f, 2.5f, 2f);
    }
}
```

# EditorWindow
summary: Your own dockable editor window, written in C# and drawn with EditorGUILayout.
see: EditorGUILayout, MenuItemAttribute, EditorGUI

Put editor scripts in `Scripts/Editor/`. They are compiled into a separate editor-only assembly that is never shipped with the game. Derive from `EditorWindow`, add a `void OnGUI()` method to draw it (it is found by name, so it is not an `override`; `OnEnable`, `OnDisable` and `Update` work the same way), and open it from a [[MenuItemAttribute|MenuItem]] with `GetWindow<T>()`. Windows dock like the built-in panels.

## Members
titleContent: The tab's title.
position: The window's drawing area, (0, 0) at its top-left.
width: The drawing area's width.
height: The drawing area's height.
GetWindow: The open window of type `T`, made and shown if there is none.
@title: The tab title for a new window.
@t: The window type.
Find: The open window of type `T`, or null; never makes one.
Show: Shows the window.
Focus: Brings the window to the front.
Close: Closes the tab; GetWindow brings it back as it was.
Repaint: Kept for Unity code; the editor repaints every frame anyway.

## Example: A simple tool window
file: Scripts/Editor/RenameTool.cs
```csharp
using Elintria;

public class RenameTool : EditorWindow
{
    string prefix = "Enemy_";
    int start = 1;

    [MenuItem("Window/My Tools/Rename Tool")]
    static void Open() { GetWindow<RenameTool>("Rename Tool"); }

    void OnGUI()
    {
        EditorGUILayout.Header("Batch rename");
        prefix = EditorGUILayout.TextField("Prefix", prefix);
        start = EditorGUILayout.IntField("Start at", start);
        if (EditorGUILayout.Button("Rename objects named 'Enemy'"))
        {
            int n = start;
            foreach (GameObject g in GameObject.FindObjectsWithNamePrefix("Enemy"))
                g.name = prefix + (n++);
        }
    }
}
```

## Example: A window that lists assets
file: Scripts/Editor/TextureBrowser.cs
```csharp
using Elintria;

public class TextureBrowser : EditorWindow
{
    string filter = "";
    string selected;

    [MenuItem("Window/My Tools/Texture Browser")]
    static void Open() { GetWindow<TextureBrowser>("Textures"); }

    void OnGUI()
    {
        filter = EditorGUILayout.SearchField(filter, "texfilter");
        EditorGUILayout.BeginScrollView(0f, "texlist");
        foreach (string path in AssetFiles.Find(".png", ".jpg"))
        {
            if (filter.Length > 0 && !path.ToLowerInvariant().Contains(filter.ToLowerInvariant())) continue;
            if (EditorGUILayout.ListRow(path, path == selected) > 0) selected = path;
        }
        EditorGUILayout.EndScrollView();
        if (selected != null) EditorGUILayout.Image(selected, 128f);
    }
}
```

## Example: Updating a window every frame
file: Scripts/Editor/PlayStats.cs
```csharp
using Elintria;

public class PlayStats : EditorWindow
{
    [MenuItem("Window/My Tools/Play Stats")]
    static void Open() { GetWindow<PlayStats>("Play Stats"); }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Window size", width + " x " + height);
        EditorGUILayout.LabelField("Time", Time.time.ToString("0.0"));
        EditorGUILayout.LabelField("Rigidbodies", GameObject.FindObjectsOfType<Rigidbody>().Length.ToString());
        if (EditorGUILayout.Button("Close")) Close();
    }
}
```

# EditorGUILayout
summary: Editor controls laid out automatically, one under another or side by side: labels, fields, buttons, lists.
see: EditorWindow, EditorGUI, GUILayout

Call these inside an [[EditorWindow]]'s `OnGUI`. Each control takes the next free space. Wrap controls in `BeginHorizontal`/`EndHorizontal` to place them side by side, and in `BeginScrollView`/`EndScrollView` to scroll. Field functions return the new value: write it back to your variable.

## Members
HelpBox: A message box with an icon: information, a warning or an error.
SearchField: A search box; returns the text typed. Give each one its own `id`.
Spacing: The gap between controls, in pixels.
RemainingHeight: The space left below the cursor in the current group.
GetRect: Reserves the next rectangle of `height` (and `width`) for your own drawing with EditorGUI.
Space: Adds an empty gap.
BeginHorizontal: Starts a row: controls until EndHorizontal sit side by side.
EndHorizontal: Ends the row.
BeginVertical: Starts a column inside a row.
EndVertical: Ends the column.
BeginArea: Lays out inside the rectangle `r` until EndArea.
EndArea: Ends the area.
BeginScrollView: Starts a scrolling region of `height` (0: the rest of the window). Give each one its own `id`.
EndScrollView: Ends the scrolling region.
Label: A line of text.
LabelField: A label and a value side by side.
Button: A button: true on the frame it is clicked.
TextField: An editable text field with a label.
FloatField: An editable number with a label.
IntField: An editable whole number with a label.
Slider: A slider from `min` to `max`.
Toggle: A checkbox.
Popup: A dropdown: returns the chosen index, or the chosen string.
Foldout: A collapsible header: returns whether it is open.
@open: Whether it is open now.
Toolbar: A row of tab buttons; returns the index of the selected one.
@labels: The tab labels.
Image: Draws an image file at `size` pixels.
Separator: A thin line across.
@o: Layout options such as GUILayout.Width(100).

## Example: A settings panel
file: Scripts/Editor/LevelSettingsWindow.cs
```csharp
using Elintria;

public class LevelSettingsWindow : EditorWindow
{
    float gravity = -9.8f, fogEnd = 120f;
    int difficulty;
    bool night;
    bool advanced;

    [MenuItem("Window/My Tools/Level Settings")]
    static void Open() { GetWindow<LevelSettingsWindow>("Level"); }

    void OnGUI()
    {
        EditorGUILayout.Header("Level");
        difficulty = EditorGUILayout.Popup("Difficulty", difficulty, new[] { "Easy", "Normal", "Hard" });
        night = EditorGUILayout.Toggle("Night", night);
        fogEnd = EditorGUILayout.Slider("Fog distance", fogEnd, 20f, 500f);
        advanced = EditorGUILayout.Foldout(advanced, "Advanced");
        if (advanced) gravity = EditorGUILayout.FloatField("Gravity", gravity);
        EditorGUILayout.Separator();
        if (EditorGUILayout.Button("Apply")) Debug.Log("difficulty " + difficulty + ", night " + night + ", fog " + fogEnd + ", gravity " + gravity);
    }
}
```

## Example: Tabs and rows
file: Scripts/Editor/TabbedTool.cs
```csharp
using Elintria;

public class TabbedTool : EditorWindow
{
    int tab;

    [MenuItem("Window/My Tools/Tabbed Tool")]
    static void Open() { GetWindow<TabbedTool>("Tabs"); }

    void OnGUI()
    {
        tab = EditorGUILayout.Toolbar(tab, new[] { "Spawn", "Clean up" });
        EditorGUILayout.Space(6f);
        if (tab == 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (EditorGUILayout.Button("Cube")) GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (EditorGUILayout.Button("Sphere")) GameObject.CreatePrimitive(PrimitiveType.Sphere);
            if (EditorGUILayout.Button("Light")) new GameObject("Light").AddComponent<Light>();
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.HelpBox("Removes every object whose name starts with 'Temp'.", MessageType.Warning);
            if (EditorGUILayout.Button("Delete temp objects"))
                foreach (GameObject g in GameObject.FindObjectsWithNamePrefix("Temp")) GameObject.Destroy(g);
        }
    }
}
```

## Example: Picking an asset
file: Scripts/Editor/MusicPicker.cs
```csharp
using Elintria;

public class MusicPicker : EditorWindow
{
    string track = "";

    [MenuItem("Window/My Tools/Music Picker")]
    static void Open() { GetWindow<MusicPicker>("Music"); }

    void OnGUI()
    {
        track = EditorGUILayout.AssetField("Track", track, ".ogg", ".wav");
        EditorGUILayout.LabelField("Chosen", track == "" ? "(none)" : track);
    }
}
```

# EditorGUI
summary: Editor controls drawn in rectangles you place yourself.
see: EditorGUILayout, EditorWindow

`EditorGUI` has the same kinds of controls as [[EditorGUILayout]], but you give each one a [[Rect]] in window coordinates. Use it for custom layouts, canvases, graphs and anything the automatic layout can't do.

## Members
LineHeight: The standard control height (20).
labelWidth: The width of the label column in EditorGUILayout fields.
Regular: The regular label style.
Label: Text in a rectangle, optionally bold, styled, dimmed and aligned.
@style: EditorGUI.Regular, Bold, Small, Large or Mono.
@dim: Whether the text is greyed out.
@align: 0 left, 1 centre, 2 right.
Button: A button in `r`; true when clicked.
@enabled: False greys it out.
ToolButton: A flat toolbar button, drawn pressed in when `on`.
@on: Whether it shows as pressed.
Toggle: A checkbox in `r`.
TextField: A text field in `r`. Give fields their own `id`.
@id: A unique id for this control, so it keeps its own state.
SearchField: A search box in `r`.
FloatField: A number field in `r`.
IntField: A whole-number field in `r`.
Slider: A slider in `r`.
Popup: A dropdown in `r`; returns the chosen index.
Foldout: A collapsible header in `r`.
AssetField: An asset slot showing `path`; its picker lists `options`.
ListRow: A list row: 1 when clicked, 2 when double-clicked, else 0.
Image: Draws an image file fitted into `r`.
DrawRect: Fills `r` with a colour.
DrawOutline: Outlines `r`.
Separator: A thin line across the middle of `r`.
Tooltip: Shows `text` when the mouse rests over `r`.
TextWidth: How wide `text` is in pixels.

## Example: A colour palette grid
file: Scripts/Editor/PaletteWindow.cs
```csharp
using Elintria;

public class PaletteWindow : EditorWindow
{
    int chosen;
    Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta, Color.white, Color.gray };

    [MenuItem("Window/My Tools/Palette")]
    static void Open() { GetWindow<PaletteWindow>("Palette"); }

    void OnGUI()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            Rect r = new Rect(10 + (i % 4) * 44, 10 + (i / 4) * 44, 40, 40);
            EditorGUI.DrawRect(r, colors[i]);
            if (i == chosen) EditorGUI.DrawOutline(r, Color.white);
            if (EditorGUI.Button(new Rect(r.x, r.y + 30, 40, 10), "")) chosen = i;
            EditorGUI.Tooltip(r, colors[i].ToString());
        }
        EditorGUI.Label(new Rect(10, 104, 200, EditorGUI.LineHeight), "Chosen: " + colors[chosen], EditorGUI.Mono, false);
    }
}
```

## Example: A custom bar chart
file: Scripts/Editor/ChartWindow.cs
```csharp
using Elintria;

public class ChartWindow : EditorWindow
{
    float[] values = { 3f, 7f, 2f, 9f, 5f };

    [MenuItem("Window/My Tools/Chart")]
    static void Open() { GetWindow<ChartWindow>("Chart"); }

    void OnGUI()
    {
        float max = 10f, w = (width - 20f) / values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            float h = values[i] / max * (height - 40f);
            EditorGUI.DrawRect(new Rect(10 + i * w, height - 20 - h, w - 6, h), new Color(0.4f, 0.6f, 1f));
            EditorGUI.Label(new Rect(10 + i * w, height - 18, w - 6, 16), values[i].ToString(), EditorGUI.Small, true, 1);
        }
    }
}
```

## Example: A hand-placed form
file: Scripts/Editor/QuickForm.cs
```csharp
using Elintria;

public class QuickForm : EditorWindow
{
    string itemName = "Sword";
    float damage = 10f;
    bool rare;

    [MenuItem("Window/My Tools/Quick Form")]
    static void Open() { GetWindow<QuickForm>("Form"); }

    void OnGUI()
    {
        EditorGUI.Label(new Rect(10, 10, 80, 20), "Name");
        itemName = EditorGUI.TextField(new Rect(90, 10, 160, 20), itemName, "name");
        EditorGUI.Label(new Rect(10, 36, 80, 20), "Damage");
        damage = EditorGUI.Slider(new Rect(90, 36, 160, 20), damage, 0f, 100f, "dmg");
        EditorGUI.Label(new Rect(10, 62, 80, 20), "Rare");
        rare = EditorGUI.Toggle(new Rect(90, 62, 20, 20), rare);
        EditorGUI.Separator(new Rect(10, 86, 240, 8));
        if (EditorGUI.Button(new Rect(10, 98, 240, 24), "Save " + itemName)) Debug.Log(itemName + " " + damage + (rare ? " (rare)" : ""));
    }
}
```

# GUILayout
summary: Layout helpers and options for editor windows: widths, heights, spacing and groups.
see: EditorGUILayout

`GUILayout.Width`, `Height` and `MinWidth` make options you pass to [[EditorGUILayout]] controls. `FlexibleSpace` pushes controls apart in a row. The group functions match those in EditorGUILayout, for Unity code.

## Members
Width: An option that makes a control exactly `w` pixels wide.
Height: An option that makes a control `h` pixels tall.
MinWidth: An option that makes a control at least `w` wide.
FlexibleSpace: In a row, takes a share of the spare width (pushes later controls to the right).
Space: An empty gap of `px` pixels.
Label: A line of text.
Button: A button; true when clicked.
BeginHorizontal: Starts a row.
EndHorizontal: Ends the row.
BeginVertical: Starts a column.
EndVertical: Ends the column.
BeginArea: Lays out inside `r`.
EndArea: Ends the area.

## Example: Right-aligned buttons
file: Scripts/Editor/DialogButtons.cs
```csharp
using Elintria;

public class DialogButtons : EditorWindow
{
    [MenuItem("Window/My Tools/Dialog Buttons")]
    static void Open() { GetWindow<DialogButtons>("Buttons"); }

    void OnGUI()
    {
        GUILayout.Label("Apply the changes?");
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel", GUILayout.Width(80))) Close();
        if (GUILayout.Button("OK", GUILayout.Width(80))) { Debug.Log("Applied"); Close(); }
        GUILayout.EndHorizontal();
    }
}
```

## Example: Two columns
file: Scripts/Editor/TwoColumns.cs
```csharp
using Elintria;

public class TwoColumns : EditorWindow
{
    [MenuItem("Window/My Tools/Two Columns")]
    static void Open() { GetWindow<TwoColumns>("Columns"); }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.Width(150));
        GUILayout.Label("Left column");
        GUILayout.Button("A");
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label("Right column");
        GUILayout.Button("B");
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}
```

## Example: A fixed-height preview area
file: Scripts/Editor/PreviewArea.cs
```csharp
using Elintria;

public class PreviewArea : EditorWindow
{
    [MenuItem("Window/My Tools/Preview Area")]
    static void Open() { GetWindow<PreviewArea>("Preview"); }

    void OnGUI()
    {
        GUILayout.Label("Preview:");
        Rect r = EditorGUILayout.GetRect(120f, GUILayout.MinWidth(200));
        EditorGUI.DrawRect(r, new Color(0.1f, 0.1f, 0.12f));
        GUILayout.Space(8f);
        GUILayout.Button("Refresh", GUILayout.Height(28));
    }
}
```

# MenuItemAttribute
summary: Adds a static method to the editor's menus.
see: EditorWindow

Mark a `static` method in an editor script with `[MenuItem("Menu/Sub/Item")]`. The first part of the path names the top-level menu. A second method with `isValidateFunction` true decides whether the item is enabled. Lower `priority` numbers are listed first.

## Members
menuItem: The menu path.
validate: Whether this marks the validate method.
priority: The item's order in its menu (lower comes first).
MenuItemAttribute: Puts the method in the menu at `itemName`.

## Example: A menu command
file: Scripts/Editor/MenuCommands.cs
```csharp
using Elintria;

public static class MenuCommands
{
    [MenuItem("Tools/Create Ground Plane")]
    static void CreateGround()
    {
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Plane);
        g.name = "Ground";
        g.transform.localScale = new Vector3(10f, 1f, 10f);
    }
}
```

## Example: An item that is only enabled sometimes
file: Scripts/Editor/ValidatedItem.cs
```csharp
using Elintria;

public static class ValidatedItem
{
    [MenuItem("Tools/Reset Player Position")]
    static void ResetPlayer() { GameObject.FindWithTag("Player").transform.position = Vector3.zero; }

    [MenuItem("Tools/Reset Player Position", true)]
    static bool CanResetPlayer() => GameObject.FindWithTag("Player") != null;
}
```

## Example: Ordering items
file: Scripts/Editor/OrderedItems.cs
```csharp
using Elintria;

public static class OrderedItems
{
    [MenuItem("Tools/Level/Build", false, 10)] static void Build() { Debug.Log("Build"); }
    [MenuItem("Tools/Level/Validate", false, 20)] static void Validate() { Debug.Log("Validate"); }
    [MenuItem("Tools/Level/Clear", false, 100)] static void Clear() { Debug.Log("Clear"); }
}
```

# MessageType
summary: The icon and colour of an EditorGUILayout.HelpBox.
see: EditorGUILayout

## Members
None: No icon.
Info: Information.
Warning: A warning.
Error: An error.

## Example: Validation messages
file: Scripts/Editor/Validator.cs
```csharp
using Elintria;

public class Validator : EditorWindow
{
    [MenuItem("Window/My Tools/Validator")]
    static void Open() { GetWindow<Validator>("Validator"); }

    void OnGUI()
    {
        bool hasPlayer = GameObject.FindWithTag("Player") != null;
        bool hasLight = GameObject.FindObjectsOfType<Light>().Length > 0;
        EditorGUILayout.HelpBox(hasPlayer ? "Player found." : "No object is tagged Player.", hasPlayer ? MessageType.Info : MessageType.Error);
        if (!hasLight) EditorGUILayout.HelpBox("The scene has no lights.", MessageType.Warning);
    }
}
```

## Example: A plain note
file: Scripts/Editor/Note.cs
```csharp
using Elintria;

public class Note : EditorWindow
{
    [MenuItem("Window/My Tools/Note")]
    static void Open() { GetWindow<Note>("Note"); }
    void OnGUI() { EditorGUILayout.HelpBox("Remember to bake the nav mesh after editing the level.", MessageType.None); }
}
```

## Example: Choosing the type from a count
file: Scripts/Editor/ErrorCount.cs
```csharp
using Elintria;

public static class ErrorCount
{
    public static void Show(int errors)
    {
        MessageType t = errors == 0 ? MessageType.Info : errors < 5 ? MessageType.Warning : MessageType.Error;
        EditorGUILayout.HelpBox(errors + " problems found", t);
    }
}
```

# EditorApplication
summary: The editor itself: an every-frame update event and the open project's paths.
see: EditorWindow, AssetFiles

## Members
update: Raised every editor frame. Use it for background tool work.
projectPath: The open project's folder.
ProjectFile: The absolute path of a project file.
@rel: A project-relative path like "Assets/Data/x.json".

## Example: An autosave timer
file: Scripts/Editor/AutoNote.cs
```csharp
using Elintria;

public static class AutoNote
{
    static float last;

    [MenuItem("Tools/Start Reminder")]
    static void Start()
    {
        EditorApplication.update += () =>
        {
            if (Time.realtimeSinceStartup - last > 600f) { last = Time.realtimeSinceStartup; Debug.Log("Ten minutes passed: remember to save!"); }
        };
    }
}
```

## Example: Writing a project file
file: Scripts/Editor/ExportNames.cs
```csharp
using System.IO;
using Elintria;

public static class ExportNames
{
    [MenuItem("Tools/Export Enemy Names")]
    static void Export()
    {
        string path = EditorApplication.ProjectFile("Assets/Data/enemies.txt");
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        using (var w = new StreamWriter(path))
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy")) w.WriteLine(g.name);
        AssetFiles.Refresh();
        Debug.Log("Wrote " + path);
    }
}
```

## Example: Showing the project folder
file: Scripts/Editor/ProjectInfo.cs
```csharp
using Elintria;

public class ProjectInfo : EditorWindow
{
    [MenuItem("Window/My Tools/Project Info")]
    static void Open() { GetWindow<ProjectInfo>("Project"); }
    void OnGUI() { EditorGUILayout.LabelField("Folder", EditorApplication.projectPath); }
}
```

# EditorUtility
summary: Editor dialogs, such as a modal yes/no question.
see: EditorWindow

## Members
DisplayDialog: Shows a modal question and returns true when `ok` is chosen.
@message: The question.

## Example: Confirming a destructive action
file: Scripts/Editor/ClearScene.cs
```csharp
using Elintria;

public static class ClearScene
{
    [MenuItem("Tools/Delete All Enemies")]
    static void DeleteEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (!EditorUtility.DisplayDialog("Delete enemies", "Delete " + enemies.Length + " enemies?", "Delete", "Cancel")) return;
        foreach (GameObject e in enemies) GameObject.Destroy(e);
    }
}
```

## Example: An OK-only message
file: Scripts/Editor/Done.cs
```csharp
using Elintria;

public static class Done
{
    [MenuItem("Tools/Say Hello")]
    static void Hello() { EditorUtility.DisplayDialog("Hello", "Your tool ran.", "OK"); }
}
```

## Example: Asking before overwriting
file: Scripts/Editor/OverwriteCheck.cs
```csharp
using System.IO;
using Elintria;

public static class OverwriteCheck
{
    public static bool OkToWrite(string path) =>
        !File.Exists(path) || EditorUtility.DisplayDialog("File exists", Path.GetFileName(path) + " already exists. Replace it?", "Replace", "Keep");
}
```

# AssetFiles
summary: Lists the project's files by extension, as "Assets/..." paths.
see: EditorGUILayout, EditorApplication

## Members
Find: Every file under Assets/ ending in one of `extensions` (all files when none are given). Cached for two seconds.
Refresh: Forgets the cached lists (after you write files).

## Example: Counting assets by type
file: Scripts/Editor/AssetCounts.cs
```csharp
using Elintria;

public class AssetCounts : EditorWindow
{
    [MenuItem("Window/My Tools/Asset Counts")]
    static void Open() { GetWindow<AssetCounts>("Assets"); }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Textures", AssetFiles.Find(".png", ".jpg", ".tga").Length.ToString());
        EditorGUILayout.LabelField("Models", AssetFiles.Find(".fbx", ".obj").Length.ToString());
        EditorGUILayout.LabelField("Sounds", AssetFiles.Find(".wav", ".ogg").Length.ToString());
    }
}
```

## Example: Finding unused materials (a start)
file: Scripts/Editor/MaterialList.cs
```csharp
using Elintria;

public static class MaterialList
{
    [MenuItem("Tools/List Materials")]
    static void List()
    {
        foreach (string m in AssetFiles.Find(".elmat")) Debug.Log(m);
    }
}
```

## Example: Refreshing after generating files
file: Scripts/Editor/GenerateData.cs
```csharp
using System.IO;
using Elintria;

public static class GenerateData
{
    [MenuItem("Tools/Generate Item Files")]
    static void Generate()
    {
        for (int i = 0; i < 3; i++)
            File.WriteAllText(EditorApplication.ProjectFile("Assets/Items/item" + i + ".json"), "{ \"id\": \"item" + i + "\" }");
        AssetFiles.Refresh();
        Debug.Log(AssetFiles.Find(".json").Length + " json files now");
    }
}
```
