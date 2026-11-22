# BallisticFlight
summary: Carries a character through the air with the speed it was given, since a controller soon slows a jump's sideways speed. Jump pads, ropes, ziplines and boost rings start one; it ends on landing.
see: JumpPad, BoostRing, StdMotor

## Members
flying: Whether a flight is going on now.
Launch: Starts (or restarts) a flight for an object. Returns the flight.
@velocity: Metres a second to start with.
@steer: 0..1: how much the move keys steer.
@maxTime: Seconds before the controller takes over anyway.
@gravityScale: 1 normal gravity, 0.5 half.
IsFlying: Whether an object is in a flight now.

## Example: A cannon that fires the player
file: Scripts/HumanCannon.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HumanCannon : Interactable
{
    public float power = 25f;

    public override void Interact(Interactor who)
    {
        Vector3 v = (transform.forward + Vector3.up).normalized * power;
        BallisticFlight.Launch(who.gameObject, v, 0.1f, 8f);
    }
}
```

## Example: A float pad
file: Scripts/FloatPad.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FloatPad : ElintriaScript
{
    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null || BallisticFlight.IsFlying(p)) return;
        if (Vector3.Distance(p.transform.position, transform.position) < 1.5f)
            BallisticFlight.Launch(p, Vector3.up * 8f, 0.6f, 4f, 0.3f);   // slow, floaty
    }
}
```

## Example: Land a flight early
file: Scripts/StopFlightOnKey.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StopFlightOnKey : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && BallisticFlight.IsFlying(gameObject))
            BallisticFlight.Cancel(gameObject);       // the controller's own gravity takes over
    }
}
```

# Zipline
summary: A cable from this object to an end point: E near it rides along it (downhill, or the way the player faces), Space or E again lets go with the speed it had. The cable draws itself.
see: RopeSwing, BallisticFlight, Interactable

## Members
endOffset: Where the far end is when there is no end point, from this object (world axes).
minSpeed: The slowest it goes, metres a second.
acceleration: Metres a second squared it speeds up by on the flat (slopes add to it).
cableWidth: How thick the cable is drawn.
cableColor: The cable's colour.
riding: Whether someone is on it now.
rider: Who is riding.
currentSpeed: Metres a second now.
A: This end of the cable.
B: The far end of the cable.
Release: Lets the rider go; they fly on with their speed.
@jump: Add a hop upward.

## Example: Ride on a trigger
file: Scripts/AutoZip.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AutoZip : ElintriaScript
{
    public Zipline line;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !line.riding) line.Attach(other.gameObject);
    }
}
```

## Example: A speed readout
file: Scripts/ZipSpeed.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ZipSpeed : ElintriaScript
{
    public override void Update()
    {
        foreach (Zipline z in FindObjectsOfType<Zipline>())
            if (z.riding) ScreenGUI.Text(20f, 80f, Mathf.Round(z.currentSpeed * 3.6f) + " km/h", null, Color.white);
    }
}
```

## Example: A one-way line
file: Scripts/DownhillOnly.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DownhillOnly : ElintriaScript
{
    public override void Start()
    {
        Zipline z = GetComponent<Zipline>();
        z.bothWays = false;     // always toward the lower end
        z.speed = 20f;
    }
}
```

# RopeSwing
summary: A rope hanging from this object: grab it with E or by jumping into it, pump the swing with W and S, and let go with Space to fly on. The rope draws itself.
see: Zipline, BallisticFlight

## Members
length: Metres from the pivot to the end.
pumpForce: How hard W and S push the swing.
ropeWidth: How thick the rope is drawn.
ropeColor: The rope's colour.
swinging: Whether someone hangs on it now.
rider: Who is hanging on.
end: Where the rope's end is now.
swingVelocity: How fast the end moves now.
pivot: Where it hangs from.
restEnd: Where the end hangs when still.
Release: Lets go: the rider flies on with the swing's speed.

## Example: A longer rope over a pit
file: Scripts/PitRope.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PitRope : ElintriaScript
{
    public override void Start()
    {
        RopeSwing r = GetComponent<RopeSwing>();
        r.length = 9f;
        r.releaseBoost = 5f;
        r.grabReach = 2f;
    }
}
```

## Example: A swing that snaps
file: Scripts/WeakRope.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WeakRope : ElintriaScript
{
    public float holdSeconds = 3f;
    private float m_since = -1f;

    public override void Update()
    {
        RopeSwing r = GetComponent<RopeSwing>();
        if (!r.swinging) { m_since = -1f; return; }
        if (m_since < 0f) m_since = Time.time;
        if (Time.time - m_since > holdSeconds) { r.Release(); StdToast.Show("The rope snapped!"); }
    }
}
```

## Example: Push the player out on a cue
file: Scripts/RopePush.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RopePush : ElintriaScript
{
    public RopeSwing rope;

    public void Push()
    {
        if (rope.swinging) rope.swingVelocity += transform.forward * 6f;
    }
}
```

# Glider
summary: Hold the key while falling from high enough and the player glides forward, sinking slowly; W speeds up, S slows down, and wind lifts it. With a Jetpack on the same key it opens once the fuel is gone.
see: Jetpack, WindZone, BoostRing

## Members
key: The key, by name (default Space).
glideSpeed: Metres a second forward.
sinkSpeed: Metres a second it sinks.
showWing: Draw a wing above the player while gliding.
wingColor: The wing's colour.
gliding: Whether it is gliding now.
velocity: Its velocity while gliding.
Close: Folds the glider; the player falls on with its speed.

## Example: A slower, steadier glider
file: Scripts/HangGlider.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HangGlider : ElintriaScript
{
    public override void Start()
    {
        Glider g = GetComponent<Glider>();
        g.glideSpeed = 7f;
        g.sinkSpeed = 1.2f;
        g.minHeight = 4f;
    }
}
```

## Example: Only after finding the glider
file: Scripts/GliderUnlock.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GliderUnlock : ElintriaScript
{
    public override void Start() { GetComponent<Glider>().enabled = false; }

    public void OnInventoryChanged()
    {
        if (GetComponent<Inventory>().Has("glider")) GetComponent<Glider>().enabled = true;
    }
}
```

## Example: An altitude readout
file: Scripts/Altimeter.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Altimeter : ElintriaScript
{
    public override void Update()
    {
        Glider g = GetComponent<Glider>();
        if (g.gliding) ScreenGUI.Text(20f, 100f, Mathf.Round(g.HeightAboveGround()) + " m", null, Color.white);
    }
}
```

