# ElintriaScript
summary: The base class of every C# script: derive from it, override Start and Update, and attach the script to a GameObject.
see: GameObject, Transform, Time, Coroutine, manual-lifecycle

`ElintriaScript` is what makes a C# class a component. Derive from it, and the class shows up in **Add Component**. The engine creates one instance per object it is attached to. It calls the lifecycle methods you override (`Awake`, `Start`, `Update`, `FixedUpdate`, `LateUpdate`, the collision callbacks and so on) and saves your public fields with the scene.

From inside a script, `gameObject` and `transform` give you the object the script is on. `GetComponent<T>()` finds its other components, and the static helpers (`Instantiate`, `Destroy`, `FindObjectOfType`) work on the whole scene.

> note: Override the lifecycle methods with `public override void Update()`. Writing a new method with the same name, without `override`, never gets called.

## Members
GetComponent: The first component of type `T` on this script's object, or null. `T` can be an engine component (Rigidbody, Light...) or another script.
Start: Called once, just before the script's first Update, when Play mode (or the built game) starts or when the object is created. Most setup goes here.
Update: Called once per frame while the game runs. Read input and move things here, scaling by [[Time.deltaTime]] so speed does not depend on frame rate.
Awake: Called once when the script is created, before any Start, even when the script starts disabled. Use it to cache components on the same object.
OnEnable: Called each time the script or its object becomes enabled (including right after Awake).
OnDisable: Called each time the script or its object is disabled, and before it is destroyed.
FixedUpdate: Called at the fixed physics rate ([[Time.fixedDeltaTime]]), possibly several times in one frame or none. Apply forces and other physics changes here.
LateUpdate: Called every frame after every script's Update. The place for cameras and anything that should react to where things ended up this frame.
OnDestroy: Called when the script or its object is destroyed, and when Play mode stops. Release anything the script created.
OnTriggerEnter: Called on the first physics step this object overlaps a trigger collider (or, if this object is the trigger, something enters it).
@other: The other collider. Use `other.gameObject` to reach its object.
OnTriggerStay: Called every physics step while the overlap continues.
@other: The other collider.
OnTriggerExit: Called on the first physics step after the overlap ends.
@other: The other collider.
OnCollisionEnter: Called on the first physics step this object touches another solid collider.
@collision: Details of the contact: the other object, the contact point and normal, and the relative velocity.
OnCollisionStay: Called every physics step the contact continues.
@collision: Details of the contact.
OnCollisionExit: Called on the first physics step after the contact ends.
@collision: Details of the contact that ended.
OnControllerColliderHit: Called when a CharacterController on this object hits something while moving.
@hit: What was hit, where, and in which direction the controller was moving.
StartCoroutine: Starts a coroutine: a method that returns `IEnumerator` and pauses with `yield return`. Returns a [[Coroutine]] handle you can pass to StopCoroutine.
@routine: The result of calling your coroutine method, e.g. `StartCoroutine(FadeOut())`.
@methodName: The name of a coroutine method on this script that takes no parameters.
StopCoroutine: Stops a running coroutine, given the handle StartCoroutine returned (or the same IEnumerator).
@c: The handle StartCoroutine returned.
StopAllCoroutines: Stops every coroutine this script started.
Invoke: Calls the method `methodName` on this script once, after `time` seconds.
@time: The delay in seconds.
InvokeRepeating: Calls `methodName` after `time` seconds, then again every `repeatRate` seconds until cancelled.
CancelInvoke: Cancels pending Invoke and InvokeRepeating calls: all of them, or only those for `methodName`.
IsInvoking: True while an Invoke of `methodName` (or any Invoke, with no argument) is pending.
print: Logs `message` to the Console, like [[Debug.Log]].

## Example: A rotating pickup that bobs up and down
file: Scripts/FloatingPickup.cs
Uses `Start` to remember where the object began, and `Update` to spin and bob it. Frame-rate independent thanks to [[Time.deltaTime]] and [[Time.time]].
```csharp
using Elintria;

public class FloatingPickup : ElintriaScript
{
    public float spinSpeed = 90f;     // degrees a second
    public float bobHeight = 0.25f;   // metres
    public float bobSpeed = 2f;       // cycles a second

    Vector3 startPos;

    public override void Start()
    {
        startPos = transform.position;
    }

    public override void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
        float offset = Mathf.Sin(Time.time * bobSpeed * Mathf.PI * 2f) * bobHeight;
        transform.position = startPos + Vector3.up * offset;
    }
}
```

## Example: Reacting to the player with a trigger and a coroutine
file: Scripts/SpikeTrap.cs
When something tagged "Player" walks into the trap's trigger collider, a coroutine waits, raises the spikes, deals damage and resets. `OnTriggerEnter` is a lifecycle callback; `StartCoroutine` runs the sequence over several frames.
```csharp
using System.Collections;
using Elintria;

public class SpikeTrap : ElintriaScript
{
    public GameObject spikes;          // child object that pops up
    public float delay = 0.4f;
    public float damage = 30f;

    bool armed = true;

    public override void OnTriggerEnter(Collider other)
    {
        if (!armed || !other.gameObject.CompareTag("Player")) return;
        StartCoroutine(Fire(other.gameObject));
    }

    IEnumerator Fire(GameObject victim)
    {
        armed = false;
        yield return new WaitForSeconds(delay);
        spikes.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        victim.SendMessage("ApplyDamage", damage);
        yield return new WaitForSeconds(1.5f);
        spikes.transform.localPosition = Vector3.zero;
        armed = true;
    }
}
```

## Example: A spawner using Instantiate, Destroy and InvokeRepeating
file: Scripts/RockSpawner.cs
`InvokeRepeating` calls `Spawn` every few seconds. Each rock is a copy of a template object, and `Destroy` with a delay cleans it up later. `OnDisable` stops the timer when the spawner is switched off.
```csharp
using Elintria;

public class RockSpawner : ElintriaScript
{
    public GameObject rockTemplate;   // an inactive rock in the scene
    public float every = 2f;
    public float lifetime = 10f;

    public override void OnEnable()
    {
        InvokeRepeating("Spawn", 0.5f, every);
    }

    public override void OnDisable()
    {
        CancelInvoke("Spawn");
    }

    void Spawn()
    {
        Vector3 at = transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
        GameObject rock = Instantiate(rockTemplate, at, Random.rotation);
        rock.SetActive(true);
        Destroy(rock, lifetime);
    }
}
```

## Example: Talking to other components
file: Scripts/PlayerStats.cs
`GetComponent` finds engine components and other scripts on the same object. `FindObjectOfType` finds one anywhere in the scene. Cache what you find in `Awake` or `Start` rather than searching every frame.
```csharp
using Elintria;

public class PlayerStats : ElintriaScript
{
    public float maxStamina = 100f;
    public float stamina;

    Rigidbody body;
    AudioSource audioSource;

    public override void Awake()
    {
        body = GetComponent<Rigidbody>();
        audioSource = gameObject.audioSource;   // adds one if missing
        stamina = maxStamina;
    }

    public override void Update()
    {
        bool running = Input.GetKey(KeyCode.Shift) && body != null && body.velocity.magnitude > 0.1f;
        stamina = Mathf.Clamp(stamina + (running ? -20f : 10f) * Time.deltaTime, 0f, maxStamina);
        if (stamina <= 0f && !audioSource.isPlaying)
            audioSource.PlayOneShot("Audio/out_of_breath.wav");
    }
}
```

# GameObject
summary: An object in the scene: it has a name, a Transform, and holds components and scripts.
see: Transform, ElintriaScript, Component, PrimitiveType

Everything in a scene is a `GameObject`. On its own it is only a named point with a [[Transform]]. What it does comes from the components attached to it: a mesh to draw, a [[Rigidbody]], a [[Light]], or your scripts.

Use the static methods to find objects (`Find`, `FindWithTag`) and to make new ones (`new GameObject(name)`, `CreatePrimitive`). Use the instance methods to switch an object on or off, read and add components, and send it messages.

