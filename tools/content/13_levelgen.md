# LevelGenerator
summary: Makes levels while the game runs, with the same generator as Window > Level Generator: rooms, corridors, stairs, props, lights and spawn points, drawn and given colliders.
see: LevelGenSettings, GeneratedLevel, LevelMarker, LevelGenUtil

`LevelGenerator.Generate` takes a plain-English description (the words the window understands: "large dark bunker, two floors, six robots") and/or `"key=value;..."` values: `seed`, `theme`, `rooms`, `floors`, and every option of the window (`style=2`, `mission=1`, `vertical=1`, `secrets=1`, `objective=1`...). With the values left out a level is exactly what the window makes for the same description and seed.

The level's meshes are drawn by the script renderer ([[GpuMesh]], [[MeshInstance]]), its floors and walls get box colliders and each stair flight a ramp, and its spawn points, pickups, lights and keys come back as [[LevelMarker]]s for your own code to fill. The generator lives in the program running the game - the editor in Play mode and a built game - so `available` is false anywhere else.

## Members
available: Whether the program running the game has the generator.
lastError: Why the last Generate (or BuildManifest) returned null.
BuildManifest: Plans the level and writes its meshes into `outDir`, returning the generator's JSON manifest (null on failure). Generate reads it for you; this is for code that wants the raw data.
@description: A plain-English description, read first (may be empty).
@values: "key=value;..." on top of it.
@outDir: A folder for the .obj files (empty: one under the temp folder).
@floor: -1 for every floor, else that floor only.
Generate: Makes the level and puts it at `origin`. Returns null (see lastError) when it cannot.
@settings: The description and values, built in code.
@origin: Where the level's centre goes.
@draw: Draw its meshes.
@colliders: Give its floors, walls and custom props box colliders.
@lights: Make its Light objects (flickering and strobing as the options ask, and the fill light) and set its fog.

## Example: A new dungeon every run
file: Scripts/RunStart.cs
A roguelike's first room: a fresh seed each time, and the player put at the start.
```csharp
using Elintria;

public class RunStart : ElintriaScript
{
    public GameObject player;

    public override void Start()
    {
        int seed = (int)(System.DateTime.Now.Ticks % 100000);
        GeneratedLevel level = LevelGenerator.Generate("small dungeon, lots of loot", "seed=" + seed);
        if (level == null) { Debug.LogWarning("No level: " + LevelGenerator.lastError); return; }
        player.transform.position = level.playerStart + new Vector3(0f, 1f, 0f);
        Debug.Log("Seed " + seed + ": " + level.rooms + " rooms");
    }
}
```

## Example: Locks, keys and a mini-boss
file: Scripts/KeyHunt.cs
Any of the window's options by key: here the mission structure, with lever gates.
```csharp
using Elintria;

public class KeyHunt : ElintriaScript
{
    public GameObject keyTemplate;
    public GameObject bossTemplate;

    public override void Start()
    {
        GeneratedLevel level = LevelGenerator.Generate("abandoned facility", "seed=12;mission=1;missionLocks=3;missionLockKind=0");
        if (level == null) return;
        level.PlaceCopies("Key", keyTemplate);
        level.PlaceCopies("MiniBoss", bossTemplate);
        Debug.Log(level.Markers("LockedDoor").Count + " locked doors on the way out");
    }
}
```

## Example: Checking a level before using it
file: Scripts/RawManifest.cs
BuildManifest only plans and writes the files; read the JSON yourself.
```csharp
using System.Collections.Generic;
using Elintria;

public class RawManifest : ElintriaScript
{
    public override void Start()
    {
        if (!LevelGenerator.available) return;
        string json = LevelGenerator.BuildManifest("cave", "seed=4;style=2", "", -1);
        if (json == null) return;
        var root = (Dictionary<string, object>)Json.Parse(json);
        Debug.Log("Rooms: " + Json.Int(root, "rooms") + ", meshes: " + Json.List(root, "meshes").Count);
    }
}
```

# LevelGenSettings
summary: The description and values for LevelGenerator, built up in code instead of a "key=value" string.
see: LevelGenerator

Every `Set` replaces an earlier value for the same key; booleans are written 1 and 0, numbers with a dot, whatever the machine's language. The keys are the window's: `levelgen.options` on the editor bridge lists them all with their ranges.

## Members
description: A plain-English description, read first ("large dark bunker, two floors").
LevelGenSettings: New settings, with no values (or with a description).
@description: The description to read first.
Set: Sets any value by its key: "seed", "theme", "rooms", "floors", or an option ("mission", "style", "vertical"...). Returns the settings, so calls chain.
@key: The value's key.
@value: The value: a bool, number or text.
Seed: The seed (same seed and values, same level).
Theme: The theme by name: Facility, Hospital, Office, Industrial, Dungeon, Cave, Space Station, Bunker.
@theme: The theme's name.
Rooms: Rooms per floor.
@rooms: How many, 3 to 60.
Floors: Stacked floors.
@floors: How many, 1 to 8.
Values: The "key=value;key=value" text.
ToString: The same as Values.

