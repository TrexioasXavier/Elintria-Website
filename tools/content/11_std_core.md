# StdEvent
summary: Inspector events: call a method on a list of objects, optionally with a value, with no code.
see: manual-messages, Health, Door, TriggerZone

Every Standard component exposes its events as two fields, such as `onOpenTargets` (a list of objects) and `onOpenMessage` (a method name). When the event happens, `StdEvent.Fire` calls that method on every script of each target.

- `"Open"` calls `Open()`.
- `"GoToFloor:2"` passes a value: it calls `GoToFloor(2)`. Floats, ints, bools and strings are converted for you.
- With no value, the event's own value (the floor reached, the object that came in) goes to a method that takes that type.
- `"SetActive:true"` / `"SetActive:false"` switches the targets on or off, and `"Destroy"` removes them. Neither needs a script.

Give your own components events the same way: two fields, then call `Fire`.

## Members
Fire: Sends `message` to every object in `targets` (or to one `target`), passing `arg` when the method takes a parameter.
@targets: The objects to send to.
@target: The one object to send to.
@message: A method name, optionally with ":value".
@arg: The event's own value, passed to methods that take that type.

## Example: Your own component with an event
file: Scripts/Button3D.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class Button3D : ElintriaScript
{
    public List<GameObject> onPressTargets = new List<GameObject>();
    public string onPressMessage = "Open";

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            StdEvent.Fire(onPressTargets, onPressMessage, other.gameObject);
    }
}
```

## Example: Messages with values
file: Scripts/EventValues.cs
The same fields can call different methods with different values. The receiving scripts are ordinary methods.
```csharp
using Elintria;
using Elintria.Standard;

public class EventValues : ElintriaScript
{
    public GameObject target;

    public override void Start()
    {
        StdEvent.Fire(target, "SetSpeed:4.5");     // SetSpeed(float)
        StdEvent.Fire(target, "GoToFloor:2");      // GoToFloor(int)
        StdEvent.Fire(target, "SetActive:false");  // no script needed
    }
}

public class Receiver : ElintriaScript
{
    public void SetSpeed(float s) { Debug.Log("speed " + s); }
    public void GoToFloor(int f) { Debug.Log("floor " + f); }
}
```

## Example: Passing the event's own value
file: Scripts/WelcomeSign.cs
A TriggerZone's onEnter event passes the object that came in, so a method taking a GameObject receives it.
```csharp
using Elintria;

public class WelcomeSign : ElintriaScript
{
    // TriggerZone: onEnterTargets = [this sign], onEnterMessage = "Greet"
    public void Greet(GameObject visitor)
    {
        Debug.Log("Welcome, " + (visitor != null ? visitor.name : "stranger") + "!");
    }
}
```

# StdInput
summary: The keyboard and mouse as Standard components read them, which code can override to drive them.
see: ThirdPersonController, VehicleController, Weapon

Each Standard controller, vehicle and weapon has an `input` field of this type. Normally it reads the real keyboard and mouse. Set `overridden` and fill in the values yourself to drive the component from code instead: an AI, a cutscene, a replay, or an automated test.

## Members
overridden: When true, the keyboard and mouse are ignored and the values you set are used.
move: Movement: x right, y forward, each -1..1.
look: Mouse look this frame, in degrees (yaw, pitch).
jump: Jump held.
sprint: Sprint held.
fire: Fire pressed this frame.
fireHeld: Fire held.
aim: Aim held.
interact: Interact pressed.
reload: Reload pressed.
lockOn: Lock-on pressed.
handbrake: Handbrake held.
JumpPressed: True on the frame jump went down.
FirePressed: True on the frame fire went down.
InteractPressed: True on the frame interact went down.
ReloadPressed: True on the frame reload went down.
LockOnPressed: True on the frame lock-on went down.
Read: Reads the keyboard and mouse into the fields, unless overridden.
@mouseSensitivity: Degrees of look per pixel of mouse movement.
@lockedCursor: Whether the cursor is locked for mouse look.

## Example: An AI driving the third-person controller
file: Scripts/AiWalker.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AiWalker : ElintriaScript
{
    public GameObject goal;

    public override void Update()
    {
        ThirdPersonController tpc = GetComponent<ThirdPersonController>();
        tpc.input.overridden = true;
        Vector3 to = goal.transform.position - transform.position;
        to.y = 0f;
        tpc.input.move = to.magnitude > 1f ? new Vector2(0f, 1f) : Vector2.zero;   // walk forward
        tpc.yaw = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg;                          // face the goal
        tpc.input.jump = Physics.Raycast(transform.position + Vector3.up * 0.3f, transform.forward, 1f);   // hop over low things
    }
}
```