> warning: Once destroyed, a GameObject reference stays non-null in C#. Test `IsAlive` (or `if (obj)`) before using one you kept from an earlier frame.

## Members
SendMessage: Calls the method `methodName` on every script on this object, passing `value` when given. Objects without such a method are skipped silently.
transform: The object's [[Transform]]: position, rotation, scale and parent. Every GameObject has exactly one.
name: The object's name, as shown in the Hierarchy and used by Find.
GetInstanceID: A number that identifies this object while it exists. Two references to the same object give the same id.
activeSelf: Whether this object itself is switched on. Setting it is the same as calling SetActive.
SetActive: Switches the object on or off. An inactive object is not drawn, does not collide, and its scripts stop updating.
@value: True to activate, false to deactivate.
activeInHierarchy: True when this object and every parent above it are active.
layer: A layer number, kept for Unity code. Elintria does not filter by layer yet.
tag: The object's tag, such as "Player" or "Enemy". Set it in the Inspector or from code.
Rename: Renames the object. The same as setting `name`.
@newName: The new name.
GameObject: Makes a new, empty object in the scene, with only a Transform. Add components to give it something to do.
@name: The object's name (default "GameObject").
Create: Makes a new, empty object in the scene, like `new GameObject(name)`.
CreatePrimitive: Makes a cube, sphere, capsule, cylinder, plane or quad, with a mesh and a matching collider.
@type: Which shape to make.
FindWithTag: The first active object with the tag `tag`, or null.
FindGameObjectsWithTag: Every active object with the tag `tag`.
FindObjectsWithNamePrefix: Every object whose name starts with `prefix`. A handy way to collect spawn points or waypoints named "Spawn_1", "Spawn_2" and so on.
GetMeta: A text value attached to this object in the scene (by a level importer, say), or "" when there is none.
@key: The value's name.
SetMeta: Attaches a text value to this object, or removes it when `value` is null or empty.
FindByMeta: The object whose meta value `key` equals `value`, or null.
audioSource: The [[AudioSource]] on this object, adding one first if there is none. Lets a script play sounds without any setup in the editor.
GetAudioSource: The [[AudioSource]] on this object, or null when it has none.
Find: Finds an active object by name and returns it, or null when there is none. It searches the whole scene, so call it once (in Start) and keep the result.
@name: The exact name to look for.

## Example: Building a small scene from code
file: Scripts/ArenaBuilder.cs
`CreatePrimitive` makes visible, solid shapes. The transform places them, and `AddComponent` gives the ball a Rigidbody so it falls.
```csharp
using Elintria;

public class ArenaBuilder : ElintriaScript
{
    public int pillars = 8;
    public float radius = 10f;

    public override void Start()
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Arena Floor";
        floor.transform.localScale = new Vector3(radius * 2.5f, 0.5f, radius * 2.5f);

        for (int i = 0; i < pillars; i++)
        {
            float a = i * Mathf.PI * 2f / pillars;
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            p.name = "Pillar " + i;
            p.transform.position = new Vector3(Mathf.Cos(a) * radius, 2f, Mathf.Sin(a) * radius);
            p.transform.localScale = new Vector3(1f, 4f, 1f);
        }

        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.transform.position = new Vector3(0f, 8f, 0f);
        ball.AddComponent<Rigidbody>().mass = 2f;
    }
}
```

## Example: Finding objects by name, tag and prefix
file: Scripts/GameDirector.cs
Find each kind of object once in `Start` and keep the references. `FindObjectsWithNamePrefix` collects numbered markers without hard-coding how many there are.
```csharp
using Elintria;

public class GameDirector : ElintriaScript
{
    GameObject player;
    GameObject[] enemies;
    GameObject[] spawnPoints;

    public override void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        spawnPoints = GameObject.FindObjectsWithNamePrefix("Spawn_");
        GameObject exit = GameObject.Find("Exit Door");

        Debug.Log("Player: " + (player != null ? player.name : "none") +
                  ", enemies: " + enemies.Length +
                  ", spawn points: " + spawnPoints.Length +
                  ", exit found: " + (exit != null));
    }

    public void RespawnPlayer()
    {
        if (spawnPoints.Length == 0 || !player) return;
        GameObject p = spawnPoints[Random.Range(0, spawnPoints.Length)];
        player.transform.position = p.transform.position;
    }
}
```

## Example: Switching objects on and off, safely
file: Scripts/LightSwitch.cs
`SetActive` hides and disables an object and everything under it. `activeSelf` reports the current state, and `IsAlive` guards against a light that someone destroyed.
```csharp
using System.Collections.Generic;
using Elintria;

public class LightSwitch : ElintriaScript
{
    public List<GameObject> lamps = new List<GameObject>();
    public bool startOn = true;

    public override void Start()
    {
        Set(startOn);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            Set(!(lamps.Count > 0 && lamps[0].activeSelf));
    }

    void Set(bool on)
    {
        foreach (GameObject lamp in lamps)
            if (lamp != null && lamp.IsAlive)
                lamp.SetActive(on);
    }
}
```

## Example: Sending messages to whatever is hit
file: Scripts/Shockwave.cs
`SendMessage` calls a method by name on every script of the object, without needing to know which script it is. `BroadcastMessage` also reaches every child.
```csharp
using Elintria;

public class Shockwave : ElintriaScript
{
    public float radius = 6f;
    public float damage = 40f;

    public void Detonate()
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, radius))
        {
            GameObject target = c.gameObject;
            if (target == gameObject) continue;
            float falloff = 1f - Vector3.Distance(target.transform.position, transform.position) / radius;
            target.SendMessage("ApplyDamage", damage * Mathf.Clamp01(falloff));
            target.BroadcastMessage("OnShockwave");   // e.g. lamps flicker
        }
        Destroy(gameObject);
    }
}
```

# Transform
summary: The position, rotation and scale of an object, and its place in the parent-child hierarchy.
see: GameObject, Vector3, Quaternion, Space

Every [[GameObject]] has a `Transform`. Read or set `position` and `rotation` to move and turn the object in world space. Use `localPosition` and `localRotation` to work relative to its parent. `Translate`, `Rotate`, `RotateAround` and `LookAt` are the usual ways to move things a little each frame.

The hierarchy lives here too: `parent`, `childCount`, `GetChild`, `Find` and `SetParent`. A child moves, turns and scales with its parent.

Elintria's convention is that +Z is an object's forward and +Y is up. Cameras and lights look down their -Z.

## Members
parent: The Transform this one is under, or null for a root object. Setting it re-parents the object, keeping its world position.
position: The object's position in world space. Setting it moves the object (and its children) at once.
forward: The object's forward direction (+Z) in world space, unit length. Setting it turns the object to face that way.
right: The object's right direction (+X) in world space, unit length.
up: The object's up direction (+Y) in world space, unit length.
Translate: Moves the object by `delta`, in world space by default or in the object's own axes with `Space.Self`.
@delta: How far to move.
Rotate(Vector3 axis, float angle): Turns the object by `angle` degrees about `axis`.
LookAt: Turns the object so its +Z points at `target` (a point or another Transform).
@target: What to face.
localPosition: The position relative to the parent (the same as `position` when there is no parent).
localRotation: The rotation relative to the parent.
localEulerAngles: The local rotation as Euler angles in degrees.
localToWorldMatrix: The matrix that turns this object's local points into world points.
worldToLocalMatrix: The matrix that turns world points into this object's local points.
SetPositionAndRotation: Sets the world position and rotation in one call.
@p: The new world position.
@q: The new world rotation.
RotateAround: Orbits the object around `point`, turning it `angle` degrees about `axis` (it also turns to keep facing the same way relative to the orbit).
@point: The centre of the orbit, in world space.
TransformPoint: Turns a point from this object's local space into world space (position, rotation and scale apply).
@p: A point in local space.
TransformDirection: Turns a direction from local space into world space (rotation only).
@d: A direction in local space.
TransformVector: Turns a vector from local space into world space (rotation and scale).
@v: A vector in local space.
InverseTransformPoint: Turns a world point into this object's local space.
@p: A point in world space.
InverseTransformDirection: Turns a world direction into local space (rotation only).
@d: A direction in world space.
InverseTransformVector: Turns a world vector into local space (rotation and scale).
@v: A vector in world space.
root: The topmost Transform above this one (itself when it has no parent).
childCount: How many direct children this object has.
GetChild: The child at `index` (0-based), in the order they were added.
SetParent: Puts this object under `p` (null to make it a root). With `worldPositionStays` false, its local position is kept instead of its world position.
@p: The new parent, or null.
DetachChildren: Makes every child a root object, keeping its world position.
GetEnumerator: Lets you loop over the children with `foreach (Transform child in transform)`.
SetAsFirstSibling: Kept for Unity code; the engine does not reorder children yet.
SetAsLastSibling: Kept for Unity code; the engine does not reorder children yet.

