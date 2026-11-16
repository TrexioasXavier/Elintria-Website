# Coroutine
summary: A handle to a running coroutine, returned by StartCoroutine and accepted by StopCoroutine.
see: ElintriaScript, WaitForSeconds, manual-coroutines

[[ElintriaScript.StartCoroutine]] returns a `Coroutine`. Keep it if you might need to stop that coroutine early with [[ElintriaScript.StopCoroutine]]. You can also `yield return` it from another coroutine to wait until it finishes.

## Example: Stopping a coroutine early
file: Scripts/ChargeAttack.cs
```csharp
using System.Collections;
using Elintria;

public class ChargeAttack : ElintriaScript
{
    Coroutine charging;
    float charge;

    public override void Update()
    {
        if (Input.GetMouseButtonDown(0)) charging = StartCoroutine(Charge());
        if (Input.GetMouseButtonUp(0) && charging != null)
        {
            StopCoroutine(charging);
            charging = null;
            Debug.Log("Released at " + Mathf.RoundToInt(charge * 100f) + "% charge");
            charge = 0f;
        }
    }

    IEnumerator Charge()
    {
        while (charge < 1f)
        {
            charge = Mathf.Min(1f, charge + Time.deltaTime / 2f);   // full in 2 seconds
            yield return null;
        }
    }
}
```

## Example: Waiting for another coroutine
file: Scripts/Cutscene.cs
Yielding a `Coroutine` pauses until it completes, which chains steps cleanly.
```csharp
using System.Collections;
using Elintria;

public class Cutscene : ElintriaScript
{
    public GameObject door;

    public override void Start() { StartCoroutine(Play()); }

    IEnumerator Play()
    {
        yield return StartCoroutine(MoveTo(door, door.transform.position + Vector3.up * 3f, 2f));
        Debug.Log("Door is open");
        yield return new WaitForSeconds(1f);
        Debug.Log("Cutscene over");
    }

    IEnumerator MoveTo(GameObject g, Vector3 to, float seconds)
    {
        Vector3 from = g.transform.position;
        for (float t = 0f; t < 1f; t += Time.deltaTime / seconds)
        {
            g.transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
        g.transform.position = to;
    }
}
```

## Example: Only one at a time
file: Scripts/Flash.cs
Stopping the previous coroutine before starting a new one stops two effects fighting over the same value.
```csharp
using System.Collections;
using Elintria;

public class Flash : ElintriaScript
{
    public GameObject lampObject;
    Coroutine current;

    public void Pulse()
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        Light lamp = lampObject.GetComponent<Light>();
        for (float t = 0f; t < 1f; t += Time.deltaTime * 2f)
        {
            lamp.intensity = Mathf.Lerp(5f, 1f, t);
            yield return null;
        }
        current = null;
    }
}
```

# WaitForSeconds
summary: Pauses a coroutine for a number of seconds of game time.
see: WaitForSecondsRealtime, Coroutine, Time

`yield return new WaitForSeconds(2f);` pauses the coroutine for two seconds of game time. Game time slows down and stops with [[Time.timeScale]]. Make one and reuse it if a loop waits the same amount every time.

## Members
WaitForSeconds: Makes a wait of `s` seconds.
@s: How long to wait, in seconds of game time.

## Example: A countdown
file: Scripts/Countdown.cs
```csharp
using System.Collections;
using Elintria;

public class Countdown : ElintriaScript
{
    public int from = 3;

    public override void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        for (int i = from; i > 0; i--)
        {
            Debug.Log(i + "...");
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("GO!");
    }
}
```

## Example: Reusing one wait in a loop
file: Scripts/Regenerate.cs
```csharp
using System.Collections;
using Elintria;

public class Regenerate : ElintriaScript
{
    public float health = 50f;
    public float maxHealth = 100f;
    readonly WaitForSeconds tick = new WaitForSeconds(0.5f);

    public override void Start() { StartCoroutine(Regen()); }

    IEnumerator Regen()
    {
        while (true)
        {
            health = Mathf.Min(maxHealth, health + 2f);
            yield return tick;
        }
    }
}
```