## Example: Chained settings
file: Scripts/TowerLevel.cs
```csharp
using Elintria;

public class TowerLevel : ElintriaScript
{
    public override void Start()
    {
        var s = new LevelGenSettings("bright office")
            .Seed(77).Floors(3).Rooms(10)
            .Set("vertical", true)      // ladders, lifts and an atrium
            .Set("lightPreset", 5);     // Daylight
        GeneratedLevel level = LevelGenerator.Generate(s);
        if (level != null) Debug.Log("Floors: " + level.floors);
    }
}
```

## Example: Difficulty from the player's progress
file: Scripts/Difficulty.cs
```csharp
using Elintria;

public class Difficulty : ElintriaScript
{
    public int depth = 1;

    public GeneratedLevel Next()
    {
        var s = new LevelGenSettings("dark caves")
            .Seed(1000 + depth)
            .Set("enemies", 3 + depth * 2)
            .Set("loot", 4 + depth)
            .Set("boss", depth % 5 == 0);
        ++depth;
        return LevelGenerator.Generate(s);
    }
}
```

## Example: Logging what will be asked for
file: Scripts/SettingsLog.cs
```csharp
using Elintria;

public class SettingsLog : ElintriaScript
{
    public override void Start()
    {
        var s = new LevelGenSettings().Theme("Hospital").Seed(3).Set("secrets", true).Set("secretRooms", 2);
        Debug.Log("Asking for: " + s.Values());   // theme=Hospital;seed=3;secrets=1;secretRooms=2
    }
}
```

# GeneratedLevel
summary: A level LevelGenerator made at run time: its markers, its drawn meshes and its colliders.
see: LevelGenerator, LevelMarker

Positions are world positions: the generator's own plus `origin`. `Destroy` removes everything the level made, including copies placed with `PlaceCopies`.

## Members
seed: The seed it was made with.
rooms: How many rooms it has.
floors: How many floors were built.
floorHeight: Metres from one floor to the next.
wallHeight: Metres from floor to ceiling.
origin: Where the level was put (added to every position).
markers: Every marker: spawns, pickups, lights, keys, the exit...
meshFiles: The .obj files written for it.
manifest: The generator's JSON, for anything this class does not read.
playerStart: The player start (the origin when there is none).
exit: The exit (the origin when there is none).
Markers: Every marker of one kind ("Enemy", "Loot", "Light"...).
@kind: The kind, matched without regard to case.
PlaceCopies: A copy of `template` at every marker of `kind`, turned to its yaw. The copies go when the level is destroyed.
@template: The object copied (usually kept switched off).
SetVisible: Shows or hides the drawn meshes; colliders stay.
@visible: Shown or not.
Destroy: Removes the meshes, colliders and placed copies.

## Example: Filling a level with enemies and loot
file: Scripts/Populate.cs
```csharp
using Elintria;

public class Populate : ElintriaScript
{
    public GameObject zombie;
    public GameObject crate;

    public override void Start()
    {
        GeneratedLevel level = LevelGenerator.Generate("hospital, eight zombies", "seed=5");
        if (level == null) return;
        int z = level.PlaceCopies("Enemy", zombie).Count;
        int c = level.PlaceCopies("Loot", crate).Count;
        Debug.Log(z + " zombies, " + c + " crates");
    }
}
```

## Example: Moving on to the next level
file: Scripts/NextLevel.cs
```csharp
using Elintria;

public class NextLevel : ElintriaScript
{
    public GameObject player;
    GeneratedLevel current;
    int number;

    public override void Start() { Load(); }

    public override void Update()
    {
        if (current != null && Vector3.Distance(player.transform.position, current.exit) < 2f) Load();
    }

    void Load()
    {
        if (current != null) current.Destroy();
        current = LevelGenerator.Generate("bunker", "seed=" + (++number));
        if (current != null) player.transform.position = current.playerStart + Vector3.up;
    }
}
```

## Example: A map view that hides the level
file: Scripts/MapToggle.cs
```csharp
using Elintria;

public class MapToggle : ElintriaScript
{
    public GeneratedLevel level;
    bool shown = true;

    public override void Update()
    {
        if (level == null || !Input.GetKeyDown(KeyCode.M)) return;
        shown = !shown;
        level.SetVisible(shown);
    }
}
```

# LevelMarker
summary: A spot the generator marked in a level: a spawn, a pickup, a light, a key, a door...
see: GeneratedLevel