# Jetpack
summary: Hold the key in the air to thrust upward while the fuel lasts; the move keys steer. The fuel refills on the ground, and a bar shows it while it is not full.
see: Glider, StdMotor

## Members
key: The key, by name (default Space).
maxRiseSpeed: The fastest it climbs, metres a second.
refuelDelay: Seconds on the ground before refilling starts.
showFuel: Show the fuel bar.
flameColor: The flame's colour.
fuel: 0..1 of a full tank.
thrusting: Whether it is thrusting now.

## Example: Fuel canisters
file: Scripts/FuelCanister.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FuelCanister : Interactable
{
    public override void Interact(Interactor who)
    {
        Jetpack j = who.GetComponent<Jetpack>();
        if (j == null) return;
        j.fuel = 1f;
        Destroy(gameObject);
    }
}
```

## Example: No refilling in the air base
file: Scripts/NoRefuelZone.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NoRefuelZone : ElintriaScript
{
    public float radius = 20f;
    private float m_normal = -1f;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        Jetpack j = p != null ? p.GetComponent<Jetpack>() : null;
        if (j == null) return;
        if (m_normal < 0f) m_normal = j.refuelSeconds;
        j.refuelSeconds = Vector3.Distance(p.transform.position, transform.position) < radius ? 9999f : m_normal;
    }
}
```

## Example: A warning when nearly empty
file: Scripts/FuelWarning.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FuelWarning : ElintriaScript
{
    public override void Update()
    {
        Jetpack j = GetComponent<Jetpack>();
        if (j.thrusting && j.fuel < 0.2f)
            ScreenGUI.Text(GameWindow.width * 0.5f - 40f, GameWindow.height - 110f, "LOW FUEL", null, new Color(1f, 0.3f, 0.2f, 1f));
    }
}
```

# FallDamage
summary: Hurts the player on a hard landing, more the further it fell; a fall from the lethal height kills. Pressing crouch just before landing rolls and takes less. Landing in water does no harm.
see: Health, JumpPad, StdMotor

## Members
rollKey: The roll key, by name (default C).
rollSpeed: Metres a second the roll carries on forward.
onHardLandingTargets: Objects told about a landing that hurt.
onHardLandingMessage: The message they get (with the damage).
lastDamage: The damage the last landing did.
lastRolled: Whether the last landing was a roll.
OnRespawn: Forgets the fall in progress (Player Respawn sends it).

## Example: Tougher legs from a perk
file: Scripts/ToughLegs.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ToughLegs : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id != "leg_braces") return;
        FallDamage f = GetComponent<FallDamage>();
        f.safeHeight = 8f;
        f.lethalHeight = 30f;
    }
}
```

## Example: A crunch on bad landings
file: Scripts/LandingSound.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LandingSound : ElintriaScript
{
    public void Landed(float damage)
    {
        StdToast.Show(damage > 30f ? "Ouch!" : "Oof", 1f);
    }
}
```
Put LandingSound on the player, and set Fall Damage's On Hard Landing to the player with the message "Landed".

## Example: A safe drop into a level
file: Scripts/SafeDrop.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SafeDrop : ElintriaScript
{
    public override void Start()
    {
        // the level starts with a fall from the sky
        FallDamage f = GetComponent<FallDamage>();
        if (f != null) f.Forgive();
    }
}
```

# JumpPad
summary: Launches whatever steps on it: straight up (and on along its forward), or in an arc that lands exactly on a target. Characters and Rigidbodies both fly. With a Box Collider it uses its top; without one, a radius.
see: BoostRing, BallisticFlight, FallDamage

## Members
arcHeight: Metres above the higher end the arc to the target rises.
onLaunchTargets: Objects told about each launch.
onLaunchMessage: The message they get (with what was launched).
launches: How many times it has launched something.

## Example: A pad that needs power
file: Scripts/PoweredPad.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PoweredPad : ElintriaScript
{
    public void PowerOn() { GetComponent<JumpPad>().enabled = true; }
    public void PowerOff() { GetComponent<JumpPad>().enabled = false; }
}
```

## Example: Launch crates at the player
file: Scripts/CrateLauncher.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CrateLauncher : ElintriaScript
{
    public GameObject crate;

    public void Fire()
    {
        JumpPad pad = GetComponent<JumpPad>();
        pad.target = GameObject.FindWithTag("Player");
        pad.Launch(crate);
    }
}
```

## Example: Where an arc will go
file: Scripts/PadPreview.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PadPreview : ElintriaScript
{
    public override void Start()
    {
        JumpPad pad = GetComponent<JumpPad>();
        Vector3 v = pad.VelocityFrom(transform.position, 20f);
        Debug.Log("Launches at " + v.magnitude.ToString("0.0") + " m/s, " + (Mathf.Asin(v.normalized.y) * Mathf.Rad2Deg).ToString("0") + " degrees up");
    }
}
```

# BoostRing
summary: A ring the player flies or jumps through for a burst of speed the way it faces. Works on gliders too. It draws itself as a glowing ring (turn that off when you give it a model).
see: Glider, JumpPad, BallisticFlight

## Members
radius: Metres from the middle that count as through it.
boostSpeed: Metres a second the boost gives.
targetTag: What it boosts (default Player).
drawRing: Draw the ring itself.
color: The drawn ring's colour.
onPassedTargets: Objects told when someone goes through.
onPassedMessage: The message they get (with who went through).
passes: How many times someone has gone through.

## Example: A ring course timer
file: Scripts/RingCourse.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class RingCourse : ElintriaScript
{
    public List<BoostRing> rings = new List<BoostRing>();
    private float m_start = -1f;

    public override void Update()
    {
        int done = 0;
        foreach (BoostRing r in rings) if (r.passes > 0) ++done;
        if (done > 0 && m_start < 0f) m_start = Time.time;
        if (m_start >= 0f) ScreenGUI.Text(20f, 60f, done + " / " + rings.Count + "   " + (Time.time - m_start).ToString("0.0") + " s", null, Color.white);
    }
}
```