## Example: Respawn after a delay
file: Scripts/Respawn.cs
```csharp
using System.Collections;
using Elintria;

public class Respawn : ElintriaScript
{
    public GameObject spawnPoint;
    public float delay = 3f;

    public void OnDeath() { StartCoroutine(Back()); }

    IEnumerator Back()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(delay);
        transform.position = spawnPoint.transform.position;
        gameObject.SetActive(true);
    }
}
```

> note: Coroutines stop when their object is deactivated. For a respawn like this, run the coroutine on a manager object that stays active.

# WaitForSecondsRealtime
summary: Pauses a coroutine for a number of seconds; kept for Unity code, and currently the same as WaitForSeconds.
see: WaitForSeconds, Time

In Unity this waits in real time, ignoring [[Time.timeScale]]. In Elintria it is currently measured the same way as [[WaitForSeconds]], so code written for Unity compiles and runs, but the wait slows down with the game.

> warning: Don't use it to un-pause a game you paused with `Time.timeScale = 0`, because the wait would never finish. Time real seconds in `Update` with `System.Diagnostics.Stopwatch` instead (see the third example).

## Members
WaitForSecondsRealtime: Makes a wait of `s` seconds.
@s: How long to wait, in seconds.

## Example: Unity code that runs unchanged
file: Scripts/IntroText.cs
```csharp
using System.Collections;
using Elintria;

public class IntroText : ElintriaScript
{
    string line;

    public override void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        line = "Chapter One";
        yield return new WaitForSecondsRealtime(2f);
        line = "The Old Mill";
        yield return new WaitForSecondsRealtime(2f);
        line = null;
    }

    public override void Update()
    {
        if (line != null) ScreenGUI.Label(new Rect(0, 80, ScreenGUI.screenWidth, 40), line, null, Color.white, TextAnchor.MiddleCenter);
    }
}
```

## Example: A toast message
file: Scripts/Toast.cs
```csharp
using System.Collections;
using Elintria;

public class Toast : ElintriaScript
{
    string text;

    public void Show(string message) { StartCoroutine(Run(message)); }

    IEnumerator Run(string message)
    {
        text = message;
        yield return new WaitForSecondsRealtime(2f);
        text = null;
    }

    public override void Update()
    {
        if (text != null) ScreenGUI.Label(new Rect(20, 60, 400, 30), text);
    }
}
```

## Example: Timing a hit-stop without a coroutine
file: Scripts/HitStop.cs
While the game is frozen, `Time.time` stands still but `Update` keeps running, so a `Stopwatch` measures the real time and `Update` ends the freeze.
```csharp
using System.Diagnostics;
using Elintria;

public class HitStop : ElintriaScript
{
    readonly Stopwatch frozenFor = new Stopwatch();
    float length;

    public void Freeze(float seconds)
    {
        length = seconds;
        Time.timeScale = 0f;
        frozenFor.Restart();
    }

    public override void Update()
    {
        if (frozenFor.IsRunning && frozenFor.Elapsed.TotalSeconds >= length)
        {
            frozenFor.Reset();
            Time.timeScale = 1f;
        }
    }
}
```

# WaitForEndOfFrame
summary: Pauses a coroutine until everything this frame has updated.
see: WaitForFixedUpdate, Coroutine

`yield return new WaitForEndOfFrame();` is kept so Unity code compiles. In Elintria the coroutine resumes on the next frame, the same as `yield return null`. Use it to spread work over frames, or to act once the current frame is done.

## Example: Capturing a screenshot after the frame
file: Scripts/PhotoMode.cs
```csharp
using System.Collections;
using Elintria;

public class PhotoMode : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2)) StartCoroutine(Snap());
    }

    IEnumerator Snap()
    {
        yield return new WaitForEndOfFrame();
        GameWindow.Capture(Project.Resolve("photo.bmp"));
    }
}
```

## Example: Reading where things ended up
file: Scripts/FinalPosition.cs
```csharp
using System.Collections;
using Elintria;

public class FinalPosition : ElintriaScript
{
    public GameObject watched;

    public override void Start() { StartCoroutine(Watch()); }

    IEnumerator Watch()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (watched != null) Debug.DrawRay(watched.transform.position, Vector3.up, Color.magenta);
        }
    }
}
```

