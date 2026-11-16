# Vector3
summary: A 3D vector: positions, directions, velocities and scales.
see: Vector2, Quaternion, Transform, Mathf

`Vector3` holds three floats, `x`, `y` and `z`. It is used for positions, directions, velocities and scales throughout the engine. Vectors add, subtract, scale by a number and divide by one with the usual operators. Static helpers do the common geometry: `Dot`, `Cross`, `Distance`, `Lerp`, `MoveTowards`, `Reflect`, `Project`, `SmoothDamp` and more.

Elintria is Y-up: `Vector3.up` is (0, 1, 0) and `Vector3.forward` is (0, 0, 1).

> note: `==` compares with a small tolerance, because two vectors that came from different sums almost never match exactly.

## Members
x: The X component (right).
y: The Y component (up).
z: The Z component (forward).
Vector3: Makes a vector from its three components.
zero: (0, 0, 0).
one: (1, 1, 1).
up: (0, 1, 0), world up.
down: (0, -1, 0).
right: (1, 0, 0).
left: (-1, 0, 0).
forward: (0, 0, 1).
back: (0, 0, -1).
magnitude: The vector's length.
sqrMagnitude: The length squared. Faster than `magnitude`; compare it with a squared distance.
normalized: The same direction with length 1 (zero stays zero). The vector itself is not changed.
Dot: The dot product: positive when `a` and `b` point the same way, 0 at right angles, negative when opposite. For unit vectors it is the cosine of the angle between them.
Cross: The cross product: a vector at right angles to both `a` and `b` (left-hand rule), whose length is the area of the parallelogram they span.
Distance: The distance between the points `a` and `b`.
Min: A vector of the smaller component of each pair.
Max: A vector of the larger component of each pair.
Normalize(Vector3 v): `v` with length 1.
SqrMagnitude: `v`'s length squared.
Magnitude: `v`'s length.
Set: Sets all three components at once.
Lerp: The point `t` of the way from `a` to `b` (0 gives `a`, 1 gives `b`); `t` is clamped to 0..1.
LerpUnclamped: Like Lerp, but `t` may go below 0 or above 1 to extrapolate.
Slerp: Interpolates between two directions along the arc between them, blending their lengths.
RotateTowards: Turns `current` towards `target` by at most `maxRadiansDelta` radians, and changes its length by at most `maxMagnitudeDelta`.
@maxRadiansDelta: The most it may turn, in radians.
@maxMagnitudeDelta: The most its length may change.
SmoothDamp: Eases `current` towards `target` like a critically damped spring, taking roughly `smoothTime` seconds. Keep `currentVelocity` in a field between calls.
OrthoNormalize: Makes `normal` unit length, and `tangent` unit length and at right angles to it.
@normal: Made unit length.
@tangent: Made unit length and perpendicular to normal.
Scale(Vector3 s): Multiplies this vector by `s`, component by component.
positiveInfinity: A vector of positive infinities, handy as the start of a "closest so far" search.
negativeInfinity: A vector of negative infinities.
@dir: The incoming direction.
@normal: The surface normal.
@onto: The direction to project onto.
@planeNormal: The normal of the plane.
@nx: The new x.
@ny: The new y.
@nz: The new z.

## Example: Moving towards a target at a steady speed
file: Scripts/Homing.cs
`MoveTowards` never overshoots, and `Distance` decides when it has arrived.
```csharp
using Elintria;

public class Homing : ElintriaScript
{
    public GameObject target;
    public float speed = 5f;

    public override void Update()
    {
        if (target == null) return;
        Vector3 goal = target.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, goal) < 0.05f)
            Debug.Log("Reached " + target.name);
    }
}
```

## Example: Is the player in front, and how far to the side?
file: Scripts/VisionCone.cs
`Dot` with a unit direction gives the cosine of the angle; `Angle` gives degrees directly. `sqrMagnitude` avoids a square root in the range test.
```csharp
using Elintria;

public class VisionCone : ElintriaScript
{
    public GameObject player;
    public float range = 15f;
    public float halfAngle = 40f;

    public bool CanSee()
    {
        if (player == null) return false;
        Vector3 to = player.transform.position - transform.position;
        if (to.sqrMagnitude > range * range) return false;
        return Vector3.Angle(transform.forward, to) < halfAngle;
    }

    public float SideOf()
    {
        // +1 when the player is on our right, -1 on our left.
        Vector3 to = (player.transform.position - transform.position).normalized;
        return Mathf.Sign(Vector3.Dot(transform.right, to));
    }

    public override void Update()
    {
        if (CanSee()) Debug.DrawLine(transform.position, player.transform.position, Color.red);
    }
}
```

## Example: Bouncing a ball off walls
file: Scripts/BounceBall.cs
`Reflect` mirrors the velocity about the wall's normal from a raycast.
```csharp
using Elintria;

public class BounceBall : ElintriaScript
{
    public Vector3 velocity = new Vector3(4f, 0f, 3f);
    public float radius = 0.5f;

    public override void Update()
    {
        Vector3 step = velocity * Time.deltaTime;
        if (Physics.Raycast(transform.position, velocity, out RaycastHit hit, step.magnitude + radius))
            velocity = Vector3.Reflect(velocity, hit.normal) * 0.9f;   // lose a little energy
        transform.position += velocity * Time.deltaTime;
    }
}
```