## Example: A scripted car for a cutscene
file: Scripts/CutsceneDriver.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CutsceneDriver : ElintriaScript
{
    float t;

    public override void Update()
    {
        VehicleController car = GetComponent<VehicleController>();
        car.input.overridden = true;
        t += Time.deltaTime;
        car.input.move = new Vector2(Mathf.Sin(t * 0.5f) * 0.4f, t < 6f ? 1f : 0f);   // weave, then coast
        car.input.handbrake = t > 6f && t < 7f;
        if (t > 9f) car.input.overridden = false;   // hand control back to the player
    }
}
```

## Example: A turret that fires on a timer
file: Scripts/AutoTurret.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AutoTurret : ElintriaScript
{
    public float every = 0.5f;
    float next;

    public override void Update()
    {
        Weapon gun = GetComponent<HitscanWeapon>();
        gun.input.overridden = true;
        gun.input.fire = Time.time >= next;
        gun.input.fireHeld = gun.input.fire;
        if (gun.input.fire) next = Time.time + every;
    }
}
```

# Health
summary: Hit points: take damage, heal, die, and fire events. Weapons and hazards work with it automatically.
see: StdDamage, Weapon, Destructible, Spawner

Add `Health` to anything that can be hurt. Weapons call `Damage`, and other scripts can `SendMessage("ApplyDamage", amount)`. At zero health the object dies. Its `onDeath` event fires, `"OnDeath"` goes to its own scripts (a [[Ragdoll]] or [[Destructible]] reacts to it), and with `destroyOnDeath` it is removed after `destroyDelay` seconds.

## Members
maxHealth: The most health it can have.
health: Its health now.
invulnerableTime: Seconds after a hit during which further damage is ignored.
destroyOnDeath: Remove the object when it dies.
destroyDelay: How long after death it is removed.
onDeathTargets: Objects told when it dies.
onDeathMessage: The message they get.
onDamageTargets: Objects told when it is hurt.
onDamageMessage: The message they get (with the damage amount).
isDead: Whether it is dead.
lastDamage: The size of the last hit.
lastAttacker: Who dealt the last hit.
Damage: Takes `amount` off. Returns whether it hurt (false while invulnerable or dead).
ApplyDamage: The SendMessage form of Damage, for scripts that don't know about Health.
Heal: Adds `amount`, up to maxHealth.
Die: Kills it now.
Revive: Back to full health, alive again (for respawns).

## Example: A health bar above the player
file: Scripts/PlayerHealthHud.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PlayerHealthHud : ElintriaScript
{
    public override void Update()
    {
        Health h = GetComponent<Health>();
        ScreenGUI.Bar(new Rect(20, 20, 220, 18), h.health / h.maxHealth, Color.red, new Color(0f, 0f, 0f, 0.5f));
        if (h.isDead) ScreenGUI.Label(new Rect(0, 200, ScreenGUI.screenWidth, 40), "YOU DIED", null, Color.red, TextAnchor.MiddleCenter);
    }
}
```

## Example: Respawning
file: Scripts/RespawnOnDeath.cs
`"OnDeath"` arrives on the object's own scripts. This one waits, moves the player, and calls `Revive`.
```csharp
using System.Collections;
using Elintria;
using Elintria.Standard;

public class RespawnOnDeath : ElintriaScript
{
    public GameObject spawnPoint;

    public void OnDeath() { StartCoroutine(Back()); }

    IEnumerator Back()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<ThirdPersonController>().Teleport(spawnPoint.transform.position);
        GetComponent<Health>().Revive();
    }
}
```

## Example: Hazards, healing and attribution
file: Scripts/AcidPool.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AcidPool : ElintriaScript
{
    public float damagePerSecond = 15f;

    public override void OnTriggerStay(Collider other)
    {
        Health h = other.gameObject.GetComponent<Health>();
        if (h != null && h.Damage(damagePerSecond * Time.fixedDeltaTime, gameObject))
            Debug.Log(other.gameObject.name + " burns (" + h.health.ToString("0") + " left)");
    }
}

public class MedKit : ElintriaScript
{
    public override void OnTriggerEnter(Collider other)
    {
        Health h = other.gameObject.GetComponent<Health>();
        if (h == null || h.health >= h.maxHealth) return;
        h.Heal(50f);
        Destroy(gameObject);
    }
}
```

