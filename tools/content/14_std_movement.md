# TopDownController
summary: A top-down or isometric character: WASD (or click to move) under a fixed camera, turning to face the mouse - ARPGs and twin-stick games.
see: FirstPersonController, ThirdPersonController, StdMotor

Put it next to a Character Controller (one is added if there is none). The camera follows from `cameraHeight` up and `cameraDistance` back, turned by `cameraYaw` (45 for an isometric look). **GameObject > Standard > Characters > Player (Top Down)** makes one.

## Members
moveSpeed: Metres a second.
acceleration: How fast it gets up to speed.
gravity: Metres a second squared while falling.
cameraObject: The camera it moves (empty: the scene's MainCamera).
fieldOfView: The camera's field of view, degrees.
cameraSmoothing: How quickly the camera catches up (higher: snappier).
velocity: Its velocity now.
facing: Degrees it faces about +Y.
externalControl: Something else moves it now (a dash, a mount): it stops moving itself.
speedMultiplier: Speeds are multiplied by this (stamina, needs, gear).
mouseWorld: The ground point under the mouse.
hasTarget: Click to move: walking to `target`.
target: Click to move: where it is walking to.
Teleport: Puts it at a point at once (a respawn).
@pos: Where its feet go.
MouseOnGround: The ground point under the mouse cursor, at the player's height; false when the mouse is outside the view.
@point: Set to the point.

## Example: Isometric camera
file: Scripts/IsoSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class IsoSetup : ElintriaScript
{
    public override void Start()
    {
        TopDownController td = GetComponent<TopDownController>();
        td.cameraYaw = 45f;
        td.cameraHeight = 14f;
        td.cameraDistance = 14f;
        td.fieldOfView = 35f;
    }
}
```

## Example: Click to move with a marker
file: Scripts/ClickMarker.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ClickMarker : ElintriaScript
{
    public GameObject marker;

    public override void Update()
    {
        TopDownController td = GetComponent<TopDownController>();
        td.clickToMove = true;
        if (marker != null) { marker.SetActive(td.hasTarget); marker.transform.position = td.target; }
    }
}
```

## Example: Firing toward the mouse
file: Scripts/MouseShooter.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MouseShooter : ElintriaScript
{
    public override void Update()
    {
        TopDownController td = GetComponent<TopDownController>();
        if (!Input.GetMouseButtonDown(0)) return;
        Vector3 dir = td.mouseWorld - transform.position;
        dir.y = 0f;
        Debug.Log("Fire toward " + dir.normalized);
    }
}
```

# StealthPlayer
summary: How visible and how loud the player is: the lights near it, crouching and moving decide how fast Basic Enemies with stealth notice it, and footsteps reach them as noise.
see: BasicEnemy, HidingSpot, StdNoise

An eye at the top of the screen fills as guards grow suspicious and turns red when one is hunting the player.

## Members
darkVisibility: Visibility with no light near (0..1).
crouchFactor: Visibility is multiplied by this while crouched.
stillFactor: Visibility is multiplied by this while standing still.
walkNoise: Metres walking footsteps carry.
runNoise: Metres running footsteps carry.
crouchNoise: Metres crouched footsteps carry.
showMeter: Draw the eye meter.
visibility: How visible it is now, 0..1.
lightLevel: The light falling on it now, 0..1.
hidden: In a Hiding Spot: unseen.

## Example: A shadow hint
file: Scripts/ShadowHint.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ShadowHint : ElintriaScript
{
    public override void Update()
    {
        StealthPlayer s = GetComponent<StealthPlayer>();
        if (s.lightLevel < 0.2f) ScreenGUI.Text(20f, 100f, "In shadow", null, new Color(0.6f, 0.7f, 1f, 1f));
    }
}
```

## Example: Quieter boots
file: Scripts/QuietBoots.cs
```csharp
using Elintria;
using Elintria.Standard;

public class QuietBoots : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id != "soft_boots") return;
        StealthPlayer s = GetComponent<StealthPlayer>();
        s.walkNoise = 2f;
        s.runNoise = 6f;
    }
}
```

## Example: A distraction
file: Scripts/ThrowBottle.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ThrowBottle : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.T)) return;
        Vector3 landing = transform.position + transform.forward * 10f;
        StdNoise.Make(landing, 15f, gameObject);     // guards go and look there
    }
}
```

# HidingSpot
summary: A locker, a wardrobe or a bush: E to hide inside, unseen by Basic Enemies, and E again to come out.
see: StealthPlayer, BasicEnemy, Interactable

## Members
inside: Where the player waits inside (empty: this object).
exitPoint: Where the player steps out (empty: in front).
occupied: Someone is hiding in it.
Leave: Brings the hidden player out.

## Example: Hiding heals slowly
file: Scripts/RestingLocker.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RestingLocker : ElintriaScript
{
    public override void Update()
    {
        HidingSpot spot = GetComponent<HidingSpot>();
        if (!spot.occupied) return;
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) p.GetComponent<Health>().Heal(2f * Time.deltaTime);
    }
}
```