## Members
kind: PlayerStart, Enemy, Boss, Loot, Exit, Light, Key, LockedDoor, Lever, MiniBoss, Objective, Npc, Plate, Ambient or Waypoint.
position: Where it is, in the world.
yaw: Its facing, degrees about +Y.
room: The room it is in (-1 for none).
floor: The floor it is on, from 0.
count: How many (a loot pile's size; 1 for a room tone).
item: A pickup's or key's item id, a locked door's key.
color: A light's colour.
intensity: A light's brightness (0: a dead fitting).
range: A light's reach, or a sound's.

## Example: Lights from the markers
file: Scripts/LevelLights.cs
```csharp
using Elintria;

public class LevelLights : ElintriaScript
{
    public GameObject lampTemplate;

    public override void Start()
    {
        GeneratedLevel level = LevelGenerator.Generate("dark facility", "seed=8");
        if (level == null) return;
        foreach (LevelMarker m in level.Markers("Light"))
        {
            if (m.intensity <= 0f) continue;          // a dead fitting
            GameObject lamp = Instantiate(lampTemplate, m.position, Quaternion.identity);
            Light l = lamp.GetComponent<Light>();
            if (l != null) { l.color = m.color; l.intensity = m.intensity; l.range = m.range; }
        }
    }
}
```

## Example: Keys with their ids
file: Scripts/KeyPlacer.cs
```csharp
using Elintria;

public class KeyPlacer : ElintriaScript
{
    public override void Start()
    {
        GeneratedLevel level = LevelGenerator.Generate("dungeon", "seed=2;mission=1");
        if (level == null) return;
        foreach (LevelMarker m in level.Markers("Key"))
            Debug.Log("Key \"" + m.item + "\" in room " + m.room + " on floor " + (m.floor + 1));
    }
}
```

## Example: Counting by kind
file: Scripts/MarkerCount.cs
```csharp
using System.Collections.Generic;
using Elintria;

public class MarkerCount : ElintriaScript
{
    public override void Start()
    {
        GeneratedLevel level = LevelGenerator.Generate("office", "seed=3");
        if (level == null) return;
        var counts = new Dictionary<string, int>();
        foreach (LevelMarker m in level.markers)
            counts[m.kind] = counts.TryGetValue(m.kind, out int n) ? n + 1 : 1;
        foreach (var kv in counts) Debug.Log(kv.Key + ": " + kv.Value);
    }
}
```

# LevelGenUtil
summary: What the Visual Scripting Level Generator nodes remember: the last level LevelGenerator made.
see: LevelGenerator

The **Generate Level**, **Place At Markers**, **Count Markers** and **Destroy Generated Level** nodes call these; scripts can too.

## Members
last: The last level LevelGenerator.Generate made (null before the first, or after DestroyLast).
Place: A copy of the object named `templateName` at every marker of `kind` in the last level. Returns how many.
@kind: The marker kind.
@templateName: The name of an object in the scene.
Count: How many markers of `kind` the last level has.
DestroyLast: Destroys the last level and forgets it.

## Example: The nodes' calls in code
file: Scripts/NodeEquivalent.cs
```csharp
using Elintria;

public class NodeEquivalent : ElintriaScript
{
    public override void Start()
    {
        LevelGenerator.Generate("warehouse", "seed=9");
        int placed = LevelGenUtil.Place("Enemy", "Template_Robot");
        Debug.Log(placed + " of " + LevelGenUtil.Count("Enemy") + " enemy spots filled");
    }
}
```

## Example: Clearing up on restart
file: Scripts/Restart.cs
```csharp
using Elintria;

public class Restart : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.R)) return;
        LevelGenUtil.DestroyLast();
        LevelGenerator.Generate("cave", "seed=" + Random.Range(1, 9999));
    }
}
```

## Example: Reading the last level
file: Scripts/LastLevelInfo.cs
```csharp
using Elintria;

public class LastLevelInfo : ElintriaScript
{
    public override void Update()
    {
        GeneratedLevel l = LevelGenUtil.last;
        if (l != null && Input.GetKeyDown(KeyCode.I))
            Debug.Log("Seed " + l.seed + ", " + l.rooms + " rooms, start " + l.playerStart);
    }
}
```

# StdPatrol
summary: Walks a list of waypoints in turn, waiting at each: with a Nav Mesh Agent when the object has one, straight there otherwise.
see: LevelObjective

The Level Generator adds it to enemies when **Navigation and AI > Patrols** is on, with the waypoints it marked round their rooms.

## Members
waypoints: The objects whose positions it visits, in order.
speed: Metres per second when there is no Nav Mesh Agent.
waitTime: Seconds spent at each waypoint.
loop: Round the list (on) or back and forth along it (off).
arriveDistance: How close counts as there.
patrolling: Off stops it (a chase script can switch it off and on).
current: The waypoint it is heading for.

## Example: Stop to chase the player
file: Scripts/Guard.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Guard : ElintriaScript
{
    public GameObject player;
    public float sight = 8f;

    public override void Update()
    {
        StdPatrol patrol = GetComponent<StdPatrol>();
        bool sees = Vector3.Distance(transform.position, player.transform.position) < sight;
        patrol.patrolling = !sees;
        if (sees) transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 3f * Time.deltaTime);
    }
}
```

## Example: A route made in code
file: Scripts/RouteBuilder.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RouteBuilder : ElintriaScript
{
    public GameObject a, b, c;

    public override void Start()
    {
        StdPatrol p = GetComponent<StdPatrol>();
        p.waypoints.Clear();
        p.waypoints.Add(a); p.waypoints.Add(b); p.waypoints.Add(c);
        p.loop = false;          // a, b, c, b, a ...
        p.waitTime = 3f;
    }
}
```

