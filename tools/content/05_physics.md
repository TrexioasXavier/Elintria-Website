# Physics
summary: Ask the physics world questions: raycasts, sphere and capsule casts, overlaps, and gravity.
see: RaycastHit, Ray, Collider, Rigidbody

`Physics` is how scripts find out what is where. `Raycast` shoots a line and reports the first collider it hits. `SphereCast` and `CapsuleCast` sweep a shape, for things with thickness such as a character or a thick bullet. `OverlapSphere` and `OverlapBox` list colliders inside an area. `Linecast` checks whether anything blocks a line between two points.

> note: Layer masks and `QueryTriggerInteraction` are accepted so Unity code compiles, but Elintria does not filter by layer yet. Box colliders are treated as axis-aligned boxes in queries, so the orientation argument of `OverlapBox` is ignored.

## Members
Raycast: Shoots a ray from `origin` along `direction` and returns true when it hits a collider within `maxDistance`. The overloads with `hitInfo` also say what was hit, where, and the surface normal.
@ray: The ray to cast.
SphereCast: Sweeps a sphere of `radius` from `origin` along `direction`; true when it touches something within `maxDistance`. Use it for thick bullets and to check room for a character.
CapsuleDepenetration: How far a capsule (the segment `point0`-`point1` grown by `radius`) has to move to stop overlapping, ignoring `ignore` and its children. True when it overlaps something.
@point0: The centre of one end sphere.
@point1: The centre of the other end sphere.
@ignore: An object (and everything under it) to leave out, usually the one asking.
@push: Filled in with the smallest move that frees the capsule.
IgnoreTerrain: While on, the character `mover` passes through `terrain` (for terrain holes, caves and tunnels).
@mover: The object with the CharacterController.
@terrain: The terrain object.
@ignore: True to pass through, false to collide again.
SphereCastIgnoring: Like SphereCast, but leaves out `ignore` and everything under it, and can skip triggers.
@hitTriggers: Whether triggers count as hits.
Linecast: True when something lies on the line from `start` to `end`.
OverlapBox: Every collider that overlaps the box at `centre` with `halfExtents`.
@centre: The middle of the box.
@halfExtents: Half the box's size on each axis.
@orientation: Ignored; boxes are axis-aligned.
CheckBox: True when any collider overlaps the box.
CheckSphere: True when any collider overlaps the sphere.
BakeMesh: Does nothing; kept for Unity code (Elintria needs no baking).
@meshID: Ignored.
@convex: Ignored.
IgnoreCollision: Stops the two colliders from colliding with each other.
IgnoreLayerCollision: Kept for Unity code; layers are not used yet.
GetIgnoreLayerCollision: Kept for Unity code; always false.
SphereCastAll: Everything a sphere swept along the ray touches, nearest first.
CapsuleCast: Sweeps a capsule (the segment `point1`-`point2` grown by `radius`) along `direction`; true when it touches something.
@point1: The centre of one end sphere.
@point2: The centre of the other end sphere.
OverlapCapsule: Every collider overlapping the capsule.
CheckCapsule: True when anything overlaps the capsule.

## Example: Ground check under a character
file: Scripts/GroundCheck.cs
A short ray straight down says whether the feet touch something, and what slope it is.
```csharp
using Elintria;

public class GroundCheck : ElintriaScript
{
    public float footOffset = 0.1f;
    public float checkDistance = 0.3f;
    public bool grounded;
    public float slope;

    public override void FixedUpdate()
    {
        Vector3 from = transform.position + Vector3.up * footOffset;
        grounded = Physics.Raycast(from, Vector3.down, out RaycastHit hit, footOffset + checkDistance);
        slope = grounded ? Vector3.Angle(hit.normal, Vector3.up) : 0f;
    }
}
```

## Example: An explosion that damages everything nearby
file: Scripts/Grenade.cs
`OverlapSphere` finds colliders in range, `Linecast` checks for cover, and `AddExplosionForce` pushes bodies.
```csharp
using Elintria;

public class Grenade : ElintriaScript
{
    public float fuse = 3f;
    public float radius = 6f;
    public float damage = 80f;

    public override void Start() { Invoke("Explode", fuse); }

    void Explode()
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, radius))
        {
            GameObject g = c.gameObject;
            if (g == gameObject) continue;
            Vector3 target = c.bounds.center;
            bool covered = Physics.Linecast(transform.position + Vector3.up * 0.2f, target, out RaycastHit block) && block.gameObject != g;
            if (covered) continue;
            float falloff = 1f - Vector3.Distance(transform.position, target) / radius;
            g.SendMessage("ApplyDamage", damage * Mathf.Clamp01(falloff));
            Rigidbody rb = g.GetComponent<Rigidbody>();
            if (rb != null) rb.AddExplosionForce(700f, transform.position, radius);
        }
        Destroy(gameObject);
    }
}
```