## Example: A score per ring
file: Scripts/RingScore.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RingScore : ElintriaScript
{
    public int score;
    public void RingPassed(GameObject who) { score += 100; StdToast.Show("+100"); }
}
```
Set each ring's On Passed to this object with the message "RingPassed".

## Example: A ring that pushes up
file: Scripts/UpRing.cs
```csharp
using Elintria;
using Elintria.Standard;

public class UpRing : ElintriaScript
{
    public override void Start()
    {
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);   // facing up
        GetComponent<BoostRing>().boostSpeed = 18f;
    }
}
```

# GravityZone
summary: Gravity scaled inside a box: moon jumps, zero-g rooms, or falling upward with a negative scale. Characters' gravity and jump height change while inside; Rigidbodies are pushed to match.
see: WindZone, StdMotor

## Members
size: The box, metres; this object is its middle.
affectRigidbodies: Change Rigidbodies' gravity too.
Contains: Whether a point is inside the box.

## Example: Flip gravity with a lever
file: Scripts/GravityFlip.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GravityFlip : ElintriaScript
{
    public GravityZone zone;
    public void Flip() { zone.gravityScale = zone.gravityScale > 0f ? -0.5f : 0.3f; }
}
```

## Example: A low-gravity power-up
file: Scripts/MoonBoots.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MoonBoots : ElintriaScript
{
    public float seconds = 10f;
    private GameObject m_zone;

    public void Use()
    {
        m_zone = GameObject.Create("Moon Boots Zone");
        GravityZone z = m_zone.AddComponent<GravityZone>();
        z.size = new Vector3(3f, 4f, 3f);
        z.gravityScale = 0.3f;
        Destroy(m_zone, seconds);
    }

    public override void Update()
    {
        if (m_zone != null && m_zone.IsAlive) m_zone.transform.position = transform.position + Vector3.up;
    }
}
```

## Example: Warn the player inside
file: Scripts/ZeroGWarning.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ZeroGWarning : ElintriaScript
{
    public override void Update()
    {
        foreach (GravityZone z in FindObjectsOfType<GravityZone>())
            if (z.Contains(transform.position) && z.gravityScale < 0.1f)
                ScreenGUI.Text(20f, 120f, "No gravity", null, new Color(0.7f, 0.6f, 1f, 1f));
    }
}
```

# WindZone
summary: Wind inside a box: it pushes characters and Rigidbodies, and lifts gliders. Wind with an upward part is an updraft. Streaks drawn in the box show which way it blows.
see: Glider, GravityZone, BallisticFlight

## Members
size: The box, metres; this object is its middle.
affectRigidbodies: Push Rigidbodies too.
showStreaks: Draw streaks that blow along with it.
Contains: Whether a point is inside the box.

## Example: A fan you switch on
file: Scripts/Fan.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Fan : ElintriaScript
{
    public void On() { GetComponent<WindZone>().enabled = true; }
    public void Off() { GetComponent<WindZone>().enabled = false; }
}
```

## Example: A storm that turns
file: Scripts/TurningWind.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TurningWind : ElintriaScript
{
    public float degreesPerSecond = 10f;

    public override void Update()
    {
        WindZone w = GetComponent<WindZone>();
        w.direction = Quaternion.Euler(0f, degreesPerSecond * Time.deltaTime, 0f) * w.direction;
    }
}
```

## Example: A wind sock
file: Scripts/WindSock.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WindSock : ElintriaScript
{
    public override void Update()
    {
        Vector3 w = WindZone.At(transform.position);
        if (new Vector3(w.x, 0f, w.z).sqrMagnitude > 0.01f) transform.rotation = Quaternion.LookRotation(new Vector3(w.x, 0f, w.z));
    }
}
```

# ConveyorBelt
summary: Carries whatever stands on it along its forward, characters and Rigidbodies alike. Needs a Box Collider; stripes drawn on top move with it.
see: MovingPlatform, WindZone

## Members
running: Whether it moves now.
showStripes: Draw stripes on top that move with the belt.
stripeColor: The stripes' colour.
SetRunning: Starts or stops it.
@on: True to run.
Toggle: Starts it if stopped, stops it if running.

## Example: Run while a plate is pressed
file: Scripts/BeltSwitch.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BeltSwitch : ElintriaScript
{
    public ConveyorBelt belt;
    public void Pressed() { belt.SetRunning(true); }
    public void Released() { belt.SetRunning(false); }
}
```

## Example: Reverse every few seconds
file: Scripts/BeltReverser.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BeltReverser : ElintriaScript
{
    public float every = 4f;
    private float m_next;

    public override void Update()
    {
        if (Time.time < m_next) return;
        m_next = Time.time + every;
        GetComponent<ConveyorBelt>().speed *= -1f;
    }
}
```

## Example: Speed up over time
file: Scripts/BeltRamp.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BeltRamp : ElintriaScript
{
    public override void Update()
    {
        ConveyorBelt b = GetComponent<ConveyorBelt>();
        b.speed = Mathf.Min(8f, b.speed + 0.1f * Time.deltaTime);
    }
}
```

# StdThrowKinds
summary: The kinds of throwable by number: 0 grenade, 1 smoke, 2 molotov, 3 flashbang, 4 decoy. Their item ids are what the Inventory counts.
see: ThrowableUser, Throwable

## Members
ids: The item ids: grenade, smoke, molotov, flashbang, decoy.
names: The names shown: Grenade, Smoke, Molotov, Flashbang, Decoy.
Of: The kind for an item id (-1 when it is none).

## Example: A crate of every kind
file: Scripts/ThrowableCache.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ThrowableCache : Interactable
{
    public override void Interact(Interactor who)
    {
        Inventory inv = who.inventory;
        foreach (string id in StdThrowKinds.ids) inv.Add(id, 1);
        StdToast.Show("Got one of each throwable");
    }
}
```

## Example: Is it a throwable?
file: Scripts/ThrowableCheck.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ThrowableCheck : ElintriaScript
{
    public void OnUseItem(string id)
    {
        int k = StdThrowKinds.Of(id);
        if (k >= 0) GetComponent<ThrowableUser>().selected = k;    // using it from the bag puts it in hand
    }
}
```