## Example: Showing where it is going
file: Scripts/PatrolDebug.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PatrolDebug : ElintriaScript
{
    public override void Update()
    {
        StdPatrol p = GetComponent<StdPatrol>();
        if (p.waypoints.Count > 0 && Input.GetKeyDown(KeyCode.P))
            Debug.Log(gameObject.name + " heads for " + p.waypoints[p.current].name);
    }
}
```

# LevelObjective
summary: The level's goal - find items, escape, survive waves, rescue survivors or activate terminals - counted, shown on screen, with events when it is met.
see: StdPatrol, WaveSpawner, TriggerZone

The Level Generator adds one when **Objective** is set. Items count once they are picked up (switched off or gone); terminals and survivors send `Progress` / `Rescued` through their Interactable events; waves send `Progress` when cleared, and the escape zone when entered. With an `exitPoint` the level finishes when the player reaches it after the goal is met.

## Members
mode: 0 none, 1 find items, 2 escape, 3 survive waves, 4 rescue, 5 terminals.
count: How many it takes (items, terminals, survivors).
itemId: The item's name, for the on-screen text.
targets: The items, terminals or survivors.
exitPoint: Reach this to finish once the goal is met (optional).
exitRadius: How close to the exit point counts.
showHud: Show the goal and progress on screen.
onCompleteTargets: Objects told when the goal is met.
onCompleteMessage: The message they get.
onLevelFinishedTargets: Objects told when the level is finished.
onLevelFinishedMessage: The message they get.
progress: How many done.
complete: The goal is met.
finished: The level is finished.
Progress: One step done (a terminal, the waves, the escape zone).
Rescued: A survivor rescued (the same as Progress).
Text: The on-screen text for where things stand.

## Example: Loading the next scene when done
file: Scripts/OnFinished.cs
Put this on an object in `onLevelFinishedTargets`, with message `LevelDone`.
```csharp
using Elintria;

public class OnFinished : ElintriaScript
{
    public void LevelDone()
    {
        Debug.Log("Level finished");
        SceneManager.LoadScene("Level2");
    }
}
```

## Example: A terminal of your own
file: Scripts/HackPanel.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HackPanel : Interactable
{
    public LevelObjective objective;
    bool done;

    public override string Prompt(Interactor who) => done ? "Hacked" : "Hack";

    public override void Interact(Interactor who)
    {
        if (done) return;
        done = true;
        objective.Progress();
    }
}
```

## Example: Your own HUD
file: Scripts/ObjectiveHud.cs
Switch `showHud` off and draw the text your way.
```csharp
using Elintria;
using Elintria.Standard;

public class ObjectiveHud : ElintriaScript
{
    public LevelObjective objective;

    public override void Update()
    {
        objective.showHud = false;
        ScreenGUI.BeginFrame();
        ScreenGUI.Label(new Rect(20f, 20f, 400f, 30f), objective.Text(), null, Color.yellow);
    }
}
```

# LightFlicker
summary: Makes a Light flicker like a failing tube, or pulse like an alarm strobe.
see: LevelObjective

The Level Generator adds it to lights when **Lighting > Flickering Lights** or **Strobes** is on.

## Members
mode: 0 flicker (random dips and cut-outs), 1 strobe (a steady pulse).
rate: Flicker: how often it dips. Strobe: pulses per second.
minimum: How dark the dips go (0 off, 1 not at all).

## Example: An alarm that starts the strobes
file: Scripts/Alarm.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Alarm : ElintriaScript
{
    public LightFlicker[] lights;

    public void RaiseAlarm()
    {
        foreach (LightFlicker f in lights) { f.mode = 1; f.rate = 2f; f.minimum = 0f; }
    }
}
```

## Example: Failing worse over time
file: Scripts/PowerFailing.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PowerFailing : ElintriaScript
{
    public override void Update()
    {
        LightFlicker f = GetComponent<LightFlicker>();
        f.rate = Mathf.Min(6f, 0.5f + Time.time / 60f);   // more often each minute
    }
}
```

## Example: Fixed by the player
file: Scripts/FuseBox.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FuseBox : Interactable
{
    public LightFlicker broken;

    public override string Prompt(Interactor who) => "Replace fuse";

    public override void Interact(Interactor who)
    {
        if (broken != null) broken.enabled = false;
    }
}
```

# Ladder
summary: A climbable ladder: walk into it and hold forward to go up, back to go down; at the top the climber steps off forward.
see: StdPatrol

It works with any object that has a Character Controller and the `requiredTag`; a Third Person Controller's fall is held while on it. The Level Generator adds one at each ladder it builds (Vertical Play, with Standard Wiring).

## Members
height: How tall it is (metres up from the object).
width: How wide the climbing area is.
climbSpeed: Metres per second.
requiredTag: Only objects with this tag climb (empty: any with a Character Controller).

## Example: A ladder made in code
file: Scripts/AddLadder.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AddLadder : ElintriaScript
{
    public override void Start()
    {
        Ladder l = AddComponent<Ladder>();
        l.height = 6f;
        l.climbSpeed = 3f;
    }
}
```