## Example: A smooth camera rig with SmoothDamp and Cross
file: Scripts/OrbitRig.cs
`SmoothDamp` keeps a velocity between frames for a soft follow. `Cross` builds a sideways axis from forward and up.
```csharp
using Elintria;

public class OrbitRig : ElintriaScript
{
    public GameObject target;
    public float distance = 8f;
    public float height = 3f;

    Vector3 vel;

    public override void LateUpdate()
    {
        if (target == null) return;
        Vector3 fwd = target.transform.forward;
        Vector3 side = Vector3.Cross(Vector3.up, fwd).normalized;   // the target's right
        Vector3 wanted = target.transform.position - fwd * distance + Vector3.up * height + side * 1.5f;
        transform.position = Vector3.SmoothDamp(transform.position, wanted, ref vel, 0.3f);
    }
}
```

# Vector2
summary: A 2D vector: screen positions, UI sizes, 2D movement and texture coordinates.
see: Vector3, Rect

`Vector2` holds `x` and `y`. It is used for mouse and screen positions, UI sizes, 2D gameplay and texture coordinates. It converts to and from [[Vector3]] implicitly: z is dropped, or set to 0.

## Members
x: The X component.
y: The Y component.
Vector2: Makes a vector from x and y.
zero: (0, 0).
one: (1, 1).
up: (0, 1).
down: (0, -1).
left: (-1, 0).
right: (1, 0).
positiveInfinity: Both components positive infinity.
negativeInfinity: Both components negative infinity.
magnitude: The vector's length.
sqrMagnitude: The length squared (faster).
normalized: The same direction with length 1.
Dot: The dot product of `a` and `b`.
Distance: The distance between two points.
Lerp: The point `t` (0..1) of the way from `a` to `b`.
LerpUnclamped: Like Lerp, allowing `t` outside 0..1.
Set: Sets both components.
Normalize: Makes this vector length 1.
Scale: Multiplies component by component.
Min: The smaller component of each pair.
Max: The larger component of each pair.
Perpendicular: `d` turned 90 degrees anticlockwise.
@d: The direction to turn.
Reflect: `d` bounced off a surface with normal `n`.
@d: The incoming direction.
@n: The surface normal.
Angle: The unsigned angle between `a` and `b`, in degrees.
SignedAngle: The angle from `a` to `b` in degrees, negative when clockwise.
ClampMagnitude: `v` shortened to at most `max` long.
MoveTowards: Moves `current` towards `target` by at most `maxDelta`.
SmoothDamp: Eases towards `target` like a spring, keeping `currentVelocity` between calls.

## Example: A 2D top-down mover
file: Scripts/TopDown2D.cs
Reads the keys into a `Vector2`, normalises so diagonals are not faster, and moves in the XY plane.
```csharp
using Elintria;

public class TopDown2D : ElintriaScript
{
    public float speed = 5f;

    public override void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisHorizontal(), Input.GetAxisVertical());
        if (input.sqrMagnitude > 1f) input = input.normalized;
        Vector2 step = input * speed * Time.deltaTime;
        transform.position += new Vector3(step.x, step.y, 0f);
    }
}
```

## Example: Clicking a button drawn on screen
file: Scripts/ClickTarget.cs
`Input.mousePosition` is a `Vector2`; `Rect.Contains` tests it.
```csharp
using Elintria;

public class ClickTarget : ElintriaScript
{
    Rect button = new Rect(40, 40, 160, 48);
    int clicks;

    public override void Update()
    {
        Vector2 m = Input.mousePosition;
        bool over = button.Contains(m);
        ScreenGUI.Fill(button, over ? Color.cyan : Color.gray);
        ScreenGUI.Label(button, "Clicks: " + clicks, null, Color.black, TextAnchor.MiddleCenter);
        if (over && Input.GetMouseButtonDown(0)) clicks++;
    }
}
```

## Example: Steering in 2D with angles
file: Scripts/Asteroids2D.cs
`SignedAngle` tells which way to turn; `Perpendicular` gives a side-thrust direction.
```csharp
using Elintria;

public class Asteroids2D : ElintriaScript
{
    public GameObject target;
    public Vector2 heading = Vector2.up;
    public float turnRate = 180f;

    public override void Update()
    {
        if (target == null) return;
        Vector2 to = (Vector2)(target.transform.position - transform.position);
        float turn = Mathf.Clamp(Vector2.SignedAngle(heading, to), -turnRate * Time.deltaTime, turnRate * Time.deltaTime);
        float r = turn * Mathf.Deg2Rad;
        heading = new Vector2(heading.x * Mathf.Cos(r) - heading.y * Mathf.Sin(r), heading.x * Mathf.Sin(r) + heading.y * Mathf.Cos(r));
        Vector2 strafe = Vector2.Perpendicular(heading) * Mathf.Sin(Time.time) * 0.5f;
        Vector3 step = (heading * 3f + strafe) * Time.deltaTime;
        transform.position += step;
    }
}
```

# Vector4
summary: A four-component vector, for shader values, colours as vectors and homogeneous coordinates.
see: Vector3, Matrix4x4, Color

`Vector4` holds `x`, `y`, `z` and `w`. It turns up where four numbers belong together: matrix rows and columns, colours treated as vectors, plane equations and Inspector fields with four values. It converts implicitly to [[Vector3]] (dropping `w`) and from it (with `w` = 0).