## Example: First-person style movement relative to facing
file: Scripts/SimpleWalker.cs
`Translate` with `Space.Self` moves along the object's own axes, so W always goes "forward" whichever way it faces. `Rotate` turns it with the mouse.
```csharp
using Elintria;

public class SimpleWalker : ElintriaScript
{
    public float moveSpeed = 4f;
    public float turnSpeed = 120f;   // degrees a second per unit of mouse movement

    public override void Update()
    {
        float forward = Input.GetAxisVertical();     // W/S
        float strafe = Input.GetAxisHorizontal();    // A/D
        transform.Translate(new Vector3(strafe, 0f, forward) * moveSpeed * Time.deltaTime, Space.Self);

        if (Input.GetMouseButton(1))
            transform.Rotate(0f, Input.mouseDelta.x * turnSpeed * Time.deltaTime, 0f);
    }
}
```

## Example: A smooth follow camera with LateUpdate
file: Scripts/FollowCamera.cs
The camera moves in `LateUpdate`, after the target has moved this frame. `TransformPoint` turns an offset behind the target into world space, and `LookAt` aims the camera. Cameras look down their -Z, so the look direction is flipped.
```csharp
using Elintria;

public class FollowCamera : ElintriaScript
{
    public GameObject target;
    public Vector3 offset = new Vector3(0f, 3f, -6f);   // behind and above, in the target's space
    public float smoothTime = 0.25f;

    Vector3 velocity;

    public override void LateUpdate()
    {
        if (target == null) return;
        Transform t = target.transform;
        Vector3 wanted = t.TransformPoint(offset);
        transform.position = Vector3.SmoothDamp(transform.position, wanted, ref velocity, smoothTime);

        // Cameras look down -Z: point +Z away from the target.
        Vector3 look = (t.position + Vector3.up) - transform.position;
        transform.rotation = Quaternion.LookRotation(-look);
    }
}
```

## Example: Working with the hierarchy
file: Scripts/Turntable.cs
Walks the children with `foreach`, finds one by path with `Find`, re-parents an object with `SetParent`, and orbits children around the parent with `RotateAround`.
```csharp
using Elintria;

public class Turntable : ElintriaScript
{
    public float degreesPerSecond = 30f;
    public GameObject newcomer;

    public override void Start()
    {
        Debug.Log(name + " has " + transform.childCount + " children:");
        foreach (Transform child in transform)
            Debug.Log(" - " + child.name + " at local " + child.localPosition);

        Transform lid = transform.Find("Box/Lid");          // a grandchild, by path
        if (lid != null) lid.localEulerAngles = new Vector3(-60f, 0f, 0f);

        if (newcomer != null)
            newcomer.transform.SetParent(transform, true);  // keeps its world position
    }

    public override void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).RotateAround(transform.position, Vector3.up, degreesPerSecond * Time.deltaTime);
    }
}
```

## Example: Local and world space conversions
file: Scripts/MuzzleFlash.cs
`TransformPoint` and `InverseTransformPoint` move between world and local space; `TransformDirection` does the same for directions, ignoring position and scale.
```csharp
using Elintria;

public class MuzzleFlash : ElintriaScript
{
    public Vector3 muzzleLocal = new Vector3(0f, 0.1f, 0.6f);   // the barrel tip, in the gun's space

    public Vector3 MuzzleWorld()
    {
        return transform.TransformPoint(muzzleLocal);
    }

    public Vector3 ShotDirection()
    {
        return transform.TransformDirection(Vector3.forward);
    }

    public bool IsInFront(Vector3 worldPoint)
    {
        // In local space, "in front" simply means z > 0.
        return transform.InverseTransformPoint(worldPoint).z > 0f;
    }

    public override void Update()
    {
        Debug.DrawRay(MuzzleWorld(), ShotDirection() * 5f, Color.yellow);
    }
}
```

# Component
summary: The interface every component shares, engine-made or script: its object, its transform and component lookups.
see: ComponentBase, ManagedComponent, ElintriaScript

`Component` is what [[ElintriaScript]], the engine's own components ([[ComponentBase]]) and plain C# components ([[ManagedComponent]]) have in common. Code that should work with *any* component, such as a helper that takes "something on an object", can accept a `Component`.

It is an interface, not a class. A script must derive from [[ElintriaScript]], and C# allows only one base class.

## Example: A helper that works with any component
file: Scripts/ComponentUtil.cs
Engine components and scripts both pass through the same method, because both are `Component`s.
```csharp
using Elintria;

public static class ComponentUtil
{
    public static string Describe(Component c)
    {
        if (c == null) return "(none)";
        return c.GetType().Name;
    }
}

public class Inspectorish : ElintriaScript
{
    public override void Start()
    {
        Debug.Log(ComponentUtil.Describe(GetComponent<Rigidbody>()));
        Debug.Log(ComponentUtil.Describe(GetComponent<Inspectorish>()));
    }
}
```

## Example: Listing every component on an object
file: Scripts/ComponentLister.cs
`GetComponents<Component>()` returns everything attached, of every kind.
```csharp
using Elintria;

public class ComponentLister : ElintriaScript
{
    public GameObject target;

    public override void Start()
    {
        GameObject g = target != null ? target : gameObject;
        foreach (Component c in g.GetComponents<Component>())
            Debug.Log(g.name + " has " + c.GetType().Name);
    }
}
```

## Example: Accepting a component in your own API
file: Scripts/Highlighter.cs
Taking a `Component` lets callers pass whichever component they hold, and you can still reach its object.
```csharp
using Elintria;

public class Highlighter : ElintriaScript
{
    public static void Flash(Component anyComponentOnTheObject)
    {
        if (anyComponentOnTheObject is ComponentBase engine && engine.IsAlive)
            engine.gameObject.SendMessage("OnHighlight");
        else if (anyComponentOnTheObject is ElintriaScript script && script)
            script.gameObject.SendMessage("OnHighlight");
    }
}
```

# ComponentBase
summary: The base class of the engine's own components: Rigidbody, colliders, Light, SpriteRenderer and more.
see: Component, Rigidbody, Collider, Light

The engine's built-in components (for example [[Rigidbody]], [[Collider]], [[Light]], [[SpriteRenderer]], [[Terrain]] and [[ParticleEmitter]]) derive from `ComponentBase`. Each is a thin handle on a C++ component. Reading a property always gives the live value, and writing one changes the engine at once.

You don't derive from `ComponentBase` yourself. You get one from `GetComponent<T>()` or `AddComponent<T>()`.

## Members
ToString: The component's type and object name, for logs.

## Example: Checking a component is still alive
file: Scripts/BodyWatcher.cs
`IsAlive` turns false once the component (or its object) is destroyed. Reading it is always safe.
```csharp
using Elintria;

public class BodyWatcher : ElintriaScript
{
    public GameObject crate;
    Rigidbody body;

    public override void Start()
    {
        if (crate != null) body = crate.GetComponent<Rigidbody>();
    }

    public override void Update()
    {
        if (body == null || !body.IsAlive) return;   // the crate was destroyed
        if (body.velocity.magnitude > 10f)
            Debug.Log(body.gameObject.name + " is flying!");
    }
}
```