## Example: Faster when sprinting
file: Scripts/QuickClimb.cs
```csharp
using Elintria;
using Elintria.Standard;

public class QuickClimb : ElintriaScript
{
    public override void Update()
    {
        Ladder l = GetComponent<Ladder>();
        l.climbSpeed = Input.GetKey(KeyCode.LeftShift) ? 4.5f : 2.5f;
    }
}
```

## Example: A ladder for enemies too
file: Scripts/AnyoneClimbs.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AnyoneClimbs : ElintriaScript
{
    public override void Start()
    {
        GetComponent<Ladder>().requiredTag = "";   // any Character Controller
    }
}
```

# LevelStreamer
summary: Draw zones for a level's rooms, and objects switched off while far from the player - for big generated levels.
see: RenderZones

The Level Generator adds one to the level's root when **Streaming > Draw Zones** or **Stream Objects** is on: a zone per room (rooms share zones past 64), each seeing the rooms two doors away, and the level's enemies, pickups, mechanisms and sounds as streamed objects.

## Members
zones: The zones, "minX,minY,minZ,maxX,maxY,maxZ;..." - the Level Generator fills this in.
neighbours: The zones seen from each zone, "0:1,2;1:0".
useZones: Hand the zones to RenderZones at Start.
streamed: Objects switched off beyond streamDistance from the player (or camera).
streamDistance: Metres within which streamed objects are on.
checkInterval: Seconds between distance checks.
ApplyZones: Hands the zones and neighbours to RenderZones now.

## Example: Nearer streaming on low quality
file: Scripts/StreamQuality.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StreamQuality : ElintriaScript
{
    public override void Start()
    {
        LevelStreamer s = GetComponent<LevelStreamer>();
        s.streamDistance = Quality.level == 0 ? 20f : 45f;
    }
}
```

## Example: Zones of your own
file: Scripts/TwoZones.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TwoZones : ElintriaScript
{
    public override void Start()
    {
        LevelStreamer s = AddComponent<LevelStreamer>();
        s.zones = "-20,0,-20,0,4,20;0,0,-20,20,4,20";
        s.neighbours = "0:1;1:0";
        s.ApplyZones();
    }
}
```

## Example: How much the zones hide
file: Scripts/ZoneStats.cs
```csharp
using Elintria;

public class ZoneStats : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
            Debug.Log("Zone " + RenderZones.ZoneAt(transform.position) + ", hidden last frame: " + RenderZones.HiddenLastFrame);
    }
}
```

# LevelLightAnimator
summary: Flickers or strobes a run-time level's Light. LevelGenerator.Generate adds it to the lights the Flickering Lights and Strobes options ask for.
see: LevelGenerator, GeneratedLevel

A generated level's lights are real Light objects (`GeneratedLevel.lights`); the ones that flicker or pulse get this script. It needs nothing from the Standard package.

## Members
mode: 1 flickers like a failing tube, 2 strobes like an alarm.
rate: Flicker: how often it dips. Strobe: pulses per second.
minimum: How dark the dips go (0 off, 1 not at all).

## Example: Every light strobes in an alarm
file: Scripts/LevelAlarm.cs
```csharp
using Elintria;

public class LevelAlarm : ElintriaScript
{
    public void Raise(GeneratedLevel level)
    {
        foreach (GameObject g in level.lights)
        {
            LevelLightAnimator a = g.GetComponent<LevelLightAnimator>();
            if (a == null) a = g.AddComponent<LevelLightAnimator>();
            a.mode = 2;
            a.rate = 2f;
        }
    }
}
```

## Example: A level without the lights it would make
file: Scripts/DarkLevel.cs
```csharp
using Elintria;

public class DarkLevel : ElintriaScript
{
    public override void Start()
    {
        // draw and colliders, but no Light objects and no fog: this script lights it itself
        GeneratedLevel level = LevelGenerator.Generate("cave, three spiders", "seed=4", Vector3.zero, -1, true, true, false);
        if (level != null) Debug.Log(level.Markers("Light").Count + " light spots left for my torches");
    }
}
```

## Example: Lights out when the power fails
file: Scripts/Blackout.cs
```csharp
using Elintria;