## Members
x: The first component.
y: The second component.
z: The third component.
w: The fourth component.
Vector4: Makes a vector from four components.
zero: (0, 0, 0, 0).
one: (1, 1, 1, 1).

## Example: Four tuning values in one Inspector field
file: Scripts/WeaponTuning.cs
```csharp
using Elintria;

public class WeaponTuning : ElintriaScript
{
    // x damage, y fire rate, z range, w spread
    public Vector4 stats = new Vector4(12f, 8f, 60f, 1.5f);

    public override void Start()
    {
        Debug.Log("Damage " + stats.x + ", rate " + stats.y + "/s, range " + stats.z + " m, spread " + stats.w + " deg");
    }
}
```

## Example: Transforming a point with a matrix
file: Scripts/MatrixPoint.cs
A position has `w` = 1 so the matrix's translation applies; a direction has `w` = 0.
```csharp
using Elintria;

public class MatrixPoint : ElintriaScript
{
    public override void Start()
    {
        Matrix4x4 m = transform.localToWorldMatrix;
        Vector4 p = m * new Vector4(0f, 1f, 0f, 1f);   // a point one unit above our origin
        Vector4 d = m * new Vector4(0f, 0f, 1f, 0f);   // our forward direction
        Debug.Log("World point " + (Vector3)p + ", world direction " + (Vector3)d);
    }
}
```

## Example: A colour as a vector
file: Scripts/ColorMath.cs
```csharp
using Elintria;

public class ColorMath : ElintriaScript
{
    public Vector4 tint = new Vector4(1f, 0.5f, 0.2f, 1f);

    public override void Start()
    {
        Color c = tint;                 // Vector4 -> Color
        Vector4 back = c * 0.5f;        // Color -> Vector4
        Debug.Log("Half bright: " + back);
    }
}
```

# Quaternion
summary: A rotation. Build one from Euler angles, an axis and angle, or a look direction; combine and blend them.
see: Transform, Vector3

`Quaternion` is how rotations are stored. Because it never suffers gimbal lock and blends smoothly, it is the right type for turning things. You rarely touch the four numbers: make one with `Euler`, `AngleAxis` or `LookRotation`, combine two with `*` (apply the right one first, then the left), rotate a vector with `q * v`, and blend with `Slerp` or `RotateTowards`.

Euler angles are in degrees, applied Z, then X, then Y, the same order the Inspector uses.

## Members
x: The x part of the quaternion. Don't set the parts by hand unless you know quaternion maths.
y: The y part.
z: The z part.
w: The w (scalar) part.
Quaternion: Makes a quaternion from its four raw parts.
identity: No rotation.
operator *: `a * b` combines two rotations (b first, then a). `q * v` rotates the vector `v`.
magnitude: The quaternion's length (1 for a valid rotation).
normalized: This rotation scaled to length 1.
conjugate: The conjugate; for a unit quaternion, the opposite rotation.
Inverse: The rotation that undoes `q`.
Dot: The dot product of two rotations; close to ±1 when they are nearly the same.
Euler: The rotation of `x` degrees about X, `y` about Y and `z` about Z.
@e: Euler angles in degrees.
LookRotation: A rotation that points +Z along `forward` with +Y as near to `up` as it can be.
Slerp: Blends from `a` to `b` along the shortest arc; `t` from 0 to 1.
Lerp: A cheaper blend from `a` to `b`, fine for small differences.
@maxDegrees: The most it may turn this call.

## Example: Turning smoothly towards a target
file: Scripts/SmoothTurret.cs
`LookRotation` gives the rotation that faces the target, and `RotateTowards` turns at a fixed speed.
```csharp
using Elintria;

public class SmoothTurret : ElintriaScript
{
    public GameObject target;
    public float degreesPerSecond = 90f;

    public override void Update()
    {
        if (target == null) return;
        Vector3 to = target.transform.position - transform.position;
        to.y = 0f;                                   // turn only left and right
        if (to.sqrMagnitude < 0.0001f) return;
        Quaternion wanted = Quaternion.LookRotation(to);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, wanted, degreesPerSecond * Time.deltaTime);
    }
}
```

## Example: Rotating vectors and combining rotations
file: Scripts/Shotgun.cs
`AngleAxis(angle, axis) * direction` turns a direction, which here spreads pellets in a fan.
```csharp
using Elintria;

public class Shotgun : ElintriaScript
{
    public int pellets = 7;
    public float spreadDegrees = 20f;

    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        for (int i = 0; i < pellets; i++)
        {
            float a = Mathf.Lerp(-spreadDegrees, spreadDegrees, i / (float)(pellets - 1));
            Vector3 dir = Quaternion.AngleAxis(a, transform.up) * transform.forward;
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 40f))
                Debug.DrawLine(transform.position, hit.point, Color.yellow, 0.5f);
        }
    }
}
```

## Example: Opening a door with Slerp
file: Scripts/SwingDoor.cs
Stores the closed rotation, builds the open one with `*` and `Euler`, and blends between them.
```csharp
using Elintria;

public class SwingDoor : ElintriaScript
{
    public float openAngle = 100f;
    public float speed = 2f;
    public bool open;

    Quaternion closedRot, openRot;
    float t;

    public override void Start()
    {
        closedRot = transform.rotation;
        openRot = closedRot * Quaternion.Euler(0f, openAngle, 0f);   // turn about the door's own up
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) open = !open;
        t = Mathf.MoveTowards(t, open ? 1f : 0f, speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(closedRot, openRot, Mathf.SmoothStep(0f, 1f, t));
    }
}
```