## Example: A thick bullet with SphereCast
file: Scripts/PlasmaBolt.cs
Sweeping a sphere each frame means a fast bolt cannot pass through thin walls between frames.
```csharp
using Elintria;

public class PlasmaBolt : ElintriaScript
{
    public float speed = 40f;
    public float radius = 0.2f;

    public override void Update()
    {
        float step = speed * Time.deltaTime;
        if (Physics.SphereCast(transform.position, radius, transform.forward, out RaycastHit hit, step))
        {
            hit.gameObject.SendMessage("ApplyDamage", 25f);
            Destroy(gameObject);
            return;
        }
        transform.position += transform.forward * step;
    }
}
```

## Example: Shooting through several targets
file: Scripts/Railgun.cs
`RaycastAll` lists every hit along the ray, nearest first.
```csharp
using Elintria;

public class Railgun : ElintriaScript
{
    public int maxPierce = 3;

    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 100f);
        for (int i = 0; i < hits.Length && i < maxPierce; i++)
        {
            hits[i].gameObject.SendMessage("ApplyDamage", 50f / (i + 1));
            Debug.DrawLine(transform.position, hits[i].point, Color.cyan, 0.3f);
        }
    }
}
```

# Rigidbody
summary: Puts an object under physics control: gravity, forces, velocity and collisions.
see: Collider, ForceMode, Physics, RigidbodyConstraints

Add a `Rigidbody` to make an object move under physics. It falls under gravity, is pushed by forces, and bounces off colliders. It needs a [[Collider]] on the same object to collide.

Change physics in `FixedUpdate`. Use `AddForce` for pushes and thrust, set `velocity` for instant changes, and use `MovePosition` and `MoveRotation` for kinematic bodies you drive yourself.

## Members
mass: The body's mass in kilograms. Heavier bodies need more force to move.
drag: Air resistance against movement. 0 is none.
angularDrag: Resistance against spinning.
useGravity: Whether gravity pulls the body down.
isKinematic: When true, physics no longer moves the body: you move it from code, and it still pushes other bodies.
freezePositionX: Stops movement along X.
freezePositionY: Stops movement along Y.
freezePositionZ: Stops movement along Z.
freezeRotationX: Stops turning about X.
freezeRotationY: Stops turning about Y.
freezeRotationZ: Stops turning about Z.
velocity: The body's velocity in world units a second. Setting it changes speed instantly.
angularVelocity: How fast it spins, in radians a second about each axis.
position: The body's position. Setting it teleports the body.
rotation: The body's rotation. Setting it turns the body at once.
AddForce: Pushes the body. With the default `ForceMode.Force` the push is spread over the physics step (call it every FixedUpdate for thrust); `Impulse` applies it all at once.
@force: The force, in world space.
@mode: How the force is applied.
AddRelativeForce: Like AddForce, but `force` is in the object's own axes ("forward" means the way it faces).
AddTorque: Spins the body about the axis of `torque`.
@torque: The turning force, in world space.
AddExplosionForce: Pushes the body away from `position`, harder the closer it is, out to `radius`.
@force: The strength of the explosion.
MovePosition: Moves a body (usually kinematic) to `p` during the next physics step, carrying what stands on it.
MoveRotation: Turns a body to `q` (or Euler angles) during the next physics step.
@eulerDegrees: The rotation as Euler angles in degrees.
Sleep: Puts the body to sleep until something touches it.
WakeUp: Wakes a sleeping body.

## Example: A rocket with thrust and torque
file: Scripts/Rocket.cs
Forces belong in `FixedUpdate`. `AddRelativeForce` pushes along the rocket's own up axis, wherever it points.
```csharp
using Elintria;

public class Rocket : ElintriaScript
{
    public float thrust = 25f;
    public float turn = 4f;
    Rigidbody rb;

    public override void Start() { rb = GetComponent<Rigidbody>(); }

    public override void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space)) rb.AddRelativeForce(Vector3.up * thrust);
        rb.AddTorque(new Vector3(Input.GetAxisVertical(), 0f, -Input.GetAxisHorizontal()) * turn);
    }
}
```

## Example: Jumping with an impulse
file: Scripts/BallJump.cs
`ForceMode.Impulse` applies a whole push at once, which suits jumps and hits. `freezeRotation` stops a character capsule from toppling.
```csharp
using Elintria;

public class BallJump : ElintriaScript
{
    public float jumpImpulse = 6f;
    public float moveForce = 10f;
    Rigidbody rb;

    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    public override void Update()
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, 0.6f);
        if (grounded && Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(Vector3.up * jumpImpulse * rb.mass, ForceMode.Impulse);
    }

    public override void FixedUpdate()
    {
        rb.AddForce(new Vector3(Input.GetAxisHorizontal(), 0f, Input.GetAxisVertical()) * moveForce);
    }
}
```