## Example: Thrown out when found
file: Scripts/FoundYou.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FoundYou : ElintriaScript
{
    public BasicEnemy guard;

    public override void Update()
    {
        HidingSpot spot = GetComponent<HidingSpot>();
        if (spot.occupied && guard != null && Vector3.Distance(guard.transform.position, transform.position) < 1.5f) spot.Leave();
    }
}
```

## Example: Counting hiding places used
file: Scripts/HideCounter.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HideCounter : ElintriaScript
{
    private bool m_was;

    public override void Update()
    {
        bool now = GetComponent<HidingSpot>().occupied;
        if (now && !m_was) StdVars.Add("times_hidden", 1);
        m_was = now;
    }
}
```

# Companion
summary: A follower: walks after the player, waits when told (H nearby, or E on it), fights enemies that attack the player, and catches up if left far behind.
see: BasicEnemy, Mount

## Members
followDistance: Metres it keeps from the player.
walkSpeed: Metres a second walking.
runSpeed: Metres a second running.
commandKey: The key that toggles wait / follow when the player is near.
fights: Attacks enemies hunting the player.
attackRange: Metres it hits from.
attackDamage: Damage a hit.
attackCooldown: Seconds between hits.
playerTag: The player's tag.
waiting: Waiting where it is.
foe: The enemy it is fighting.
Toggle: Switches between waiting and following.

## Example: A companion that heals you
file: Scripts/MedicCompanion.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MedicCompanion : ElintriaScript
{
    private float m_next;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null || Time.time < m_next) return;
        Health h = p.GetComponent<Health>();
        if (h.health < h.maxHealth * 0.5f && Vector3.Distance(p.transform.position, transform.position) < 4f)
        {
            h.Heal(25f);
            m_next = Time.time + 20f;
            StdToast.Show("Rook patches you up");
        }
    }
}
```

## Example: Wait at the door
file: Scripts/WaitHere.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WaitHere : ElintriaScript
{
    public Companion companion;

    public void OnTriggered()
    {
        if (!companion.waiting) companion.Toggle();
    }
}
```

## Example: A pacifist companion
file: Scripts/Pacifist.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Pacifist : ElintriaScript
{
    public override void Start()
    {
        Companion c = GetComponent<Companion>();
        c.fights = false;
        c.followDistance = 4f;
    }
}
```

# Mount
summary: A creature to ride: E to get on and off; WASD rides it the way you are looking, Shift gallops (using stamina), Space jumps.
see: Companion, Stamina, VehicleEntry

Give it a Character Controller (one is added for templates) and a body model. The rider sits at `saddle`.

## Members
walkSpeed: Metres a second walking.
gallopSpeed: Metres a second galloping.
turnSpeed: Degrees a second it turns.
jumpHeight: Metres a jump rises.
gravity: Metres a second squared.
rider: Who is riding (null: nobody).
speed: Its speed now.
GetOn: Puts |who| in the saddle.
@who: The rider.
GetOff: The rider steps off to the side.

## Example: Whistle for the horse
file: Scripts/Whistle.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Whistle : ElintriaScript
{
    public Mount horse;

    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Y) || horse.rider != null) return;
        horse.transform.position = transform.position - transform.forward * 3f;
        StdToast.Show("Your horse arrives");
    }
}
```