# Mathf
summary: Float maths helpers: clamping, interpolation, angles, rounding, smoothing and noise.
see: Vector3, Random

`Mathf` collects float-typed maths so scripts are not full of casts to and from `double`. It has the trigonometry (`Sin`, `Cos`, `Atan2`, all in radians), clamping and interpolation (`Clamp`, `Lerp`, `InverseLerp`, `SmoothStep`, `MoveTowards`), angle helpers that wrap correctly (`DeltaAngle`, `LerpAngle`), springs (`SmoothDamp`) and `PerlinNoise`.

## Members
PI: π, about 3.14159.
Infinity: Positive infinity.
NegativeInfinity: Negative infinity.
Epsilon: The smallest positive float.
Deg2Rad: Multiply degrees by this to get radians.
Rad2Deg: Multiply radians by this to get degrees.
Abs: The absolute value.
Sqrt: The square root.
Sin: The sine of an angle in radians.
Cos: The cosine of an angle in radians.
Tan: The tangent of an angle in radians.
Asin: The arc sine, in radians.
Acos: The arc cosine, in radians.
Atan: The arc tangent, in radians.
Atan2: The angle of the point (x, y) from the +x axis, in radians, correct in every quadrant.
Pow: `b` raised to the power `e`.
@e: The exponent.
Exp: e raised to the power `v`.
Log: The natural logarithm, or the logarithm in `newBase`.
@newBase: The base of the logarithm.
Log10: The base-10 logarithm.
SmoothDampAngle: Like SmoothDamp, but for angles in degrees: it takes the short way round.
Gamma: Applies a gamma curve to `value` within ±`absmax`.
@absmax: The range the value lies in.
@gamma: The gamma exponent.
ClosestPowerOfTwo: The power of two nearest `v`.
IsPowerOfTwo: True when `v` is a power of two.
NextPowerOfTwo: The smallest power of two at least `v`.
Floor: The largest whole number not above `v`.
Ceil: The smallest whole number not below `v`.
Round: `v` rounded to the nearest whole number.
FloorToInt: Floor, as an int.
CeilToInt: Ceil, as an int.
RoundToInt: Round, as an int.
Sign: 1 for zero or positive, -1 for negative.
Min: The smaller of two values.
Max: The larger of two values.
Clamp: `v` limited to the range `lo`..`hi`.
Clamp01: `v` limited to 0..1.
Lerp: The value `t` (clamped to 0..1) of the way from `a` to `b`.
LerpUnclamped: Like Lerp, with `t` unclamped.
InverseLerp: Where `v` lies between `a` and `b`, as 0..1. The opposite of Lerp.
MoveTowards: Moves `cur` towards `target` by at most `maxDelta`, never overshooting.
PingPong: A value that goes up from 0 to `length` and back as `t` grows.
MoveTowardsAngle: Like MoveTowards for angles in degrees, taking the short way round.
LerpAngle: Like Lerp for angles in degrees, taking the short way round.
Approximately: True when two floats are equal within a tiny tolerance. Use it instead of `==` on floats.
SmoothStep: Like Lerp from `from` to `to`, but easing in and out.

## Example: Health bar maths with Clamp and InverseLerp
file: Scripts/HealthBar.cs
```csharp
using Elintria;

public class HealthBar : ElintriaScript
{
    public float health = 75f;
    public float maxHealth = 100f;

    public override void Update()
    {
        health = Mathf.Clamp(health, 0f, maxHealth);
        float fill = Mathf.InverseLerp(0f, maxHealth, health);   // 0..1
        Color bar = Color.Lerp(Color.red, Color.green, fill);
        ScreenGUI.Bar(new Rect(20, 20, 200, 18), fill, bar, new Color(0f, 0f, 0f, 0.5f));
        ScreenGUI.Text(230, 20, Mathf.RoundToInt(health) + " / " + Mathf.RoundToInt(maxHealth));
    }
}
```

## Example: Waves, ping-pong and Perlin wobble
file: Scripts/Lantern.cs
`Sin` makes a smooth swing, `PingPong` a back-and-forth, and `PerlinNoise` a natural flicker.
```csharp
using Elintria;

public class Lantern : ElintriaScript
{
    public GameObject lamp;   // an object with a Light

    public override void Update()
    {
        float swing = Mathf.Sin(Time.time * 1.5f) * 15f;                  // -15..15 degrees
        transform.localEulerAngles = new Vector3(0f, 0f, swing);

        float glide = Mathf.PingPong(Time.time * 0.5f, 2f) - 1f;          // -1..1 linear
        transform.localPosition = new Vector3(glide * 0.2f, transform.localPosition.y, 0f);

        Light l = lamp != null ? lamp.GetComponent<Light>() : null;
        if (l != null) l.intensity = Mathf.Lerp(1.5f, 2.5f, Mathf.PerlinNoise(Time.time * 4f, 0.3f));
    }
}
```