public class Blackout : ElintriaScript
{
    public void Fail(GeneratedLevel level)
    {
        foreach (GameObject g in level.lights)
        {
            LevelLightAnimator a = g.GetComponent<LevelLightAnimator>();
            if (a != null) a.enabled = false;
            Light l = g.GetComponent<Light>();
            if (l != null) l.intensity *= 0.1f;
        }
    }
}
```

# FirstPersonController
summary: Walks (WASD), runs (Shift), jumps (Space) and crouches (C or Ctrl) a player with a Character Controller, looking with the mouse; holds the scene's camera at eye height.
see: ThirdPersonController, PlayerRespawn, Interactor

The Level Generator window's **Initiate Player** button (First Person) makes a Player with this, a Character Controller, Health, an Inventory, an Interactor and Player Respawn. Crouching shrinks the controller to `crouchHeight` - low enough for the generator's crawl vents - and stands back up only where there is room.

## Members
walkSpeed: Metres a second walking.
sprintSpeed: Metres a second with Shift held.
crouchSpeed: Metres a second crouched.
acceleration: How fast it gets up to speed.
jumpHeight: Metres a jump rises.
gravity: Metres a second squared.
airControl: 0..1: how much the keys still steer in the air.
canCrouch: C or Ctrl crouches.
crouchHeight: The Character Controller's height while crouched.
cameraObject: The camera it holds (empty: the scene's MainCamera).
eyeHeight: Metres from the feet to the eyes, standing.
mouseSensitivity: Degrees per pixel of mouse movement.
minPitch: How far down it can look.
maxPitch: How far up it can look.
fieldOfView: The camera's field of view, degrees.
lockCursor: Hide and lock the cursor while playing.
headBob: Bob the view while walking.
bobAmount: Metres the view bobs.
bobSpeed: How fast it bobs.
input: The keys it reads; set `overridden` to drive it from code.
yaw: Degrees it faces about +Y.
pitch: Degrees it looks up or down.
velocity: Its velocity now.
crouching: Whether it is crouched.
crouchInput: Crouch held (tests and AI set it with input overridden).
isGrounded: On the ground.
eyePosition: Where the eyes are now.
Teleport: Puts the player at a point at once, facing a way (a respawn).
@pos: Where its feet go.
@yawDegrees: The way it faces.

## Example: Slower in water
file: Scripts/Wading.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Wading : ElintriaScript
{
    public float waterTop = 0.4f;

    public override void Update()
    {
        FirstPersonController fp = GetComponent<FirstPersonController>();
        bool wet = transform.position.y < waterTop;
        fp.walkSpeed = wet ? 2f : 4f;
        fp.sprintSpeed = wet ? 3f : 7f;
    }
}
```

## Example: A cutscene that turns the head
file: Scripts/LookAtDoor.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LookAtDoor : ElintriaScript
{
    public GameObject door;

    public void Play()
    {
        FirstPersonController fp = GetComponent<FirstPersonController>();
        Vector3 to = door.transform.position - fp.eyePosition;
        fp.input.overridden = true;                 // the keys and mouse do nothing for now
        fp.yaw = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg;
        fp.pitch = 0f;
    }

    public void Stop() => GetComponent<FirstPersonController>().input.overridden = false;
}
```

## Example: Making a player in code
file: Scripts/SpawnPlayer.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SpawnPlayer : ElintriaScript
{
    public override void Start()
    {
        GameObject p = GameObject.Create("Player");
        p.tag = "Player";
        p.transform.position = transform.position;
        CharacterController cc = p.AddComponent<CharacterController>();
        cc.height = 1.8f;
        cc.radius = 0.35f;
        p.AddComponent<Health>();
        p.AddComponent<Inventory>();
        p.AddComponent<Interactor>();
        p.AddComponent<PlayerRespawn>();
        p.AddComponent<FirstPersonController>();
    }
}
```

# PlayerRespawn
summary: Brings the player back when its Health runs out: a moment's black, then at the last Level Checkpoint (or where it started) at full health. Draws a health bar.
see: FirstPersonController, LevelCheckpoint, Health

## Members
respawnDelay: Seconds of black before coming back.
showHealthBar: Draw a health bar in the bottom left.
deathText: What the black screen says.
spawnPoint: Where the player comes back (checkpoints set it).
spawnYaw: The way it faces when it comes back.
deaths: How many times it has died.
current: The player's Player Respawn.
SetCheckpoint: Come back here from now on.
@position: Where the feet go.
@yawDegrees: The way it faces.
Respawn: Back at the spawn point at full health, now. Sends OnRespawn to the player's scripts.

## Example: Losing loot on death
file: Scripts/DropOnDeath.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DropOnDeath : ElintriaScript
{
    public void OnRespawn()
    {
        Inventory inv = GetComponent<Inventory>();
        if (inv != null) inv.Remove("coin", inv.Count("coin") / 2);
        Debug.Log("Deaths so far: " + GetComponent<PlayerRespawn>().deaths);
    }
}
```

## Example: A save point of your own
file: Scripts/Campfire.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Campfire : Interactable
{
    public override string Prompt(Interactor who) => "Rest";

    public override void Interact(Interactor who)
    {
        PlayerRespawn pr = who.GetComponent<PlayerRespawn>();
        if (pr != null) pr.SetCheckpoint(transform.position + transform.forward, transform.eulerAngles.y);
        who.Say("Rested");
    }
}
```