## Example: A kinematic lift with MovePosition
file: Scripts/KinematicLift.cs
A kinematic body follows your code exactly, and bodies standing on it are carried along.
```csharp
using Elintria;

public class KinematicLift : ElintriaScript
{
    public float height = 5f;
    public float period = 6f;
    Rigidbody rb;
    Vector3 start;

    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        start = transform.position;
    }

    public override void FixedUpdate()
    {
        float t = Mathf.PingPong(Time.time / period * 2f, 1f);
        rb.MovePosition(start + Vector3.up * Mathf.SmoothStep(0f, height, t));
    }
}
```

## Example: Speed limit and drag
file: Scripts/HoverCraft.cs
Reading and writing `velocity` directly caps the top speed; `drag` makes it coast to a stop.
```csharp
using Elintria;

public class HoverCraft : ElintriaScript
{
    public float maxSpeed = 15f;
    Rigidbody rb;

    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.8f;
        rb.angularDrag = 2f;
    }

    public override void FixedUpdate()
    {
        rb.AddForce(transform.forward * Input.GetAxisVertical() * 30f);
        rb.AddTorque(Vector3.up * Input.GetAxisHorizontal() * 5f);
        if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;
    }
}
```

# Collider
summary: The base class of every collider: the shape an object has for physics, raycasts and triggers.
see: BoxCollider, SphereCollider, MeshCollider, CharacterController, Rigidbody

A collider gives an object a physical shape. Rays and casts hit it, rigidbodies bounce off it, and with `isTrigger` set it becomes a volume that reports `OnTriggerEnter` instead of blocking. The concrete shapes are [[BoxCollider]], [[SphereCollider]], [[MeshCollider]], [[TerrainCollider]] and the [[CharacterController]] capsule.

## Members
isTrigger: When true, things pass through the collider, and scripts get OnTriggerEnter/Stay/Exit instead of a collision.
center: The centre of the shape, relative to the object.
ClosestPoint: The point on (or inside) the collider nearest `p`.
attachedRigidbody: The Rigidbody that moves this collider, or null for a static one.

## Example: A trigger that heals
file: Scripts/HealingSpring.cs
```csharp
using Elintria;

public class HealingSpring : ElintriaScript
{
    public float healPerSecond = 10f;

    public override void Start() { GetComponent<Collider>().isTrigger = true; }

    public override void OnTriggerStay(Collider other)
    {
        other.gameObject.SendMessage("Heal", healPerSecond * Time.fixedDeltaTime);
    }
}
```

## Example: Nearest point for a magnet
file: Scripts/Magnet.cs
```csharp
using Elintria;

public class Magnet : ElintriaScript
{
    public GameObject plate;   // has a collider
    public float pull = 12f;

    public override void FixedUpdate()
    {
        Collider c = plate.GetComponent<Collider>();
        Rigidbody rb = GetComponent<Rigidbody>();
        if (c == null || rb == null) return;
        Vector3 to = c.ClosestPoint(transform.position) - transform.position;
        if (to.magnitude < 5f) rb.AddForce(to.normalized * pull);
    }
}
```

## Example: Telling moving and static colliders apart
file: Scripts/WhatDidIHit.cs
```csharp
using Elintria;

public class WhatDidIHit : ElintriaScript
{
    public override void OnCollisionEnter(Collision collision)
    {
        Collider c = collision.collider;
        string kind = c.attachedRigidbody != null ? "a moving body" : "static scenery";
        Debug.Log("Hit " + c.gameObject.name + " (" + kind + "), its box is " + c.bounds.size);
    }
}
```

# BoxCollider
summary: A box-shaped collider.
see: Collider, SphereCollider

A box collider with an adjustable `size` and `center`. It is the cheapest and most common collider, used for crates, walls, floors and trigger volumes.

> note: In physics, a rotated box is treated as the axis-aligned box that contains it.

## Members
size: The box's size in the object's local units, before the object's scale.

## Example: Fitting a trigger volume
file: Scripts/RoomVolume.cs
```csharp
using Elintria;

public class RoomVolume : ElintriaScript
{
    public Vector3 roomSize = new Vector3(8f, 3f, 6f);

    public override void Start()
    {
        BoxCollider box = GetComponent<BoxCollider>() ?? gameObject.AddComponent<BoxCollider>();
        box.size = roomSize;
        box.center = new Vector3(0f, roomSize.y * 0.5f, 0f);
        box.isTrigger = true;
    }

    public override void OnTriggerEnter(Collider other) { Debug.Log(other.gameObject.name + " entered " + name); }
}
```

## Example: Crouching shrinks the hitbox
file: Scripts/CrouchBox.cs
```csharp
using Elintria;

public class CrouchBox : ElintriaScript
{
    BoxCollider box;

    public override void Start() { box = GetComponent<BoxCollider>(); }

    public override void Update()
    {
        bool crouch = Input.GetKey(KeyCode.C);
        float h = crouch ? 1f : 2f;
        box.size = new Vector3(0.6f, h, 0.6f);
        box.center = new Vector3(0f, h * 0.5f, 0f);
    }
}
```