## Example: List what the player carries
file: Scripts/ThrowableList.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ThrowableList : ElintriaScript
{
    public override void Update()
    {
        ThrowableUser t = GetComponent<ThrowableUser>();
        float y = 140f;
        for (int k = 0; k < StdThrowKinds.names.Length; ++k, y += 18f)
            ScreenGUI.Text(20f, y, StdThrowKinds.names[k] + "  " + t.Count(k), null, Color.white);
    }
}
```

# Throwable
summary: A thrown grenade, smoke, molotov, flashbang or decoy: it flies, bounces, and goes off when its fuse runs out (a molotov when it hits). Grenades hurt everything in the radius, less further out.
see: ThrowableUser, ThrowEffect, StdThrowKinds

## Members
radius: Metres round it that are affected.
bounciness: Share of its speed kept on each bounce.
gravity: Metres a second squared it falls.
velocity: Its velocity now.
owner: Who threw it (credited with the damage).
wentOff: Whether it has gone off.
GoOff: Goes off now, at a point.
@at: Where.

## Example: A mine that goes off when stepped on
file: Scripts/Landmine.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Landmine : ElintriaScript
{
    private bool m_done;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (m_done) return;
        if (p == null || Vector3.Distance(p.transform.position, transform.position) > 1f) return;
        m_done = true;
        Throwable t = gameObject.AddComponent<Throwable>();
        t.damage = 120f;
        t.radius = 4f;
        t.GoOff(transform.position);
    }
}
```

## Example: An enemy that lobs grenades
file: Scripts/GrenadeLobber.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GrenadeLobber : ElintriaScript
{
    public float every = 5f;
    private float m_next;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null || Time.time < m_next) return;
        m_next = Time.time + every;
        Vector3 to = p.transform.position - transform.position;
        GameObject g = StdBoxVisual.Make("Enemy Grenade", transform.position + Vector3.up * 1.6f, Vector3.one * 0.2f, new Color(0.3f, 0.35f, 0.2f, 1f));
        Throwable t = g.AddComponent<Throwable>();
        t.owner = gameObject;
        t.velocity = new Vector3(to.x, 0f, to.z) * 0.6f + Vector3.up * 7f;
    }
}
```

## Example: A shorter fuse
file: Scripts/QuickFuse.cs
```csharp
using Elintria;
using Elintria.Standard;

public class QuickFuse : ElintriaScript
{
    public override void Update()
    {
        ThrowableUser u = GetComponent<ThrowableUser>();
        if (u.lastThrown == null || !u.lastThrown.IsAlive) return;
        Throwable t = u.lastThrown.GetComponent<Throwable>();
        if (t != null && t.kind == 0) t.fuse = Mathf.Min(t.fuse, 1.2f);
    }
}
```

# ThrowEffect
summary: What a throwable leaves behind: a blast's flash, a smoke cloud that enemies and turrets cannot see through, a fire that burns what stands in it, a flash's white-out, or a decoy's noise every second.
see: Throwable, BasicEnemy, StdNoise

## Members
radius: Metres it reaches.
seconds: How long it lasts.
owner: Who threw it.
Make: Starts an effect at a point; returns it.
@at: Where.

## Example: A smoke machine
file: Scripts/SmokeMachine.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SmokeMachine : ElintriaScript
{
    public void Puff() { ThrowEffect.Make(1, transform.position, 5f, 12f, 0f, gameObject); }
}
```

## Example: A burning floor trap
file: Scripts/FireTrap.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FireTrap : ElintriaScript
{
    public float every = 6f;
    private float m_next;

    public override void Update()
    {
        if (Time.time < m_next) return;
        m_next = Time.time + every;
        ThrowEffect.Make(2, transform.position, 2.5f, 3f, 12f, gameObject);
    }
}
```

## Example: Is the guard's view blocked?
file: Scripts/SmokeCheck.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SmokeCheck : ElintriaScript
{
    public GameObject guard;

    public override void Update()
    {
        if (ThrowEffect.SmokeBlocks(guard.transform.position + Vector3.up * 1.5f, transform.position + Vector3.up * 1.5f))
            ScreenGUI.Text(20f, 160f, "Hidden by smoke", null, Color.white);
    }
}
```

# ThrowableUser
summary: On the player: hold T to aim (an arc shows where it lands), let go to throw; X picks the next kind you have. Counts come from the Inventory (the starting ones are put in the back of the bag), or from the component when there is none. Top down, it throws at the mouse.
see: Throwable, StdThrowKinds, Inventory

## Members
throwKey: The throw key, by name (default T).
cycleKey: The key that picks the next kind (default X).
throwSpeed: Metres a second a throw gives.
cooldown: Seconds between throws.
showArc: Show the arc while aiming.
showHud: Show what is in hand and how many.
arcColor: The arc's colour.
grenadeRadius: Metres a grenade reaches.
aiming: Whether the throw key is held now.
thrown: How many it has thrown.
lastThrown: The last thing it threw.
Count: How many of a kind the player has.
@k: The kind (0 grenade .. 4 decoy).
Give: Gives the player some of a kind.
@k: The kind (0 grenade .. 4 decoy).
@n: How many.
Aim: Where a throw starts and its velocity now.
@velocity: The velocity it would leave with.

## Example: Grenades from an ammo box
file: Scripts/GrenadeBox.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GrenadeBox : Interactable
{
    public override void Interact(Interactor who)
    {
        ThrowableUser t = who.GetComponent<ThrowableUser>();
        if (t != null) { t.Give(0, 2); StdToast.Show("+2 grenades"); }
    }
}
```

## Example: Throw with the mouse wheel button
file: Scripts/WheelThrow.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WheelThrow : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetMouseButtonDown(2)) GetComponent<ThrowableUser>().Throw();
    }
}
```

## Example: Stealth: decoys only
file: Scripts/DecoysOnly.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DecoysOnly : ElintriaScript
{
    public override void Start()
    {
        ThrowableUser t = GetComponent<ThrowableUser>();
        t.startingCounts = "decoy:5|smoke:1";
        t.selected = 4;
    }
}
```

# Turret
summary: A fixed gun: it sweeps while it sees nothing, turns its head to track what it sees (not through walls or smoke), and fires bullets or dodgeable projectiles. Target "Enemy" for a sentry that guards the player.
see: BasicEnemy, Projectile, ThrowEffect

## Members
targetTag: What it shoots at (default Player).
range: Metres it sees and shoots.
damage: Damage a shot.
projectileSpeed: Metres a second its projectiles fly.
active: Whether it is switched on.
laserSight: Draw a laser along its aim.
laserColor: The laser's colour.
target: What it is shooting at now.
yaw: Degrees it is turned now.
pitch: Degrees its head is tilted now.
shots: How many shots it has fired.
Activate: Switches it on.
Deactivate: Switches it off.
Toggle: Switches it on or off.
Aim: The way it aims now.
CanSee: Whether it sees an object now.

## Example: Hack a turret to your side
file: Scripts/TurretHack.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TurretHack : Interactable
{
    public override void Interact(Interactor who)
    {
        Turret t = GetComponent<Turret>();
        t.targetTag = "Enemy";
        t.laserColor = new Color(0.2f, 0.6f, 1f, 0.6f);
        StdToast.Show("Turret hacked");
    }
}
```