# StdDamage
summary: Hurts an object (its Health, or a parent's) and pushes its Rigidbody, in one call.
see: Health, Weapon

All Standard weapons deal damage through `StdDamage.Apply`. It finds the [[Health]] on the object or above it (so hitting an arm hurts the body), applies the damage with the attacker recorded, and pushes the hit Rigidbody. Use it in your own weapons and traps so they behave the same way.

## Members
Apply: Hurts `target` by `amount` and pushes it along `dir` with `force` at `point`. Returns whether there was anything to hurt.
@target: The object that was hit.
@point: Where it was hit.
@dir: The direction of the hit.
@force: How hard to push its Rigidbody.

## Example: A swinging axe trap
file: Scripts/AxeTrap.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AxeTrap : ElintriaScript
{
    public float damage = 40f;

    public override void Update()
    {
        transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time * 2f) * 70f);
    }

    public override void OnTriggerEnter(Collider other)
    {
        Vector3 dir = (other.gameObject.transform.position - transform.position).normalized;
        StdDamage.Apply(other.gameObject, damage, gameObject, other.ClosestPoint(transform.position), dir, 8f);
    }
}
```

## Example: Your own ray weapon
file: Scripts/ZapGun.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ZapGun : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 40f))
            if (!StdDamage.Apply(hit.gameObject, 25f, gameObject, hit.point, transform.forward, 3f))
                Debug.Log("Hit " + hit.gameObject.name + ", which can't be hurt");
    }
}
```

## Example: Fall damage
file: Scripts/LandingDamage.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LandingDamage : ElintriaScript
{
    public override void OnCollisionEnter(Collision c)
    {
        float speed = c.relativeVelocity.magnitude;
        if (speed > 12f) StdDamage.Apply(gameObject, (speed - 12f) * 6f, c.gameObject, transform.position, Vector3.down, 0f);
    }
}
```

# StdCamera
summary: Finds the scene's camera and places it, shared by the Standard controllers.
see: ScriptCamera, ThirdPersonController

## Members
Find: The scene's camera object: one tagged MainCamera, else one named "Main Camera" or "Camera", else null.
Place: Puts the view at `pos` looking along `dir`: moves the camera object if there is one (cameras look down their -Z), or the script camera otherwise.
@cam: The camera object, or null for the script camera.
@pos: Where the view is.
@dir: Which way it looks.
@fov: Field of view in degrees.

## Example: A fixed security-camera view
file: Scripts/FixedView.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FixedView : ElintriaScript
{
    public GameObject lookAt;

    public override void LateUpdate()
    {
        Vector3 dir = (lookAt.transform.position - transform.position).normalized;
        StdCamera.Place(StdCamera.Find(), transform.position, dir, 50f);
    }
}
```

## Example: A top-down camera
file: Scripts/TopDownCam.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TopDownCam : ElintriaScript
{
    public float height = 18f;

    public override void LateUpdate()
    {
        StdCamera.Place(StdCamera.Find(), transform.position + new Vector3(0f, height, -height * 0.4f),
                        new Vector3(0f, -1f, 0.4f).normalized, 55f);
    }
}
```

## Example: Checking for a scene camera
file: Scripts/CameraCheck.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CameraCheck : ElintriaScript
{
    public override void Start()
    {
        GameObject cam = StdCamera.Find();
        Debug.Log(cam != null ? "Using camera object " + cam.name : "No camera object: the script camera will be used");
    }
}
```

# ThirdPersonController
summary: A complete third-person character: walk, run, jump, an orbit camera that avoids walls, strafe mode and lock-on.
see: LockOnTarget, StdInput, Health, Interactor

Put it on the player next to a [[CharacterController]]. W A S D walk, Shift runs, Space jumps, and the mouse orbits the camera, which pulls in rather than going through walls.

- **Free turn** (`strafeMode` off): the character turns to face where it walks, as in an adventure game.
- **Strafe** (`strafeMode` on): it always faces the camera's way and steps sideways, as in a shooter.
- **Lock-on** (middle mouse or Q): picks the target nearest the middle of the view within `lockOnRange` (a [[LockOnTarget]], or anything tagged `lockOnTag`). The camera keeps it framed and the character circles it.

Set `model` to the rigged child and fill in the clip names to animate it.