## Example: Building walls with colliders
file: Scripts/InvisibleWalls.cs
```csharp
using Elintria;

public class InvisibleWalls : ElintriaScript
{
    public float arenaSize = 30f;

    public override void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject wall = new GameObject("Wall " + i);
            float a = i * 90f * Mathf.Deg2Rad;
            wall.transform.position = new Vector3(Mathf.Sin(a), 0f, Mathf.Cos(a)) * arenaSize * 0.5f + Vector3.up * 5f;
            BoxCollider b = wall.AddComponent<BoxCollider>();
            b.size = (i % 2 == 0) ? new Vector3(arenaSize, 10f, 1f) : new Vector3(1f, 10f, arenaSize);
        }
    }
}
```

# SphereCollider
summary: A ball-shaped collider.
see: Collider, BoxCollider

A sphere collider with an adjustable `radius`. It rolls smoothly, and suits balls, pickups, and round trigger ranges such as aggro or pickup radii.

## Members
radius: The sphere's radius in local units, before the object's scale.

## Example: A pickup radius
file: Scripts/MagnetPickup.cs
```csharp
using Elintria;

public class MagnetPickup : ElintriaScript
{
    public float pickupRadius = 1.5f;

    public override void Start()
    {
        SphereCollider s = gameObject.AddComponent<SphereCollider>();
        s.radius = pickupRadius;
        s.isTrigger = true;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { other.gameObject.SendMessage("AddCoins", 1); Destroy(gameObject); }
    }
}
```

## Example: An aggro radius that grows when alerted
file: Scripts/AggroRange.cs
```csharp
using Elintria;

public class AggroRange : ElintriaScript
{
    public float calmRadius = 6f;
    public float alertRadius = 14f;
    public bool alerted;

    public override void Update()
    {
        SphereCollider s = GetComponent<SphereCollider>();
        if (s != null) s.radius = Mathf.MoveTowards(s.radius, alerted ? alertRadius : calmRadius, 4f * Time.deltaTime);
    }
}
```

## Example: A rolling ball
file: Scripts/RollingBall.cs
```csharp
using Elintria;

public class RollingBall : ElintriaScript
{
    public override void Start()
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);   // already has a SphereCollider
        ball.transform.position = transform.position + Vector3.up * 3f;
        Rigidbody rb = ball.AddComponent<Rigidbody>();
        rb.velocity = new Vector3(3f, 0f, 0f);
        Debug.Log("Ball radius " + ball.GetComponent<SphereCollider>().radius);
    }
}
```

# MeshCollider
summary: A collider that uses a model's own triangles, for level geometry and complex static shapes.
see: Collider, BoxCollider

A `MeshCollider` makes a model solid using its own triangles, so rays, casts, character controllers and falling bodies meet the real shape. It never moves under physics and is never a trigger, so it suits level geometry: floors, ramps, rocks and buildings.

## Members
sharedMesh: Kept for Unity code. The triangles are always the model's own (or the mesh set in the Inspector).
convex: Kept for Unity code. Mesh colliders are never convex here.

## Example: Making imported scenery solid
file: Scripts/SolidScenery.cs
```csharp
using Elintria;

public class SolidScenery : ElintriaScript
{
    public override void Start()
    {
        foreach (Transform child in transform)
            if (child.GetComponent<Collider>() == null)
                child.gameObject.AddComponent<MeshCollider>();
    }
}
```

## Example: Walking on uneven ground
file: Scripts/SurfaceInfo.cs
A raycast against a mesh collider returns the true triangle normal, which shows the slope.
```csharp
using Elintria;

public class SurfaceInfo : ElintriaScript
{
    public override void Update()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 3f) && hit.collider is MeshCollider)
            ScreenGUI.Text(20, 20, "Slope: " + Vector3.Angle(hit.normal, Vector3.up).ToString("0") + " deg on " + hit.gameObject.name);
    }
}
```

## Example: Checking the collider type
file: Scripts/ColliderReport.cs
```csharp
using Elintria;

public class ColliderReport : ElintriaScript
{
    public override void Start()
    {
        Collider c = GetComponent<Collider>();
        if (c is MeshCollider m) Debug.Log(name + ": mesh collider, convex=" + m.convex);
        else if (c is BoxCollider b) Debug.Log(name + ": box " + b.size);
        else if (c is SphereCollider s) Debug.Log(name + ": sphere r=" + s.radius);
    }
}
```

# CharacterController
summary: An upright capsule moved by code that slides along walls, climbs steps and handles slopes.
see: ControllerColliderHit, CollisionFlags, Collider