## Example: Shut down when the power is cut
file: Scripts/TurretPower.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TurretPower : ElintriaScript
{
    public System.Collections.Generic.List<Turret> turrets = new System.Collections.Generic.List<Turret>();
    public void PowerCut() { foreach (Turret t in turrets) t.Deactivate(); }
    public void PowerBack() { foreach (Turret t in turrets) t.Activate(); }
}
```

## Example: An alarm when a turret sees you
file: Scripts/TurretAlarm.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TurretAlarm : ElintriaScript
{
    public override void Update()
    {
        foreach (Turret t in FindObjectsOfType<Turret>())
            if (t.target != null && t.target.CompareTag("Player"))
                ScreenGUI.Text(GameWindow.width * 0.5f - 40f, 60f, "DETECTED", null, new Color(1f, 0.2f, 0.2f, 1f));
    }
}
```

# BuildingPlacer
summary: Build mode for the player. B toggles it; a see-through piece follows where you look (or the mouse, top down), snapped to a grid, green when it fits and red when not. Left click places it, R turns it, the wheel or 1-9 picks the piece, right click takes a placed piece back with a refund. Pieces cost Inventory items or Wallet money, and what was built is saved with the Save System.
see: BuildPiece, Inventory, Wallet, SaveSystem

## Members
toggleKey: The key that turns build mode on and off (default B).
rotateKey: The key that turns the piece (default R).
showHelp: Show the controls and the cost while building.
building: Whether build mode is on.
selected: The piece picked now.
previewValid: Whether the piece fits where it is now.
invalidReason: Why it does not fit ("Blocked", "Need 2 wood").
placed: Every piece placed.
pieceCount: How many pieces there are to pick from.
PieceName: A piece's name.
PieceCost: A piece's cost text.
SetBuilding: Turns build mode on or off.
@on: True for on.
Ray: The ray pieces are aimed with.
@eye: Where it starts.
Preview: Where the selected piece would go along a ray, and whether it fits.
@eye: Where the ray starts.
@centre: The piece's middle.
@valid: Whether it fits.
CostText: A cost as words ("2 wood, 1 stone").
Place: Places a piece; returns it.
@k: Which piece.
@centre: Its middle.
@pay: Take the cost.
RemoveLookedAt: Takes back the placed piece a ray hits.
@eye: Where the ray starts.
Remove: Takes back a placed piece, refunded.
@piece: The piece.
Rebuild: Takes down everything built and puts back a layout, free.
@layout: From Layout.

## Example: Creative mode
file: Scripts/CreativeMode.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CreativeMode : ElintriaScript
{
    public override void Start()
    {
        BuildingPlacer b = GetComponent<BuildingPlacer>();
        b.freeBuild = true;
        b.range = 20f;
    }
}
```

## Example: No building near the town
file: Scripts/NoBuildZone.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NoBuildZone : ElintriaScript
{
    public float radius = 25f;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        BuildingPlacer b = p != null ? p.GetComponent<BuildingPlacer>() : null;
        if (b != null && b.building && Vector3.Distance(p.transform.position, transform.position) < radius)
        {
            b.SetBuilding(false);
            StdToast.Show("No building in town");
        }
    }
}
```

## Example: Count what was built
file: Scripts/BuildCount.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BuildCount : ElintriaScript
{
    public override void Update()
    {
        BuildingPlacer b = GetComponent<BuildingPlacer>();
        if (b.building) ScreenGUI.Text(20f, 180f, b.placed.Count + " pieces built", null, Color.white);
    }
}
```

# BuildPiece
summary: On a piece you made yourself (kept inactive as a template in the Building Placer's list): its name, its cost and its size. The placer adds one to every piece it places.
see: BuildingPlacer

## Members
kind: Which of the placer's pieces it is.
Name: Its name (the object's name when none is set).
Size: Its size, metres.

## Example: A door piece that costs money
file: Scripts/DoorPieceSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DoorPieceSetup : ElintriaScript
{
    public override void Awake()
    {
        BuildPiece p = GetComponent<BuildPiece>();
        p.pieceName = "Door";
        p.cost = "money:25";
        p.size = new Vector3(1f, 2.2f, 0.2f);
    }
}
```

## Example: Pieces that are worth points
file: Scripts/PieceValue.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PieceValue : ElintriaScript
{
    public override void Update()
    {
        int value = 0;
        foreach (BuildPiece p in FindObjectsOfType<BuildPiece>()) if (p.placed) value += p.Name == "Wall" ? 3 : 1;
        ScreenGUI.Text(20f, 200f, "Base value " + value, null, Color.white);
    }
}
```

## Example: Break a piece
file: Scripts/PieceHealth.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PieceHealth : ElintriaScript
{
    public void OnDeath()
    {
        GameObject p = GameObject.FindWithTag("Player");
        BuildingPlacer b = p != null ? p.GetComponent<BuildingPlacer>() : null;
        if (b != null) b.placed.Remove(gameObject);   // gone, no refund
    }
}
```
Give the piece template a Health with Destroy On Death, and this script.

# CropInfo
summary: One crop's rules, read from a Farm Plot's crops text: its seed item, the game hours it takes, how much a harvest gives, and whether it grows back.
see: FarmPlot