## Example: Spreading work across frames
file: Scripts/LazyBuilder.cs
```csharp
using System.Collections;
using Elintria;

public class LazyBuilder : ElintriaScript
{
    public int blocks = 500;

    public override void Start() { StartCoroutine(Build()); }

    IEnumerator Build()
    {
        for (int i = 0; i < blocks; i++)
        {
            GameObject b = GameObject.CreatePrimitive(PrimitiveType.Cube);
            b.transform.position = new Vector3(i % 25, 0f, i / 25);
            if (i % 50 == 49) yield return new WaitForEndOfFrame();   // 50 blocks a frame
        }
    }
}
```

# WaitForFixedUpdate
summary: Pauses a coroutine until the next physics step.
see: WaitForEndOfFrame, Rigidbody

`yield return new WaitForFixedUpdate();` is kept so Unity code compiles. In Elintria the coroutine resumes on the next frame (not exactly on the next physics step), the same as `yield return null`. For work that must run once per physics step, override `FixedUpdate` instead.

## Example: Applying a force over several physics steps
file: Scripts/Thruster.cs
```csharp
using System.Collections;
using Elintria;

public class Thruster : ElintriaScript
{
    public float force = 30f;
    public int steps = 25;

    public void Burn() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        for (int i = 0; i < steps; i++)
        {
            rb.AddForce(transform.up * force);
            yield return new WaitForFixedUpdate();
        }
    }
}
```

## Example: Waiting for physics to settle
file: Scripts/SettleCheck.cs
```csharp
using System.Collections;
using Elintria;

public class SettleCheck : ElintriaScript
{
    public override void Start() { StartCoroutine(Wait()); }

    IEnumerator Wait()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        do { yield return new WaitForFixedUpdate(); } while (rb.velocity.magnitude > 0.05f);
        Debug.Log(name + " came to rest at " + transform.position);
    }
}
```

## Example: A physics-timed dash
file: Scripts/Dash.cs
```csharp
using System.Collections;
using Elintria;

public class Dash : ElintriaScript
{
    public float speed = 18f;
    public float duration = 0.2f;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Shift)) StartCoroutine(Go());
    }

    IEnumerator Go()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float end = Time.time + duration;
        while (Time.time < end)
        {
            rb.velocity = transform.forward * speed;
            yield return new WaitForFixedUpdate();
        }
    }
}
```

# WaitUntil
summary: Pauses a coroutine until a condition becomes true.
see: WaitWhile, Coroutine

`yield return new WaitUntil(() => condition);` checks the condition every frame and resumes the coroutine as soon as it is true.

## Members
WaitUntil: Makes a wait that ends when `p` returns true.
@p: The condition, usually a lambda such as `() => door.isOpen`.

## Example: Waiting for the player to arrive
file: Scripts/AmbushTrigger.cs
```csharp
using System.Collections;
using Elintria;

public class AmbushTrigger : ElintriaScript
{
    public GameObject player;
    public float radius = 8f;
    public GameObject enemies;

    public override void Start() { StartCoroutine(Ambush()); }

    IEnumerator Ambush()
    {
        yield return new WaitUntil(() => Vector3.Distance(player.transform.position, transform.position) < radius);
        enemies.SetActive(true);
        Debug.Log("Ambush!");
    }
}
```

## Example: A tutorial step that waits for input
file: Scripts/Tutorial.cs
```csharp
using System.Collections;
using Elintria;

public class Tutorial : ElintriaScript
{
    string hint;

    public override void Start() { StartCoroutine(Steps()); }

    IEnumerator Steps()
    {
        hint = "Press W to walk";
        yield return new WaitUntil(() => Input.GetKey(KeyCode.W));
        hint = "Press Space to jump";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        hint = null;
    }

    public override void Update()
    {
        if (hint != null) ScreenGUI.Label(new Rect(0, 40, ScreenGUI.screenWidth, 30), hint, null, Color.white, TextAnchor.MiddleCenter);
    }
}
```

## Example: Waiting for a scene object to appear
file: Scripts/WaitForBoss.cs
```csharp
using System.Collections;
using Elintria;

public class WaitForBoss : ElintriaScript
{
    public override void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        GameObject boss = null;
        yield return new WaitUntil(() => (boss = GameObject.FindWithTag("Boss")) != null);
        Debug.Log("Boss spawned: " + boss.name);
    }
}
```