The `CharacterController` is the usual way to move a player or NPC that should not be pushed around by physics. Call `Move` with how far to go this frame. It slides along whatever it meets, steps up ledges no higher than `stepOffset`, and refuses slopes steeper than `slopeLimit`. `isGrounded` says whether the last move touched the ground.

Elintria's controller can apply gravity for you (`useGravity`), so `SimpleMove` and simple `Move` calls fall naturally.

## Members
slopeLimit: The steepest slope, in degrees, the character can walk up.
stepOffset: The tallest step, in units, the character climbs without jumping.
skinWidth: A thin margin kept between the capsule and what it touches, to stop it sticking.
minMoveDistance: Moves shorter than this are ignored.
radius: The capsule's radius.
height: The capsule's total height.
detectCollisions: Whether other bodies and controllers collide with this one.
enableOverlapRecovery: Whether the controller pushes itself out when it starts a move inside something.
Move: Moves by `motion` (no gravity added; multiply by Time.deltaTime yourself), sliding along obstacles. Returns which sides it touched.
@motion: How far to move this frame, in world units.
SimpleMove: Moves at `speed` units a second along the ground, ignoring y and applying gravity. Returns isGrounded.
@speed: The speed, in units a second.

## Example: A complete walking and jumping character
file: Scripts/PlayerMotor.cs
Keeps its own vertical speed for jumping and gravity, and passes the whole frame's motion to `Move`. Set `useGravity` off when you handle gravity yourself.
```csharp
using Elintria;

public class PlayerMotor : ElintriaScript
{
    public float speed = 5f;
    public float jumpHeight = 1.4f;
    public float gravity = -20f;

    CharacterController cc;
    float vy;

    public override void Start()
    {
        cc = GetComponent<CharacterController>();
        cc.useGravity = false;   // this script handles gravity
    }

    public override void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisHorizontal(), 0f, Input.GetAxisVertical());
        Vector3 move = transform.TransformDirection(input.normalized) * speed;

        if (cc.isGrounded)
        {
            vy = -1f;   // keep pressed against the ground
            if (Input.GetKeyDown(KeyCode.Space)) vy = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }
        vy += gravity * Time.deltaTime;
        move.y = vy;

        CollisionFlags hit = cc.Move(move * Time.deltaTime);
        if ((hit & CollisionFlags.Above) != 0 && vy > 0f) vy = 0f;   // bumped the ceiling
    }
}
```

## Example: An NPC that wanders with SimpleMove
file: Scripts/Wanderer.cs
`SimpleMove` takes a speed, not a distance, and handles gravity, so it is the shortest way to move a character.
```csharp
using Elintria;

public class Wanderer : ElintriaScript
{
    public float speed = 1.5f;
    Vector3 dir = Vector3.forward;
    float nextTurn;

    public override void Update()
    {
        if (Time.time > nextTurn)
        {
            dir = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) * Vector3.forward;
            nextTurn = Time.time + Random.Range(2f, 5f);
        }
        CharacterController cc = GetComponent<CharacterController>();
        cc.SimpleMove(dir * speed);
        if (cc.velocity.sqrMagnitude > 0.01f) transform.rotation = Quaternion.LookRotation(dir);
    }
}
```

## Example: Pushing crates
file: Scripts/CratePusher.cs
`OnControllerColliderHit` reports each bump during a move, which lets the character shove rigidbodies.
```csharp
using Elintria;

public class CratePusher : ElintriaScript
{
    public float pushPower = 2f;

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.rigidbody;
        if (rb == null || rb.isKinematic || hit.moveDirection.y < -0.3f) return;   // not pushing down on it
        Vector3 push = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
        rb.velocity = push * pushPower;
    }
}
```

# ControllerColliderHit
summary: Details of one bump during a CharacterController move, passed to OnControllerColliderHit.
see: CharacterController, ElintriaScript

When a [[CharacterController]] runs into something during `Move` or `SimpleMove`, every script on its object gets `OnControllerColliderHit` with one of these.

## Members
controller: The controller that moved.
collider: The collider it hit.
rigidbody: The hit object's Rigidbody, or null.
gameObject: The object it hit.
transform: The hit object's Transform.
point: Where the contact happened, in world space.
normal: The surface normal at the contact.
moveDirection: The direction the controller was moving.
moveLength: How far the controller was trying to move.

## Example: Landing on a bounce pad
file: Scripts/BouncePadUser.cs
```csharp
using Elintria;

public class BouncePadUser : ElintriaScript
{
    public float launchSpeed = 15f;
    public float verticalSpeed;

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("BouncePad") && hit.normal.y > 0.7f)
            verticalSpeed = launchSpeed;   // a motor script reads this
    }
}
```

## Example: Footstep surfaces
file: Scripts/FootstepSurface.cs
```csharp
using Elintria;

public class FootstepSurface : ElintriaScript
{
    public string surface = "default";

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0.5f) surface = hit.gameObject.tag;   // ground under the feet
    }
}
```