## Members
seed: The item that plants it (default: the crop's id with "_seed").
yieldMin: The fewest a harvest gives.
yieldMax: The most a harvest gives.
color: The ripe crop's colour.

## Example: List the crops
file: Scripts/CropList.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CropList : ElintriaScript
{
    public override void Start()
    {
        foreach (CropInfo c in CropInfo.Parse(GetComponent<FarmPlot>().crops))
            Debug.Log(c.id + ": " + c.hours + " hours, " + c.yieldMin + "-" + c.yieldMax + (c.regrow > 0f ? ", grows back" : ""));
    }
}
```

## Example: A greenhouse that grows faster
file: Scripts/Greenhouse.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Greenhouse : ElintriaScript
{
    public override void Start()
    {
        foreach (FarmPlot p in FindObjectsOfType<FarmPlot>())
            if (Vector3.Distance(p.transform.position, transform.position) < 6f)
                p.crops = "carrot:hours=18,yield=3-5|tomato:hours=30,yield=3-4,regrow=12";
    }
}
```

## Example: Seed prices from growing time
file: Scripts/SeedPrices.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SeedPrices : ElintriaScript
{
    public override void Start()
    {
        string stock = "";
        foreach (CropInfo c in CropInfo.Parse(GetComponent<FarmPlot>().crops))
            stock += (stock.Length > 0 ? "|" : "") + c.seed + ":" + Mathf.Max(1, Mathf.RoundToInt(c.hours / 12f));
        Debug.Log(stock);
    }
}
```

# FarmPlot
summary: Soil to plant, water and harvest. E with a seed plants it (the seed in hand, else the first in the bag), E again waters it (with a watering can if it asks for one). It grows only while watered, through its stages, until ripe; E harvests into the bag. Some crops grow back; left dry too long it withers. Rain waters it, and growth follows the Day Night Cycle's hours.
see: CropInfo, Sprinkler, DayNightCycle, Inventory

## Members
needsWater: Whether it grows only while watered.
showStatus: Show the crop and how far along it is when the player is near.
onHarvestTargets: Objects told about each harvest.
onHarvestMessage: The message they get (with how many).
dryHours: Game hours it has gone without water.
withered: Whether the crop has died.
harvests: How many harvests it has given.
Info: A crop's rules by id.
SeedFor: The crop an item plants (null when it is no seed).
@item: The item id.
planted: Whether something grows here.
ripe: Whether it is ready to harvest.
thirsty: Whether it needs water now.
Plant: Plants a crop (no seed taken).
@cropId: The crop.
Water: Waters it for a day.
Clear: Empties the plot.
Grow: Grows by some game hours now (what the clock does).
@hours: Game hours.

## Example: Fertiliser
file: Scripts/Fertiliser.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Fertiliser : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id != "fertiliser") return;
        foreach (FarmPlot p in FindObjectsOfType<FarmPlot>())
            if (p.planted && Vector3.Distance(p.transform.position, transform.position) < 2.5f) { p.water = 99f; p.Grow(12f); }
        GetComponent<Inventory>().Remove("fertiliser", 1);
    }
}
```

## Example: A harvest quest counter
file: Scripts/HarvestCounter.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HarvestCounter : ElintriaScript
{
    public void Harvested(int count) { StdVars.Add("crops_harvested", count); }
}
```
Set each plot's On Harvest to this object with the message "Harvested"; a quest step can then check crops_harvested.

## Example: Crows eat ripe crops at night
file: Scripts/Crows.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Crows : ElintriaScript
{
    private bool m_wasDay = true;

    public override void Update()
    {
        DayNightCycle d = DayNightCycle.current;
        if (d == null || d.isDay == m_wasDay) return;
        m_wasDay = d.isDay;
        if (d.isDay) return;
        foreach (FarmPlot p in FindObjectsOfType<FarmPlot>())
            if (p.ripe && Random.value < 0.25f) { p.Clear(); StdToast.Show("Crows ate a crop!"); }
    }
}
```

# Sprinkler
summary: Keeps every Farm Plot within its radius watered, with a spray drawn round it.
see: FarmPlot

## Members
radius: Metres it reaches.
on: Whether it is running.
showSpray: Draw the spray.
Toggle: Turns it on or off.

## Example: Only in the morning
file: Scripts/MorningSprinkler.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MorningSprinkler : ElintriaScript
{
    public override void Update()
    {
        DayNightCycle d = DayNightCycle.current;
        if (d != null) GetComponent<Sprinkler>().on = d.hour >= 6f && d.hour < 8f;
    }
}
```

## Example: An upgrade that reaches further
file: Scripts/SprinklerUpgrade.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SprinklerUpgrade : Interactable
{
    public override void Interact(Interactor who)
    {
        Wallet w = who.GetComponent<Wallet>();
        if (w != null && w.Spend(50)) { GetComponent<Sprinkler>().radius += 2f; StdToast.Show("Sprinkler upgraded"); }
    }
}
```

## Example: Place sprinklers from the bag
file: Scripts/SprinklerItem.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SprinklerItem : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id != "sprinkler") return;
        GameObject s = GameObject.Create("Sprinkler");
        s.transform.position = transform.position + StdMotor.Facing(gameObject) * 1.5f;
        s.AddComponent<Sprinkler>();
        GetComponent<Inventory>().Remove("sprinkler", 1);
    }
}
```

# TdGame
summary: Runs a tower defense match: money, lives and waves, clicking build spots to build towers (Gun, Cannon, Frost and Sniper unless you give templates), clicking towers to upgrade or sell them, and the camera (WASD pans, the wheel zooms). N sends the next wave early, for a bonus.
see: TdPath, TdTower, TdEnemy, TdBuildSpot

## Members
startMoney: Money at the start.
lives: Lives at the start.
waveCount: How many waves win the match.
firstWaveCount: Enemies in the first wave.
countIncrease: More enemies each wave.
spawnInterval: Seconds between enemies in a wave.
nextWaveKey: The key that sends the next wave (default N).
cameraObject: The camera it moves (empty: the scene's).
cameraHeight: Metres above the ground the camera sits.
panSpeed: Metres a second WASD moves the camera.
showHud: Show the money, lives, wave and build menus.
onWinTargets: Objects told when every wave is beaten.
onWinMessage: The message they get.
onLoseTargets: Objects told when the lives run out.
onLoseMessage: The message they get.
money: Money now.
livesLeft: Lives now.
wave: The wave reached.
selectedSpot: The build spot clicked.
selectedTower: The tower clicked.
kills: Enemies killed.
towers: Every tower built.
current: The match running now.
TowerName: A tower kind's name.
@k: The kind.
TowerCost: A tower kind's price.
@k: The kind.
Build: Builds a tower kind on a spot if there is money; the tower, or null.
@spot: The spot.
@k: The kind.
Upgrade: Upgrades a tower if there is money; whether it did.
Sell: Sells a tower.
aliveEnemies: Enemies on the path now.

## Example: Start with a free tower
file: Scripts/FreeTower.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FreeTower : ElintriaScript
{
    public TdBuildSpot spot;
    private bool m_done;

    public override void Update()
    {
        TdGame g = TdGame.current;
        if (m_done || g == null) return;
        m_done = true;
        g.money += g.TowerCost(0);
        g.Build(spot, 0);
    }
}
```