## Example: Faster on roads
file: Scripts/RoadSpeed.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RoadSpeed : ElintriaScript
{
    public float roadHalfWidth = 3f;

    public override void Update()
    {
        Mount m = GetComponent<Mount>();
        m.gallopSpeed = Mathf.Abs(transform.position.x) < roadHalfWidth ? 14f : 10f;
    }
}
```

## Example: Thrown off when hit
file: Scripts/Skittish.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Skittish : ElintriaScript
{
    public void OnDamaged(float amount)
    {
        Mount m = GetComponent<Mount>();
        if (amount > 10f && m.rider != null) m.GetOff();
    }
}
```

# StdMotor
summary: What the movement components share: the player's controller - first person, third person or top down - read and steered through one set of calls.
see: DashAbility, FirstPersonController, ThirdPersonController, TopDownController

Use it in your own abilities so they work with any of the three controllers.

## Members
Body: The object's Character Controller.
@g: The object.
GetVelocity: The controller's velocity.
SetVelocity: Sets the controller's velocity (a knockback, a jump pad).
@v: The new velocity.
External: Hands movement to another component (true) - the controller stops moving the object - or back (false).
@on: True to take over.
IsExternal: Whether something else is moving it now.
Facing: Where it looks, flat (the camera's way in first and third person).
Look: The full look direction, with pitch.
MoveInput: The WASD keys as a world direction relative to the facing, length 0..1.
HasController: Whether it has a First Person, Third Person or Top Down controller.
JumpHeight: The controller's jump height, metres (0 when it has none).
SetJumpHeight: Sets the controller's jump height (Gravity Zones raise it where gravity is weak).
@h: Metres.
SetGravity: Sets the controller's gravity (Gravity Zones).
@gravity: Metres a second squared.
Launch: Throws the character with a velocity, lifting it off the floor first so a grounded controller does not pin it back down.
@velocity: The velocity.
Gravity: The controller's gravity.
Grounded: On the ground.
Key: A key by name ("Alt", "F", "Space"); KeyCode.None when unknown.
@name: The key's name.
Spend: Spends stamina when the object has a Stamina; true when there was enough (or no Stamina).
@amount: How much.
SetSpeedFactor: One source's speed multiplier (needs, status effects, gear, stats); the controller moves at the product of them all.
@source: A name for the source ("gear").
@factor: Its multiplier (1: no change).

## Example: A jump pad
file: Scripts/JumpPad.cs
```csharp
using Elintria;
using Elintria.Standard;

public class JumpPad : ElintriaScript
{
    public float power = 14f;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null || Vector3.Distance(p.transform.position, transform.position) > 1.2f) return;
        Vector3 v = StdMotor.GetVelocity(p);
        v.y = power;
        StdMotor.SetVelocity(p, v);
    }
}
```

## Example: A slowing swamp
file: Scripts/Swamp.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Swamp : ElintriaScript
{
    public float radius = 6f;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null) return;
        bool inside = Vector3.Distance(p.transform.position, transform.position) < radius;
        StdMotor.SetSpeedFactor(p, "swamp", inside ? 0.5f : 1f);
    }
}
```

## Example: Your own ability - a blink
file: Scripts/Blink.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Blink : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(StdMotor.Key("B")) || StdMotor.IsExternal(gameObject)) return;
        if (!StdMotor.Spend(gameObject, 30f)) return;
        transform.position += StdMotor.Facing(gameObject) * 5f;
    }
}
```

# DashAbility
summary: A quick burst along the way you are moving (or facing), on the ground or in the air, costing stamina.
see: StdMotor, Stamina, SlideAbility

## Members
key: The key, by name (default Alt).
distance: Metres a dash covers.
duration: Seconds it takes.
cooldown: Seconds before the next.
staminaCost: Stamina a dash costs (with a Stamina component).
dashing: Dashing now.
Dash: Dashes now (when stamina allows); false when it could not.

## Example: Invulnerable while dashing
file: Scripts/DashDodge.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DashDodge : ElintriaScript
{
    public override void Update()
    {
        Health h = GetComponent<Health>();
        h.invulnerableTime = GetComponent<DashAbility>().dashing ? 0.3f : 0f;
    }
}
```

## Example: Dash on double-tap
file: Scripts/DoubleTapDash.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DoubleTapDash : ElintriaScript
{
    private float m_last = -1f;

    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.W)) return;
        if (Time.time - m_last < 0.25f) GetComponent<DashAbility>().Dash();
        m_last = Time.time;
    }
}
```