## Example: Angles that wrap round
file: Scripts/CompassNeedle.cs
`Atan2` turns a direction into an angle, and `DeltaAngle` and `MoveTowardsAngle` handle the jump from 359 to 0 degrees.
```csharp
using Elintria;

public class CompassNeedle : ElintriaScript
{
    public GameObject north;   // a marker far to the north
    float yaw;

    public override void Update()
    {
        if (north == null) return;
        Vector3 to = north.transform.position - transform.position;
        float wanted = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg;          // degrees about +y
        yaw = Mathf.MoveTowardsAngle(yaw, wanted, 180f * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        if (Mathf.Abs(Mathf.DeltaAngle(yaw, wanted)) < 1f) Debug.Log("Pointing north");
    }
}
```

## Example: A springy number with SmoothDamp
file: Scripts/ZoomCamera.cs
```csharp
using Elintria;

public class ZoomCamera : ElintriaScript
{
    public float minDistance = 2f;
    public float maxDistance = 20f;
    float targetDistance = 8f, distance = 8f, velocity;

    public override void Update()
    {
        targetDistance = Mathf.Clamp(targetDistance - Input.mouseScrollDelta, minDistance, maxDistance);
        distance = Mathf.SmoothDamp(distance, targetDistance, ref velocity, 0.2f);
        transform.localPosition = new Vector3(0f, distance * 0.4f, -distance);
    }
}
```

# Matrix4x4
summary: A 4x4 transformation matrix: translation, rotation and scale in one value.
see: Transform, Quaternion, Vector4

A `Matrix4x4` combines translation, rotation and scale. [[Transform.localToWorldMatrix]] and [[Transform.worldToLocalMatrix]] give an object's matrices. You can also build your own with `TRS`, `Translate`, `Rotate` and `Scale`, and multiply them together. `MultiplyPoint3x4` transforms positions and `MultiplyVector` transforms directions.

The fields `mRC` are row R, column C. The matrix is stored column by column.

## Members
m00: Row 0, column 0.
m10: Row 1, column 0.
m20: Row 2, column 0.
m30: Row 3, column 0.
m01: Row 0, column 1.
m11: Row 1, column 1.
m21: Row 2, column 1.
m31: Row 3, column 1.
m02: Row 0, column 2.
m12: Row 1, column 2.
m22: Row 2, column 2.
m32: Row 3, column 2.
m03: Row 0, column 3 (x translation).
m13: Row 1, column 3 (y translation).
m23: Row 2, column 3 (z translation).
m33: Row 3, column 3.
identity: The matrix that changes nothing.
zero: All zeros.
this[int row, int col]: The element at `row`, `col`.
this[int i]: The element at index `i`, counting down each column in turn.
operator *: `a * b` combines two transforms (b first, then a); `m * v` transforms a Vector4.
MultiplyPoint: Transforms a position, including perspective division.
Translate: A matrix that moves by `t`.
@t: The offset.
Scale: A matrix that scales by `s`.
@s: The scale per axis.
Rotate: A matrix that rotates by `q`.
TRS: A matrix that scales by `s`, rotates by `q` and moves to `pos`, in that order: what a Transform does.
SetTRS: Sets this matrix to TRS(pos, q, s).
transpose: The matrix with rows and columns swapped.
determinant: The determinant: how much the matrix scales volume (negative when it mirrors).
inverse: The matrix that undoes this one.
GetColumn: Column `i` as a Vector4.
GetRow: Row `i` as a Vector4.
SetColumn: Sets column `i`.
SetRow: Sets row `i`.

## Example: Placing objects in another object's space
file: Scripts/Formation.cs
`TRS` builds a leader's transform; `MultiplyPoint3x4` places each follower's slot in it.
```csharp
using System.Collections.Generic;
using Elintria;

public class Formation : ElintriaScript
{
    public List<GameObject> followers = new List<GameObject>();
    public float spacing = 2f;

    public override void Update()
    {
        Matrix4x4 m = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        for (int i = 0; i < followers.Count; i++)
        {
            int row = i / 3 + 1, col = i % 3 - 1;
            Vector3 slot = new Vector3(col * spacing, 0f, -row * spacing);   // behind the leader
            followers[i].transform.position = m.MultiplyPoint3x4(slot);
            followers[i].transform.rotation = transform.rotation;
        }
    }
}
```

## Example: Converting between spaces with the inverse
file: Scripts/LocalHit.cs
```csharp
using Elintria;

public class LocalHit : ElintriaScript
{
    public override void OnCollisionEnter(Collision collision)
    {
        Vector3 world = collision.GetContact(0).point;
        Vector3 local = transform.worldToLocalMatrix.MultiplyPoint3x4(world);
        Vector3 back = transform.localToWorldMatrix.MultiplyPoint3x4(local);
        Debug.Log("Hit at local " + local + " (world " + back + ")");
    }
}
```

## Example: Combining transforms and checking for mirroring
file: Scripts/MirrorCheck.cs
```csharp
using Elintria;

public class MirrorCheck : ElintriaScript
{
    public override void Start()
    {
        Matrix4x4 lift = Matrix4x4.Translate(new Vector3(0f, 2f, 0f));
        Matrix4x4 spin = Matrix4x4.Rotate(Quaternion.Euler(0f, 45f, 0f));
        Matrix4x4 flip = Matrix4x4.Scale(new Vector3(-1f, 1f, 1f));
        Matrix4x4 all = lift * spin * flip;                 // flip, then spin, then lift

        Debug.Log("Mirrored: " + (all.determinant < 0f));
        Debug.Log("Forward becomes " + all.MultiplyVector(Vector3.forward));
        Debug.Log("Undo check: " + (all.inverse * all).MultiplyPoint3x4(new Vector3(1f, 2f, 3f)));
    }
}
```

