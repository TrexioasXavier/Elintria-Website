# ConvexCollider
summary: A convex shape that turns with its object - the hull of its model, or a box, cylinder, capsule, cone or sphere. On a Rigidbody it tumbles, rolls and comes to rest on a face.
see: ConvexShape, CompoundCollider, Rigidbody, Collider

A [[BoxCollider]] stays lined up with the world. A `ConvexCollider` turns with its object, so a crate knocked over lands on its side and a barrel rolls. Use `ConvexShape.ModelHull` to wrap the object's own model (or the `.obj` in `meshPath`) in a hull of at most `maxVertices` points.

Rays, sphere casts, overlaps and character controllers all hit it, and it works as a trigger.

## Members
shape: Which shape it is: the model's hull, or a box, cylinder, capsule, cone or sphere.
meshPath: An `.obj` to wrap when the shape is ModelHull. Empty: the object's own model.
center: The shape's middle, in the object's space.
size: The shape's size before the object's scale: a box's sides; a cylinder's, capsule's or cone's width, height and depth; a sphere's diameter.
maxVertices: The most points a model's hull keeps (8 or more). Fewer is faster.
friction: How much it grips what it rests on: 0 slides like ice, 1 holds on a steep slope.
bounciness: How much it bounces: 0 not at all, 1 as high as it fell.
convex: Always true (Unity's flag).

## Example: A crate that tips over
file: Scripts/TippingCrate.cs
```csharp
using Elintria;

public class TippingCrate : ElintriaScript
{
    public override void Start()
    {
        ConvexCollider box = gameObject.AddComponent<ConvexCollider>();
        box.shape = ConvexShape.Box;
        box.size = Vector3.one;
        gameObject.AddComponent<Rigidbody>().mass = 5f;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            GetComponent<Rigidbody>().AddForce(new Vector3(0f, 1f, 4f), ForceMode.Impulse);
    }
}
```

## Example: A rolling barrel
file: Scripts/Barrel.cs
The cylinder lies on its side, so it rolls downhill.
```csharp
using Elintria;

public class Barrel : ElintriaScript
{
    public override void Start()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        ConvexCollider c = gameObject.AddComponent<ConvexCollider>();
        c.shape = ConvexShape.Cylinder;
        c.size = new Vector3(1f, 1.4f, 1f);
        c.friction = 0.8f;
        gameObject.AddComponent<Rigidbody>().mass = 20f;
    }
}
```

## Example: An icy ramp
file: Scripts/IceRamp.cs
A static convex shape (no Rigidbody) is the ground things slide on.
```csharp
using Elintria;

public class IceRamp : ElintriaScript
{
    public override void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 15f);
        ConvexCollider ramp = gameObject.AddComponent<ConvexCollider>();
        ramp.shape = ConvexShape.Box;
        ramp.size = new Vector3(8f, 0.3f, 3f);
        ramp.friction = 0.02f;
    }
}
```

# ConvexShape
summary: The shapes a ConvexCollider can be.
see: ConvexCollider

## Members
ModelHull: The convex hull of the object's model (or of ConvexCollider.meshPath).
Box: A box of ConvexCollider.size.
Cylinder: An upright cylinder, as wide as size.x and as tall as size.y.
Capsule: An upright capsule, as wide as size.x and as tall as size.y.
Cone: An upright cone with its point at the top.
Sphere: A sphere as wide as size.x.

## Example: Pick a shape from a field
file: Scripts/ShapedProp.cs
```csharp
using Elintria;

public class ShapedProp : ElintriaScript
{
    public int shapeIndex = 1;      // 1 box, 2 cylinder, 3 capsule, 4 cone, 5 sphere

    public override void Start()
    {
        ConvexCollider c = gameObject.AddComponent<ConvexCollider>();
        c.shape = (ConvexShape)shapeIndex;
        gameObject.AddComponent<Rigidbody>();
    }
}
```

## Example: Wrap a model
file: Scripts/RockHull.cs
```csharp
using Elintria;

public class RockHull : ElintriaScript
{
    public override void Start()
    {
        ConvexCollider c = gameObject.AddComponent<ConvexCollider>();
        c.shape = ConvexShape.ModelHull;
        c.meshPath = "Assets/Models/Rock.obj";
        c.maxVertices = 32;
    }
}
```

## Example: Rolling balls
file: Scripts/BallDropper.cs
```csharp
using Elintria;

public class BallDropper : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.B)) return;
        GameObject ball = GameObject.Create("Ball");
        ball.transform.position = transform.position;
        ConvexCollider c = ball.AddComponent<ConvexCollider>();
        c.shape = ConvexShape.Sphere;
        c.size = new Vector3(0.5f, 0.5f, 0.5f);
        c.bounciness = 0.6f;
        ball.AddComponent<Rigidbody>();
    }
}
```

# CompoundCollider
summary: One body made of several shapes - the Box, Sphere and Convex Colliders on its object and its children - that moves and turns as one.
see: ConvexCollider, BoxCollider, SphereCollider, Rigidbody

Put a `CompoundCollider` and a [[Rigidbody]] on a parent, and colliders on its children (a table's top and legs, a chair, an L-shaped beam). When play starts it takes those shapes in, so the whole thing falls, tips and rests together. When play stops they are given back.

## Members
includeInactive: Also take in the colliders of inactive children.
friction: How much the whole body grips what it rests on.
bounciness: How much the whole body bounces.
partCount: How many shapes it took in (once play has run a step).

## Example: A table that falls on its legs
file: Scripts/TableBuilder.cs
```csharp
using Elintria;

public class TableBuilder : ElintriaScript
{
    public override void Start()
    {
        Part("Top", new Vector3(0f, 0f, 0f), new Vector3(2f, 0.15f, 1.2f));
        Part("Leg 1", new Vector3(-0.9f, -0.5f, -0.5f), new Vector3(0.12f, 1f, 0.12f));
        Part("Leg 2", new Vector3(0.9f, -0.5f, -0.5f), new Vector3(0.12f, 1f, 0.12f));
        Part("Leg 3", new Vector3(-0.9f, -0.5f, 0.5f), new Vector3(0.12f, 1f, 0.12f));
        Part("Leg 4", new Vector3(0.9f, -0.5f, 0.5f), new Vector3(0.12f, 1f, 0.12f));
        gameObject.AddComponent<Rigidbody>().mass = 15f;
        gameObject.AddComponent<CompoundCollider>();
    }

    void Part(string name, Vector3 offset, Vector3 size)
    {
        GameObject p = GameObject.Create(name);
        p.transform.position = transform.position + offset;
        p.transform.parent = transform;
        p.AddComponent<BoxCollider>().size = size;
    }
}
```

## Example: How many parts it took
file: Scripts/CompoundReport.cs
```csharp
using Elintria;

public class CompoundReport : ElintriaScript
{
    float t;

    public override void Update()
    {
        t += Time.deltaTime;
        if (t < 0.5f) return;
        CompoundCollider c = GetComponent<CompoundCollider>();
        Debug.Log(gameObject.name + " is " + c.partCount + " shapes");
        enabled = false;
    }
}
```

## Example: A slippery sled
file: Scripts/Sled.cs
```csharp
using Elintria;

public class Sled : ElintriaScript
{
    public override void Start()
    {
        CompoundCollider c = GetComponent<CompoundCollider>();
        if (c != null) { c.friction = 0.05f; c.bounciness = 0f; }
    }
}
```

# Joint
summary: What every joint shares: the body on the other side, where it holds, and how much it can take before it breaks.
see: FixedJoint, HingeJoint, BallJoint, SpringJoint, SliderJoint

A joint goes on an object with a [[Rigidbody]]. It holds that body to `connectedBody`, or to the world when that is empty. `anchor` is where it holds, in the object's own space; the other side's anchor is found from where things are when play starts, unless `autoConfigureConnectedAnchor` is off.

## Members
connectedBody: The body on the other side. Null: the joint holds to the world.
connectedObject: The object on the other side (it need not have a Rigidbody: then it is the world, where that object is).
anchor: Where the joint holds, in this object's space.
connectedAnchor: Where it holds on the other side, when autoConfigureConnectedAnchor is off.
autoConfigureConnectedAnchor: Work out the other side's anchor from where things are when play starts.
breakForce: Newtons that break it. Infinity: it never breaks.
breakTorque: Newton-metres of twist that break it. Infinity: never.
enableCollision: Whether the two bodies still collide with each other.
broken: Whether it has broken. A broken joint holds nothing; it mends when play starts again.
currentForce: Newtons it held on the last physics step.
currentTorque: Newton-metres it held on the last physics step.
worldAnchor: The anchor on this side, in the world, now.
connectedWorldAnchor: The anchor on the other side, in the world, now.
Break: Breaks it now.

## Example: A sign that breaks off when hit
file: Scripts/BreakableSign.cs
```csharp
using Elintria;

public class BreakableSign : ElintriaScript
{
    public override void Start()
    {
        FixedJoint j = gameObject.AddComponent<FixedJoint>();
        j.breakForce = 300f;
    }

    public override void Update()
    {
        FixedJoint j = GetComponent<FixedJoint>();
        if (j != null && j.broken) Debug.Log("The sign came off");
    }
}
```

## Example: Strain gauge
file: Scripts/StrainGauge.cs
```csharp
using Elintria;

public class StrainGauge : ElintriaScript
{
    public override void Update()
    {
        HingeJoint j = GetComponent<HingeJoint>();
        if (j == null) return;
        if (j.currentForce > 0.8f * j.breakForce) Debug.Log("Creaking...");
    }
}
```

## Example: Cut it loose
file: Scripts/CutLoose.cs
```csharp
using Elintria;

public class CutLoose : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.X)) return;
        SpringJoint j = GetComponent<SpringJoint>();
        if (j != null) j.Break();
    }
}
```

# FixedJoint
summary: Holds two bodies so they move as one, or pins a body where it is - until it breaks.
see: Joint, HingeJoint

## Example: Glue two crates
file: Scripts/GlueCrates.cs
```csharp
using Elintria;

public class GlueCrates : ElintriaScript
{
    public GameObject other;

    public override void Start()
    {
        if (other == null) return;
        FixedJoint j = gameObject.AddComponent<FixedJoint>();
        j.connectedBody = other.GetComponent<Rigidbody>();
    }
}
```

## Example: A weak shelf
file: Scripts/WeakShelf.cs
Pinned to the wall until too much is piled on it.
```csharp
using Elintria;

public class WeakShelf : ElintriaScript
{
    public override void Start()
    {
        gameObject.AddComponent<FixedJoint>().breakForce = 400f;
    }
}
```

## Example: Pick up by gluing
file: Scripts/GlueGrab.cs
```csharp
using Elintria;

public class GlueGrab : ElintriaScript
{
    public GameObject hand;
    FixedJoint held;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && held == null && hand != null)
        {
            held = gameObject.AddComponent<FixedJoint>();
            held.connectedBody = hand.GetComponent<Rigidbody>();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && held != null)
        {
            held.Break();
            held = null;
        }
    }
}
```

# HingeJoint
summary: Turns about one axis only - doors, wheels, flaps, levers - with optional limits, a motor and a spring.
see: Joint, JointLimits, JointMotor, JointSpring

## Members
axis: The axis it turns about, in this object's space.
useLimits: Whether limits stops it turning past them.
limits: The degrees it may turn, from where it started.
useMotor: Whether motor drives it.
motor: The speed it is driven at and the force it can drive with.
useSpring: Whether spring pulls it toward an angle.
spring: How hard it is pulled toward targetPosition (degrees), and the damping.
angle: Degrees it has turned since play started.
velocity: Degrees a second it is turning.

## Example: A swinging door
file: Scripts/SwingDoor.cs
```csharp
using Elintria;

public class SwingDoor : ElintriaScript
{
    public override void Start()
    {
        gameObject.AddComponent<Rigidbody>().useGravity = false;
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.anchor = new Vector3(-0.5f, 0f, 0f);       // the edge with the hinges
        h.axis = Vector3.up;
        h.useLimits = true;
        h.limits = new JointLimits(-100f, 100f);
    }
}
```

## Example: A motor-driven wheel
file: Scripts/MotorWheel.cs
```csharp
using Elintria;

public class MotorWheel : ElintriaScript
{
    public float speed = 180f;

    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.axis = Vector3.right;
        h.useMotor = true;
        h.motor = new JointMotor { targetVelocity = speed, force = 200f };
    }
}
```

## Example: A self-closing gate
file: Scripts/SpringGate.cs
```csharp
using Elintria;

public class SpringGate : ElintriaScript
{
    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.axis = Vector3.up;
        h.useSpring = true;
        h.spring = new JointSpring { spring = 40f, damper = 6f, targetPosition = 0f };
    }

    public override void Update()
    {
        if (Mathf.Abs(GetComponent<HingeJoint>().angle) > 60f) Debug.Log("Gate wide open");
    }
}
```

# BallJoint
summary: Turns every way about one point - a pendulum, a ragdoll's shoulder, a hanging lamp - with a cone to limit the swing.
see: Joint, HingeJoint

## Members
axis: The middle of the swing cone, in this object's space.
swingLimit: Degrees it may swing away from the axis. 0: freely.
swing: Degrees it is swung away from the axis now.

## Example: A hanging lamp
file: Scripts/HangingLamp.cs
```csharp
using Elintria;

public class HangingLamp : ElintriaScript
{
    public override void Start()
    {
        gameObject.AddComponent<Rigidbody>().mass = 2f;
        BallJoint b = gameObject.AddComponent<BallJoint>();
        b.anchor = new Vector3(0f, 1.5f, 0f);     // the ceiling hook, above it
    }
}
```

## Example: A limited shoulder
file: Scripts/Shoulder.cs
```csharp
using Elintria;

public class Shoulder : ElintriaScript
{
    public GameObject torso;

    public override void Start()
    {
        BallJoint b = gameObject.AddComponent<BallJoint>();
        if (torso != null) b.connectedBody = torso.GetComponent<Rigidbody>();
        b.axis = Vector3.down;
        b.swingLimit = 70f;
    }
}
```

## Example: Swing meter
file: Scripts/SwingMeter.cs
```csharp
using Elintria;

public class SwingMeter : ElintriaScript
{
    public override void Update()
    {
        BallJoint b = GetComponent<BallJoint>();
        if (b != null && b.swing > 45f) Debug.Log("Big swing: " + b.swing.ToString("0") + " degrees");
    }
}
```

# SpringJoint
summary: Pulls a body back toward a rest length - a bungee, a bouncy platform, a car's suspension. With maxDistance it is a rope that cannot stretch further.
see: Joint, Rope

## Members
spring: How hard it pulls back, in newtons a metre.
damper: How much it slows the bounce.
restLength: The length it pulls toward, in metres. Negative: the distance when play started.
maxDistance: The longest it can get; 0: no limit.
distance: How long it is now.

## Example: A bouncy platform
file: Scripts/BouncyPlatform.cs
```csharp
using Elintria;

public class BouncyPlatform : ElintriaScript
{
    public override void Start()
    {
        gameObject.AddComponent<Rigidbody>().mass = 10f;
        SpringJoint s = gameObject.AddComponent<SpringJoint>();
        s.anchor = new Vector3(0f, -1f, 0f);
        s.spring = 400f;
        s.damper = 10f;
        s.restLength = 1f;
    }
}
```

## Example: A bungee
file: Scripts/Bungee.cs
```csharp
using Elintria;

public class Bungee : ElintriaScript
{
    public GameObject jumper;

    public override void Start()
    {
        if (jumper == null) return;
        SpringJoint s = jumper.AddComponent<SpringJoint>();
        s.connectedObject = gameObject;
        s.spring = 60f;
        s.damper = 2f;
        s.restLength = 8f;
        s.maxDistance = 14f;
    }
}
```

## Example: Show the stretch
file: Scripts/StretchReadout.cs
```csharp
using Elintria;

public class StretchReadout : ElintriaScript
{
    public override void Update()
    {
        SpringJoint s = GetComponent<SpringJoint>();
        if (s != null) Debug.Log("Stretched to " + s.distance.ToString("0.00") + " m");
    }
}
```

# SliderJoint
summary: Slides along one axis without turning - pistons, drawers, lifts - between limits, with an optional motor.
see: Joint

## Members
axis: The axis it slides along, in this object's space.
useLimits: Whether minDistance and maxDistance stop it.
minDistance: The furthest back it may slide from where it started, in metres.
maxDistance: The furthest forward it may slide, in metres.
useMotor: Whether a motor drives it.
motorSpeed: Metres a second the motor drives it at.
motorForce: Newtons the motor can push with.
position: Metres from where it started.

## Example: A drawer
file: Scripts/Drawer.cs
```csharp
using Elintria;

public class Drawer : ElintriaScript
{
    public override void Start()
    {
        gameObject.AddComponent<Rigidbody>().useGravity = false;
        SliderJoint s = gameObject.AddComponent<SliderJoint>();
        s.axis = Vector3.forward;
        s.useLimits = true;
        s.minDistance = 0f;
        s.maxDistance = 0.6f;
    }
}
```

## Example: A piston
file: Scripts/Piston.cs
```csharp
using Elintria;

public class Piston : ElintriaScript
{
    float t;

    public override void Start()
    {
        SliderJoint s = gameObject.AddComponent<SliderJoint>();
        s.axis = Vector3.up;
        s.minDistance = 0f;
        s.maxDistance = 2f;
        s.useMotor = true;
        s.motorForce = 500f;
    }

    public override void Update()
    {
        t += Time.deltaTime;
        GetComponent<SliderJoint>().motorSpeed = Mathf.Sin(t * 2f) > 0f ? 1.5f : -1.5f;
    }
}
```

## Example: A lift that reports its floor
file: Scripts/LiftFloor.cs
```csharp
using Elintria;

public class LiftFloor : ElintriaScript
{
    public float floorHeight = 4f;

    public override void Update()
    {
        SliderJoint s = GetComponent<SliderJoint>();
        if (s != null) Debug.Log("Floor " + Mathf.RoundToInt(s.position / floorHeight));
    }
}
```

# JointLimits
summary: The degrees a HingeJoint may turn, from where it started.
see: HingeJoint

## Members
min: The lowest angle, in degrees.
max: The highest angle, in degrees.
JointLimits: Limits from `min` to `max` degrees.
@min: The lowest angle, in degrees.
@max: The highest angle, in degrees.

## Example: A door that only opens one way
file: Scripts/OneWayDoor.cs
```csharp
using Elintria;

public class OneWayDoor : ElintriaScript
{
    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.useLimits = true;
        h.limits = new JointLimits(0f, 110f);
    }
}
```

## Example: A lever with two stops
file: Scripts/LeverStops.cs
```csharp
using Elintria;

public class LeverStops : ElintriaScript
{
    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.axis = Vector3.right;
        h.useLimits = true;
        h.limits = new JointLimits(-45f, 45f);
    }

    public override void Update()
    {
        HingeJoint h = GetComponent<HingeJoint>();
        if (h.angle > h.limits.max - 2f) Debug.Log("Lever up");
    }
}
```

## Example: Widen the limits
file: Scripts/LooseHinge.cs
```csharp
using Elintria;

public class LooseHinge : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.L)) return;
        HingeJoint h = GetComponent<HingeJoint>();
        JointLimits l = h.limits;
        l.min -= 20f;
        l.max += 20f;
        h.limits = l;
    }
}
```

# JointMotor
summary: How a HingeJoint's motor drives it: the speed it aims for and the force it can use.
see: HingeJoint

## Members
targetVelocity: Degrees a second the motor aims for.
force: Newton-metres it can push with to get there.
freeSpin: Unity's flag (kept for code that sets it).

## Example: A slow fan
file: Scripts/SlowFan.cs
```csharp
using Elintria;

public class SlowFan : ElintriaScript
{
    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.axis = Vector3.forward;
        h.useMotor = true;
        h.motor = new JointMotor { targetVelocity = 90f, force = 20f };
    }
}
```

## Example: Reverse the motor
file: Scripts/ReverseMotor.cs
```csharp
using Elintria;

public class ReverseMotor : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.R)) return;
        HingeJoint h = GetComponent<HingeJoint>();
        JointMotor m = h.motor;
        m.targetVelocity = -m.targetVelocity;
        h.motor = m;
    }
}
```

## Example: A winch too weak for its load
file: Scripts/WeakWinch.cs
```csharp
using Elintria;

public class WeakWinch : ElintriaScript
{
    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.useMotor = true;
        h.motor = new JointMotor { targetVelocity = 60f, force = 2f };   // stalls under weight
    }
}
```

# JointSpring
summary: How a HingeJoint's spring pulls it toward an angle.
see: HingeJoint

## Members
spring: How hard it pulls, in newton-metres a radian.
damper: How much it slows the swing.
targetPosition: The angle it pulls toward, in degrees.

## Example: A springy flap
file: Scripts/SpringFlap.cs
```csharp
using Elintria;

public class SpringFlap : ElintriaScript
{
    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.axis = Vector3.right;
        h.useSpring = true;
        h.spring = new JointSpring { spring = 15f, damper = 1f, targetPosition = 0f };
    }
}
```

## Example: Hold a lever at an angle
file: Scripts/HoldLever.cs
```csharp
using Elintria;

public class HoldLever : ElintriaScript
{
    public float hold = 30f;

    public override void Start()
    {
        HingeJoint h = gameObject.AddComponent<HingeJoint>();
        h.useSpring = true;
        h.spring = new JointSpring { spring = 80f, damper = 8f, targetPosition = hold };
    }
}
```

## Example: Stiffen when pressed
file: Scripts/StiffenSpring.cs
```csharp
using Elintria;

public class StiffenSpring : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Alpha1)) return;
        HingeJoint h = GetComponent<HingeJoint>();
        JointSpring s = h.spring;
        s.spring *= 2f;
        h.spring = s;
    }
}
```

# Rope
summary: A rope from its object to End Object that sags, swings and cannot stretch. A body on either end is pulled by it - a wrecking ball, a tow line, a hanging bridge's cable.
see: SpringJoint, Joint

The rope is drawn as a tube. With `length` 0 it is as long as the gap when play starts plus `slack`. With `collide` on, it drapes over things. It breaks when `tension` passes `breakForce`.

## Members
endObject: The other end. One with a Rigidbody is pulled; empty: the end hangs free.
startOffset: Where it starts, in this object's space.
endOffset: Where it ends, in End Object's space.
length: Metres. 0: the gap when play starts, plus slack.
slack: The share longer than the gap, when length is 0.
segments: How many pieces it is made of: more bends more smoothly.
thickness: Its width, drawn and for collisions.
mass: Kilograms, all of it.
stiffness: 0..1: how firmly the pieces keep their length.
iterations: Solver passes a step: more is stiffer.
damping: 0..0.5: how quickly its swing dies down.
attachStart: Whether the start holds to this object.
attachEnd: Whether the end holds to End Object.
collide: Whether it drapes over colliders.
breakForce: Newtons of tension that snap it. 0: never.
broken: Whether it has snapped.
tension: Newtons pulling on its end on the last step.
pointCount: How many points it has (segments + 1).
GetPoint: A point along it, in the world, from 0 (start) to pointCount - 1 (end).
DetachStart: Lets the start go.
DetachEnd: Lets the end go.

## Example: A wrecking ball
file: Scripts/WreckingBall.cs
```csharp
using Elintria;

public class WreckingBall : ElintriaScript
{
    public GameObject ball;

    public override void Start()
    {
        if (ball == null) return;
        Rope r = gameObject.AddComponent<Rope>();
        r.endObject = ball;
        r.segments = 20;
        r.thickness = 0.08f;
    }
}
```

## Example: Cut the rope
file: Scripts/RopeCutter.cs
```csharp
using Elintria;

public class RopeCutter : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) GetComponent<Rope>().DetachEnd();
    }
}
```

## Example: Snap under load
file: Scripts/SnapRope.cs
```csharp
using Elintria;

public class SnapRope : ElintriaScript
{
    public override void Start()
    {
        Rope r = GetComponent<Rope>();
        r.breakForce = 800f;
    }

    public override void Update()
    {
        Rope r = GetComponent<Rope>();
        if (r.broken) Debug.Log("Snap!");
        else if (r.tension > 600f) Debug.Log("The rope strains (" + r.tension.ToString("0") + " N)");
    }
}
```

# SoftBody
summary: Makes its object's shape squashy - jelly, a balloon, a cushion. It falls, squashes, springs back, and holds up what lands on it.
see: SoftBodyShape, Rigidbody

A `SoftBody` does not need a [[Rigidbody]]: it moves itself and moves its object. Rigidbodies land on it and press it in; characters sink into it a little; rays hit its surface. `shapeMatching` is how hard it springs back to its shape.

## Members
shape: Its own model, a box or a sphere.
meshPath: An `.obj` to use when the shape is Model. Empty: the object's own model.
size: The box's sides or the sphere's diameter, before the object's scale.
resolution: How finely a box or sphere is divided: more points bend more smoothly.
mass: Kilograms, all of it.
stiffness: 0..1: how firmly its edges keep their length.
shapeMatching: 0..1: how hard it springs back to its shape (low: floppy, high: rubbery).
damping: 0..0.5: the share of its speed lost each 60th of a second.
friction: 0..1: how much it grips what it lies on.
bounciness: 0..1: how much it bounces.
iterations: Solver passes a step.
useGravity: Whether gravity pulls it.
particleRadius: How thick its skin is for collisions.
AddForce: Pushes the whole body, spread over a step.
AddImpulse: Pushes the whole body at once.
@impulse: The push, in world space (newton-seconds).
AddImpulseAt: Pushes the points near `point` (within `radius`, the nearest most), denting it.
@impulse: The push, in world space.
@point: Where it lands, in the world.
@radius: How far round `point` the points are pushed.
centre: Its middle, in the world.
velocity: How fast its middle is moving.
volume: 1 at its rest volume; less when squashed.
pointCount: How many points it is made of.
GetPoint: A point of it, in the world.

## Example: A jelly cube
file: Scripts/JellyCube.cs
```csharp
using Elintria;

public class JellyCube : ElintriaScript
{
    public override void Start()
    {
        SoftBody s = gameObject.AddComponent<SoftBody>();
        s.shape = SoftBodyShape.Box;
        s.resolution = 4;
        s.shapeMatching = 0.35f;
        s.bounciness = 0.4f;
    }
}
```

## Example: Poke it
file: Scripts/PokeSoft.cs
```csharp
using Elintria;

public class PokeSoft : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.P)) return;
        SoftBody s = GetComponent<SoftBody>();
        Vector3 top = s.centre + Vector3.up * 0.5f;
        s.AddImpulseAt(Vector3.down * 3f, top, 0.4f);
    }
}
```

## Example: A cushion that reports being sat on
file: Scripts/Cushion.cs
```csharp
using Elintria;

public class Cushion : ElintriaScript
{
    public override void Update()
    {
        SoftBody s = GetComponent<SoftBody>();
        if (s != null && s.volume < 0.85f) Debug.Log("Someone is sitting on " + gameObject.name);
    }
}
```

# SoftBodyShape
summary: The shapes a SoftBody can take.
see: SoftBody

## Members
Model: The object's own model (or SoftBody.meshPath).
Box: A box of SoftBody.size, divided by resolution.
Sphere: A sphere as wide as SoftBody.size.x.

## Example: A soft ball
file: Scripts/SoftBall.cs
```csharp
using Elintria;

public class SoftBall : ElintriaScript
{
    public override void Start()
    {
        SoftBody s = gameObject.AddComponent<SoftBody>();
        s.shape = SoftBodyShape.Sphere;
        s.size = new Vector3(1f, 1f, 1f);
        s.bounciness = 0.7f;
    }
}
```

## Example: A soft model
file: Scripts/SoftModel.cs
```csharp
using Elintria;

public class SoftModel : ElintriaScript
{
    public override void Start()
    {
        SoftBody s = gameObject.AddComponent<SoftBody>();
        s.shape = SoftBodyShape.Model;
        s.meshPath = "Assets/Models/Blob.obj";
    }
}
```

## Example: Switch by setting
file: Scripts/SoftShapePicker.cs
```csharp
using Elintria;

public class SoftShapePicker : ElintriaScript
{
    public bool round;

    public override void Start()
    {
        SoftBody s = gameObject.AddComponent<SoftBody>();
        s.shape = round ? SoftBodyShape.Sphere : SoftBodyShape.Box;
    }
}
```