## Example: Interest between waves
file: Scripts/Interest.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Interest : ElintriaScript
{
    private int m_wave;

    public override void Update()
    {
        TdGame g = TdGame.current;
        if (g == null || g.wave == m_wave) return;
        m_wave = g.wave;
        int bonus = g.money / 10;
        g.money += bonus;
        StdToast.Show("Interest +" + bonus);
    }
}
```

## Example: Endless mode
file: Scripts/EndlessTd.cs
```csharp
using Elintria;
using Elintria.Standard;

public class EndlessTd : ElintriaScript
{
    public override void Update()
    {
        TdGame g = TdGame.current;
        if (g != null && g.wave >= g.waveCount - 1) g.waveCount += 5;   // never runs out
    }
}
```

# TdPath
summary: The road enemies walk: its points in order (or its children, in order). It can draw the road itself.
see: TdGame, TdEnemy

## Members
points: The corners in order (empty: the children).
drawPath: Draw the road while playing.
width: The drawn road's width, metres.
color: The drawn road's colour.
At: The point some metres along the path, and which way it goes there.
@distance: Metres from the start.

## Example: Mark the halfway point
file: Scripts/HalfwayMarker.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HalfwayMarker : ElintriaScript
{
    public override void Start()
    {
        TdPath p = FindObjectOfType<TdPath>();
        Vector3 dir;
        transform.position = p.At(p.length * 0.5f, out dir);
    }
}
```

## Example: Show the path's length
file: Scripts/PathLength.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PathLength : ElintriaScript
{
    public override void Start()
    {
        TdPath p = GetComponent<TdPath>();
        Debug.Log("The road is " + Mathf.Round(p.length) + " m with " + p.Points().Count + " corners");
    }
}
```

## Example: A danger line near the end
file: Scripts/DangerLine.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DangerLine : ElintriaScript
{
    public override void Update()
    {
        TdPath p = GetComponent<TdPath>();
        foreach (TdEnemy e in TdEnemy.all)
            if (e != null && e.alive && e.distance > p.length - 10f)
                ScreenGUI.Text(20f, 60f, "Enemies near the base!", null, new Color(1f, 0.3f, 0.3f, 1f));
    }
}
```

# TdBuildSpot
summary: A place a tower can be built: click it while a Td Game runs.
see: TdGame, TdTower

## Members
tower: The tower on it (null: none).
occupied: Whether a tower stands on it.

## Example: Spots that cost more
file: Scripts/HillSpot.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HillSpot : ElintriaScript
{
    public override void Update()
    {
        TdBuildSpot s = GetComponent<TdBuildSpot>();
        if (s.occupied && s.tower.range < 12f) s.tower.range = 12f;   // high ground sees further
    }
}
```

## Example: Count free spots
file: Scripts/FreeSpots.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FreeSpots : ElintriaScript
{
    public override void Update()
    {
        int free = 0;
        foreach (TdBuildSpot s in FindObjectsOfType<TdBuildSpot>()) if (!s.occupied) ++free;
        ScreenGUI.Text(20f, 80f, free + " spots free", null, Color.white);
    }
}
```

## Example: Unlock a spot later
file: Scripts/LockedSpot.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LockedSpot : ElintriaScript
{
    public int unlockWave = 3;

    public override void Update()
    {
        TdGame g = TdGame.current;
        GetComponent<TdBuildSpot>().enabled = g != null && g.wave >= unlockWave;
    }
}
```

# TdTower
summary: A tower: finds the enemies in range (first on the path, nearest, strongest or last), turns its head and fires shots that home in, for single targets, splash, or cold that slows. Upgrades raise damage, range and speed.
see: TdGame, TdShot, TdEnemy

## Members
towerName: Its name in the menus.
cost: Its price.
range: Metres it reaches.
damage: Damage a shot.
shotSpeed: Metres a second its shots fly.
maxLevel: The highest level.
shotColor: Its shots' colour.
level: Its level now.
spent: Money spent on it so far.
shots: Shots fired.
target: What it is shooting at now.
spot: The spot it stands on.
targetingNames: The names of the targeting modes.
upgradeCost: What the next level costs (0 at the top).
sellValue: What selling it gives back.
Pick: The enemy it would shoot now.

## Example: A tower kind of your own
file: Scripts/PoisonTower.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PoisonTower : ElintriaScript
{
    public override void Awake()
    {
        TdTower t = GetComponent<TdTower>();
        t.towerName = "Poison";
        t.cost = 80;
        t.damage = 3f;
        t.damageType = 2;        // poison keeps hurting
        t.splashRadius = 1.5f;
    }
}
```
Put it on an inactive template with a Td Tower and add the template to the Td Game's Tower Templates.

## Example: Show each tower's level
file: Scripts/TowerLevels.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TowerLevels : ElintriaScript
{
    public override void Update()
    {
        foreach (TdTower t in FindObjectsOfType<TdTower>())
        {
            Vector2 s;
            if (StdScreen.WorldToScreen(t.transform.position + Vector3.up * 2.2f, out s)) ScreenGUI.Text(s.x - 6f, s.y, "L" + t.level, null, Color.white);
        }
    }
}
```

## Example: A hero tower that levels by itself
file: Scripts/VeteranTower.cs
```csharp
using Elintria;
using Elintria.Standard;

public class VeteranTower : ElintriaScript
{
    public override void Update()
    {
        TdTower t = GetComponent<TdTower>();
        if (t.shots >= 50 * t.level) t.LevelUp();
    }
}
```

# TdEnemy
summary: An enemy that walks a Td Path (it needs a Health). Reaching the end costs lives; killing it pays its reward. Cold shots slow it.
see: TdGame, TdPath, Health

## Members
speed: Metres a second.
path: The path it walks.
sideOffset: Metres to the side of the road's middle.
finished: Whether it reached the end.
all: Every enemy on the path.
alive: Whether it is still walking.
hover: Metres it stays above the road, taken from where it was spawned (built-in enemies float at half their height; a template's root should sit at its feet).

## Example: An enemy that heals the others
file: Scripts/TdHealer.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TdHealer : ElintriaScript
{
    private float m_next;

    public override void Update()
    {
        if (Time.time < m_next) return;
        m_next = Time.time + 2f;
        foreach (TdEnemy e in TdEnemy.all)
            if (e != null && e.alive && Vector3.Distance(e.transform.position, transform.position) < 4f) e.GetComponent<Health>().Heal(10f);
    }
}
```