# Color
summary: An RGBA colour with float components from 0 to 1.
see: Color32, Light, ScreenGUI

`Color` holds red, green, blue and alpha as floats, usually 0 to 1 (values above 1 are allowed for bright light colours). Use the named colours (`Color.red`, `Color.white`), blend with `Lerp`, and multiply colours together to tint.

## Members
r: Red, 0..1.
g: Green, 0..1.
b: Blue, 0..1.
a: Alpha (opacity), 0 transparent to 1 opaque.
Color: Makes a colour from red, green, blue and optionally alpha (1 when left out).
red: Solid red (1, 0, 0, 1).
green: Solid green (0, 1, 0, 1).
blue: Solid blue (0, 0, 1, 1).
white: Solid white.
black: Solid black.
yellow: Solid yellow.
cyan: Solid cyan.
magenta: Solid magenta.
gray: Middle grey (0.5).
grey: Middle grey; the same as gray.
clear: Fully transparent black.
grayscale: The colour's brightness as one number.
maxColorComponent: The largest of r, g and b.
this[int i]: A component by index: 0 r, 1 g, 2 b, 3 a.
operator *: Multiplies colours component by component (a tint), or every component by a number.
Lerp: The colour `t` (0..1) of the way from `x` to `y`.
@x: The first colour.
@y: The second colour.

## Example: Fading an object's light by distance
file: Scripts/DangerLight.cs
```csharp
using Elintria;

public class DangerLight : ElintriaScript
{
    public GameObject player;
    public float range = 20f;

    public override void Update()
    {
        Light l = GetComponent<Light>();
        if (l == null || player == null) return;
        float near = 1f - Mathf.Clamp01(Vector3.Distance(player.transform.position, transform.position) / range);
        l.color = Color.Lerp(Color.green, Color.red, near);
    }
}
```

## Example: Text that fades out
file: Scripts/FloatingText.cs
The alpha channel controls opacity.
```csharp
using Elintria;

public class FloatingText : ElintriaScript
{
    public string text = "+100";
    float age;

    public override void Update()
    {
        age += Time.deltaTime;
        Color c = Color.yellow;
        c.a = Mathf.Clamp01(1f - age / 1.5f);
        ScreenGUI.Label(new Rect(300, 200 - age * 40f, 200, 30), text, null, c);
        if (age > 1.5f) Destroy(gameObject);
    }
}
```

## Example: Colours from numbers
file: Scripts/TeamColors.cs
```csharp
using Elintria;

public class TeamColors : ElintriaScript
{
    public static Color ForTeam(int team)
    {
        Color[] palette = { new Color(0.9f, 0.2f, 0.2f), new Color(0.2f, 0.4f, 0.95f), new Color(0.2f, 0.8f, 0.3f) };
        return palette[Mathf.Abs(team) % palette.Length];
    }

    public static Color Darker(Color c, float amount)
    {
        return c * (1f - amount) + Color.black * amount;   // keeps alpha close to the original
    }

    public override void Start()
    {
        Debug.Log("Team 1 is " + ForTeam(1) + ", shaded " + Darker(ForTeam(1), 0.3f));
    }
}
```

# Color32
summary: An RGBA colour stored as four bytes, 0 to 255.
see: Color, GpuTexture

`Color32` stores a colour as bytes. That makes it compact, and it is the natural format for pixel data. It converts implicitly to and from [[Color]].

## Members
r: Red, 0..255.
g: Green, 0..255.
b: Blue, 0..255.
a: Alpha, 0..255.
Color32: Makes a colour from four bytes.

## Example: Converting between Color and Color32
file: Scripts/HexColor.cs
```csharp
using Elintria;

public class HexColor : ElintriaScript
{
    public static string ToHex(Color c)
    {
        Color32 b = c;   // implicit conversion
        return "#" + b.r.ToString("X2") + b.g.ToString("X2") + b.b.ToString("X2");
    }

    public override void Start()
    {
        Debug.Log(ToHex(new Color(1f, 0.5f, 0f)));   // #FF7F00
    }
}
```

## Example: Building texture pixels
file: Scripts/Checker.cs
```csharp
using Elintria;

public class Checker : ElintriaScript
{
    public static byte[] Make(int size, Color32 a, Color32 b)
    {
        byte[] rgba = new byte[size * size * 4];
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                Color32 c = ((x / 8 + y / 8) % 2 == 0) ? a : b;
                int i = (y * size + x) * 4;
                rgba[i] = c.r; rgba[i + 1] = c.g; rgba[i + 2] = c.b; rgba[i + 3] = c.a;
            }
        return rgba;
    }

    public override void Start()
    {
        int tex = GpuTexture.Create(64, 64, Make(64, new Color32(255, 255, 255, 255), new Color32(40, 40, 40, 255)), TextureFlags.Nearest);
        Debug.Log("Texture id " + tex);
    }
}
```