## Example: Reaching sibling components from an engine component
file: Scripts/LightFromCollider.cs
A `ComponentBase` has the same lookups as a script: `GetComponent`, `gameObject`, `transform`, `CompareTag`.
```csharp
using Elintria;

public class LightFromCollider : ElintriaScript
{
    public override void OnTriggerEnter(Collider other)
    {
        // other is a Collider (a ComponentBase); ask its object for a Light.
        Light lamp = other.GetComponent<Light>();
        if (lamp != null && other.CompareTag("Lamp"))
            lamp.intensity *= 2f;
    }
}
```

## Example: Adding engine components at run time
file: Scripts/MakeDynamic.cs
`AddComponent` returns the new engine component, ready to configure.
```csharp
using Elintria;

public class MakeDynamic : ElintriaScript
{
    public override void Start()
    {
        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(1f, 2f, 1f);

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 5f;
        rb.useGravity = true;
        Debug.Log("Added " + box + " and " + rb);
    }
}
```

# Behaviour
summary: An engine component that can be switched on and off.
see: ComponentBase, ElintriaScript

`Behaviour` adds `enabled` and `isActiveAndEnabled` to [[ComponentBase]], for engine components that can be disabled without being removed. It exists mainly for code written against Unity's `Behaviour`.

## Example: Disabling a component temporarily
file: Scripts/Stun.cs
```csharp
using System.Collections;
using Elintria;

public class Stun : ElintriaScript
{
    public ElintriaScript movementScript;   // drag the movement script's object... or find it in Start

    public void Apply(float seconds)
    {
        StartCoroutine(Run(seconds));
    }

    IEnumerator Run(float seconds)
    {
        if (movementScript != null) movementScript.enabled = false;
        yield return new WaitForSeconds(seconds);
        if (movementScript != null) movementScript.enabled = true;
    }
}
```

## Example: Checking isActiveAndEnabled before acting
file: Scripts/SafeCall.cs
```csharp
using Elintria;

public class SafeCall : ElintriaScript
{
    public static void Ping(ElintriaScript s)
    {
        if (s != null && s.isActiveAndEnabled)
            s.SendMessage("OnPing");
    }
}
```

## Example: Toggling with a key
file: Scripts/ToggleBehaviour.cs
```csharp
using Elintria;

public class ToggleBehaviour : ElintriaScript
{
    public KeyCode key = KeyCode.T;
    public GameObject target;

    public override void Update()
    {
        if (!Input.GetKeyDown(key) || target == null) return;
        foreach (ElintriaScript s in target.GetComponents<ElintriaScript>())
            s.enabled = !s.enabled;
    }
}
```

# ManagedComponent
summary: A component written in plain C# with no engine part behind it, attachable with AddComponent.
see: Component, IComponentProvider, ElintriaScript

Most components either live in the engine ([[ComponentBase]]) or are scripts ([[ElintriaScript]]). `ManagedComponent` is for a third kind: data or behaviour you want to hang off an object, but that needs no Update and no Inspector entry. Examples are a bag of settings, a UI canvas model, or a cache.

Derive from it, then call `gameObject.AddComponent<YourType>()`; `GetComponent<YourType>()` finds it again later. A [[IComponentProvider]] can also create them on demand.

## Example: Attaching run-time data to objects
file: Scripts/LootTable.cs
```csharp
using System.Collections.Generic;
using Elintria;

public class LootTable : ManagedComponent
{
    public List<string> items = new List<string>();
    public float dropChance = 0.5f;
}

public class EnemySetup : ElintriaScript
{
    public override void Start()
    {
        LootTable loot = gameObject.AddComponent<LootTable>();
        loot.items.Add("coin");
        loot.items.Add("potion");
    }

    public override void OnDestroy()
    {
        LootTable loot = GetComponent<LootTable>();
        if (loot != null && Random.value < loot.dropChance)
            Debug.Log("Dropped " + loot.items[Random.Range(0, loot.items.Count)]);
    }
}
```

## Example: A shared cache keyed by object
file: Scripts/PathCache.cs
```csharp
using Elintria;

public class PathCache : ManagedComponent
{
    public Vector3[] corners = new Vector3[0];
    public float computedAt = -1f;
}

public class Pathing : ElintriaScript
{
    public static Vector3[] PathFor(GameObject who, Vector3 goal)
    {
        PathCache cache = who.GetComponent<PathCache>() ?? who.AddComponent<PathCache>();
        if (Time.time - cache.computedAt > 1f)
        {
            NavAgent.CalculatePath(who.transform.position, goal, out cache.corners);
            cache.computedAt = Time.time;
        }
        return cache.corners;
    }
}
```

## Example: Reading it from another script
file: Scripts/LootReader.cs
```csharp
using Elintria;

public class LootReader : ElintriaScript
{
    public GameObject chest;

    public override void Start()
    {
        if (chest != null && chest.TryGetComponent<LootTable>(out LootTable loot))
            Debug.Log(chest.name + " can drop " + loot.items.Count + " kinds of item");
    }
}
```

# SerializeFieldAttribute
summary: Shows a private field in the Inspector and saves it with the scene.
see: HideInInspectorAttribute, manual-inspector

Public fields are shown in the Inspector automatically. Mark a **private** field with `[SerializeField]` to make it editable and saved too, without making it part of your script's public API.

## Example: Private tuning values
file: Scripts/Jumper.cs
```csharp
using Elintria;

public class Jumper : ElintriaScript
{
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private int maxJumps = 2;

    int jumpsLeft;

    public override void Start() { jumpsLeft = maxJumps; }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0)
        {
            jumpsLeft--;
            Debug.Log("Jump to " + jumpHeight + " m, " + jumpsLeft + " left");
        }
    }
}
```

## Example: Mixing public, private and serialized fields
file: Scripts/Door2.cs
```csharp
using Elintria;

public class Door2 : ElintriaScript
{
    public bool locked;                       // in the Inspector, public
    [SerializeField] float openSpeed = 90f;   // in the Inspector, private
    float angle;                              // not in the Inspector

    public override void Update()
    {
        if (locked) return;
        angle = Mathf.MoveTowards(angle, 90f, openSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0f, angle, 0f);
    }
}
```

## Example: A serialized object reference
file: Scripts/Alarm.cs
```csharp
using Elintria;

public class Alarm : ElintriaScript
{
    [SerializeField] GameObject siren;   // drag an object here

    public void Trigger()
    {
        if (siren != null) siren.audioSource.Play();
    }
}
```

# HideInInspectorAttribute
summary: Hides a public field from the Inspector.
see: SerializeFieldAttribute, manual-inspector

Mark a public field with `[HideInInspector]` when other scripts need to read or set it but designers should not edit it, such as run-time state or values computed in Start.

## Example: Public state that is not a setting
file: Scripts/Score.cs
```csharp
using Elintria;

public class Score : ElintriaScript
{
    public int pointsPerCoin = 10;         // a setting: shown
    [HideInInspector] public int score;    // state: hidden

    public void AddCoin() { score += pointsPerCoin; }
}
```

## Example: Values set by another component
file: Scripts/Projectile2.cs
```csharp
using Elintria;

public class Projectile2 : ElintriaScript
{
    public float speed = 20f;
    [HideInInspector] public GameObject shooter;   // set by the gun that fires it

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == shooter) return;
        other.gameObject.SendMessage("ApplyDamage", 10f);
        Destroy(gameObject);
    }

    public override void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
```

## Example: Hiding a cached reference
file: Scripts/Cached.cs
```csharp
using Elintria;

public class Cached : ElintriaScript
{
    [HideInInspector] public Rigidbody body;

    public override void Awake() { body = GetComponent<Rigidbody>(); }
}
```

# Debug
summary: Write messages to the Console and draw debug lines in the Scene and Game views.
see: Color, manual-getting-started