## Example: Hazard on contact
file: Scripts/SpikeContact.cs
```csharp
using Elintria;

public class SpikeContact : ElintriaScript
{
    float lastHurt;

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Spikes") && Time.time - lastHurt > 1f)
        {
            lastHurt = Time.time;
            SendMessage("ApplyDamage", 15f);
            Debug.Log("Ouch, at " + hit.point);
        }
    }
}
```

# CollisionFlags
summary: Which sides of a CharacterController touched something during its last move.
see: CharacterController

`CharacterController.Move` returns these flags, combined with `|`. Test one with `(flags & CollisionFlags.Below) != 0`.

## Members
None: It touched nothing.
Sides: It touched something at its sides (a wall).
CollidedSides: The same as Sides.
Above: It touched something above (a ceiling).
CollidedAbove: The same as Above.
Below: It touched something below (the ground).
CollidedBelow: The same as Below.

## Example: Wall sliding
file: Scripts/WallSlide.cs
```csharp
using Elintria;

public class WallSlide : ElintriaScript
{
    float vy;

    public override void Update()
    {
        CharacterController cc = GetComponent<CharacterController>();
        vy -= 20f * Time.deltaTime;
        CollisionFlags f = cc.Move(new Vector3(Input.GetAxisHorizontal() * 5f, vy, 0f) * Time.deltaTime);
        if ((f & CollisionFlags.Sides) != 0 && vy < -2f) vy = -2f;   // slide slowly down walls
        if ((f & CollisionFlags.Below) != 0) vy = 0f;
    }
}
```

## Example: Head bump
file: Scripts/HeadBump.cs
```csharp
using Elintria;

public class HeadBump : ElintriaScript
{
    public float vy = 8f;

    public override void Update()
    {
        CollisionFlags f = GetComponent<CharacterController>().Move(Vector3.up * vy * Time.deltaTime);
        if ((f & CollisionFlags.Above) != 0) { vy = 0f; Debug.Log("Bonk"); }
    }
}
```

## Example: Logging every contact side
file: Scripts/FlagLogger.cs
```csharp
using Elintria;

public class FlagLogger : ElintriaScript
{
    public override void Update()
    {
        CollisionFlags f = GetComponent<CharacterController>().collisionFlags;
        if (f != CollisionFlags.None) ScreenGUI.Text(20, 20, "Touching: " + f);
    }
}
```

# Collision
summary: What one object hit, passed to OnCollisionEnter, Stay and Exit.
see: ContactPoint, ElintriaScript, Rigidbody

When two solid colliders touch, scripts on both objects get a `Collision`. It says what the other object is, how fast the two met (`relativeVelocity`), and where they touched (`contacts`).

## Members
collider: The other object's collider.
gameObject: The other object.
transform: The other object's Transform.
rigidbody: The other object's Rigidbody, or null.
contacts: The contact points. The solver reports one per pair of colliders.
contactCount: How many contact points there are.
GetContact: The contact at `index`.

## Example: Damage from impact speed
file: Scripts/FallDamage.cs
```csharp
using Elintria;

public class FallDamage : ElintriaScript
{
    public float safeSpeed = 8f;

    public override void OnCollisionEnter(Collision collision)
    {
        float speed = collision.relativeVelocity.magnitude;
        if (speed > safeSpeed) SendMessage("ApplyDamage", (speed - safeSpeed) * 5f);
    }
}
```

## Example: Sparks at the contact point
file: Scripts/Sparks.cs
```csharp
using Elintria;

public class Sparks : ElintriaScript
{
    public GameObject sparkTemplate;

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.contactCount == 0) return;
        ContactPoint c = collision.GetContact(0);
        GameObject s = Instantiate(sparkTemplate, c.point, Quaternion.LookRotation(c.normal));
        s.SetActive(true);
        Destroy(s, 1f);
    }
}
```

## Example: Tracking what is touching
file: Scripts/ContactCounter.cs
```csharp
using System.Collections.Generic;
using Elintria;

public class ContactCounter : ElintriaScript
{
    readonly HashSet<GameObject> touching = new HashSet<GameObject>();

    public override void OnCollisionEnter(Collision c) { touching.Add(c.gameObject); }
    public override void OnCollisionExit(Collision c) { touching.Remove(c.gameObject); }

    public override void Update() { ScreenGUI.Text(20, 40, name + " touches " + touching.Count + " objects"); }
}
```

# ContactPoint
summary: One point where two colliders touched.
see: Collision

## Members
point: Where the colliders touched, in world space.
normal: The contact normal, pointing away from the other collider.
thisCollider: This object's collider.
otherCollider: The other object's collider.
separation: How far apart (positive) or overlapping (negative) the colliders are at the contact.

## Example: Is it standing on top?
file: Scripts/StandingOn.cs
```csharp
using Elintria;

public class StandingOn : ElintriaScript
{
    public override void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint p in collision.contacts)
            if (p.normal.y > 0.7f) { ScreenGUI.Text(20, 60, "Standing on " + p.otherCollider.gameObject.name); break; }
    }
}
```