## Example: A longer dash after an upgrade
file: Scripts/DashUpgrade.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DashUpgrade : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id != "dash_core") return;
        DashAbility d = GetComponent<DashAbility>();
        d.distance = 9f;
        d.cooldown = 0.5f;
    }
}
```

# DoubleJumpAbility
summary: Jumps again in the air - once, or as many times as `extraJumps` allows - until landing.
see: StdMotor, DashAbility

## Members
extraJumps: Jumps allowed in the air before landing.
jumpHeight: Metres an air jump rises.
staminaCost: Stamina an air jump costs.
jumpsLeft: Air jumps left before landing.

## Example: A triple jump
file: Scripts/TripleJump.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TripleJump : ElintriaScript
{
    public override void Start() { GetComponent<DoubleJumpAbility>().extraJumps = 2; }
}
```

## Example: Showing the jumps left
file: Scripts/JumpPips.cs
```csharp
using Elintria;
using Elintria.Standard;

public class JumpPips : ElintriaScript
{
    public override void Update()
    {
        int left = GetComponent<DoubleJumpAbility>().jumpsLeft;
        for (int i = 0; i < left; ++i) ScreenGUI.Fill(new Rect(20f + i * 14f, 120f, 10f, 10f), Color.white);
    }
}
```

## Example: Only with the boots on
file: Scripts/JumpBoots.cs
```csharp
using Elintria;
using Elintria.Standard;

public class JumpBoots : ElintriaScript
{
    public override void Update()
    {
        Equipment e = GetComponent<Equipment>();
        GetComponent<DoubleJumpAbility>().enabled = e != null && e.feet == "jump_boots";
    }
}
```

# SlideAbility
summary: Running, press crouch: slide along the ground, slowing down, under low gaps.
see: DashAbility, FirstPersonController

## Members
key: The key, by name (default C).
startSpeed: Metres a second at the start of a slide.
duration: Seconds a slide lasts at most.
cooldown: Seconds before the next.
sliding: Sliding now.

## Example: A longer slide downhill
file: Scripts/SlopeSlide.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SlopeSlide : ElintriaScript
{
    public override void Update()
    {
        SlideAbility s = GetComponent<SlideAbility>();
        s.duration = transform.position.y > 5f ? 1.4f : 0.8f;
    }
}
```

## Example: Kicking enemies while sliding
file: Scripts/SlideKick.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SlideKick : ElintriaScript
{
    public override void Update()
    {
        if (!GetComponent<SlideAbility>().sliding) return;
        foreach (BasicEnemy e in FindObjectsOfType<BasicEnemy>())
            if (Vector3.Distance(e.transform.position, transform.position) < 1.2f)
                StdDamage.Apply(e.gameObject, 5f, gameObject, e.transform.position, transform.forward, 6f);
    }
}
```

## Example: Sliding makes noise
file: Scripts/SlideNoise.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SlideNoise : ElintriaScript
{
    private bool m_was;

    public override void Update()
    {
        bool now = GetComponent<SlideAbility>().sliding;
        if (now && !m_was) StdNoise.Make(transform.position, 8f, gameObject);
        m_was = now;
    }
}
```

# MantleAbility
summary: Jump at a ledge within reach and climb up onto it - when there is room to stand on top.
see: Climber, StdMotor

## Members
minHeight: The lowest ledge it climbs, metres above the feet.
maxHeight: The highest ledge it climbs.
reach: Metres from the player's edge to the wall.
duration: Seconds a climb takes.
mantling: Climbing up now.
TryMantle: Climbs onto a ledge in front if there is one; false when there is none.

## Example: Mantle with E as well
file: Scripts/MantleOnE.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MantleOnE : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) GetComponent<MantleAbility>().TryMantle();
    }
}
```

## Example: Higher ledges for a tall character
file: Scripts/TallClimber.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TallClimber : ElintriaScript
{
    public override void Start()
    {
        MantleAbility m = GetComponent<MantleAbility>();
        m.maxHeight = 2.8f;
        m.duration = 0.35f;
    }
}
```

## Example: No mantling while carrying
file: Scripts/HeavyLoad.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HeavyLoad : ElintriaScript
{
    public override void Update()
    {
        GetComponent<MantleAbility>().enabled = GetComponent<PhysicsGrabber>().held == null;
    }
}
```

# WallRunAbility
summary: Run along a wall in the air for a moment, holding forward; jump to kick off it.
see: DoubleJumpAbility, StdMotor