`Debug.Log` and friends write to the editor's Console window (and to the output of a built game). `LogWarning` and `LogError` stand out in yellow and red. `DrawLine` and `DrawRay` draw lines over the game, which is handy for visualising raycasts, paths and AI decisions.

## Members
Log: Writes `message` to the Console. Anything can be passed; it is converted with ToString.
@message: What to write.
@context: The object the message is about (shown with it).
LogWarning: Writes `message` to the Console as a warning.
LogError: Writes `message` to the Console as an error.
LogFormat: Writes a formatted message: `Debug.LogFormat("{0} took {1:0.0} damage", name, dmg)`.
@args: The values for the placeholders.
LogWarningFormat: Writes a formatted warning.
LogErrorFormat: Writes a formatted error.
LogException: Writes an exception, with its message and stack trace, as an error.
@e: The exception to report.
Assert: Logs an error when `condition` is false. Use it to catch "this should never happen" bugs early.
@condition: What must be true.
DrawRay: Draws a line from `start` along `dir` (its length is the vector's length).
@dir: The direction and length of the ray.
isDebugBuild: True in the editor and in development builds.
Break: Pauses the game in the editor (like pressing Pause).

## Example: Logging with different levels
file: Scripts/HealthLogger.cs
```csharp
using Elintria;

public class HealthLogger : ElintriaScript
{
    public float health = 100f;

    public void ApplyDamage(float amount)
    {
        health -= amount;
        Debug.LogFormat("{0} took {1:0.#} damage ({2:0.#} left)", name, amount, health);
        if (health < 25f) Debug.LogWarning(name + " is badly hurt", gameObject);
        if (health <= 0f) Debug.LogError(name + " died");
    }
}
```

## Example: Visualising a raycast
file: Scripts/LaserSight.cs
Green when the ray hits something, red when it does not. The lines show in both the Scene and Game views.
```csharp
using Elintria;

public class LaserSight : ElintriaScript
{
    public float range = 30f;

    public override void Update()
    {
        Vector3 from = transform.position;
        Vector3 dir = transform.forward;
        if (Physics.Raycast(from, dir, out RaycastHit hit, range))
            Debug.DrawLine(from, hit.point, Color.green);
        else
            Debug.DrawRay(from, dir * range, Color.red);
    }
}
```

## Example: Assertions and exceptions
file: Scripts/WaveConfig.cs
`Assert` catches bad setup early; `LogException` reports a caught exception without stopping the game.
```csharp
using System;
using Elintria;

public class WaveConfig : ElintriaScript
{
    public int waves = 5;
    public string dataFile = "Assets/Data/waves.json";

    public override void Start()
    {
        Debug.Assert(waves > 0, "waves must be at least 1");
        try
        {
            object data = Json.Parse(Content.ReadAllText(dataFile) ?? "");
            Debug.Log("Loaded " + dataFile);
        }
        catch (Exception e)
        {
            Debug.LogException(e, gameObject);
        }
    }
}
```

# Time
summary: Frame timing: deltaTime for smooth movement, time since start, and the game's time scale.
see: ElintriaScript, Mathf

Multiply anything that changes "per second" by `Time.deltaTime`, the seconds since the last frame, so the game runs at the same speed on every frame rate. `Time.time` counts seconds since play started. Set `Time.timeScale` to slow the game down, speed it up, or pause it at 0.

> note: `time` and `deltaTime` are both scaled by `timeScale`. While it is 0, `Update` still runs (so menus work), but `deltaTime` is 0 and `time` stands still. The "unscaled" members are kept for Unity code and currently return the scaled values, so for real elapsed time use `System.Diagnostics.Stopwatch`.

## Members
fixedDeltaTime: The interval between physics steps, in seconds. Use it inside FixedUpdate.
unscaledTime: Kept for Unity code. In Elintria it currently returns the same as `time`, so it slows and stops with timeScale too.
unscaledDeltaTime: Kept for Unity code. It currently returns the same as `deltaTime` (0 while paused).
realtimeSinceStartup: Kept for Unity code. It currently returns the same as `time`. For real elapsed time, use `System.Diagnostics.Stopwatch`.
fixedTime: Kept for Unity code. It currently returns the same as `time`.
smoothDeltaTime: Kept for Unity code. It currently returns `deltaTime` without smoothing.
maximumDeltaTime: The longest deltaTime a single frame reports, so one long hitch does not move things too far.

## Example: Frame-rate independent movement
file: Scripts/Conveyor.cs
```csharp
using Elintria;

public class Conveyor : ElintriaScript
{
    public float metresPerSecond = 2f;
    public GameObject cargo;

    public override void Update()
    {
        if (cargo != null)
            cargo.transform.position += Vector3.right * metresPerSecond * Time.deltaTime;
    }
}
```

## Example: Pause menu with timeScale
file: Scripts/PauseMenu.cs
Setting `timeScale` to 0 freezes everything driven by `deltaTime`. `Update` keeps running, so the menu can still read keys and draw. A `Stopwatch` measures real time for the pulse, because `Time.time` stands still during the pause.
```csharp
using System.Diagnostics;
using Elintria;

public class PauseMenu : ElintriaScript
{
    bool paused;
    readonly Stopwatch clock = Stopwatch.StartNew();

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
            Time.timeScale = paused ? 0f : 1f;
            GameWindow.SetEscapeQuits(!paused);
        }
        if (paused)
        {
            float pulse = 0.5f + 0.5f * Mathf.Sin((float)clock.Elapsed.TotalSeconds * 4f);
            ScreenGUI.Label(new Rect(20, 20, 300, 40), "PAUSED", null, new Color(1f, 1f, 1f, pulse));
        }
    }
}
```

## Example: Slow motion and an FPS counter
file: Scripts/SlowMo.cs
```csharp
using Elintria;

public class SlowMo : ElintriaScript
{
    public float slowScale = 0.25f;

    public override void Update()
    {
        Time.timeScale = Input.GetKey(KeyCode.Q) ? slowScale : 1f;

        float fps = Time.deltaTime > 0f ? Time.timeScale / Time.deltaTime : 0f;
        ScreenGUI.Text(10, 10, "FPS " + Mathf.RoundToInt(fps) + "   frame " + Time.frameCount +
                               "   played " + Time.time.ToString("0.0") + " s");
    }
}
```

# Random
summary: Quick random numbers, points and rotations from a shared generator.
see: Rng, Mathf

`Random` gives random floats, ints, points in circles and spheres, and random rotations, all from one shared generator. Call `InitState` with a seed to make a sequence repeatable. For several independent repeatable streams (per level, per chunk), use [[Rng]] from Elintria.Procedural.

## Members
InitState: Seeds the generator: the same seed gives the same sequence of numbers.
@seed: Any number.
value: A random float from 0 to 1 (inclusive).
Range(float min, float max): A random float from `min` to `max`, both inclusive.
Range(int min, int max): A random int from `min` up to but **not** including `max`. `Range(0, 3)` gives 0, 1 or 2.
insideUnitSphere: A random point inside a sphere of radius 1.
insideUnitCircle: A random point inside a circle of radius 1.
onUnitSphere: A random point on the surface of a sphere of radius 1 (a random direction).
rotation: A random rotation.

## Example: Random loot with weights
file: Scripts/LootDrop.cs
```csharp
using Elintria;

public class LootDrop : ElintriaScript
{
    public override void OnDestroy()
    {
        float roll = Random.value;
        string item = roll < 0.6f ? "coin" : roll < 0.9f ? "potion" : "rare gem";
        int amount = item == "coin" ? Random.Range(1, 6) : 1;   // 1..5
        Debug.Log(name + " dropped " + amount + " x " + item);
    }
}
```

## Example: Scattering objects
file: Scripts/Scatter.cs
`insideUnitCircle` spreads objects over a disc; `rotation` gives each a random orientation.
```csharp
using Elintria;

public class Scatter : ElintriaScript
{
    public GameObject rockTemplate;
    public int count = 30;
    public float radius = 15f;

    public override void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 c = Random.insideUnitCircle * radius;
            GameObject r = Instantiate(rockTemplate, transform.position + new Vector3(c.x, 0f, c.y), Random.rotation);
            r.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
            r.SetActive(true);
        }
    }
}
```

## Example: Repeatable randomness with a seed
file: Scripts/DailyChallenge.cs
The same seed gives the same "random" layout every time, so everyone gets the same daily level.
```csharp
using System;
using Elintria;

public class DailyChallenge : ElintriaScript
{
    public override void Start()
    {
        int seed = DateTime.UtcNow.Year * 1000 + DateTime.UtcNow.DayOfYear;
        Elintria.Random.InitState(seed);   // "Elintria." because System also has a Random
        for (int i = 0; i < 5; i++)
            Debug.Log("Enemy " + i + " at " + (Elintria.Random.onUnitSphere * 20f));
    }
}
```

# Project
summary: Where the running project's files are: the project folder in the editor, the game folder in a build.
see: Content

`Project.path` is the open project's folder in the editor and the game's own folder in a built game. The two are laid out the same way, so `Project.Resolve("Assets/x")` works in both. To *read* assets in a built game, use [[Content]], which can also read the encrypted bundle.

## Example: Finding a folder next to the game
file: Scripts/ScreenshotTaker.cs
```csharp
using System.IO;
using Elintria;

public class ScreenshotTaker : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F12)) return;
        string dir = Project.Resolve("Screenshots");
        Directory.CreateDirectory(dir);
        string file = Path.Combine(dir, "shot_" + Time.frameCount + ".bmp");
        GameWindow.Capture(file);
        Debug.Log("Saved " + file);
    }
}
```

## Example: Showing where the project lives
file: Scripts/WhereAmI.cs
```csharp
using Elintria;

public class WhereAmI : ElintriaScript
{
    public override void Start()
    {
        Debug.Log("Project folder: " + Project.path);
        Debug.Log("Assets folder:  " + Project.assetsPath);
        Debug.Log("Level file:     " + Project.Resolve("Assets/Levels/1.json"));
    }
}
```

## Example: Accepting absolute or relative paths
file: Scripts/ConfigPath.cs
`Resolve` leaves an absolute path alone, so a setting can be either.
```csharp
using Elintria;

public class ConfigPath : ElintriaScript
{
    public string configFile = "Settings/game.ini";   // or C:\Games\game.ini

    public override void Start()
    {
        string full = Project.Resolve(configFile);
        Debug.Log(System.IO.File.Exists(full) ? "Config at " + full : "No config at " + full);
    }
}
```

# Content
summary: Read your project's files in the editor and from inside a built game's encrypted bundle.
see: Json, Project, manual-files

A built game packs its assets into an encrypted `Game.econtent` bundle, which `System.IO` cannot open. `Content` reads files the same way everywhere: from the bundle in a build, from disk in the editor. Pass project-relative paths such as `"Assets/Data/items.json"`.

DLC: a game can take more bundles made for it (Build Settings > DLC), which it mounts on top of its own from the `DLC` folder beside `Game.exe` as it starts, or when a script calls `MountBundle`. A DLC's files are then read like the game's, its scenes load by name, and its scripts can be used. `Bundles` lists the DLC mounted, and `BundleOf` tells which one a file comes from.

## Example: Loading a JSON data file
file: Scripts/ShopData.cs
```csharp
using System.Collections.Generic;
using Elintria;

public class ShopData : ElintriaScript
{
    public string file = "Assets/Data/shop.json";

    public override void Start()
    {
        string json = Content.ReadAllText(file);
        if (json == null) { Debug.LogError(file + " not found"); return; }
        var root = Json.Parse(json) as Dictionary<string, object>;
        foreach (object o in Json.List(root, "items"))
        {
            var item = (Dictionary<string, object>)o;
            Debug.Log(Json.Str(item, "name") + " costs " + Json.Int(item, "price"));
        }
    }
}
```

## Example: Reading every level in a folder
file: Scripts/LevelList.cs
```csharp
using Elintria;

public class LevelList : ElintriaScript
{
    public override void Start()
    {
        if (!Content.DirectoryExists("Assets/Levels")) return;
        foreach (string f in Content.List("Assets/Levels"))
            if (f.EndsWith(".txt"))
                Debug.Log(f + ": " + Content.ReadAllLines("Assets/Levels/" + f).Length + " rows");
    }
}
```

## Example: Text-based tile maps
file: Scripts/AsciiLevel.cs
Each character of a text file becomes a block, a quick way to sketch levels.
```csharp
using Elintria;

public class AsciiLevel : ElintriaScript
{
    public string levelFile = "Assets/Levels/level1.txt";

    public override void Start()
    {
        string[] rows = Content.ReadAllLines(levelFile);
        if (rows == null) return;
        for (int z = 0; z < rows.Length; z++)
            for (int x = 0; x < rows[z].Length; x++)
                if (rows[z][x] == '#')
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.position = new Vector3(x, 0.5f, -z);
                }
    }
}
```


## Members
MountBundle: Mounts a DLC bundle built for this game, on top of the game's own content, and loads its scripts. A path not rooted is taken from the game's folder. False when it cannot: a file built for another project, a renamed one, a damaged one, or the editor (which reads the project instead).
@path: The bundle, such as `"DLC/Expansion"` (".econtent" may be left off).
@error: Why it did not mount.
UnmountBundle: Takes a DLC away again; what was loaded from it stays loaded.
@name: The DLC's name (its file name without .econtent).
Bundles: The DLC mounted now, oldest first.
BundleErrors: Why a DLC beside the game did not mount at start, one "Name: reason" each.
BundleOf: Where a file is read from: "Game", a DLC's name, or "" for disk.
SupportsBundles: True when this game was built able to open DLC.
IsMounted: Whether the DLC called `name` is mounted.
BundleMounted: Raised after a DLC mounts and its scripts are loaded.
BundleUnmounted: Raised after a DLC is unmounted.

## Example: Mounting a downloaded DLC
file: Scripts/DlcMenu.cs
```csharp
using Elintria;

public class DlcMenu : ElintriaScript
{
    public override void Start()
    {
        Content.BundleMounted += name => Debug.Log("DLC ready: " + name);
        foreach (string dlc in Content.Bundles) Debug.Log("Installed: " + dlc);
        foreach (string why in Content.BundleErrors) Debug.LogWarning("Not loaded: " + why);
    }

    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F5)) return;
        string error;
        if (Content.MountBundle("DLC/Expansion", out error))
            SceneManager.LoadScene("ExpansionHub");
        else
            Debug.LogWarning("Could not load the expansion: " + error);
    }
}
```

# Json
summary: Parse and write JSON with plain dictionaries and lists, plus helpers to read values safely.
see: Content

`Json.Parse` turns JSON text into plain C# values: `Dictionary<string, object>` for objects, `List<object>` for arrays, and `string`, `double`, `bool` or `null` for the rest. `Json.Stringify` writes them back. The `Str`, `Float`, `Int`, `Bool`, `List`, `Obj` and `Strings` helpers read a key from a dictionary and fall back to a default when it is missing.

## Members
Parse: The value `text` holds: dictionaries, lists, strings, numbers (double), bools or null. Throws FormatException, naming the line, when it is not JSON.
Stringify: `value` written as JSON text; `pretty` indents it two spaces a level.
Str: `d[key]` as a string, or `def` when it is missing or null.
Float: `d[key]` as a float, or `def` when it is missing or not a number.
Int: `d[key]` as an int, or `def` when it is missing or not a number.
Bool: `d[key]` as a bool, or `def` when it is missing.
List: `d[key]` as a list, or null when it is missing.
Obj: `d[key]` as a dictionary (a JSON object), or null.

## Example: Reading nested settings
file: Scripts/Settings.cs
```csharp
using System.Collections.Generic;
using Elintria;

public class Settings : ElintriaScript
{
    public override void Start()
    {
        string text = "{ \"player\": { \"speed\": 5.5, \"name\": \"Ash\" }, \"music\": true, \"levels\": [\"Forest\", \"Cave\"] }";
        var root = (Dictionary<string, object>)Json.Parse(text);
        var player = Json.Obj(root, "player");

        float speed = Json.Float(player, "speed", 4f);
        string playerName = Json.Str(player, "name", "Player");
        bool music = Json.Bool(root, "music");
        string[] levels = Json.Strings(root, "levels");

        Debug.Log(playerName + " runs at " + speed + "; music " + music + "; " + levels.Length + " levels");
    }
}
```

## Example: Saving and loading a game
file: Scripts/SaveGame.cs
`Stringify` turns dictionaries and lists back into text; `pretty` makes it readable.
```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Elintria;

public class SaveGame : ElintriaScript
{
    string SavePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyGame", "save.json");

    public void Save(int level, float health, List<object> items)
    {
        var data = new Dictionary<string, object> { { "level", level }, { "health", health }, { "items", items } };
        Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
        File.WriteAllText(SavePath, Json.Stringify(data, true));
    }

    public int LoadLevel()
    {
        if (!File.Exists(SavePath)) return 1;
        var data = Json.TryParse(File.ReadAllText(SavePath)) as Dictionary<string, object>;
        return data == null ? 1 : Json.Int(data, "level", 1);
    }
}
```

## Example: Tolerating bad input
file: Scripts/ModLoader.cs
`TryParse` returns a fallback instead of throwing on broken JSON; `Parse` throws with the line number, for tools.
```csharp
using System;
using Elintria;

public class ModLoader : ElintriaScript
{
    public string modFile = "Assets/Mods/mod.json";

    public override void Start()
    {
        string text = Content.ReadAllText(modFile) ?? "";
        object mod = Json.TryParse(text, null);
        if (mod == null)
        {
            try { Json.Parse(text); }
            catch (FormatException e) { Debug.LogWarning("Mod skipped: " + e.Message); }
            return;
        }
        Debug.Log("Mod loaded: " + Json.Stringify(mod));
    }
}
```

# SceneManager
summary: Load another scene, pass data to it, and track loading progress.
see: Content

`SceneManager.LoadScene("Level2")` switches scenes at the end of the frame, behind the loading screen. Pass a string with `data` to tell the next scene something (a spawn point, a difficulty); the new scene reads it from `sceneData`.

## Members
LoadScene: Loads `scene` (by name or path) once this frame ends. Returns false when the scene does not exist.
@scene: "Level2" or "Scenes/Level2.elscene".
@data: Any text to hand to the new scene (read it with `sceneData`).

## Example: A level exit
file: Scripts/LevelExit.cs
```csharp
using Elintria;

public class LevelExit : ElintriaScript
{
    public string nextScene = "Level2";
    public string spawnPoint = "Spawn_FromLevel1";

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            SceneManager.LoadScene(nextScene, spawnPoint);
    }
}
```

## Example: Reading the data in the next scene
file: Scripts/SpawnAtEntrance.cs
```csharp
using Elintria;

public class SpawnAtEntrance : ElintriaScript
{
    public override void Start()
    {
        Debug.Log("Now in " + SceneManager.activeSceneName);
        string spawn = SceneManager.sceneData;
        GameObject point = string.IsNullOrEmpty(spawn) ? null : GameObject.Find(spawn);
        if (point != null) transform.position = point.transform.position;
    }
}
```

## Example: A main menu with a loading bar
file: Scripts/MainMenu.cs
```csharp
using Elintria;

public class MainMenu : ElintriaScript
{
    public override void Update()
    {
        float p = SceneManager.loadProgress;
        if (p >= 0f)
        {
            ScreenGUI.Bar(new Rect(100, 400, 400, 16), p, Color.cyan, new Color(0f, 0f, 0f, 0.5f));
            return;
        }
        if (ScreenGUI.Button(new Rect(100, 300, 200, 48), "New Game")) SceneManager.LoadScene("Level1", "difficulty=normal");
        if (ScreenGUI.Button(new Rect(100, 360, 200, 48), "Quit")) GameWindow.Quit();
    }
}
```

# Frame
summary: A once-per-frame event that runs before any script's Update.
see: ElintriaScript

Subscribe to `Frame.onUpdate` from code that is not a script (a static manager, a plugin) to get a call every frame, before the first script updates.

## Members
onUpdate: Raised once per frame, before scripts update. Add a handler with `+=` and remove it with `-=`.

## Example: A static game clock
file: Scripts/GameClock.cs
```csharp
using Elintria;

public static class GameClock
{
    public static float minutesOfDay = 8f * 60f;
    public static float speed = 10f;   // game minutes per real second
    static bool hooked;

    public static void Begin()
    {
        if (hooked) return;
        hooked = true;
        Frame.onUpdate += () => minutesOfDay = (minutesOfDay + speed * Time.deltaTime) % (24f * 60f);
    }
}

public class ClockStarter : ElintriaScript
{
    public override void Start() { GameClock.Begin(); }
}
```

## Example: Resetting per-frame counters
file: Scripts/HitCounter.cs
```csharp
using Elintria;

public static class HitCounter
{
    public static int thisFrame, lastFrame;
    static HitCounter() { Frame.onUpdate += () => { lastFrame = thisFrame; thisFrame = 0; }; }
    public static void Hit() { thisFrame++; }
}
```

## Example: Unsubscribing when done
file: Scripts/OneShotHook.cs
```csharp
using System;
using Elintria;

public class OneShotHook : ElintriaScript
{
    Action hook;

    public override void Start()
    {
        hook = () => Debug.Log("frame " + Time.frameCount);
        Frame.onUpdate += hook;
    }

    public override void OnDestroy()
    {
        Frame.onUpdate -= hook;   // always unhook what you hooked
    }
}
```

# HostCommands
summary: Text commands between the host (editor, tests, tools) and the running game.
see: Debug

The editor's tools and automated tests can post one-line commands to a running game. A script picks up the ones meant for it with `Take("prefix")` and can answer with `Reply`. Scripts can also use `Post` to send commands to each other.

## Example: A cheat console command
file: Scripts/Cheats.cs
```csharp
using Elintria;

public class Cheats : ElintriaScript
{
    public override void Update()
    {
        string cmd;
        while ((cmd = HostCommands.Take("cheat.")) != null)
        {
            if (cmd == "cheat.god") { SendMessage("SetInvulnerable", true); HostCommands.Reply("god mode on"); }
            else if (cmd.StartsWith("cheat.give ")) { SendMessage("Give", cmd.Substring(11)); HostCommands.Reply("gave " + cmd.Substring(11)); }
        }
    }
}
```

## Example: Scripts talking through commands
file: Scripts/Doorbell.cs
```csharp
using Elintria;

public class Doorbell : ElintriaScript
{
    public void Ring() { HostCommands.Post("house.bell " + name); }
}

public class House : ElintriaScript
{
    public override void Update()
    {
        string c = HostCommands.Take("house.bell");
        if (c != null) Debug.Log("Someone rang: " + c.Substring(11));
    }
}
```

## Example: Reporting state to a test
file: Scripts/TestProbe.cs
```csharp
using Elintria;

public class TestProbe : ElintriaScript
{
    public override void Update()
    {
        string c = HostCommands.Take("probe.where");
        if (c != null) HostCommands.Reply("player at " + GameObject.FindWithTag("Player").transform.position);
    }
}
```

# IComponentProvider
summary: Lets a plugin supply components the engine has no native version of.
see: ManagedComponent

Implement `IComponentProvider` in a class compiled with your scripts, and the engine creates one at load. When a script asks an object for a component type that nothing else supplies, the engine asks each provider. Whatever `Provide` returns is attached to the object, so the same instance is found the next time.

## Members
Provide: Returns a new component of type `wanted` (or a subclass) for `go`, or null when this provider does not supply that type or the object should not have one.
@go: The object the component is wanted on.
@wanted: The component type that was asked for.

## Example: Supplying a stats component on demand
file: Scripts/StatsProvider.cs
```csharp
using System;
using Elintria;

public class Stats : ManagedComponent
{
    public int strength = 10;
    public int agility = 10;
}

public class StatsProvider : IComponentProvider
{
    public ManagedComponent Provide(GameObject go, Type wanted)
    {
        if (wanted != typeof(Stats)) return null;
        return go.CompareTag("Enemy") ? new Stats { strength = 12, agility = 8 } : new Stats();
    }
}
```

## Example: Reading a provided component
file: Scripts/StatsUser.cs
```csharp
using Elintria;

public class StatsUser : ElintriaScript
{
    public override void Start()
    {
        Stats s = GetComponent<Stats>();   // created by StatsProvider the first time
        Debug.Log(name + " strength " + s.strength);
    }
}
```

## Example: Providing data from a file per object
file: Scripts/DialogueProvider.cs
```csharp
using System;
using Elintria;

public class Dialogue : ManagedComponent { public string[] lines = new string[0]; }

public class DialogueProvider : IComponentProvider
{
    public ManagedComponent Provide(GameObject go, Type wanted)
    {
        if (wanted != typeof(Dialogue)) return null;
        string[] lines = Content.ReadAllLines("Assets/Dialogue/" + go.name + ".txt");
        return lines == null ? null : new Dialogue { lines = lines };
    }
}
```

# Space
summary: Whether a movement or rotation is in world space or the object's own local space.
see: Transform

Pass `Space.Self` to [[Transform.Translate]] or [[Transform.Rotate]] to move or turn along the object's own axes, and `Space.World` to use the world's fixed axes.

## Members
World: The world's axes: +Y is always straight up, whichever way the object faces.
Self: The object's own axes: "forward" is wherever the object is facing.

## Example: Moving along world or local axes
file: Scripts/SpaceDemo.cs
```csharp
using Elintria;

public class SpaceDemo : ElintriaScript
{
    public override void Update()
    {
        // Forward the way the object faces:
        transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
        // Always drift up in the world, even if the object is tilted:
        transform.Translate(Vector3.up * 0.2f * Time.deltaTime, Space.World);
    }
}
```

## Example: Rolling an aircraft
file: Scripts/Plane.cs
Roll and pitch about the plane's own axes; yaw about the world up so the horizon stays level.
```csharp
using Elintria;

public class PlaneControl : ElintriaScript
{
    public float rate = 60f;

    public override void Update()
    {
        float roll = -Input.GetAxisHorizontal() * rate * Time.deltaTime;
        float pitch = Input.GetAxisVertical() * rate * Time.deltaTime;
        transform.Rotate(new Vector3(pitch, 0f, roll), Space.Self);
        if (Input.GetKey(KeyCode.Q)) transform.Rotate(Vector3.up, -rate * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(Vector3.up, rate * Time.deltaTime, Space.World);
    }
}
```

## Example: Passing Space through your own API
file: Scripts/Nudge.cs
```csharp
using Elintria;

public class Nudge : ElintriaScript
{
    public bool local = true;

    public void By(Vector3 offset)
    {
        transform.Translate(offset, local ? Space.Self : Space.World);
    }
}
```

# PrimitiveType
summary: The built-in shapes GameObject.CreatePrimitive can make.
see: GameObject

## Members
Sphere: A sphere of diameter 1 with a sphere collider.
Capsule: A capsule 2 units tall and 1 wide.
Cylinder: A cylinder 2 units tall and 1 wide.
Cube: A 1x1x1 cube with a box collider.
Plane: A flat 10x10 plane, facing up.
Quad: A flat 1x1 square, facing +Z.

## Example: Making one of each
file: Scripts/ShapeShowcase.cs
```csharp
using System;
using Elintria;

public class ShapeShowcase : ElintriaScript
{
    public override void Start()
    {
        PrimitiveType[] all = (PrimitiveType[])Enum.GetValues(typeof(PrimitiveType));
        for (int i = 0; i < all.Length; i++)
        {
            GameObject g = GameObject.CreatePrimitive(all[i]);
            g.name = all[i].ToString();
            g.transform.position = new Vector3(i * 2.5f, 1f, 0f);
        }
    }
}
```

## Example: Choosing a shape from the Inspector
file: Scripts/ShapeSpawner.cs
Inspector fields cannot hold an enum, so the shape is picked with an int.
```csharp
using Elintria;

public class ShapeSpawner : ElintriaScript
{
    public int shape = 3;   // 0 Sphere, 1 Capsule, 2 Cylinder, 3 Cube, 4 Plane, 5 Quad

    public override void Start()
    {
        GameObject g = GameObject.CreatePrimitive((PrimitiveType)Mathf.Clamp(shape, 0, 5));
        g.transform.position = transform.position;
    }
}
```

## Example: A quick bullet
file: Scripts/QuickBullet.cs
```csharp
using Elintria;

public class QuickBullet : ElintriaScript
{
    public static GameObject Fire(Vector3 from, Vector3 dir, float speed)
    {
        GameObject b = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        b.transform.position = from;
        b.transform.localScale = Vector3.one * 0.15f;
        Rigidbody rb = b.AddComponent<Rigidbody>();
        rb.velocity = dir.normalized * speed;
        ElintriaScript.Destroy(b, 4f);
        return b;
    }
}
```

# FindObjectsSortMode
summary: How FindObjectsByType orders what it finds.
see: ElintriaScript

## Members
None: No particular order (fastest).
InstanceID: Sorted by instance id, which is stable from frame to frame.

## Example: Unsorted search
file: Scripts/CountEnemies.cs
```csharp
using Elintria;

public class CountEnemies : ElintriaScript
{
    public override void Start()
    {
        var all = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
        Debug.Log(all.Length + " rigidbodies in the scene");
    }
}
```

## Example: Stable order for round-robin updates
file: Scripts/RoundRobin.cs
```csharp
using Elintria;

public class RoundRobin : ElintriaScript
{
    int next;

    public override void Update()
    {
        Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.InstanceID);
        if (lights.Length == 0) return;
        next = (next + 1) % lights.Length;
        lights[next].intensity = 0.5f + Random.value;   // flicker one light a frame
    }
}
```

## Example: Including inactive objects, sorted
file: Scripts/AllCheckpoints.cs
```csharp
using Elintria;

public class Checkpoint : ElintriaScript { }

public class AllCheckpoints : ElintriaScript
{
    public override void Start()
    {
        Checkpoint[] cps = FindObjectsByType<Checkpoint>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
        Debug.Log(cps.Length + " checkpoints, active or not");
    }
}
```

# FindObjectsInactive
summary: Whether a Find call should also return objects that are switched off.
see: FindObjectsSortMode

## Members
Exclude: Only active objects (the default).
Include: Active and inactive objects alike.

## Example: Finding a hidden menu
file: Scripts/MenuFinder.cs
```csharp
using Elintria;

public class PauseScreen : ElintriaScript { }

public class MenuFinder : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        PauseScreen menu = FindFirstObjectByType<PauseScreen>(FindObjectsInactive.Include);
        if (menu != null) menu.gameObject.SetActive(!menu.gameObject.activeSelf);
    }
}
```

## Example: Only what is active
file: Scripts/ActiveTurrets.cs
```csharp
using Elintria;

public class Turret2 : ElintriaScript { }

public class ActiveTurrets : ElintriaScript
{
    public override void Start()
    {
        Debug.Log(FindObjectsByType<Turret2>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length + " turrets on");
    }
}
```

## Example: Any one, including disabled
file: Scripts/AnyBoss.cs
```csharp
using Elintria;

public class Boss : ElintriaScript { }

public class AnyBoss : ElintriaScript
{
    public override void Start()
    {
        Boss b = FindAnyObjectByType<Boss>(FindObjectsInactive.Include);
        if (b != null && !b.gameObject.activeSelf) Debug.Log("The boss is waiting, inactive, in the scene");
    }
}
```