## Example: Bullet holes
file: Scripts/Decals.cs
```csharp
using Elintria;

public class Decals : ElintriaScript
{
    public GameObject decalTemplate;

    public override void OnCollisionEnter(Collision collision)
    {
        ContactPoint p = collision.GetContact(0);
        GameObject d = Instantiate(decalTemplate, p.point + p.normal * 0.01f, Quaternion.LookRotation(-p.normal));
        d.SetActive(true);
    }
}
```

## Example: Penetration check
file: Scripts/DeepContact.cs
```csharp
using Elintria;

public class DeepContact : ElintriaScript
{
    public override void OnCollisionStay(Collision collision)
    {
        ContactPoint p = collision.GetContact(0);
        if (p.separation < -0.1f) Debug.LogWarning(p.thisCollider.gameObject.name + " is sinking into " + p.otherCollider.gameObject.name);
    }
}
```

# RaycastHit
summary: What a raycast or shape cast found: the point, normal, distance and the object hit.
see: Physics, Ray

## Members
point: Where the ray hit, in world space.
normal: The surface normal at the hit point.
distance: How far along the ray the hit is.
collider: The collider that was hit.
gameObject: The object that was hit.
transform: The Transform of the object hit.
rigidbody: The Rigidbody of the object hit, or null.

## Example: Placing objects on surfaces
file: Scripts/Placer.cs
```csharp
using Elintria;

public class Placer : ElintriaScript
{
    public GameObject propTemplate;

    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 20f))
        {
            GameObject p = Instantiate(propTemplate, hit.point, Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal), hit.normal));
            p.SetActive(true);
        }
    }
}
```

## Example: Pushing what you shoot
file: Scripts/ForceGun.cs
```csharp
using Elintria;

public class ForceGun : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 30f) && hit.rigidbody != null)
            hit.rigidbody.AddForce(transform.forward * 15f, ForceMode.Impulse);
    }
}
```

## Example: Distance read-out
file: Scripts/RangeFinder.cs
```csharp
using Elintria;

public class RangeFinder : ElintriaScript
{
    public override void Update()
    {
        string text = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 500f)
            ? hit.gameObject.name + " - " + hit.distance.ToString("0.0") + " m"
            : "no target";
        ScreenGUI.Text(20, 20, text);
    }
}
```

# ForceMode
summary: How Rigidbody.AddForce applies a force: as a force, an impulse, a velocity change or an acceleration.
see: Rigidbody

## Members
Force: A continuous push, spread over the physics step and divided by mass. Call it every FixedUpdate for thrust or wind.
Impulse: An instant push, divided by mass. Use it for jumps, hits and explosions.
VelocityChange: An instant change of velocity, ignoring mass.
Acceleration: A continuous acceleration that ignores mass, like gravity.

## Example: Jump with Impulse
file: Scripts/ImpulseJump.cs
```csharp
using Elintria;

public class ImpulseJump : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
}
```

## Example: Wind with Acceleration
file: Scripts/WindZone2.cs
Acceleration ignores mass, so light and heavy objects drift the same.
```csharp
using Elintria;

public class WindZone2 : ElintriaScript
{
    public Vector3 wind = new Vector3(3f, 0f, 0f);

    public override void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null) rb.AddForce(wind, ForceMode.Acceleration);
    }
}
```

## Example: Dash with VelocityChange
file: Scripts/DashBody.cs
```csharp
using Elintria;

public class DashBody : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.VelocityChange);
    }
}
```

# RigidbodyConstraints
summary: Flags that freeze a Rigidbody's movement or rotation on chosen axes.
see: Rigidbody

Combine flags with `|` and assign them to [[Rigidbody.constraints]]. They set the same switches as the `freezePosition` and `freezeRotation` properties.

## Members
None: Nothing frozen.
FreezePositionX: No movement along X.
FreezePositionY: No movement along Y.
FreezePositionZ: No movement along Z.
FreezeRotationX: No turning about X.
FreezeRotationY: No turning about Y.
FreezeRotationZ: No turning about Z.
FreezePosition: No movement at all.
FreezeRotation: No turning at all.
FreezeAll: Neither movement nor turning.

## Example: A 2.5D side-scroller body
file: Scripts/SideScroller.cs
```csharp
using Elintria;

public class SideScroller : ElintriaScript
{
    public override void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }
}
```

## Example: An upright character body
file: Scripts/UprightBody.cs
```csharp
using Elintria;

public class UprightBody : ElintriaScript
{
    public override void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;   // may still turn about Y
    }
}
```

## Example: Freezing and unfreezing
file: Scripts/Freezer.cs
```csharp
using Elintria;

public class Freezer : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F)) return;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = rb.constraints == RigidbodyConstraints.FreezeAll ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
    }
}
```