## Members
speed: Metres a second along the wall.
maxTime: Seconds a wall run lasts at most.
jumpOffSpeed: Metres a second a kick off gives.
wallDistance: Metres the wall may be to the side.
running: Running on a wall now.

## Example: Longer runs after an upgrade
file: Scripts/WallRunUpgrade.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WallRunUpgrade : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id == "grip_gloves") GetComponent<WallRunAbility>().maxTime = 2.5f;
    }
}
```

## Example: A wall-run counter
file: Scripts/WallRunStat.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WallRunStat : ElintriaScript
{
    private bool m_was;

    public override void Update()
    {
        bool now = GetComponent<WallRunAbility>().running;
        if (now && !m_was) StdVars.Add("wall_runs", 1);
        m_was = now;
    }
}
```

## Example: Tilting the view
file: Scripts/WallRunTilt.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WallRunTilt : ElintriaScript
{
    public override void Update()
    {
        if (GetComponent<WallRunAbility>().running) StdCameraShake.Shake(0.02f, 0.1f, 4f);
    }
}
```

# GrappleHook
summary: Fire a hook where you look (F); it pulls you to the point. F again lets go.
see: StdMotor, StdLine

## Members
key: The key, by name (default F).
range: Metres it reaches.
pullSpeed: Metres a second it pulls.
stopDistance: Stops this far from the point.
maxTime: Seconds a pull lasts at most.
ropeColor: The rope's colour.
pulling: Being pulled now.
point: Where the hook caught.
Release: Lets go now.

## Example: Only on grapple points
file: Scripts/GrapplePoints.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GrapplePoints : ElintriaScript
{
    public override void Update()
    {
        GrappleHook g = GetComponent<GrappleHook>();
        if (!g.pulling) return;
        bool ok = false;
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("GrapplePoint"))
            if (Vector3.Distance(p.transform.position, g.point) < 1.5f) ok = true;
        if (!ok) g.Release();
    }
}
```

## Example: A longer hook
file: Scripts/LongHook.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LongHook : ElintriaScript
{
    public override void Start()
    {
        GrappleHook g = GetComponent<GrappleHook>();
        g.range = 50f;
        g.pullSpeed = 25f;
    }
}
```

## Example: Pull costs stamina
file: Scripts/GrappleStamina.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GrappleStamina : ElintriaScript
{
    public override void Update()
    {
        GrappleHook g = GetComponent<GrappleHook>();
        if (g.pulling && !StdMotor.Spend(gameObject, 10f * Time.deltaTime)) g.Release();
    }
}
```

# Swimmer
summary: Swims in water - a Water Volume or the World's water: WASD where you look, Space up, C down; oxygen runs out while under.
see: WaterVolume, Water, Buoyancy

## Members
swimSpeed: Metres a second.
swimDepth: Metres from the feet the water must reach to swim.
eyeHeight: Metres from the feet to the eyes (underwater when they are).
oxygenSeconds: Seconds of air under water.
drownDamagePerSecond: Damage a second with no air left.
showOxygen: Draw the oxygen bar.
swimming: Swimming now.
underwater: The head is under.
oxygen: Air left, 0..1.

## Example: A diving mask
file: Scripts/DivingMask.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DivingMask : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id == "diving_mask") GetComponent<Swimmer>().oxygenSeconds = 90f;
    }
}
```

## Example: Muffled sound under water
file: Scripts/Muffle.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Muffle : ElintriaScript
{
    public override void Update()
    {
        bool under = GetComponent<Swimmer>().underwater;
        if (under) ScreenGUI.Fill(new Rect(0f, 0f, GameWindow.width, GameWindow.height), new Color(0.1f, 0.3f, 0.5f, 0.3f));
    }
}
```

## Example: A fast swimmer
file: Scripts/FastSwimmer.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FastSwimmer : ElintriaScript
{
    public override void Start() { GetComponent<Swimmer>().swimSpeed = 6f; }
}
```

# WaterVolume
summary: A box of water: its top is the surface. Swimmers swim in it; it can hurt (toxic water, lava).
see: Swimmer, Water

## Members
size: The box, metres (world axes), centred on this object.
damagePerSecond: Damage a second to a Swimmer in it (0: none).
surface: The surface's height (world y).
Contains: Whether a point is inside.
@p: The point.
At: The volume a point is in (or the World's water there) and its surface height; false when the point is dry.
@surfaceY: Set to the surface height.
@volume: Set to the volume (null for the World's water).

## Example: A rising flood
file: Scripts/RisingFlood.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RisingFlood : ElintriaScript
{
    public float riseSpeed = 0.05f;

    public override void Update()
    {
        WaterVolume w = GetComponent<WaterVolume>();
        w.size = new Vector3(w.size.x, w.size.y + riseSpeed * Time.deltaTime, w.size.z);
    }
}
```