## Example: Exact colour comparisons
file: Scripts/PaletteMatch.cs
Byte colours compare exactly, which suits palette look-ups.
```csharp
using Elintria;

public class PaletteMatch : ElintriaScript
{
    public static bool Same(Color32 x, Color32 y) => x.r == y.r && x.g == y.g && x.b == y.b && x.a == y.a;

    public override void Start()
    {
        Color32 wall = new Color32(0, 0, 0, 255);
        Debug.Log(Same(wall, Color.black) ? "black is wall" : "no match");
    }
}
```

# Rect
summary: A 2D rectangle: position and size, for screen layout and hit tests.
see: Vector2, ScreenGUI

`Rect` is an x, y position (top-left on screen) with a width and height. [[ScreenGUI]] and editor windows lay out controls with it. `Contains` and `Overlaps` test points and other rectangles.

## Members
x: The left edge.
y: The top edge (screen space grows downwards).
width: The width.
height: The height.
Rect: Makes a rectangle from its position and size.
zero: A rectangle at (0, 0) with no size.
xMin: The left edge. Setting it keeps the right edge where it is.
yMin: The top edge. Setting it keeps the bottom edge where it is.
xMax: The right edge (x + width).
yMax: The bottom edge (y + height).
position: The top-left corner.
size: The width and height.
center: The middle point.
Contains: True when the point `p` is inside.
Overlaps: True when this rectangle and `o` share any area.
@o: The other rectangle.

## Example: A grid of buttons
file: Scripts/LevelSelect.cs
```csharp
using Elintria;

public class LevelSelect : ElintriaScript
{
    public int levels = 12;

    public override void Update()
    {
        for (int i = 0; i < levels; i++)
        {
            Rect r = new Rect(40 + (i % 4) * 130, 80 + (i / 4) * 70, 120, 60);
            if (ScreenGUI.Button(r, "Level " + (i + 1)))
                SceneManager.LoadScene("Level" + (i + 1));
        }
    }
}
```

## Example: Centring a panel
file: Scripts/CenterPanel.cs
```csharp
using Elintria;

public class CenterPanel : ElintriaScript
{
    public override void Update()
    {
        Rect panel = new Rect(0, 0, 360, 200);
        panel.position = new Vector2(ScreenGUI.screenWidth, ScreenGUI.screenHeight) * 0.5f - panel.size * 0.5f;
        ScreenGUI.Fill(panel, new Color(0f, 0f, 0f, 0.7f));
        ScreenGUI.Label(panel, "Game Over", null, Color.white, TextAnchor.MiddleCenter);
    }
}
```

## Example: Hover and overlap tests
file: Scripts/DragBox.cs
```csharp
using Elintria;

public class DragBox : ElintriaScript
{
    Rect box = new Rect(100, 100, 80, 80);
    Rect bin = new Rect(400, 300, 120, 120);
    bool dragging;

    public override void Update()
    {
        Vector2 m = Input.mousePosition;
        if (Input.GetMouseButtonDown(0) && box.Contains(m)) dragging = true;
        if (Input.GetMouseButtonUp(0)) dragging = false;
        if (dragging) box.position = m - box.size * 0.5f;

        ScreenGUI.Fill(bin, box.Overlaps(bin) ? Color.red : Color.gray);
        ScreenGUI.Fill(box, Color.cyan);
    }
}
```

# Bounds
summary: An axis-aligned box in 3D: a centre and a size.
see: Collider, Vector3

`Bounds` describes a box whose sides line up with the world axes. [[Collider.bounds]] gives a collider's world-space box. Use `Contains`, `Intersects`, `ClosestPoint` and `Encapsulate` for quick spatial tests and to grow a box around many points.

## Members
center: The middle of the box.
Bounds: Makes a box from its centre and full size.
@center: The middle of the box.
extents: Half the size: the distance from the centre to each face.
size: The full width, height and depth.
min: The corner with the smallest coordinates.
max: The corner with the largest coordinates.
SetMinMax: Sets the box from its two corners.
@mn: The smallest corner.
@mx: The largest corner.
Encapsulate: Grows the box to include the point `p` or the box `b`.
@b: Another box to include.
Expand: Grows the size by `amount` on every axis (or per axis with a Vector3).
@a: How much to grow on each axis.
Contains: True when the point is inside the box.
Intersects: True when this box and `b` overlap.
@b: The other box.
ClosestPoint: The point in or on the box nearest `p`.
SqrDistance: The squared distance from `p` to the box (0 when inside).

## Example: A box around a group of objects
file: Scripts/GroupBounds.cs
```csharp
using System.Collections.Generic;
using Elintria;

public class GroupBounds : ElintriaScript
{
    public List<GameObject> members = new List<GameObject>();

    public override void Update()
    {
        if (members.Count == 0) return;
        Bounds b = new Bounds(members[0].transform.position, Vector3.zero);
        foreach (GameObject g in members) b.Encapsulate(g.transform.position);
        b.Expand(1f);
        Debug.DrawLine(b.min, b.max, Color.cyan);
        transform.position = b.center + Vector3.up * (b.extents.y + 5f);   // hover a camera above
    }
}
```

## Example: Is the player inside a zone?
file: Scripts/SafeZone.cs
```csharp
using Elintria;

public class SafeZone : ElintriaScript
{
    public Vector3 size = new Vector3(10f, 4f, 10f);
    public GameObject player;

    public override void Update()
    {
        Bounds zone = new Bounds(transform.position, size);
        if (player != null && zone.Contains(player.transform.position))
            ScreenGUI.Text(20, 20, "Safe");
    }
}
```