# RigidbodyInterpolation
summary: Rigidbody smoothing between physics steps; accepted for Unity code but not used yet.
see: Rigidbody

## Members
None: No smoothing.
Interpolate: Smooth using the previous step (accepted, not applied yet).
Extrapolate: Smooth by predicting the next step (accepted, not applied yet).

## Example: Setting it for Unity compatibility
file: Scripts/SmoothBody.cs
```csharp
using Elintria;

public class SmoothBody : ElintriaScript
{
    public override void Start() { GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate; }
}
```

## Example: Reading the setting
file: Scripts/InterpReport.cs
```csharp
using Elintria;

public class InterpReport : ElintriaScript
{
    public override void Start() { Debug.Log(name + " interpolation: " + GetComponent<Rigidbody>().interpolation); }
}
```

## Example: Turning it off for a teleport
file: Scripts/Teleporter.cs
```csharp
using Elintria;

public class Teleporter : ElintriaScript
{
    public void TeleportBody(Rigidbody rb, Vector3 to)
    {
        RigidbodyInterpolation old = rb.interpolation;
        rb.interpolation = RigidbodyInterpolation.None;
        rb.position = to;
        rb.interpolation = old;
    }
}
```

# CollisionDetectionMode
summary: The Rigidbody collision detection mode; accepted for Unity code, the solver uses discrete checks.
see: Rigidbody

## Members
ContinuousSpeculative: Speculative continuous detection (recorded, not acted on).

## Example: Setting it on a fast body
file: Scripts/FastBody.cs
For really fast objects, sweep them yourself with [[Physics.SphereCast]].
```csharp
using Elintria;

public class FastBody : ElintriaScript
{
    public override void Start() { GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative; }
}
```

## Example: Reading it back
file: Scripts/ModeReport.cs
```csharp
using Elintria;

public class ModeReport : ElintriaScript
{
    public override void Start() { Debug.Log(GetComponent<Rigidbody>().collisionDetectionMode); }
}
```

## Example: Converted Unity code
file: Scripts/ConvertedBullet.cs
```csharp
using Elintria;

public class ConvertedBullet : ElintriaScript
{
    public override void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;   // compiles; the solver stays discrete
        rb.velocity = transform.forward * 60f;
    }
}
```

# QueryTriggerInteraction
summary: Whether physics queries should hit triggers; accepted for Unity code.
see: Physics

## Members
UseGlobal: Use the project's default.
Ignore: Leave triggers out.
Collide: Include triggers.

## Example: Passing it to a raycast
file: Scripts/IgnoreTriggers.cs
```csharp
using Elintria;

public class IgnoreTriggers : ElintriaScript
{
    public override void Update()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 50f, -1, QueryTriggerInteraction.Ignore);
    }
}
```

## Example: With an overlap
file: Scripts/OverlapTriggers.cs
```csharp
using Elintria;

public class OverlapTriggers : ElintriaScript
{
    public override void Start()
    {
        Collider[] all = Physics.OverlapSphere(transform.position, 5f, -1, QueryTriggerInteraction.Collide);
        Debug.Log(all.Length + " colliders nearby");
    }
}
```

## Example: Skipping triggers yourself
file: Scripts/SolidOnly.cs
Because the setting is not applied yet, filter on `isTrigger` when it matters.
```csharp
using Elintria;

public class SolidOnly : ElintriaScript
{
    public override void Start()
    {
        int solid = 0;
        foreach (Collider c in Physics.OverlapSphere(transform.position, 5f))
            if (!c.isTrigger) solid++;
        Debug.Log(solid + " solid colliders nearby");
    }
}
```

# LayerMask
summary: A bit mask over named layers; carried for Unity code, as the engine does not filter by layer yet.
see: Physics, GameObject

## Members
value: The mask as an int, one bit per layer.
NameToLayer: The layer number with this name.
LayerToName: The name of a layer number.
@layer: A layer number.
GetMask: A mask with the bits of the named layers set.
@names: Layer names.

## Example: Building a mask
file: Scripts/MaskBuilder.cs
```csharp
using Elintria;

public class MaskBuilder : ElintriaScript
{
    public override void Start()
    {
        int mask = LayerMask.GetMask("Default", "Enemies");
        Debug.Log("Mask value " + mask);
    }
}
```

## Example: Passing a mask to Raycast
file: Scripts/MaskedRay.cs
```csharp
using Elintria;

public class MaskedRay : ElintriaScript
{
    public LayerMask hitMask = -1;   // everything

    public override void Update()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 30f, hitMask);
    }
}
```

## Example: Testing a layer bit
file: Scripts/LayerTest.cs
```csharp
using Elintria;

public class LayerTest : ElintriaScript
{
    public static bool InMask(GameObject g, LayerMask m) => (m.value & (1 << g.layer)) != 0;
}
```