## Example: Is this spot wet?
file: Scripts/WetCheck.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WetCheck : ElintriaScript
{
    public override void Update()
    {
        float y; WaterVolume v;
        if (WaterVolume.At(transform.position, out y, out v)) Debug.Log("In water, surface at " + y);
    }
}
```

## Example: Lava
file: Scripts/Lava.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Lava : ElintriaScript
{
    public override void Start() { GetComponent<WaterVolume>().damagePerSecond = 40f; }
}
```

# Climber
summary: Climbs Climbable Surfaces: walk into one holding forward; WASD moves on it, Space jumps off, and at the top it climbs over.
see: ClimbableSurface, MantleAbility, Stamina

## Members
climbSpeed: Metres a second.
reach: Metres from the wall it can grab.
staminaPerSecond: Stamina a second while moving on the wall.
climbing: Climbing now.
surface: The surface it is on.
LetGo: Lets go of the wall.
@velocity: The velocity it leaves with.

## Example: Faster with climbing gear
file: Scripts/ClimbGear.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ClimbGear : ElintriaScript
{
    public override void Update()
    {
        Equipment e = GetComponent<Equipment>();
        GetComponent<Climber>().climbSpeed = e != null && e.hands == "climbing_gloves" ? 3.5f : 2.2f;
    }
}
```

## Example: Falling rocks while climbing
file: Scripts/LooseRocks.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LooseRocks : ElintriaScript
{
    private float m_next;

    public override void Update()
    {
        Climber c = GetComponent<Climber>();
        if (!c.climbing || Time.time < m_next) return;
        m_next = Time.time + 5f;
        if (Random.value < 0.3f) c.LetGo(Vector3.zero);
    }
}
```

## Example: Climbing hint
file: Scripts/ClimbHint.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ClimbHint : ElintriaScript
{
    public override void Update()
    {
        if (GetComponent<Climber>().climbing) ScreenGUI.Text(20f, 140f, "W/S climb   A/D across   Space jump off");
    }
}
```

# ClimbableSurface
summary: Marks a wall that can be climbed: a box whose forward (+Z) face is the climbing side.
see: Climber

## Members
size: The box, metres: width, height, depth.
outward: The way the climbing side faces.
top: The world height of its top.
Near: Whether a point is within the box grown by |margin|.
@p: The point.
@margin: Metres to grow the box by.

## Example: Making a wall climbable in code
file: Scripts/MakeClimbable.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MakeClimbable : ElintriaScript
{
    public override void Start()
    {
        ClimbableSurface s = gameObject.AddComponent<ClimbableSurface>();
        s.size = new Vector3(3f, 8f, 0.6f);
    }
}
```

## Example: Ivy that withers
file: Scripts/WitheringIvy.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WitheringIvy : ElintriaScript
{
    public float lasts = 60f;

    public override void Update()
    {
        if (Time.time > lasts) GetComponent<ClimbableSurface>().enabled = false;
    }
}
```

## Example: Marking climbable walls on the HUD
file: Scripts/ClimbMarkers.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ClimbMarkers : ElintriaScript
{
    public override void Update()
    {
        foreach (ClimbableSurface s in FindObjectsOfType<ClimbableSurface>())
            StdHud.Marker(s.transform.position + Vector3.up * 2f, new Color(0.5f, 1f, 0.5f, 1f), -1f);
    }
}
```

# Stamina
summary: Drains while running, jumping, dashing and climbing; at zero, no running until it recovers.
see: DashAbility, Climber, CharacterStats

## Members
maxStamina: The most there can be.
stamina: How much there is now.
regenPerSecond: How fast it comes back.
sprintCostPerSecond: Spent a second while running.
jumpCost: Spent on each jump.
showBar: Draw a bar under the health bar (the Std Hud draws one when this is off).
exhausted: Ran out: no running until it refills to `recoverShare`.
Use: Spends |amount|; false (and nothing spent) when there is not enough.
@amount: How much.