## Example: Speed up when hurt
file: Scripts/TdPanic.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TdPanic : ElintriaScript
{
    public void OnDamaged(float amount) { GetComponent<TdEnemy>().speed *= 1.05f; }
}
```

## Example: The one in front
file: Scripts/TdLeader.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TdLeader : ElintriaScript
{
    public override void Update()
    {
        TdEnemy lead = null;
        foreach (TdEnemy e in TdEnemy.all) if (e != null && e.alive && (lead == null || e.distance > lead.distance)) lead = e;
        if (lead != null) StdHud.Marker(lead.transform.position + Vector3.up * 1.5f, new Color(1f, 0.3f, 0.3f, 1f), -1f);
    }
}
```

# TdShot
summary: A tower's shot: flies to its target (or where it was) and hurts it, and everything round it for splash.
see: TdTower

## Members
target: The enemy it flies at.
tower: The tower that fired it.
aim: Where it is flying now.
speed: Metres a second.
damage: Damage it does.
splash: Metres round the hit that are hurt too.
damageType: The kind of damage (see Damage Type).

## Example: Count shots in the air
file: Scripts/ShotCounter.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ShotCounter : ElintriaScript
{
    public override void Update()
    {
        int n = 0;
        foreach (TdShot s in FindObjectsOfType<TdShot>()) ++n;
        ScreenGUI.Text(20f, 100f, n + " shots flying", null, Color.white);
    }
}
```

## Example: Faster shots everywhere
file: Scripts/FastShots.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FastShots : ElintriaScript
{
    public override void Update()
    {
        foreach (TdShot s in FindObjectsOfType<TdShot>()) s.speed = Mathf.Max(s.speed, 40f);
    }
}
```

## Example: Critical hits
file: Scripts/CriticalShots.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class CriticalShots : ElintriaScript
{
    private readonly HashSet<TdShot> m_seen = new HashSet<TdShot>();

    public override void Update()
    {
        foreach (TdShot s in FindObjectsOfType<TdShot>())
            if (m_seen.Add(s) && Random.value < 0.1f) s.damage *= 3f;
    }
}
```

# DebugConsole
summary: An in-game console: the ` key (under Esc) opens it; type a command and press Enter. Up and Down step through what you typed, Tab completes a name, Esc closes it, and the game pauses while it is open. Built-in commands include god, noclip, heal, killall, give, money, xp, ammo, tp, spawn, timescale, time, weather, fps, save, load, scene, set and get; add your own with Register.
see: StdInput, StdVars

## Members
pauseWhileOpen: Pause the game while it is open.
showFps: Show the frame rate.
maxLines: Lines it keeps.
noclipSpeed: Metres a second while flying with noclip.
current: The console in the scene.
inputLine: What is typed so far.
noclip: Whether noclip is on.
Register: Adds (or replaces) a command.
@help: What help says about it.
@run: Gets the words after the name and returns what to print.
@cheat: Off when cheats are not allowed.
Print: Prints a line.
Execute: Runs one command line; returns what it printed.
@line: The command and its words.
Open: Opens it.
Close: Closes it.
Toggle: Opens or closes it.
SetNoclip: Turns noclip on or off.
@on: True for on.

## Example: A command of your own
file: Scripts/SkipLevelCommand.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SkipLevelCommand : ElintriaScript
{
    public string nextScene = "Level2";

    public override void Start()
    {
        DebugConsole.Register("skip", "skip: goes to the next level", args =>
        {
            SceneManager.LoadScene(nextScene);
            return "Loading " + nextScene;
        }, true);
    }
}
```

## Example: Cheats only in development
file: Scripts/ReleaseConsole.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ReleaseConsole : ElintriaScript
{
    public bool releaseBuild = true;

    public override void Start()
    {
        DebugConsole c = GetComponent<DebugConsole>();
        c.cheatsAllowed = !releaseBuild;
        c.showFps = !releaseBuild;
    }
}
```

## Example: Run commands from a script
file: Scripts/TestSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TestSetup : ElintriaScript
{
    public override void Start()
    {
        DebugConsole.Execute("give wood 50");
        DebugConsole.Execute("money 500");
        DebugConsole.Print("Test kit given");
    }
}
```

# CompassBar
summary: A compass strip at the top of the screen with headings, Hud Markers, quest targets and enemies hunting the player, how far they are (for the ones near the middle), and arrows for those above or below. Put it on the player; the Std Hud's own compass then hides.
see: StdHud, HudMarker, QuestTracker

## Members
width: The bar's width, pixels (narrower in a small window).
top: Pixels from the top of the screen.
height: The bar's height, pixels.
showHeading: Show the heading in degrees under it.
showMarkers: Show Hud Markers.
showQuestTargets: Show the quest tracker's targets.
enemyRange: Metres within which enemies show.
barColor: The bar's colour.
textColor: The headings' colour.
northColor: North's colour.

## Example: A point for this frame
file: Scripts/TreasureSense.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TreasureSense : ElintriaScript
{
    public GameObject treasure;

    public override void Update()
    {
        if (treasure != null && treasure.IsAlive) CompassBar.AddPoint(treasure.transform.position, new Color(1f, 0.85f, 0.2f, 1f), "Treasure");
    }
}
```

## Example: Face north to open a door
file: Scripts/NorthDoor.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NorthDoor : ElintriaScript
{
    public Door door;

    public override void Update()
    {
        float h = CompassBar.Heading();
        if ((h < 5f || h > 355f) && door.locked) { door.Unlock(); door.Open(); }
    }
}
```

## Example: A tidy compass for stealth
file: Scripts/StealthCompass.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StealthCompass : ElintriaScript
{
    public override void Start()
    {
        CompassBar c = GetComponent<CompassBar>();
        c.onlyAlerted = false;        // every enemy near shows
        c.enemyRange = 20f;
        c.showHeading = false;
    }
}
```