## Members
walkSpeed: Walking speed, metres a second.
sprintSpeed: Running speed (Shift).
acceleration: How quickly it reaches speed.
turnSpeed: Degrees a second the character turns to face its way.
jumpHeight: How high a jump goes, in metres.
gravity: Downward acceleration (negative).
airControl: How much the stick still steers in the air, 0..1.
strafeMode: Face the camera's way and strafe, instead of turning to face the way it walks.
cameraObject: The camera to move (empty: the scene's camera).
cameraDistance: How far behind the camera sits.
minDistance: The closest the camera comes when pulled in by walls.
pivotHeight: The height above the feet the camera orbits.
shoulderOffset: Over-the-shoulder offset to the right, in metres.
minPitch: The lowest the camera looks, in degrees.
maxPitch: The highest the camera looks, in degrees.
mouseSensitivity: Degrees of turn per pixel of mouse movement.
cameraRadius: The camera's size when testing for walls.
cameraSmoothing: How fast the camera's distance catches up (higher: snappier).
fieldOfView: The camera's field of view.
lockCursor: Lock and hide the mouse cursor while playing.
lockOnRange: How far away a target can be locked.
lockOnTag: A tag that marks lock-on targets (besides LockOnTarget components).
lockOnAngle: How far from the middle of the view a target may be, in degrees.
model: The rigged model to animate (empty: this object).
idleClip: The clip played standing still.
walkClip: The clip played walking.
runClip: The clip played running.
jumpClip: The clip played in the air.
input: The input it reads; override it to drive the character from code.
yaw: The camera's turn, in degrees.
pitch: The camera's tilt, in degrees.
lockTarget: The target locked on to, or null.
velocity: The character's velocity.
cameraActualDistance: How far the camera actually is (after walls).
isGrounded: Whether it is standing on the ground.
facing: Which way the character faces, in degrees about +y.
Pivot: The point the camera orbits.
ToggleLockOn: Locks on to the best target, or lets go of the one held.
FindLockTarget: The best target to lock on to now, or null.
Teleport: Puts the player at `pos` at once (respawns, doors to other areas).

## Example: Switching to strafe mode while aiming
file: Scripts/AimMode.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AimMode : ElintriaScript
{
    float normalFov;

    public override void Start() { normalFov = GetComponent<ThirdPersonController>().fieldOfView; }

    public override void Update()
    {
        ThirdPersonController tpc = GetComponent<ThirdPersonController>();
        bool aiming = Input.GetMouseButton(1);
        tpc.strafeMode = aiming;
        tpc.shoulderOffset = aiming ? 0.7f : 0.3f;
        tpc.fieldOfView = Mathf.Lerp(tpc.fieldOfView, aiming ? normalFov * 0.7f : normalFov, 10f * Time.deltaTime);
    }
}
```

## Example: Tuning by surface and status
file: Scripts/MoveModifiers.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MoveModifiers : ElintriaScript
{
    public bool slowed;
    float walk, run;

    public override void Start()
    {
        var tpc = GetComponent<ThirdPersonController>();
        walk = tpc.walkSpeed; run = tpc.sprintSpeed;
    }

    public override void Update()
    {
        var tpc = GetComponent<ThirdPersonController>();
        bool inWater = Water.Depth(transform.position + Vector3.up * 0.5f) > 0f;
        float k = (slowed ? 0.5f : 1f) * (inWater ? 0.6f : 1f);
        tpc.walkSpeed = walk * k;
        tpc.sprintSpeed = run * k;
        tpc.jumpHeight = inWater ? 0.4f : 1.2f;
    }
}
```

## Example: A lock-on indicator
file: Scripts/LockOnHud.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LockOnHud : ElintriaScript
{
    public override void Update()
    {
        var tpc = GetComponent<ThirdPersonController>();
        if (tpc.lockTarget != null)
            ScreenGUI.Label(new Rect(0, 40, ScreenGUI.screenWidth, 30), "Locked on: " + tpc.lockTarget.name, null, Color.red, TextAnchor.MiddleCenter);
        else if (tpc.FindLockTarget() != null)
            ScreenGUI.Label(new Rect(0, 40, ScreenGUI.screenWidth, 30), "[Q] Lock on", null, Color.white, TextAnchor.MiddleCenter);
    }
}
```

# LockOnTarget
summary: Marks an object the Third Person Controller can lock on to.
see: ThirdPersonController

## Members
aimHeight: How far above the object's origin the camera aims (the chest or head).

## Example: Marking enemies when they spawn
file: Scripts/MarkTarget.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MarkTarget : ElintriaScript
{
    public void OnSpawned(GameObject spawner)
    {
        if (GetComponent<LockOnTarget>() == null) gameObject.AddComponent<LockOnTarget>().aimHeight = 1.4f;
    }
}
```

## Example: A tall boss
file: Scripts/BossAim.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BossAim : ElintriaScript
{
    public override void Start()
    {
        LockOnTarget t = GetComponent<LockOnTarget>();
        if (t != null) t.aimHeight = 4f;   // aim at the head of a 5 m giant
    }
}
```

## Example: A weak point to lock on to
file: Scripts/WeakPoint.cs
Put the target on a child object, such as a glowing core, and the camera frames that instead of the boss's feet.
```csharp
using Elintria;
using Elintria.Standard;

public class WeakPoint : ElintriaScript
{
    public override void Start()
    {
        GameObject core = new GameObject("Weak Point");
        core.transform.SetParent(transform, false);
        core.transform.localPosition = new Vector3(0f, 3f, 0.8f);
        core.AddComponent<LockOnTarget>().aimHeight = 0f;
    }
}
```

# VehicleController
summary: An arcade car: drive, brake, reverse, steer and drift, with optional wheels and a chase camera.
see: VehicleEntry, StdInput

An arcade car, not a simulation, and it needs no Rigidbody. Four rays find the ground under the wheels, and the body rides on them at `rideHeight` and leans with the slope. W and S drive and brake (and reverse once stopped), A and D steer, and Space is the handbrake, which makes the back lose grip so the car drifts. It slides along walls and flies off ramps.

The car's forward is its +Z. `wheels` are optional visuals, in the order front-left, front-right, rear-left, rear-right. A [[VehicleEntry]] lets the player get in, or turn on `startDriving` to begin in the car.

## Members
maxSpeed: Top speed forwards, metres a second.
reverseSpeed: Top speed in reverse.
acceleration: How quickly it speeds up.
brakeDeceleration: How hard it brakes.
coastDeceleration: How quickly it slows with no throttle.
maxSteerAngle: The front wheels' full lock, in degrees.
highSpeedSteer: The fraction of full lock left at top speed (steering gets gentler as it goes faster).
grip: How quickly sideways sliding dies away (higher: more grip).
driftGrip: The grip with the handbrake on (lower: more drift).
wheelBase: The distance between the front and rear axles.
trackWidth: The distance between the left and right wheels.
rideHeight: How high the body sits above the ground.
suspensionTravel: How far the wheels can move up and down.
wheelRadius: The wheels' radius.
gravity: Downward acceleration in the air.
bodyRadius: The size of the sphere used to hit walls.
maxStepHeight: The tallest step (a kerb) the wheels climb.
wheels: Wheel objects to animate: front-left, front-right, rear-left, rear-right.
startDriving: Start with the player driving.
chaseCamera: Move the camera behind the car while driving.
cameraDistance: How far behind the chase camera sits.
cameraHeight: How high the chase camera sits.
cameraObject: The camera to move (empty: the scene's camera).
input: The input it reads; override it to drive from code.
driving: Whether someone is driving.
driver: The player driving, or null.
velocity: The car's velocity.
steer: The steering applied now, -1..1.
wheelsGrounded: How many wheels touch the ground.
drifting: Whether it is drifting.
lastWall: The wall it last slid along.
forwardSpeed: Speed along the car's nose (negative in reverse).
speedKmh: Speed in km/h.
Forward: The car's forward direction.
Enter: `player` gets in: hidden, and the car takes the controls.
Exit: The driver gets out beside the car.

## Example: A speedometer
file: Scripts/Speedometer.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Speedometer : ElintriaScript
{
    public override void Update()
    {
        VehicleController car = GetComponent<VehicleController>();
        if (!car.driving) return;
        ScreenGUI.Label(new Rect(ScreenGUI.screenWidth - 220, ScreenGUI.screenHeight - 70, 200, 50),
                        Mathf.RoundToInt(Mathf.Abs(car.speedKmh)) + " km/h" + (car.drifting ? "  DRIFT!" : ""),
                        GuiFont.Get("", 28, true), Color.white, TextAnchor.MiddleRight);
    }
}
```

## Example: Boost pads
file: Scripts/BoostPad.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BoostPad : ElintriaScript
{
    public float boost = 15f;

    public override void OnTriggerEnter(Collider other)
    {
        VehicleController car = other.gameObject.GetComponent<VehicleController>();
        if (car != null) car.velocity += car.Forward() * boost;
    }
}
```

## Example: Car setups
file: Scripts/CarSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CarSetup : ElintriaScript
{
    public int setup;   // 0 grippy, 1 drifty, 2 off-road

    public override void Start()
    {
        VehicleController c = GetComponent<VehicleController>();
        if (setup == 0) { c.grip = 10f; c.driftGrip = 4f; c.maxSpeed = 32f; }
        else if (setup == 1) { c.grip = 5f; c.driftGrip = 0.8f; c.maxSteerAngle = 38f; }
        else { c.rideHeight = 0.8f; c.suspensionTravel = 0.5f; c.maxStepHeight = 0.6f; c.maxSpeed = 22f; }
    }
}
```

# VehicleEntry
summary: An interactable seat: "Drive" puts the player into a Vehicle Controller.
see: VehicleController, Interactor

Put it on the car (or a door of the car). The player's [[Interactor]] shows "[E] Drive", and pressing it puts the player in. Pressing E again while driving gets out.

## Members
vehicle: The car (empty: the Vehicle Controller on this object or above it).

## Example: Locking cars until a key is found
file: Scripts/CarKey.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CarKey : ElintriaScript
{
    public string keyItem = "car_key";

    public override void Update()
    {
        VehicleEntry seat = GetComponent<VehicleEntry>();
        GameObject player = GameObject.FindWithTag("Player");
        Inventory inv = player != null ? player.GetComponent<Inventory>() : null;
        seat.interactable = inv != null && inv.Has(keyItem);
        seat.prompt = seat.interactable ? "Drive" : "Locked";
    }
}
```

## Example: Pointing a separate door at the car
file: Scripts/DoorSeat.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DoorSeat : ElintriaScript
{
    public GameObject car;

    public override void Start()
    {
        VehicleEntry e = gameObject.AddComponent<VehicleEntry>();
        e.vehicle = car;
        e.prompt = "Get in";
    }
}
```

## Example: Reacting when someone gets in
file: Scripts/EngineStart.cs
```csharp
using Elintria;
using Elintria.Standard;

public class EngineStart : ElintriaScript
{
    bool wasDriving;

    public override void Update()
    {
        bool d = GetComponent<VehicleController>().driving;
        if (d && !wasDriving) gameObject.audioSource.PlayOneShot("Audio/engine_start.wav");
        wasDriving = d;
    }
}
```

# Interactable
summary: The base of everything the player can use with E: shows a prompt and fires an event.
see: Interactor, Door, Lever, Pickup

Add `Interactable` to any object to make it usable. The player's [[Interactor]] shows "[E] " plus `prompt` when looking at it, and pressing E fires `onInteract`. Derive from it for custom behaviour. Override `Prompt` for the text, `CanInteract` to refuse with a reason, and `Interact` for what happens.

## Members
prompt: The text shown after [E].
interactRange: How close the player must be (0: the Interactor's own reach).
interactable: Whether it can be used now.
onInteractTargets: Objects told when it is used.
onInteractMessage: The message they get.
Prompt: The text the prompt shows now ("Open", "Close", "Locked"). Override for dynamic text.
@who: The player's Interactor.
CanInteract: Return false with a `reason` ("Needs Red Key") to refuse.
Interact: What happens when it is used. The base version fires onInteract.
Point: Where the prompt measures distance to (the object's middle).

## Example: A talking NPC
file: Scripts/TalkingNpc.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TalkingNpc : Interactable
{
    public string[] lines = { "Hello there.", "The mine is north of here.", "Be careful." };
    int line;

    public override string Prompt(Interactor who) => "Talk";

    public override void Interact(Interactor who)
    {
        who.Say(lines[line], 3f);
        line = (line + 1) % lines.Length;
        base.Interact(who);   // still fire the Inspector event
    }
}
```

## Example: A switch that needs power
file: Scripts/PoweredSwitch.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PoweredSwitch : Interactable
{
    public bool powered;

    public override bool CanInteract(Interactor who, out string reason)
    {
        reason = powered ? "" : "No power";
        return powered && interactable;
    }

    public override void Interact(Interactor who) { Debug.Log("Switch flipped"); base.Interact(who); }
}
```

## Example: No code: an event in the Inspector
file: Scripts/ShrineSetup.cs
A plain Interactable with its event set fires a method on other objects. This script sets it up from code to show the fields.
```csharp
using Elintria;
using Elintria.Standard;

public class ShrineSetup : ElintriaScript
{
    public GameObject bridge;

    public override void Start()
    {
        Interactable shrine = gameObject.AddComponent<Interactable>();
        shrine.prompt = "Pray";
        shrine.onInteractTargets.Add(bridge);
        shrine.onInteractMessage = "SetActive:true";   // the hidden bridge appears
    }
}
```

# Interactor
summary: On the player: finds the Interactable in front, shows its prompt, and uses it on E.
see: Interactable, Inventory

## Members
reach: How far away things can be used.
viewAngle: Degrees either side of where the player faces that count as "in front".
showPrompt: Draw the "[E] ..." prompt on screen.
inventoryObject: The inventory keys and pickups go to (empty: this object's).
input: The input it reads.
current: The Interactable in front now, or null.
message: A message shown for a moment ("Locked").
inventory: The Inventory used for keys and pickups.
FindBest: The best Interactable in front now.
InteractNow: Uses whatever is in front now (what E does). Returns whether something was used.
Say: Shows `text` on screen for `seconds`.

## Example: Using things from your own key
file: Scripts/UseKey.cs
```csharp
using Elintria;
using Elintria.Standard;

public class UseKey : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetMouseButtonDown(2)) GetComponent<Interactor>().InteractNow();   // middle mouse also uses
    }
}
```

## Example: Highlighting what you look at
file: Scripts/LookHighlight.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LookHighlight : ElintriaScript
{
    public override void Update()
    {
        Interactable it = GetComponent<Interactor>().current;
        if (it != null) Debug.DrawLine(transform.position + Vector3.up, it.Point(), Color.yellow);
    }
}
```

## Example: Messages to the player
file: Scripts/Hints.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Hints : ElintriaScript
{
    public override void OnTriggerEnter(Collider other)
    {
        Interactor who = other.gameObject.GetComponent<Interactor>();
        if (who != null) who.Say("It's cold in here...", 2.5f);
    }
}
```

# Door
summary: A door that swings or slides, can be locked with a key item, and can close by itself.
see: Interactable, Lever, PressurePlate, Inventory

A Door swings `openAngle` degrees about its hinge's up axis, or slides by `slideOffset` when that is set. Put the object's pivot on the hinge, or set `hinge` to the part that moves. A locked door needs `keyItemId` in the player's [[Inventory]] and can use the key up. Other components can open it by message: `Open`, `Close`, `Toggle`, `Unlock` and `Lock`.

## Members
openAngle: How far it swings open, in degrees.
slideOffset: How far it slides open (local axes). Zero means it swings.
hinge: The part that moves (empty: this object).
speed: How fast it opens, degrees (or metres) a second.
startOpen: Start open.
locked: Whether it is locked.
keyItemId: The inventory item that unlocks it.
consumeKey: Use the key up when unlocking.
autoCloseDelay: Close by itself this long after opening (0: stays open).
onOpenTargets: Objects told when it opens.
onOpenMessage: The message they get.
onCloseTargets: Objects told when it closes.
onCloseMessage: The message they get.
isOpen: Whether it is open (or opening).
amount: How open it is: 0 closed to 1 open.
Open: Opens it (if unlocked).
Close: Closes it.
Toggle: Opens it if closed, closes it if open.
Unlock: Unlocks it.
Lock: Locks it.

## Example: A door opened by a lever
file: Scripts/LeverDoorSetup.cs
No code is needed in practice: set the lever's events in the Inspector. Here it is done in code to show the fields.
```csharp
using Elintria;
using Elintria.Standard;

public class LeverDoorSetup : ElintriaScript
{
    public GameObject door;

    public override void Start()
    {
        Lever lever = GetComponent<Lever>();
        lever.onOnTargets.Add(door);
        lever.onOnMessage = "Open";
        lever.onOffTargets.Add(door);
        lever.onOffMessage = "Close";
    }
}
```

## Example: A vault that needs a keycard
file: Scripts/VaultSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class VaultSetup : ElintriaScript
{
    public override void Start()
    {
        Door vault = GetComponent<Door>();
        vault.locked = true;
        vault.keyItemId = "keycard_red";
        vault.consumeKey = false;
        vault.slideOffset = new Vector3(2.2f, 0f, 0f);   // slides sideways
        vault.speed = 1f;
        vault.autoCloseDelay = 8f;
    }
}
```

## Example: Opening every door on an alarm
file: Scripts/EmergencyRelease.cs
```csharp
using Elintria;
using Elintria.Standard;

public class EmergencyRelease : ElintriaScript
{
    public void Release()
    {
        foreach (Door d in FindObjectsOfType<Door>())
        {
            d.Unlock();
            d.Open();
        }
        Debug.Log("All doors released");
    }
}
```

# Lever
summary: A lever or switch that flips on and off, firing events, optionally only once.
see: Interactable, Door, Elevator

## Members
handle: The part that tips over (empty: none).
throwAngle: How far the handle tips, in degrees about its local X.
startOn: Start switched on.
oneShot: Flip once and stay.
onOnTargets: Objects told when it switches on.
onOnMessage: The message they get.
onOffTargets: Objects told when it switches off.
onOffMessage: The message they get.
isOn: Whether it is on.
Toggle: Flips it.

## Example: Lights on and off
file: Scripts/LightLeverSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LightLeverSetup : ElintriaScript
{
    public GameObject lamps;   // a parent of every lamp

    public override void Start()
    {
        Lever l = GetComponent<Lever>();
        l.onOnTargets.Add(lamps);  l.onOnMessage = "SetActive:true";
        l.onOffTargets.Add(lamps); l.onOffMessage = "SetActive:false";
    }
}
```

## Example: A puzzle of three levers
file: Scripts/LeverPuzzle.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class LeverPuzzle : ElintriaScript
{
    public List<GameObject> levers = new List<GameObject>();
    public GameObject door;
    bool solved;

    public override void Update()
    {
        if (solved || levers.Count < 3) return;
        bool a = levers[0].GetComponent<Lever>().isOn, b = levers[1].GetComponent<Lever>().isOn, c = levers[2].GetComponent<Lever>().isOn;
        if (a && !b && c) { solved = true; door.SendMessage("Open"); }
    }
}
```

## Example: A one-shot bridge lever
file: Scripts/BridgeLever.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BridgeLever : ElintriaScript
{
    public GameObject bridge;

    public override void Start()
    {
        Lever l = GetComponent<Lever>();
        l.oneShot = true;
        l.throwAngle = 60f;
        l.onOnTargets.Add(bridge);
        l.onOnMessage = "Activate";   // a MovingPlatform waiting for Activate
    }
}
```

# Pickup
summary: An item to collect into the Inventory: by pressing E or by walking into it. It spins, bobs, and can respawn.
see: Inventory, ItemDefinition, Interactor

## Members
itemId: The item's id (as in Assets/Items/*.json).
count: How many it gives.
autoPickup: Picked up by walking into it, without pressing E.
autoPickupRadius: How close counts as walking into it.
respawnTime: Comes back this long after being taken (0: gone for good).
spin: Spin slowly.
bobHeight: How far it bobs up and down.
taken: Whether it has been taken (and not respawned yet).
Give: Puts it in `inv` (as much as fits). Returns whether all of it was taken.
Respawn: Puts it back, ready to take again.

## Example: Coins you run through
file: Scripts/CoinSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CoinSetup : ElintriaScript
{
    public override void Start()
    {
        Pickup p = GetComponent<Pickup>();
        p.itemId = "coin";
        p.count = 1;
        p.autoPickup = true;
        p.autoPickupRadius = 1f;
        p.spin = true;
        p.respawnTime = 30f;
    }
}
```

## Example: A dropped loot bag
file: Scripts/LootBag.cs
Enemies drop a copy of a pickup template holding what they carried.
```csharp
using Elintria;
using Elintria.Standard;

public class LootBag : ElintriaScript
{
    public GameObject pickupTemplate;   // inactive, with a Pickup
    public string item = "ammo";
    public int amount = 12;

    public void OnDeath()
    {
        GameObject bag = Instantiate(pickupTemplate, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        Pickup p = bag.GetComponent<Pickup>();
        p.itemId = item;
        p.count = amount;
        p.respawnTime = 0f;
        bag.SetActive(true);
    }
}
```

## Example: Giving a pickup from code
file: Scripts/QuestReward.cs
```csharp
using Elintria;
using Elintria.Standard;

public class QuestReward : ElintriaScript
{
    public GameObject rewardPickup;

    public void Complete(GameObject player)
    {
        Pickup p = rewardPickup.GetComponent<Pickup>();
        if (!p.Give(player.GetComponent<Inventory>())) Debug.Log("Bag full: the reward stays on the ground");
    }
}
```