# WaitWhile
summary: Pauses a coroutine while a condition stays true.
see: WaitUntil, Coroutine

`yield return new WaitWhile(() => condition);` resumes the coroutine as soon as the condition becomes false. It is the opposite of [[WaitUntil]].

## Members
WaitWhile: Makes a wait that lasts while `p` returns true.
@p: The condition, usually a lambda.

## Example: Waiting for a sound to finish
file: Scripts/Announcer.cs
```csharp
using System.Collections;
using Elintria;

public class Announcer : ElintriaScript
{
    public override void Start() { StartCoroutine(Say()); }

    IEnumerator Say()
    {
        AudioSource src = gameObject.audioSource;
        src.clip = "Audio/round_one.wav";
        src.Play();
        yield return new WaitWhile(() => src.isPlaying);
        Debug.Log("Fight!");
    }
}
```

## Example: Waiting while a menu is open
file: Scripts/HoldWhilePaused.cs
```csharp
using System.Collections;
using Elintria;

public class HoldWhilePaused : ElintriaScript
{
    public IEnumerator Sequence()
    {
        Debug.Log("Step 1");
        yield return new WaitWhile(() => Time.timeScale == 0f);   // the game is paused
        Debug.Log("Step 2");
    }

    public override void Start() { StartCoroutine(Sequence()); }
}
```

## Example: Waiting until a body stops falling
file: Scripts/Landed.cs
```csharp
using System.Collections;
using Elintria;

public class Landed : ElintriaScript
{
    public override void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(0.2f);
        yield return new WaitWhile(() => rb.velocity.y < -0.1f);
        Debug.Log("Landed");
    }
}
```

# CustomYieldInstruction
summary: Base class for your own reusable coroutine waits.
see: WaitUntil, Coroutine

Derive from `CustomYieldInstruction` and override `keepWaiting`. A coroutine that yields your instruction stays paused while `keepWaiting` returns true, so wait logic you use often gets a name.

## Members
keepWaiting: Return true to keep the coroutine paused, false to let it continue. Checked once per frame.
Current: Part of IEnumerator; always null.
MoveNext: Part of IEnumerator: returns keepWaiting.
Reset: Part of IEnumerator; does nothing.

## Example: Wait for a key press
file: Scripts/WaitForKey.cs
```csharp
using System.Collections;
using Elintria;

public class WaitForKey : CustomYieldInstruction
{
    readonly KeyCode key;
    public WaitForKey(KeyCode key) { this.key = key; }
    public override bool keepWaiting => !Input.GetKeyDown(key);
}

public class Dialogue : ElintriaScript
{
    public override void Start() { StartCoroutine(Talk()); }

    IEnumerator Talk()
    {
        Debug.Log("Hello, traveller.");
        yield return new WaitForKey(KeyCode.E);
        Debug.Log("The road north is dangerous.");
        yield return new WaitForKey(KeyCode.E);
        Debug.Log("Good luck.");
    }
}
```

## Example: Wait until a NavAgent arrives
file: Scripts/WaitForArrival.cs
```csharp
using System.Collections;
using Elintria;

public class WaitForArrival : CustomYieldInstruction
{
    readonly NavAgent agent;
    public WaitForArrival(NavAgent a) { agent = a; }
    public override bool keepWaiting => agent.exists && (agent.pathStatus != 2 || agent.remainingDistance > 0.3f);
}

public class Patrol : ElintriaScript
{
    public override void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        NavAgent agent = NavAgent.Of(gameObject);
        agent.SetDestination(new Vector3(10f, 0f, 10f));
        yield return new WaitForArrival(agent);
        Debug.Log("Arrived");
    }
}
```

## Example: A wait with a timeout
file: Scripts/WaitOrTimeout.cs
```csharp
using System;
using Elintria;

public class WaitOrTimeout : CustomYieldInstruction
{
    readonly Func<bool> done;
    readonly float until;
    public WaitOrTimeout(Func<bool> done, float timeout) { this.done = done; until = Time.time + timeout; }
    public override bool keepWaiting => !done() && Time.time < until;
    public bool TimedOut => Time.time >= until;
}
```