## Example: An energy drink
file: Scripts/EnergyDrink.cs
```csharp
using Elintria;
using Elintria.Standard;

public class EnergyDrink : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id != "energy_drink") return;
        Stamina s = GetComponent<Stamina>();
        s.stamina = s.maxStamina;
        s.regenPerSecond = 40f;
    }
}
```

## Example: Heavy armour tires you
file: Scripts/HeavyArmour.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HeavyArmour : ElintriaScript
{
    public override void Update()
    {
        Equipment e = GetComponent<Equipment>();
        GetComponent<Stamina>().sprintCostPerSecond = e != null && e.body == "plate_armour" ? 25f : 15f;
    }
}
```

## Example: A melee swing costs stamina
file: Scripts/SwingCost.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SwingCost : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GetComponent<Stamina>().Use(12f)) StdToast.Show("Too tired");
    }
}
```

# Needs
summary: Hunger, thirst and body temperature. Empty or freezing hurts and slows; food and drink from the Inventory restore them.
see: Stamina, HeatSource, DayNightCycle, Inventory

Using an item (Inventory.UseSelected) that `consumables` lists eats or drinks it. The Day Night Cycle sets `ambientTemperature`; Heat Sources warm the air near them.

## Members
useHunger: Track hunger.
useThirst: Track thirst.
useTemperature: Track body temperature.
thirstPerMinute: Thirst points lost a minute.
damagePerSecond: Damage a second while starving, parched or too cold or hot.
comfortableMin: Below this air temperature the body cools.
comfortableMax: Above this air temperature the body warms.
showBars: Draw the bars.
hunger: 0..100.
thirst: 0..100.
temperature: Body temperature, degrees.
Consume: Applies an item's effects ("hunger", "thirst", "warmth", "health", "stamina").
@d: The effects and amounts.

## Example: Your own food
file: Scripts/FoodList.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FoodList : ElintriaScript
{
    public override void Start()
    {
        Needs n = GetComponent<Needs>();
        n.consumables = "apple:hunger=25|stew:hunger=50,warmth=10|coffee:thirst=20,stamina=40";
    }
}
```

## Example: Hunger only in survival mode
file: Scripts/SurvivalMode.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SurvivalMode : ElintriaScript
{
    public bool survival = false;

    public override void Start()
    {
        Needs n = GetComponent<Needs>();
        n.useHunger = survival;
        n.useThirst = survival;
        n.useTemperature = survival;
    }
}
```

## Example: A warning when hungry
file: Scripts/HungerWarning.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HungerWarning : ElintriaScript
{
    private bool m_warned;

    public override void Update()
    {
        float h = GetComponent<Needs>().hunger;
        if (h < 20f && !m_warned) { StdToast.Show("You are hungry"); m_warned = true; }
        if (h > 40f) m_warned = false;
    }
}
```

# HeatSource
summary: A fire or a heater: warms the air for Needs within its radius.
see: Needs, DayNightCycle

## Members
radius: Metres it warms.
warmth: Degrees added at its middle (less toward the edge).
WarmthAt: The extra warmth at a point from every Heat Source.
@p: The point.

## Example: A fire that burns down
file: Scripts/BurningDown.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BurningDown : ElintriaScript
{
    public float burnMinutes = 5f;

    public override void Update()
    {
        HeatSource h = GetComponent<HeatSource>();
        h.warmth = Mathf.Max(0f, h.warmth - 15f / (burnMinutes * 60f) * Time.deltaTime);
    }
}
```

## Example: Warmth on the HUD
file: Scripts/WarmthHud.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WarmthHud : ElintriaScript
{
    public override void Update()
    {
        float w = HeatSource.WarmthAt(transform.position);
        if (w > 1f) ScreenGUI.Text(20f, 160f, "Warm (+" + Mathf.Round(w) + ")");
    }
}
```

## Example: Lighting a fire
file: Scripts/LightFire.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LightFire : Interactable
{
    public override string Prompt(Interactor who) => "Light the fire";

    public override void Interact(Interactor who)
    {
        HeatSource h = GetComponent<HeatSource>();
        h.enabled = true;
        h.warmth = 15f;
    }
}
```
