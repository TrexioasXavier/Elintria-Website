# page: manual-getting-started | Getting Started
summary: Write your first C# script, attach it to an object and watch it run in Play mode.

Elintria scripts are C# classes that derive from [[ElintriaScript]]. A script is a *component*: you attach it to a [[GameObject]] in the scene, and while the game runs the engine calls its methods, such as `Start` once and `Update` every frame.

## Creating a script

- Choose **Assets > Create > C# Script** (or right-click in the Project window and pick **Create > C# Script**) and give it a name such as `Mover`.
- The file is created in the project's `Scripts` folder. The class name must match the file name, and every script class needs a unique name.
- Double-click the script to open it in the built-in Script Editor, or in Visual Studio if you set that as your external editor.

## Your first script

```csharp
using Elintria;

public class Mover : ElintriaScript
{
    public float speed = 3f;          // shows in the Inspector

    public override void Start()
    {
        Debug.Log("Mover started on " + name);
    }

    public override void Update()
    {
        // Move forward along the object's own +Z axis, frame-rate independent.
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
```

## Attaching it

- Select an object in the Hierarchy, click **Add Component** in the Inspector and find your script under **Scripts** (or drag the script file onto the object).
- Public fields like `speed` appear in the Inspector. Change them there. The value is saved with the scene.
- Press **Play** (Ctrl+P). The script compiles, `Start` runs once, and `Update` runs every frame. Press Play again to stop; the scene goes back to how it was.

> tip: `Debug.Log` writes to the Console window. It is the quickest way to see what a script is doing.

## Where to go next

- [[manual-lifecycle|Script Lifecycle]]: the order the engine calls your methods in.
- [[manual-inspector|Inspector Fields]]: which field types the Inspector can show and save.
- [[GameObject]] and [[Transform]]: the two classes almost every script uses.
- [[manual-standard|The Standard Package]]: ready-made controllers, weapons and doors to build on.

# page: manual-lifecycle | Script Lifecycle
summary: Which methods the engine calls on your script, and when.

Override these [[ElintriaScript]] methods to run code at the right moment. They are all optional: override only the ones you need.

## Order of calls

- `Awake()`: once, before any `Start`, even if the script is disabled. Use it to set up references inside the object.
- `OnEnable()`: when the script or its object becomes enabled.
- `Start()`: once, before the script's first `Update`. Use it for setup that needs other objects to exist.
- `FixedUpdate()`: at a fixed rate ([[Time.fixedDeltaTime]]), independent of frame rate. Put physics here: forces, velocity changes, [[Rigidbody]] moves.
- `Update()`: once per frame. Put input, gameplay logic and anything driven by [[Time.deltaTime]] here.
- `LateUpdate()`: after every `Update` in the scene. Use it for cameras that follow something that moved in `Update`.
- `OnDisable()` and `OnDestroy()`: when the script is switched off or removed.

## Physics callbacks

- `OnTriggerEnter / Stay / Exit(Collider other)`: when a trigger collider overlaps another collider.
- `OnCollisionEnter / Stay / Exit(Collision collision)`: when two solid colliders touch.
- `OnControllerColliderHit(ControllerColliderHit hit)`: when a [[CharacterController]] on this object bumps into something.

```csharp
using Elintria;

public class LifecycleLogger : ElintriaScript
{
    public override void Awake()       { Debug.Log("Awake"); }
    public override void OnEnable()    { Debug.Log("OnEnable"); }
    public override void Start()       { Debug.Log("Start"); }
    public override void FixedUpdate() { /* physics */ }
    public override void Update()      { /* every frame */ }
    public override void LateUpdate()  { /* after every Update */ }
    public override void OnDisable()   { Debug.Log("OnDisable"); }
    public override void OnDestroy()   { Debug.Log("OnDestroy"); }
}
```

> note: In the Standard package, components that override `FixedUpdate` (such as [[Agent]]) expect you to call `base.FixedUpdate()` from your own override.

# page: manual-inspector | Inspector Fields
summary: How public fields become editable, saved settings in the Inspector.

Every public field of a script is shown in the Inspector and saved with the scene. Private fields marked with the [[SerializeFieldAttribute|SerializeField]] attribute are shown too, and the [[HideInInspectorAttribute|HideInInspector]] attribute hides a public field.

## Supported field types

- `float`, `int`, `bool`, `string`
- [[Vector2]], [[Vector3]], [[Vector4]]
- [[GameObject]]: drag an object from the Hierarchy into the slot.
- `List<T>` of the types above, for example `List<GameObject>` or `List<float>`.