## Example: Game over after three deaths
file: Scripts/ThreeLives.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ThreeLives : ElintriaScript
{
    public string gameOverScene = "Scenes/Menu.scene";

    public void OnRespawn()
    {
        if (GetComponent<PlayerRespawn>().deaths >= 3) SceneManager.LoadScene(gameOverScene);
    }
}
```

# LevelHazard
summary: A trap that hurts whatever has Health inside its box while it is on: spike plates, fire vents, laser gates, toxic pools.
see: Health, LevelCheckpoint

The Level Generator's **Hazards** option makes them. A trap goes round a cycle - `onTime` seconds on, `offTime` off - and shows it with its moving part: spikes rise, flames and beams appear. Spikes and lasers hit once per cycle; fire and toxic hurt every second you stay.

## Members
kind: 0 spikes, 1 fire, 2 laser, 3 toxic pool, 4 toxic water.
damage: Per hit (spikes, lasers) or per second (fire, toxic).
size: The box it hurts in, metres, standing on this object (world axes).
onTime: Seconds on.
offTime: Seconds off (0: always on).
phase: Seconds into the cycle it starts, so neighbours take turns.
movingPart: What shows it is on: spikes rise, flames and beams appear.
riseHeight: Spikes: how far they rise.
requiredTag: Only objects with this tag are hurt (empty: anything with Health).
isOn: Whether it is on now.

## Example: Traps off once the power is cut
file: Scripts/PowerSwitch.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PowerSwitch : ElintriaScript
{
    public void CutPower()
    {
        foreach (LevelHazard h in FindObjectsOfType<LevelHazard>())
            if (h.kind == 1 || h.kind == 2) h.enabled = false;     // fire and lasers need power
    }
}
```

## Example: Harder traps deeper in
file: Scripts/TrapDifficulty.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TrapDifficulty : ElintriaScript
{
    public int depth = 1;

    public override void Start()
    {
        foreach (LevelHazard h in FindObjectsOfType<LevelHazard>())
        {
            h.damage *= 1f + depth * 0.25f;
            h.offTime = Mathf.Max(0.6f, h.offTime - depth * 0.2f);
        }
    }
}
```

## Example: A trap of your own
file: Scripts/CrusherSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CrusherSetup : ElintriaScript
{
    public GameObject crusherHead;

    public override void Start()
    {
        LevelHazard h = gameObject.AddComponent<LevelHazard>();
        h.kind = 0;                       // moves up and down like spikes, hits once a cycle
        h.damage = 60f;
        h.size = new Vector3(2f, 2f, 2f);
        h.onTime = 0.6f;
        h.offTime = 2.4f;
        h.movingPart = crusherHead;
        h.riseHeight = -1.5f;             // comes down instead of up
        h.requiredTag = "Player";
    }
}
```

# LevelPuzzle
summary: Opens a door when levers are pulled in the right order, when every switch is on, or when the right code is typed on its keypad.
see: Lever, Door, Readable

The Level Generator's **Puzzles** option seals a dead-end room off the main path behind a vault door and puts a reward inside. The order or the code is written on a Readable note: by the levers for a sequence, in a far room for a code.

## Members
mode: 0 lever sequence, 1 every switch on, 2 door code (this object is the keypad).
levers: The levers (each with a Lever component), numbered from 1 in this order.
order: Lever sequence: the levers' numbers in the order they must be pulled, "2,3,1".
code: Door code: the digits.
door: The door it opens (a Door: unlocked and opened).
onSolvedTargets: Objects told when it is solved.
onSolvedMessage: The message they get.
solved: Whether it is solved.
entry: The digits typed so far.
typing: Whether the keypad is open.
Solve: Opens the door now.

## Example: An alarm on a wrong guess
file: Scripts/VaultAlarm.cs
```csharp
using Elintria;
using Elintria.Standard;

public class VaultAlarm : ElintriaScript
{
    public LevelPuzzle puzzle;
    public Spawner guards;
    private bool m_was;

    public override void Update()
    {
        bool typing = puzzle.typing;
        if (m_was && !typing && !puzzle.solved) guards.StartSpawning();     // walked away without the code
        m_was = typing;
    }
}
```

## Example: A puzzle of your own
file: Scripts/ThreeSwitches.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ThreeSwitches : ElintriaScript
{
    public GameObject vaultDoor;
    public GameObject[] switches;

    public override void Start()
    {
        LevelPuzzle p = gameObject.AddComponent<LevelPuzzle>();
        p.mode = 0;
        p.order = "3,1,2";
        p.door = vaultDoor;
        foreach (GameObject s in switches) p.levers.Add(s);
    }
}
```

## Example: A hint after a while
file: Scripts/PuzzleHint.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PuzzleHint : ElintriaScript
{
    public LevelPuzzle puzzle;
    public float after = 90f;

    public override void Update()
    {
        if (puzzle.solved || Time.time < after) return;
        string hint = puzzle.mode == 2 ? "The code starts with " + puzzle.code.Substring(0, 1) : "Look for a note near the levers";
        ScreenGUI.Text(20f, 60f, hint);
    }
}
```

# LevelCheckpoint
summary: Where the player comes back after dying: reaching it sets Player Respawn's spawn point and, with heal on, restores health.
see: PlayerRespawn, LevelHazard

The Level Generator's **Checkpoints** option puts a glowing pad every few rooms along the main path.