## Example: Distance to a box
file: Scripts/ProximityAlarm.cs
```csharp
using Elintria;

public class ProximityAlarm : ElintriaScript
{
    public GameObject vault;   // has a collider
    public float alarmDistance = 3f;

    public override void Update()
    {
        Collider c = vault != null ? vault.GetComponent<Collider>() : null;
        if (c == null) return;
        Bounds b = c.bounds;
        if (b.SqrDistance(transform.position) < alarmDistance * alarmDistance)
            Debug.DrawLine(transform.position, b.ClosestPoint(transform.position), Color.red);
    }
}
```

# Ray
summary: A line starting at a point and going forever in one direction.
see: Physics, Plane, RaycastHit

A `Ray` is an origin and a direction. Pass one to [[Physics.Raycast]], or to [[Plane.Raycast]] to intersect with a flat plane. `GetPoint` gives the point a set distance along it.

## Members
Ray: Makes a ray. The direction is normalised for you.
@origin: Where the ray starts.
@direction: Which way it goes.
origin: Where the ray starts.
direction: Which way it goes (unit length).
GetPoint: The point `distance` units along the ray.
@distance: How far along.

## Example: Shooting a ray from the camera
file: Scripts/CenterRay.cs
Cameras look down their -Z, so the ray goes along `-transform.forward`.
```csharp
using Elintria;

public class CenterRay : ElintriaScript
{
    public override void Update()
    {
        Ray ray = new Ray(transform.position, -transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            ScreenGUI.Text(20, 20, "Looking at " + hit.gameObject.name);
    }
}
```

## Example: A point along a ray
file: Scripts/TeleportAim.cs
```csharp
using Elintria;

public class TeleportAim : ElintriaScript
{
    public float maxDistance = 12f;

    public override void Update()
    {
        Ray aim = new Ray(transform.position + Vector3.up, transform.forward);
        Vector3 spot = Physics.Raycast(aim, out RaycastHit hit, maxDistance) ? hit.point : aim.GetPoint(maxDistance);
        Debug.DrawLine(aim.origin, spot, Color.magenta);
        if (Input.GetKeyDown(KeyCode.T)) transform.position = spot;
    }
}
```

## Example: Where a ray meets the ground plane
file: Scripts/GroundPicker.cs
```csharp
using Elintria;

public class GroundPicker : ElintriaScript
{
    public override void Update()
    {
        Ray r = new Ray(transform.position, -transform.forward);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        if (ground.Raycast(r, out float enter))
            Debug.DrawRay(r.GetPoint(enter), Vector3.up * 2f, Color.green);
    }
}
```

# Plane
summary: An infinite flat plane, defined by a normal and a distance from the origin.
see: Ray, Vector3

`Plane` is a flat surface stretching forever: a normal pointing out of its front side, and a distance from the origin. It measures how far points are from it, tells which side they are on, and finds where a [[Ray]] crosses it.

## Members
normal: The direction the plane faces (unit length).
distance: The distance from the origin to the plane along its normal.
Plane: Makes a plane facing `inNormal` that passes through `inPoint` (or lies `d` from the origin).
@inNormal: The direction the plane faces.
@inPoint: Any point on the plane.
@d: The distance from the origin.
GetDistanceToPoint: The signed distance from the plane to `p`: positive in front, negative behind.
GetSide: True when `p` is on the side the normal points to.
ClosestPointOnPlane: The point on the plane nearest `p`.
Raycast: True when `ray` hits the plane; `enter` is the distance along the ray.
@ray: The ray to test.
@enter: Filled in with how far along the ray the hit is.

## Example: Which side of a line is the player on?
file: Scripts/Finishline.cs
```csharp
using Elintria;

public class FinishLine : ElintriaScript
{
    public GameObject runner;
    bool wasBefore = true;

    public override void Update()
    {
        Plane line = new Plane(transform.forward, transform.position);
        bool before = !line.GetSide(runner.transform.position);
        if (wasBefore && !before) Debug.Log(runner.name + " crossed the finish line!");
        wasBefore = before;
    }
}
```

## Example: Projecting onto a wall
file: Scripts/WallMarker.cs
```csharp
using Elintria;

public class WallMarker : ElintriaScript
{
    public GameObject wall;
    public GameObject marker;

    public override void Update()
    {
        Plane p = new Plane(wall.transform.forward, wall.transform.position);
        marker.transform.position = p.ClosestPointOnPlane(transform.position);
        float d = p.GetDistanceToPoint(transform.position);
        ScreenGUI.Text(20, 40, "Distance to wall: " + d.ToString("0.0"));
    }
}
```

## Example: Water level check without physics
file: Scripts/SimpleWaterline.cs
```csharp
using Elintria;

public class SimpleWaterline : ElintriaScript
{
    public float waterHeight = 0f;

    public override void Update()
    {
        Plane water = new Plane(Vector3.up, new Vector3(0f, waterHeight, 0f));   // faces up, at waterHeight
        float depth = -water.GetDistanceToPoint(transform.position);
        if (depth > 0f) transform.position += Vector3.up * Mathf.Min(depth, 2f * Time.deltaTime);
    }
}
```