> warning: Declare one field per line. `public float a, b;` only shows the first field. Don't give a `List` field a collection initializer with items in it either (`new List<float> { 1, 2 }`): fill the list in `Start` instead.

```csharp
using System.Collections.Generic;
using Elintria;

public class EnemySettings : ElintriaScript
{
    public float health = 100f;
    public int scoreValue = 10;
    public bool canFly;
    public string displayName = "Grunt";
    public Vector3 patrolOffset = new Vector3(0f, 0f, 5f);
    public GameObject target;                                   // drag the player here
    public List<GameObject> waypoints = new List<GameObject>(); // add as many as you like

    [SerializeField] private float aggroRange = 12f;            // private, but editable
    [HideInInspector] public int killCount;                     // public, but hidden
}
```

## Values while playing

Changes you make in the Inspector during Play mode are passed to the running game straight away. When you stop, they go back to the values saved with the scene.

# page: manual-coroutines | Coroutines and Timers
summary: Spread work over several frames with coroutines, Invoke and timers.

A coroutine is a method that returns `IEnumerator` and can pause with `yield return`. Start one with [[ElintriaScript.StartCoroutine]].

```csharp
using System.Collections;
using Elintria;

public class Blinker : ElintriaScript
{
    public float interval = 0.5f;

    public override void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            gameObject.SetActive(!gameObject.activeSelf);
            yield return new WaitForSeconds(interval);
        }
    }
}
```

## What you can yield

- `null`: wait one frame.
- [[WaitForSeconds]] / [[WaitForSecondsRealtime]]: wait a number of seconds.
- [[WaitForFixedUpdate]] / [[WaitForEndOfFrame]]: wait for the next physics step, or the end of this frame.
- [[WaitUntil]] / [[WaitWhile]]: wait for a condition.
- Another coroutine's `IEnumerator`, or your own [[CustomYieldInstruction]].

## Invoke

[[ElintriaScript.Invoke]] calls a method by name after a delay. [[ElintriaScript.InvokeRepeating]] keeps calling it at a fixed rate until [[ElintriaScript.CancelInvoke]].

```csharp
public override void Start()
{
    Invoke("Explode", 3f);            // once, after 3 seconds
    InvokeRepeating("Tick", 1f, 1f);  // every second, starting in 1 second
}
void Explode() { Debug.Log("Boom"); }
void Tick() { Debug.Log("tick"); }
```

# page: manual-objects | Finding, Creating and Destroying Objects
summary: Get references to other objects, spawn new ones and remove them.

## Getting a reference

- Best: a public [[GameObject]] field filled in the Inspector.
- By name: [[GameObject.Find]] (searches the scene, so cache the result in `Start`).
- By tag: [[GameObject.FindWithTag]] and [[GameObject.FindGameObjectsWithTag]].
- By type: [[ElintriaScript.FindObjectOfType]] finds a script or component anywhere in the scene.
- On the same object: [[ElintriaScript.GetComponent]].

## Creating

- [[GameObject.CreatePrimitive]] makes a cube, sphere, capsule, cylinder, plane or quad.
- [[ElintriaScript.Instantiate]] copies an existing object, including its scripts and children. A common pattern is to keep an inactive "template" object in the scene and copy it.
- `new GameObject("Name")` makes an empty object.

## Destroying

[[ElintriaScript.Destroy]] removes an object (or one component) at the end of the frame, or after a delay.

> warning: A destroyed object's C# reference is not `null`. Check [[GameObject.IsAlive]] (or `if (obj)`) before using a reference you kept from an earlier frame.

```csharp
using Elintria;

public class CoinRain : ElintriaScript
{
    public GameObject coinTemplate;   // an inactive coin object in the scene
    public int count = 20;

    public override void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 p = transform.position + new Vector3(Random.Range(-5f, 5f), 10f, Random.Range(-5f, 5f));
            GameObject coin = Instantiate(coinTemplate, p, Quaternion.identity);
            coin.SetActive(true);
            Destroy(coin, 8f);        // gone after 8 seconds
        }
    }
}
```

# page: manual-collisions | Collisions and Triggers
summary: React when objects touch or enter an area.

Collision callbacks are called on scripts attached to objects that have a collider. At least one of the two objects needs a [[Rigidbody]] or a [[CharacterController]] for the solver to report the contact.