## Members
radius: Metres from its middle that count as reaching it.
heal: Restore health on arrival.
requiredTag: The player's tag.
reached: Whether it has been reached.
current: The last checkpoint reached.
Reach: Makes this the place the player comes back to (as walking onto it does).

## Example: Counting checkpoints on the HUD
file: Scripts/CheckpointCount.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CheckpointCount : ElintriaScript
{
    public override void Update()
    {
        int got = 0, all = 0;
        foreach (LevelCheckpoint c in FindObjectsOfType<LevelCheckpoint>()) { ++all; if (c.reached) ++got; }
        ScreenGUI.Text(20f, 20f, "Checkpoints " + got + " / " + all);
    }
}
```

## Example: Saving the game at each one
file: Scripts/SaveAtCheckpoints.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SaveAtCheckpoints : ElintriaScript
{
    private LevelCheckpoint m_last;

    public override void Update()
    {
        if (LevelCheckpoint.current == null || LevelCheckpoint.current == m_last) return;
        m_last = LevelCheckpoint.current;
        Vector3 p = m_last.transform.position;
        System.IO.File.WriteAllText("checkpoint.txt", p.x + " " + p.y + " " + p.z);
    }
}
```

## Example: Back to the last checkpoint on a key
file: Scripts/StuckKey.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StuckKey : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F5)) return;
        PlayerRespawn pr = GetComponent<PlayerRespawn>();
        if (pr != null) pr.Respawn();     // the last checkpoint, or the start
    }
}
```

# Readable
summary: A note, a log or a sign: look at it and press E to read it; E, Escape or walking away puts it down.
see: Interactable, LevelPuzzle

The Level Generator's **Notes** option pins notes written for the theme (or your own, from Notes > Your Notes) to walls; puzzles put their clues on them.

## Members
title: The heading.
text: The words.
closeDistance: Metres away before it closes by itself.
open: Whether it is being read.
read: Whether it has ever been read.

## Example: A quest that starts when a note is read
file: Scripts/NoteQuest.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NoteQuest : ElintriaScript
{
    public Readable note;
    public LevelObjective objective;
    private bool m_started;

    public override void Update()
    {
        if (m_started || !note.read) return;
        m_started = true;
        objective.enabled = true;
    }
}
```

## Example: Notes from a text file
file: Scripts/NotesFromFile.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NotesFromFile : ElintriaScript
{
    public override void Start()
    {
        string[] lines = Content.ReadAllText("Assets/Story/notes.txt").Split('\n');
        int i = 0;
        foreach (Readable r in FindObjectsOfType<Readable>())
            if (i < lines.Length) r.text = lines[i++];
    }
}
```

## Example: A sign made in code
file: Scripts/WelcomeSign.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WelcomeSign : ElintriaScript
{
    public override void Start()
    {
        Readable r = gameObject.AddComponent<Readable>();
        r.title = "Welcome";
        r.text = "Find the three fuses and get the lift running.";
        r.prompt = "Read the sign";
    }
}
```

# LevelMinimap
summary: A map of a generated level in a corner of the screen while playing, centred on the player; M shows the whole floor. Rooms appear as they are visited.
see: LevelGenerator, LevelCheckpoint

The Level Generator's **Minimap** option adds it with the level's floor plan filled in. The exit, keys, objectives and checkpoints show once their room has been seen.

## Members
cells: The floor plan: per floor (separated by "|"), runs of open cells "x,z,length,cell;..." where cell is a room number or -2 for a corridor.
originX: The world X of cell (0, 0)'s corner.
originZ: The world Z of cell (0, 0)'s corner.
floorHeight: Metres from one floor to the next.
corner: 0 top left, 1 top right, 2 bottom left, 3 bottom right.
size: Pixels across.
range: Metres shown across.
reveal: Rooms appear once visited (off: the whole floor shows).
markers: "kind,x,z,floor;..." - exit, key, objective, checkpoint.
showFullMapKey: M toggles the whole-floor map.

## Example: A bigger map on a gamepad-free key
file: Scripts/MapSettings.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MapSettings : ElintriaScript
{
    public override void Start()
    {
        LevelMinimap map = FindObjectOfType<LevelMinimap>();
        if (map == null) return;
        map.size = 300f;
        map.range = 60f;
        map.corner = 0;
    }
}
```

## Example: Showing everything in an easy mode
file: Scripts/EasyMode.cs
```csharp
using Elintria;
using Elintria.Standard;

public class EasyMode : ElintriaScript
{
    public bool easy = true;

    public override void Start()
    {
        foreach (LevelMinimap m in FindObjectsOfType<LevelMinimap>()) m.reveal = !easy;
    }
}
```

## Example: Hiding the map in cutscenes
file: Scripts/CutsceneHidesMap.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CutsceneHidesMap : ElintriaScript
{
    public void BeginCutscene() { foreach (LevelMinimap m in FindObjectsOfType<LevelMinimap>()) m.enabled = false; }
    public void EndCutscene() { foreach (LevelMinimap m in FindObjectsOfType<LevelMinimap>()) m.enabled = true; }
}
```