- A **solid** contact calls `OnCollisionEnter`, `OnCollisionStay` and `OnCollisionExit` with a [[Collision]].
- A collider with **Is Trigger** ticked lets things pass through. Overlaps call `OnTriggerEnter`, `OnTriggerStay` and `OnTriggerExit` with the other [[Collider]].

```csharp
using Elintria;

public class Lava : ElintriaScript
{
    public float damagePerSecond = 25f;

    public override void OnTriggerStay(Collider other)
    {
        // Every script on the other object with an ApplyDamage(float) method gets this.
        other.gameObject.SendMessage("ApplyDamage", damagePerSecond * Time.fixedDeltaTime);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name + " landed on the lava at " + collision.relativeVelocity.magnitude + " m/s");
    }
}
```

> tip: For shapes without a script, the Standard package's [[TriggerZone]] and [[PressurePlate]] fire Inspector events with no code at all.

# page: manual-messages | Messages and Inspector Events
summary: Call methods on other objects by name, and wire up events in the Inspector.

## SendMessage

[[GameObject.SendMessage]] calls a method by name on every script of an object, so the caller does not need to know which script handles it. A method that takes one parameter receives the value you pass.

```csharp
target.SendMessage("ApplyDamage", 10f);   // any script with ApplyDamage(float) runs
target.SendMessage("Open");               // any script with Open() runs
```

## Inspector events

Standard components expose *events* as two fields: a list of target objects and a message, for example `onOpenTargets` and `onOpenMessage` on a [[Door]]. When the event fires, each target gets the message through [[StdEvent.Fire]].

- `Open`: calls `Open()` on the targets' scripts.
- `GoToFloor:2`: calls `GoToFloor(int)` with 2.
- `SetActive:false`: switches the targets off (no script needed).
- `Destroy`: removes the targets.

Your own components can offer the same thing:

```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class Button3D : ElintriaScript
{
    public List<GameObject> onPressTargets = new List<GameObject>();
    public string onPressMessage = "Activate";

    public void Press()
    {
        StdEvent.Fire(onPressTargets, onPressMessage, gameObject);
    }
}
```

# page: manual-standard | The Standard Package
summary: Ready-made gameplay components you can use as they are or build on.

**Elintria Standard** is a package of C# components for common gameplay: characters, vehicles, interaction, inventory, weapons, spawners, mechanisms, IK, ragdolls, cloth, destruction and buoyancy.

## Turning it on

- Open the **Package Manager** (Window menu) and enable **Elintria Standard**.
- The components then appear under **Add Component > Elintria Standard - <category>**.
- In your own scripts, add `using Elintria.Standard;` to use them from code or derive from them.

## How the pieces fit

- [[StdInput]] reads the keyboard and mouse for every Standard component. You can also *override* it to drive a character from code (AI, cutscenes, tests).
- [[Health]] holds hit points. Weapons, hazards and explosions damage anything that has a Health, or a script with an `ApplyDamage(float)` method.
- Events are pairs of fields (`on...Targets` and `on...Message`) that call methods on other objects. See [[manual-messages|Messages and Inspector Events]].

```csharp
using Elintria;
using Elintria.Standard;

// A player made from Standard parts plus one line of your own logic.
public class PlayerSetup : ElintriaScript
{
    public override void Start()
    {
        var controller = GetComponent<ThirdPersonController>();
        var health = GetComponent<Health>();
        var inventory = GetComponent<Inventory>();
        inventory.Add("potion", 3);
        Debug.Log("Ready: " + health.health + " HP, walking at " + controller.walkSpeed + " m/s");
    }
}
```

# page: manual-files | Reading Files in a Built Game
summary: Load your own data files in the editor and in a shipped game.

In a built game, your assets are packed into an encrypted `Game.econtent` bundle, so `System.IO.File` cannot see them. Read project files through [[Content]] instead. It reads from the bundle in a built game and from the project folder in the editor.

```csharp
using Elintria;

public class LevelLoader : ElintriaScript
{
    public string levelFile = "Assets/Data/level1.json";

    public override void Start()
    {
        string text = Content.ReadAllText(levelFile);
        if (text == null) { Debug.LogError("Missing " + levelFile); return; }

        var level = Json.Parse(text) as System.Collections.Generic.Dictionary<string, object>;
        Debug.Log("Level name: " + Json.Str(level, "name") + ", enemies: " + Json.Int(level, "enemies"));
    }
}
```

> note: To write files (save games, settings), use `System.IO` with a folder under the user's profile, such as `Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)`. [[Project.path]] points at the game's own folder, which may be read-only once installed.
