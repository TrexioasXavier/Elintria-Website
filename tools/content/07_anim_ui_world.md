# Animator
summary: Drives an Animator Controller: set parameters, trigger transitions, and play or cross-fade states.
see: AnimatorStateInfo, SkinnedModel

The `Animator` component plays an Animator Controller, the state machine you build in the Animator window. Scripts talk to it through **parameters**: `SetFloat("Speed", v)`, `SetBool("Grounded", g)`, `SetTrigger("Jump")`. The controller's transitions react to them. You can also jump straight to a state with `Play` or blend to it with `CrossFade`.

## Members
Get: The Animator on `go`, or null.
StringToHash: A number for a parameter or state name. Pass it instead of the string in hot code.
SetFloat: Sets a float parameter. The damped overload eases towards `value` over `dampTime` seconds.
@dampTime: Roughly how long the parameter takes to reach the value.
SetInteger: Sets an int parameter.
SetBool: Sets a bool parameter.
SetTrigger: Sets a trigger: it stays set until a transition uses it.
ResetTrigger: Clears a trigger that has not been used yet.
GetFloat: Reads a float parameter.
GetInteger: Reads an int parameter.
GetBool: Reads a bool parameter.
Play: Jumps straight into state `stateName`.
CrossFade: Blends into `stateName` over a fraction of the state's length.
@normalizedTransitionDuration: The blend time as a fraction of the state's length.
CrossFadeInFixedTime: Blends into `stateName` over a set number of seconds.
@fixedTransitionDuration: The blend time in seconds.
GetCurrentAnimatorStateInfo: The state playing on layer `layerIndex`: its name and how far through it is.
@layerIndex: The layer (0 for the base layer).
IsInTransition: True while layer `layerIndex` is blending between states.
layerCount: How many layers the controller has.
speed: The playback speed (1 normal).
enabled: Whether the Animator runs.
applyRootMotion: Whether the animation's own movement moves the object.
@id: A parameter id from StringToHash.

## Example: Driving locomotion parameters
file: Scripts/LocomotionAnim.cs
The damped `SetFloat` smooths changes in speed, so a blend tree eases between idle, walk and run.
```csharp
using Elintria;

public class LocomotionAnim : ElintriaScript
{
    Animator anim;
    CharacterController cc;

    public override void Start()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    public override void Update()
    {
        Vector3 v = cc.velocity; v.y = 0f;
        anim.SetFloat("Speed", v.magnitude, 0.1f, Time.deltaTime);
        anim.SetBool("Grounded", cc.isGrounded);
        if (cc.isGrounded && Input.GetKeyDown(KeyCode.Space)) anim.SetTrigger("Jump");
    }
}
```

## Example: Attack combos with triggers and state info
file: Scripts/ComboAttack.cs
```csharp
using Elintria;

public class ComboAttack : ElintriaScript
{
    static readonly int AttackId = Animator.StringToHash("Attack");
    Animator anim;

    public override void Start() { anim = GetComponent<Animator>(); }

    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        AnimatorStateInfo s = anim.GetCurrentAnimatorStateInfo(0);
        bool canChain = s.IsName("Attack1") && s.normalizedTime > 0.5f;
        if (canChain || !anim.IsInTransition(0)) anim.SetTrigger(AttackId);
    }
}
```

## Example: Cutscene control with Play and CrossFade
file: Scripts/NpcActing.cs
```csharp
using System.Collections;
using Elintria;

public class NpcActing : ElintriaScript
{
    public override void Start() { StartCoroutine(Act()); }

    IEnumerator Act()
    {
        Animator a = GetComponent<Animator>();
        a.Play("Sit");
        yield return new WaitForSeconds(3f);
        a.CrossFadeInFixedTime("StandUp", 0.25f);
        yield return new WaitForSeconds(1.5f);
        a.speed = 1.2f;
        a.CrossFade("Wave", 0.2f);
    }
}
```

# AnimatorStateInfo
summary: Which Animator state is playing on a layer and how far through it is.
see: Animator

Returned by [[Animator.GetCurrentAnimatorStateInfo]].

## Members
name: The state's name.
normalizedTime: How far through the state: 0 at the start, 1 at the end (higher when looping).
IsName: True when the state is called `n`.
@n: A state name.
shortNameHash: The state name's hash, for comparing with StringToHash.

## Example: Waiting for an animation to finish
file: Scripts/WaitForAnim.cs
```csharp
using System.Collections;
using Elintria;

public class WaitForAnim : ElintriaScript
{
    public IEnumerator PlayAndWait(string state)
    {
        Animator a = GetComponent<Animator>();
        a.Play(state);
        yield return null;
        yield return new WaitUntil(() => !a.GetCurrentAnimatorStateInfo(0).IsName(state) || a.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }
}
```

## Example: Debug read-out of the current state
file: Scripts/AnimDebug.cs
```csharp
using Elintria;

public class AnimDebug : ElintriaScript
{
    public override void Update()
    {
        AnimatorStateInfo s = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        ScreenGUI.Text(20, 20, s.name + " " + (s.normalizedTime % 1f * 100f).ToString("0") + "%");
    }
}
```

## Example: Hit window in an attack
file: Scripts/HitWindow.cs
```csharp
using Elintria;

public class HitWindow : ElintriaScript
{
    public bool DamageActive()
    {
        AnimatorStateInfo s = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        return s.IsName("Swing") && s.normalizedTime > 0.35f && s.normalizedTime < 0.55f;
    }
}
```

# SkinnedModel
summary: A rigged model's clips played by name, plus bone poses for IK, attachments and ragdolls.
see: Animator, HumanoidBones, LookAtIK

`SkinnedModel` plays an animated model's clips directly by name, with no Animator Controller needed. Call `Play("walk")`, with an optional speed, loop setting and cross-fade time. It also gives access to the skeleton: read where a bone is (`BonePose`), override a bone's rotation or position (`SetBoneRotation`, `SetBonePose`) for IK and ragdolls, and list the bones.

## Members
gameObject: The object the model is on.
clip: The name of the clip playing, or "".
time: How far into the clip, in seconds.
length: The clip's length in seconds.
isPlaying: Whether a clip is playing.
frameRate: The clip's frames per second.
clipCount: How many clips the model has.
speed: Playback speed (1 normal).
clips: Every clip's name.
@clip: The clip's name (any case).
@speed: Playback speed.
@loop: Whether the clip repeats.
@blend: Cross-fade time in seconds.
@body: The body-group number.
@skin: The skin family.
@m16: Filled in with the bone's 16-float world matrix.
@position: Filled in with the bone's world position.
@rotation: Filled in with the bone's world rotation.
@parents: Filled in with each bone's parent index (-1 for the root).

## Example: Playing clips by name
file: Scripts/SimpleAnimPlayer.cs
```csharp
using Elintria;

public class SimpleAnimPlayer : ElintriaScript
{
    SkinnedModel model;

    public override void Start()
    {
        model = SkinnedModel.Get(gameObject);
        Debug.Log("Clips: " + string.Join(", ", model.clips));
        model.Play("idle");
    }

    public override void Update()
    {
        bool moving = Input.GetAxisVertical() != 0f;
        string want = moving ? (Input.GetKey(KeyCode.Shift) ? "run" : "walk") : "idle";
        if (model.clip != want) model.Play(want, 1f, true, 0.2f);
        if (Input.GetKeyDown(KeyCode.Space)) model.Play("jump", 1f, false, 0.1f);
    }
}
```

## Example: Attaching a hat to the head bone
file: Scripts/HatAttach.cs
`BonePose` gives the bone's world position and rotation after animation.
```csharp
using Elintria;

public class HatAttach : ElintriaScript
{
    public GameObject character;
    public string bone = "Head";
    public Vector3 offset = new Vector3(0f, 0.15f, 0f);

    public override void LateUpdate()
    {
        SkinnedModel m = SkinnedModel.Get(character);
        if (m != null && m.BonePose(bone, out Vector3 p, out Quaternion r))
        {
            transform.position = p + r * offset;
            transform.rotation = r;
        }
    }
}
```

## Example: Making the head look at something
file: Scripts/HeadTurn.cs
`SetBoneRotation` turns a bone over the animation, with a weight for blending. `ClearBoneOverrides` hands it back.
```csharp
using Elintria;

public class HeadTurn : ElintriaScript
{
    public GameObject target;
    public float weight = 0.8f;

    public override void LateUpdate()
    {
        SkinnedModel m = SkinnedModel.Get(gameObject);
        if (m == null || !m.hasBones) return;
        if (target == null) { m.ClearBoneOverrides("Head"); return; }
        if (m.AnimatedBonePose("Head", out Vector3 head, out Quaternion _))
            m.SetBoneRotation("Head", Quaternion.LookRotation(target.transform.position - head), weight);
    }
}
```

## Example: Listing the skeleton
file: Scripts/BoneLister.cs
```csharp
using Elintria;

public class BoneLister : ElintriaScript
{
    public override void Start()
    {
        SkinnedModel m = SkinnedModel.Get(gameObject);
        string[] bones = m.BoneNames(out int[] parents);
        for (int i = 0; i < bones.Length; i++)
            Debug.Log(bones[i] + (parents[i] >= 0 ? " <- " + bones[parents[i]] : " (root)"));
    }
}
```

# AudioSource
summary: Plays sounds from an object: music, loops, one-shot effects, 2D or positioned in 3D.
see: Audio, AudioRolloffMode, GameObject

An `AudioSource` plays a clip, given by file name or path. Set `clip` and call `Play` for music and loops. Use `PlayOneShot` for overlapping effects such as footsteps and gunshots. `spatialBlend` chooses between a flat 2D sound (0) and one placed in the world (1) that fades with distance. Every object can get one with [[GameObject.audioSource]].

## Members
volume: The volume, 0 to 1.
pitch: The playback speed and pitch (1 normal, 2 an octave up).
loop: Whether the clip repeats.
playOnAwake: Whether it starts playing when the game starts.
mute: Silences the source without stopping it.
minDistance: Within this distance a 3D sound is at full volume.
maxDistance: Beyond this distance a 3D sound stops fading (or is silent with Linear rolloff).
rolloffMode: How a 3D sound fades with distance.
dopplerLevel: How much the pitch shifts with movement (0 none).
isPlaying: Whether it is playing now.
Play: Starts the clip from the beginning.
Stop: Stops playing.
Pause: Pauses, keeping the position.
UnPause: Resumes after Pause.
PlayDelayed: Starts the clip after `seconds`.
PlayOneShot: Plays `clipName` over whatever is playing, without interrupting it. The right way to fire repeated sounds.
@clipName: The clip's file name.
@volumeScale: A volume multiplier for this shot.
SetScheduledStartTime: Kept for Unity code.
SetScheduledEndTime: Kept for Unity code.
@time: Ignored (kept for Unity code).

## Example: Background music with a fade
file: Scripts/Music.cs
```csharp
using System.Collections;
using Elintria;

public class Music : ElintriaScript
{
    public string track = "Audio/theme.ogg";
    AudioSource src;

    public override void Start()
    {
        src = gameObject.audioSource;
        src.clip = track;
        src.loop = true;
        src.spatialBlend = 0f;   // 2D: same volume everywhere
        src.volume = 0f;
        src.Play();
        StartCoroutine(Fade(0.6f, 3f));
    }

    public IEnumerator Fade(float to, float seconds)
    {
        float from = src.volume;
        for (float t = 0f; t < 1f; t += Time.deltaTime / seconds) { src.volume = Mathf.Lerp(from, to, t); yield return null; }
        src.volume = to;
    }
}
```

## Example: Footsteps with variation
file: Scripts/Footsteps.cs
`PlayOneShot` lets steps overlap; a random pitch keeps them from sounding identical.
```csharp
using Elintria;

public class Footsteps : ElintriaScript
{
    public float stepInterval = 0.45f;
    float next;
    AudioSource src;

    public override void Start()
    {
        src = gameObject.audioSource;
        src.spatialBlend = 1f;
        src.minDistance = 1f;
        src.maxDistance = 20f;
    }

    public override void Update()
    {
        bool moving = Mathf.Abs(Input.GetAxisVertical()) + Mathf.Abs(Input.GetAxisHorizontal()) > 0.1f;
        if (!moving || Time.time < next) return;
        next = Time.time + stepInterval;
        src.pitch = Random.Range(0.9f, 1.1f);
        src.PlayOneShot("Audio/step_" + Random.Range(1, 5) + ".wav", 0.8f);
    }
}
```

## Example: A 3D engine hum that follows speed
file: Scripts/EngineHum.cs
```csharp
using Elintria;

public class EngineHum : ElintriaScript
{
    AudioSource src;
    Rigidbody rb;

    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        src = AudioSource.Add(gameObject);
        src.clip = "Audio/engine_loop.wav";
        src.loop = true;
        src.spatialBlend = 1f;
        src.rolloffMode = AudioRolloffMode.Linear;
        src.maxDistance = 60f;
        src.Play();
    }

    public override void Update()
    {
        float speed = rb != null ? rb.velocity.magnitude : 0f;
        src.pitch = Mathf.Lerp(0.8f, 2f, speed / 30f);
        src.volume = Mathf.Lerp(0.4f, 1f, speed / 30f);
    }
}
```

# Audio
summary: Mix-wide audio controls: master volume, pausing every sound, and one-off sounds at a point.
see: AudioSource

## Example: A master volume setting
file: Scripts/VolumeSetting.cs
```csharp
using Elintria;

public class VolumeSetting : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Insert)) Audio.masterVolume = Mathf.Min(1f, Audio.masterVolume + 0.1f);
        if (Input.GetKeyDown(KeyCode.Delete)) Audio.masterVolume = Mathf.Max(0f, Audio.masterVolume - 0.1f);
        ScreenGUI.Text(20, 20, "Volume " + Mathf.RoundToInt(Audio.masterVolume * 100f) + "%");
    }
}
```

## Example: Pausing all sound with the game
file: Scripts/PauseSounds.cs
```csharp
using Elintria;

public class PauseSounds : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Audio.paused = !Audio.paused;
            Time.timeScale = Audio.paused ? 0f : 1f;
        }
    }
}
```

## Example: An explosion sound with no source object
file: Scripts/BoomSound.cs
`PlayClipAtPoint` plays a positioned sound that cleans itself up, even when the object making it is destroyed straight away.
```csharp
using Elintria;

public class BoomSound : ElintriaScript
{
    public void Explode()
    {
        Audio.PlayClipAtPoint("Audio/explosion.wav", transform.position, 1f);
        Destroy(gameObject);
    }
}
```

# AudioRolloffMode
summary: How a 3D sound gets quieter with distance.
see: AudioSource

## Members
Logarithmic: Realistic falloff: loud close up, fading quickly, then lingering. The default.
Linear: Fades in a straight line from `minDistance` to silence at `maxDistance`. Easier to control in games.

## Example: A radio you can walk away from
file: Scripts/Radio.cs
```csharp
using Elintria;

public class Radio : ElintriaScript
{
    public override void Start()
    {
        AudioSource s = gameObject.audioSource;
        s.clip = "Audio/radio.ogg";
        s.loop = true;
        s.spatialBlend = 1f;
        s.rolloffMode = AudioRolloffMode.Linear;
        s.minDistance = 2f;
        s.maxDistance = 15f;   // silent beyond 15 m
        s.Play();
    }
}
```

## Example: Realistic falloff for gunfire
file: Scripts/Gunfire.cs
```csharp
using Elintria;

public class Gunfire : ElintriaScript
{
    public override void Start()
    {
        AudioSource s = gameObject.audioSource;
        s.spatialBlend = 1f;
        s.rolloffMode = AudioRolloffMode.Logarithmic;
        s.minDistance = 5f;
    }

    public void Shoot() { gameObject.audioSource.PlayOneShot("Audio/shot.wav"); }
}
```

## Example: Switching mode by setting
file: Scripts/RolloffToggle.cs
```csharp
using Elintria;

public class RolloffToggle : ElintriaScript
{
    public bool arcadeAudio = true;

    public override void Start()
    {
        foreach (AudioSource s in FindObjectsOfType<AudioSource>())
            s.rolloffMode = arcadeAudio ? AudioRolloffMode.Linear : AudioRolloffMode.Logarithmic;
    }
}
```

# ScreenGUI
summary: Immediate-mode game UI: draw text, bars, images and buttons from Update, every frame.
see: GuiFont, Rect, TextAnchor, UIRuntime

`ScreenGUI` draws the game's UI straight from your scripts. Call its functions inside `Update`, every frame you want something shown, and they draw over the game. `Button` returns true on the frame it is clicked. Coordinates are game-image pixels from the top-left. Lay out against `screenWidth` and `screenHeight` so the UI fits any window.

## Members
BeginFrame: Samples the mouse for this frame. Widgets call it themselves; call it first when the game sets `Time.timeScale` to 0.
SimulateClick: For tests: clicks at (x, y) in screen pixels.
SimulateDrag: For tests: drags with the left button from (x0, y0) to (x1, y1).
@x0: Start x.
@y0: Start y.
@x1: End x.
@y1: End y.
@frames: How many frames the action takes.
SimulateButton: For tests: the next enabled Button labelled `label` reports a click.
mousePosition: The pointer, in game-image pixels.
mouseInside: Whether the pointer is over the game image.
MouseHeld: True while mouse `button` is held.
MousePressed: True on the frame `button` went down, unless a widget already used the press.
MouseReleased: True on the frame `button` came up.
UseMouse: Marks this frame's press of `button` as handled, so later widgets and the game ignore it.
MouseUsed: True when a widget already handled this frame's press.
Hover: True when the pointer is over `r`.
Clicked: True when `r` is clicked this frame (the press is used up).
Fill: A filled rectangle.
@c: The colour.
Border: A rectangle's outline, `t` pixels thick.
@t: The thickness in pixels.
Image: Draws a texture (a GpuTexture id) in `r`, tinted, optionally rotated, or a part of it given by `uv`.
@uv: The part of the texture, 0..1 from the top-left.
@angle: A rotation in degrees.
Bar: A bar filled from the left to `fraction` (0..1), for health, stamina and progress.
@fraction: How full, 0..1.
@fill: The filled colour.
@back: The empty colour.
Line: A line from `a` to `b`.
@a: The start, in pixels.
@b: The end, in pixels.
Label: Text in `r`, placed by `anchor`; `wrap` breaks it to the rectangle's width, and `shadow` draws a drop shadow.
@wrap: Break the text into lines that fit.
Text: Text with its top-left at (x, y).
Button: A clickable button: true on the frame it is clicked.
@face: The button's colour.
@enabled: False draws it greyed out and unclickable.
Model: Draws a 3D object (and everything under it) in a rectangle of the UI, seen from `eye` looking at `target`. Use it for character previews and item inspection.
@root: The object to show.
@eye: The camera position.
PushClip: Only what lies inside `r` shows, until PopClip.
PopClip: Ends the last PushClip.
Wheel: Scroll wheel notches since the last call.
screenWidth: The game image's width in pixels.
screenHeight: The game image's height in pixels.

## Example: A HUD with health, ammo and a crosshair
file: Scripts/Hud.cs
```csharp
using Elintria;

public class Hud : ElintriaScript
{
    public float health = 80f;
    public int ammo = 24;
    GuiFont big;

    public override void Start() { big = GuiFont.Get("", 28, true); }

    public override void Update()
    {
        int w = ScreenGUI.screenWidth, h = ScreenGUI.screenHeight;
        ScreenGUI.Bar(new Rect(20, h - 40, 240, 20), health / 100f, new Color(0.85f, 0.2f, 0.2f), new Color(0f, 0f, 0f, 0.5f));
        ScreenGUI.Border(new Rect(20, h - 40, 240, 20), Color.white);
        ScreenGUI.Label(new Rect(w - 200, h - 60, 180, 40), ammo + " / 120", big, Color.white, TextAnchor.MiddleRight, false, Color.black);
        ScreenGUI.Line(new Vector2(w / 2 - 8, h / 2), new Vector2(w / 2 + 8, h / 2), Color.white, 2f);
        ScreenGUI.Line(new Vector2(w / 2, h / 2 - 8), new Vector2(w / 2, h / 2 + 8), Color.white, 2f);
    }
}
```

## Example: A main menu with buttons
file: Scripts/TitleMenu.cs
```csharp
using Elintria;

public class TitleMenu : ElintriaScript
{
    public override void Update()
    {
        float cx = ScreenGUI.screenWidth / 2f;
        ScreenGUI.Label(new Rect(0, 120, ScreenGUI.screenWidth, 60), "MY GAME", GuiFont.Get("", 48, true), Color.white, TextAnchor.MiddleCenter);
        if (ScreenGUI.Button(new Rect(cx - 110, 240, 220, 50), "Play")) SceneManager.LoadScene("Level1");
        if (ScreenGUI.Button(new Rect(cx - 110, 300, 220, 50), "Options", null, null, null, false)) { }   // greyed out
        if (ScreenGUI.Button(new Rect(cx - 110, 360, 220, 50), "Quit")) GameWindow.Quit();
    }
}
```

## Example: A scrolling list with clipping
file: Scripts/QuestLog.cs
`PushClip` keeps rows inside the panel; `Wheel` scrolls it.
```csharp
using Elintria;

public class QuestLog : ElintriaScript
{
    string[] quests = { "Find the key", "Open the vault", "Talk to Mira", "Repair the pump", "Escape the lab", "Feed the cat" };
    float scroll;

    public override void Update()
    {
        Rect panel = new Rect(40, 100, 300, 160);
        ScreenGUI.Fill(panel, new Color(0f, 0f, 0f, 0.6f));
        if (ScreenGUI.Hover(panel)) scroll = Mathf.Clamp(scroll - ScreenGUI.Wheel() * 20f, 0f, Mathf.Max(0f, quests.Length * 32 - panel.height));
        ScreenGUI.PushClip(panel);
        for (int i = 0; i < quests.Length; i++)
        {
            Rect row = new Rect(panel.x + 8, panel.y + 4 + i * 32 - scroll, panel.width - 16, 28);
            if (ScreenGUI.Hover(row)) ScreenGUI.Fill(row, new Color(1f, 1f, 1f, 0.1f));
            ScreenGUI.Label(row, quests[i], null, Color.white, TextAnchor.MiddleLeft);
        }
        ScreenGUI.PopClip();
    }
}
```

# GuiFont
summary: A font at one size, for ScreenGUI text, plus measuring and wrapping helpers.
see: ScreenGUI, TextAnchor

`GuiFont.Get(face, size, bold)` gives a system font baked at a pixel size. Pass it to [[ScreenGUI.Label]] and [[ScreenGUI.Text]]. Use `Measure`, `Wrap` and `MeasureWrapped` to size boxes around text.

## Members
face: The font's name ("" for the default).
size: The pixel size.
bold: Whether it is bold.
texture: The texture the glyphs are baked into.
ascent: Pixels above the baseline.
descent: Pixels below the baseline.
@face: A system font name such as "Segoe UI", or "" for the default.
@pixelSize: The text height in pixels.
@maxWidth: The widest a line may be, in pixels.

## Example: A speech bubble sized to its text
file: Scripts/SpeechBubble.cs
```csharp
using Elintria;

public class SpeechBubble : ElintriaScript
{
    public string line = "You shouldn't be down here. The tunnels flood at night.";
    GuiFont font;

    public override void Start() { font = GuiFont.Get("", 18); }

    public override void Update()
    {
        Vector2 size = font.MeasureWrapped(line, 260f);
        Rect box = new Rect(60, 60, size.x + 20, size.y + 16);
        ScreenGUI.Fill(box, new Color(1f, 1f, 1f, 0.9f));
        ScreenGUI.Label(new Rect(box.x + 10, box.y + 8, size.x, size.y), line, font, Color.black, TextAnchor.UpperLeft, true);
    }
}
```

## Example: Fitting a title into a width
file: Scripts/FitTitle.cs
```csharp
using Elintria;

public class FitTitle : ElintriaScript
{
    public string title = "The Long Descent";

    public override void Update()
    {
        int size = 48;
        while (size > 12 && GuiFont.Get("", size, true).Measure(title) > 300f) size -= 2;
        ScreenGUI.Text(40, 40, title, GuiFont.Get("", size, true));
    }
}
```

## Example: Drawing wrapped lines one by one
file: Scripts/Typewriter.cs
```csharp
using System.Collections.Generic;
using Elintria;

public class Typewriter : ElintriaScript
{
    public string text = "Day 14. The generator failed again. I can hear something in the vents.";
    GuiFont font;

    public override void Start() { font = GuiFont.Get("Consolas", 16); }

    public override void Update()
    {
        int shown = Mathf.Min(text.Length, (int)(Time.time * 25f));
        List<string> lines = font.Wrap(text.Substring(0, shown), 320f);
        for (int i = 0; i < lines.Count; i++)
            ScreenGUI.Text(40, 300 + i * font.lineHeight, lines[i], font, Color.green);
    }
}
```

# TextAnchor
summary: Where text sits inside its rectangle: upper, middle or lower, and left, centre or right.
see: ScreenGUI

## Members
*: Places the text at the {words} of its rectangle.
UpperLeft: Top-left (the default).
MiddleCenter: Centred both ways.

## Example: Centred titles
file: Scripts/CenterTitle.cs
```csharp
using Elintria;

public class CenterTitle : ElintriaScript
{
    public override void Update()
    {
        ScreenGUI.Label(new Rect(0, 0, ScreenGUI.screenWidth, ScreenGUI.screenHeight), "LEVEL CLEAR", null, Color.yellow, TextAnchor.MiddleCenter);
    }
}
```

## Example: Corner labels
file: Scripts/CornerLabels.cs
```csharp
using Elintria;

public class CornerLabels : ElintriaScript
{
    public override void Update()
    {
        Rect all = new Rect(10, 10, ScreenGUI.screenWidth - 20, ScreenGUI.screenHeight - 20);
        ScreenGUI.Label(all, "Score 1200", null, Color.white, TextAnchor.UpperLeft);
        ScreenGUI.Label(all, "Lives 3", null, Color.white, TextAnchor.UpperRight);
        ScreenGUI.Label(all, "v1.0", null, Color.gray, TextAnchor.LowerRight);
    }
}
```

## Example: Right-aligned numbers in a table
file: Scripts/ScoreTable.cs
```csharp
using Elintria;

public class ScoreTable : ElintriaScript
{
    string[] names = { "Ash", "Brook", "Cyd" };
    int[] scores = { 1520, 980, 75 };

    public override void Update()
    {
        for (int i = 0; i < names.Length; i++)
        {
            Rect row = new Rect(100, 100 + i * 28, 240, 26);
            ScreenGUI.Label(row, names[i], null, Color.white, TextAnchor.MiddleLeft);
            ScreenGUI.Label(row, scores[i].ToString(), null, Color.white, TextAnchor.MiddleRight);
        }
    }
}
```

# UIRuntime
summary: The live state of the UI made in the UI Designer: set text, show or hide elements, read clicks, fill bars, and drive dropdowns, minimaps and inventory grids.
see: ScreenGUI

When you design UI in the editor's UI tools, `UIRuntime` is how scripts talk to it, by element name. Set a label's text, show or hide elements or whole groups, read button clicks, drive a bar's fill, and override how an element is drawn.

## Members
SetText: Changes the text of the element called `name`.
GetText: The element's current text.
SetVisible: Shows or hides the element.
IsVisible: Whether it is shown.
SetGroupVisible: Shows or hides every element whose name starts with `prefix`, such as a whole menu.
WasClicked: True once for each click on the element; reading it clears the click.
SetFill: Sets a bar or slider element's fill, 0..1.
GetFill: The fill set from code, or -1 if none was set.
SetProp: Overrides how an element is drawn: "color" ("r,g,b,a"), "sprite" (an image path), "off" ("1" hides its own graphic), "fontSize"; and the UI Designer widgets' live state: "sel" (a dropdown's, tab bar's or radial menu's picked index), "mapx", "mapy" (a minimap's centre, 0..1) and "maprot" (degrees), "slot3" (an inventory grid slot's image) and "count3" (its count).
@key: The property: "color", "sprite", "off", "galpha", "fontSize", "sel", "mapx", "mapy", "maprot", "slotN" or "countN".
GetProp: Reads a property set with SetProp.

## Example: Driving a designed HUD
file: Scripts/DesignedHud.cs
```csharp
using Elintria;

public class DesignedHud : ElintriaScript
{
    public float health = 100f;
    public int coins;

    public override void Update()
    {
        UIRuntime.SetFill("HealthBar", health / 100f);
        UIRuntime.SetText("CoinText", coins.ToString());
        UIRuntime.SetProp("HealthBar", "color", health < 25f ? "1,0.2,0.2,1" : "0.2,0.9,0.3,1");
    }
}
```

## Example: Menu buttons
file: Scripts/DesignedMenu.cs
```csharp
using Elintria;

public class DesignedMenu : ElintriaScript
{
    public override void Update()
    {
        if (UIRuntime.WasClicked("PlayButton")) { UIRuntime.SetGroupVisible("Menu_", false); SceneManager.LoadScene("Level1"); }
        if (UIRuntime.WasClicked("QuitButton")) GameWindow.Quit();
    }
}
```

## Example: Toggling a pause screen
file: Scripts/DesignedPause.cs
```csharp
using Elintria;

public class DesignedPause : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        bool show = !UIRuntime.IsVisible("Pause_Panel");
        UIRuntime.SetGroupVisible("Pause_", show);
        Time.timeScale = show ? 0f : 1f;
    }
}
```

## Example: Minimap, inventory grid and dropdown
file: Scripts/DesignedWidgets.cs
```csharp
using Elintria;
using System.Globalization;

public class DesignedWidgets : ElintriaScript
{
    public Transform player;
    public float worldSize = 200f;
    public string[] items = { "Images/sword.png", "Images/potion.png" };
    public int[] counts = { 1, 5 };

    public override void Update()
    {
        // The minimap follows the player and turns with them.
        if (player != null)
        {
            UIRuntime.SetProp("Minimap", "mapx", (0.5f + player.position.x / worldSize).ToString(CultureInfo.InvariantCulture));
            UIRuntime.SetProp("Minimap", "mapy", (0.5f - player.position.z / worldSize).ToString(CultureInfo.InvariantCulture));
            UIRuntime.SetProp("Minimap", "maprot", player.eulerAngles.y.ToString(CultureInfo.InvariantCulture));
        }
        // The bag's pictures and counts, slot by slot.
        for (int i = 0; i < items.Length; ++i)
        {
            UIRuntime.SetProp("Bag", "slot" + i, items[i]);
            UIRuntime.SetProp("Bag", "count" + i, counts[i].ToString());
        }
        // Which option the dropdown shows.
        if (UIRuntime.WasClicked("Quality"))
            Debug.Log("Quality option " + UIRuntime.GetProp("Quality", "sel"));
    }
}
```

# Terrain
summary: The heightmap terrain component: sample ground height and reach its TerrainData.
see: TerrainData, TerrainCollider, TerrainLayer

A `Terrain` draws a [[TerrainData]] heightmap with its corner at the object's position. Use `SampleHeight` to stand things on the ground, and `terrainData` to read or reshape the heights and paint.

## Members
@worldPosition: A point in world space; only x and z are used.

## Example: Snapping objects to the ground
file: Scripts/SnapToTerrain.cs
`SampleHeight` is relative to the terrain, so add the terrain's own height.
```csharp
using Elintria;

public class SnapToTerrain : ElintriaScript
{
    public float offset = 0f;

    public override void LateUpdate()
    {
        Terrain t = Terrain.activeTerrain;
        if (t == null) return;
        Vector3 p = transform.position;
        p.y = t.SampleHeight(p) + t.GetPosition().y + offset;
        transform.position = p;
    }
}
```

## Example: Scattering rocks across every terrain
file: Scripts/RockScatter.cs
```csharp
using Elintria;

public class RockScatter : ElintriaScript
{
    public GameObject rockTemplate;
    public int perTerrain = 40;

    public override void Start()
    {
        foreach (Terrain t in Terrain.activeTerrains)
        {
            Vector3 corner = t.GetPosition(), size = t.terrainData.size;
            for (int i = 0; i < perTerrain; i++)
            {
                Vector3 p = corner + new Vector3(Random.value * size.x, 0f, Random.value * size.z);
                p.y = corner.y + t.SampleHeight(p);
                Instantiate(rockTemplate, p, Random.rotation).SetActive(true);
            }
        }
    }
}
```

## Example: Hiding the terrain mesh
file: Scripts/XRayView.cs
```csharp
using Elintria;

public class XRayView : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && Terrain.activeTerrain != null)
            Terrain.activeTerrain.drawHeightmap = !Terrain.activeTerrain.drawHeightmap;   // its collider still works
    }
}
```

# TerrainData
summary: A terrain's heightmap, size and painted layers: read, sample and reshape them from code.
see: Terrain, TerrainLayer

`TerrainData` is the data behind a [[Terrain]]. It has the grid of heights (0..1, scaled by `size.y`), the world size, and the alphamap that says how much of each [[TerrainLayer]] covers each spot. Read heights, normals and steepness at any point, and write heights and paint to deform or texture the ground at run time.

## Members
heightmapWidth: Samples across the heightmap.
heightmapHeight: Samples along the heightmap.
alphamapWidth: Texels across the alphamap.
alphamapHeight: Texels along the alphamap.
@x: A position across the terrain: a sample index, or 0..1 for the interpolated functions.
@y: A position along the terrain: a sample index, or 0..1 for the interpolated functions.
@xBase: The first sample column.
@yBase: The first sample row.
@heights: The heights, 0..1, as [y, x].
@map: The weights, as [y, x, layer].
@width: How many columns.
@height: How many rows.

## Example: Digging a crater
file: Scripts/Crater.cs
`GetHeights` and `SetHeights` work on blocks of samples; the collider follows at once.
```csharp
using Elintria;

public class Crater : ElintriaScript
{
    public float radiusSamples = 6f;
    public float depth = 0.01f;

    public void Dig(Vector3 world)
    {
        Terrain t = Terrain.activeTerrain;
        TerrainData d = t.terrainData;
        Vector3 local = world - t.GetPosition();
        int cx = (int)(local.x / d.size.x * (d.heightmapResolution - 1));
        int cz = (int)(local.z / d.size.z * (d.heightmapResolution - 1));
        int r = (int)radiusSamples, x0 = Mathf.Max(0, cx - r), z0 = Mathf.Max(0, cz - r);
        int w = Mathf.Min(d.heightmapResolution - x0, r * 2), h = Mathf.Min(d.heightmapResolution - z0, r * 2);
        float[,] hs = d.GetHeights(x0, z0, w, h);
        for (int z = 0; z < h; z++)
            for (int x = 0; x < w; x++)
            {
                float dist = new Vector2(x0 + x - cx, z0 + z - cz).magnitude / radiusSamples;
                if (dist < 1f) hs[z, x] -= depth * (1f - dist * dist);
            }
        d.SetHeights(x0, z0, hs);
    }
}
```

## Example: Steep slopes slow the player
file: Scripts/SlopeSpeed.cs
```csharp
using Elintria;

public class SlopeSpeed : ElintriaScript
{
    public float SpeedFactor()
    {
        Terrain t = Terrain.activeTerrain;
        if (t == null) return 1f;
        TerrainData d = t.terrainData;
        Vector3 local = transform.position - t.GetPosition();
        float steep = d.GetSteepness(local.x / d.size.x, local.z / d.size.z);
        return Mathf.Lerp(1f, 0.4f, Mathf.InverseLerp(15f, 45f, steep));
    }
}
```

## Example: What ground is underfoot?
file: Scripts/GroundType.cs
The alphamap says how much of each layer covers a spot; the strongest one decides the footstep sound.
```csharp
using Elintria;

public class GroundType : ElintriaScript
{
    public string Underfoot()
    {
        Terrain t = Terrain.activeTerrain;
        TerrainData d = t.terrainData;
        Vector3 local = transform.position - t.GetPosition();
        int ax = Mathf.Clamp((int)(local.x / d.size.x * d.alphamapWidth), 0, d.alphamapWidth - 1);
        int az = Mathf.Clamp((int)(local.z / d.size.z * d.alphamapHeight), 0, d.alphamapHeight - 1);
        float[,,] w = d.GetAlphamaps(ax, az, 1, 1);
        int best = 0;
        for (int l = 1; l < d.alphamapLayers; l++) if (w[0, 0, l] > w[0, 0, best]) best = l;
        return d.terrainLayers[best].name;
    }
}
```

# TerrainCollider
summary: The solid version of a terrain's heightmap, for physics, raycasts and characters.
see: Terrain, Collider, Physics

The `TerrainCollider` makes the same heightmap as the [[Terrain]] solid. Bodies land on it, rays hit it and characters walk on it. It updates at once when the heights change.

## Members
terrainData: The heightmap it is shaped by.

## Example: Is this hit the terrain?
file: Scripts/TerrainHitCheck.cs
```csharp
using Elintria;

public class TerrainHitCheck : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f))
            Debug.Log(hit.collider is TerrainCollider ? "Hit the ground" : "Hit " + hit.gameObject.name);
    }
}
```

## Example: Letting a character into a cave
file: Scripts/CaveEntrance.cs
```csharp
using Elintria;

public class CaveEntrance : ElintriaScript
{
    public GameObject terrain;

    public override void OnTriggerEnter(Collider other) { Physics.IgnoreTerrain(other.gameObject, terrain, true); }
    public override void OnTriggerExit(Collider other) { Physics.IgnoreTerrain(other.gameObject, terrain, false); }
}
```

## Example: Reading the size through the collider
file: Scripts/TerrainSize.cs
```csharp
using Elintria;

public class TerrainSize : ElintriaScript
{
    public override void Start()
    {
        TerrainCollider tc = GetComponent<TerrainCollider>();
        if (tc != null) Debug.Log("Terrain is " + tc.terrainData.size);
    }
}
```

# TerrainLayer
summary: One paintable ground texture of a terrain, with its name and tiling.
see: TerrainData

Layers are created and painted in the editor's Paint Texture tool. Scripts can read them through [[TerrainData.terrainLayers]].

## Members
name: The layer's name ("Grass", "Rock").
tileSize: How many world units one repeat of the texture covers.
tileOffset: How far the texture is shifted.

## Example: Listing the layers
file: Scripts/LayerList.cs
```csharp
using Elintria;

public class LayerList : ElintriaScript
{
    public override void Start()
    {
        foreach (TerrainLayer l in Terrain.activeTerrain.terrainData.terrainLayers)
            Debug.Log(l.name + " tiles every " + l.tileSize);
    }
}
```

## Example: Finding a layer by name
file: Scripts/FindLayer.cs
```csharp
using Elintria;

public class FindLayer : ElintriaScript
{
    public static int IndexOf(TerrainData d, string name)
    {
        TerrainLayer[] layers = d.terrainLayers;
        for (int i = 0; i < layers.Length; i++) if (layers[i].name == name) return i;
        return -1;
    }
}
```

## Example: Painting a path with a named layer
file: Scripts/PaintPath.cs
```csharp
using Elintria;

public class PaintPath : ElintriaScript
{
    public string layerName = "Dirt";

    public override void Update()
    {
        TerrainData d = Terrain.activeTerrain.terrainData;
        int layer = FindLayer.IndexOf(d, layerName);
        if (layer < 0) return;
        Vector3 local = transform.position - Terrain.activeTerrain.GetPosition();
        int x = (int)(local.x / d.size.x * d.alphamapWidth), z = (int)(local.z / d.size.z * d.alphamapHeight);
        if (x < 0 || z < 0 || x >= d.alphamapWidth || z >= d.alphamapHeight) return;
        float[,,] w = new float[1, 1, d.alphamapLayers];
        w[0, 0, layer] = 1f;
        d.SetAlphamaps(x, z, w);
    }
}
```

# Water
summary: Query the Water components in the scene: surface height and normal, depth, underwater tests.
see: Buoyancy, Foliage

Add water with **GameObject > 3D Object > Water**. Its waves are computed the same way on the CPU and the GPU, so these queries match what players see. Use them for floating, swimming, splashes and underwater effects.

## Members
HeightAt: The water surface height at (x, z) right now. False where no water covers that spot.
@point: A point in world space.
@height: Filled in with the surface height.
SurfaceAt: The surface height and its normal (which way the wave faces).
@normal: Filled in with the surface normal.
Depth: How far below the surface `point` is (negative above it, a large negative number where there is no water).
IsUnderwater: True when `point` is below the surface.

## Example: Swimming state
file: Scripts/Swimmer.cs
```csharp
using Elintria;

public class Swimmer : ElintriaScript
{
    public bool swimming;

    public override void Update()
    {
        float depth = Water.Depth(transform.position + Vector3.up * 1.2f);   // chest height
        swimming = depth > 0f;
        if (Water.IsUnderwater(transform.position + Vector3.up * 1.7f))       // head
            RenderEnvironment.SetFog(0.1f, 0.3f, 0.4f, 0f, 15f);
        else
            RenderEnvironment.SetFog(0f, 0f, 0f, 1f, 0f);                     // fog off
    }
}
```

## Example: A boat that rides the waves
file: Scripts/WaveRider.cs
```csharp
using Elintria;

public class WaveRider : ElintriaScript
{
    public float draft = 0.3f;

    public override void Update()
    {
        if (!Water.SurfaceAt(transform.position, out float h, out Vector3 n)) return;
        Vector3 p = transform.position;
        p.y = h - draft;
        transform.position = p;
        Vector3 fwd = Vector3.ProjectOnPlane(transform.forward, n).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(fwd, n), 3f * Time.deltaTime);
    }
}
```

## Example: Splashes when things enter the water
file: Scripts/Splash.cs
```csharp
using Elintria;

public class Splash : ElintriaScript
{
    public GameObject splashEffect;
    bool wasUnder;

    public override void Update()
    {
        bool under = Water.IsUnderwater(transform.position);
        if (under && !wasUnder && Water.HeightAt(transform.position, out float h))
        {
            GameObject s = Instantiate(splashEffect, new Vector3(transform.position.x, h, transform.position.z), Quaternion.identity);
            s.SetActive(true);
            Destroy(s, 2f);
        }
        wasUnder = under;
    }
}
```

# Foliage
summary: Make painted grass bend away from characters, vehicles and explosions.
see: GrassBender, Terrain

Grass painted with the Terrain's **Paint Details** tool sways in the wind and bends around up to 8 *benders*. Set a bender each frame at a character's feet, or use the [[GrassBender]] component, which does it for you.

## Members
MaxBenders: How many benders there can be at once (8).
SetBender: Makes grass within `radius` of `position` bend away. Use one slot per thing.
ClearBender: Turns a bender off.

## Example: Grass bending under the player
file: Scripts/PlayerGrass.cs
```csharp
using Elintria;

public class PlayerGrass : ElintriaScript
{
    public int slot = 0;

    public override void Update() { Foliage.SetBender(slot, transform.position, 0.8f); }
    public override void OnDisable() { Foliage.ClearBender(slot); }
}
```

## Example: A blast that flattens grass
file: Scripts/GrassBlast.cs
```csharp
using System.Collections;
using Elintria;

public class GrassBlast : ElintriaScript
{
    public IEnumerator Blast(Vector3 at)
    {
        for (float r = 0f; r < 6f; r += Time.deltaTime * 15f)
        {
            Foliage.SetBender(7, at, r);
            yield return null;
        }
        Foliage.ClearBender(7);
    }
}
```

## Example: Sharing slots between several creatures
file: Scripts/HerdGrass.cs
```csharp
using System.Collections.Generic;
using Elintria;

public class HerdGrass : ElintriaScript
{
    public List<GameObject> animals = new List<GameObject>();

    public override void Update()
    {
        for (int i = 0; i < Foliage.MaxBenders; i++)
        {
            if (i < animals.Count && animals[i] != null) Foliage.SetBender(i, animals[i].transform.position, 1.2f);
            else Foliage.ClearBender(i);
        }
    }
}
```

# Spline
summary: The curve of a Spline component: points along it, its direction, length and nearest point.
see: SplineFollower, Tilemap

Create a spline with **GameObject > Spline** and edit its points in the Scene view. A spline can also generate a road, tube or wall mesh and place objects along itself. `Spline.Get(obj)` reads the curve from scripts. `Evaluate(t)` gives a point by fraction (0..1), `EvaluateAtDistance` gives one by metres, and `NearestT` finds where a point is closest.

## Members
Get: The spline on `go`, or null.
gameObject: The object the spline is on.
length: The curve's length in metres.
pointCount: How many control points it has.
closed: Whether the curve loops back to its start.
@t: A fraction of the way along, 0..1.
TangentAtDistance: The curve's direction `metres` from the start.

## Example: A tram that runs along a spline
file: Scripts/Tram.cs
```csharp
using Elintria;

public class Tram : ElintriaScript
{
    public GameObject track;
    public float speed = 6f;
    float d;

    public override void Update()
    {
        Spline s = Spline.Get(track);
        if (s == null) return;
        d = (d + speed * Time.deltaTime) % s.length;
        transform.position = s.EvaluateAtDistance(d);
        transform.rotation = Quaternion.LookRotation(s.TangentAtDistance(d));
    }
}
```

## Example: How far along the race track is each car?
file: Scripts/RaceProgress.cs
```csharp
using Elintria;

public class RaceProgress : ElintriaScript
{
    public GameObject track;

    public float Progress()
    {
        Spline s = Spline.Get(track);
        return s == null ? 0f : s.NearestT(transform.position);   // 0..1 round the lap
    }

    public override void Update() { ScreenGUI.Text(20, 20, "Lap progress " + Mathf.RoundToInt(Progress() * 100f) + "%"); }
}
```

## Example: Fence posts along a path
file: Scripts/FencePosts.cs
```csharp
using Elintria;

public class FencePosts : ElintriaScript
{
    public GameObject path;
    public GameObject postTemplate;
    public float spacing = 2f;

    public override void Start()
    {
        Spline s = Spline.Get(path);
        for (float d = 0f; d <= s.length; d += spacing)
        {
            Vector3 side = Vector3.Cross(Vector3.up, s.TangentAtDistance(d)).normalized * 3f;
            Instantiate(postTemplate, s.EvaluateAtDistance(d) + side, Quaternion.identity).SetActive(true);
        }
    }
}
```

# Tilemap
summary: Read and change the cells of a 2D Tilemap: which tile is where, and world-to-cell conversion.
see: SpriteRenderer, Spline

Create a tilemap with **GameObject > 2D Object > Tilemap** and paint it with **Window > 2D > Tile Palette**. From scripts, `Tilemap.Get(obj)` lets you read and set cells. The drawing and colliders update when cells change, which suits destructible blocks, doors and level generators.

## Members
Get: The Tilemap on `go`, or null.
gameObject: The object the tilemap is on.
HasTile: True when the cell at the world point `world` is not empty.

## Example: Breaking blocks
file: Scripts/BlockBreaker.cs
```csharp
using Elintria;

public class BlockBreaker : ElintriaScript
{
    public GameObject level;

    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        Tilemap map = Tilemap.Get(level);
        Vector3 below = transform.position + Vector3.down * 0.6f;
        if (map.WorldToCell(below, out int x, out int y) && map.GetTile(x, y) >= 0)
            map.SetTile(x, y, -1);   // empty: the collider updates too
    }
}
```

## Example: Generating a level row by row
file: Scripts/TileGen.cs
```csharp
using Elintria;

public class TileGen : ElintriaScript
{
    public int width = 60;
    public int ground = 3;

    public override void Start()
    {
        Tilemap map = Tilemap.Get(gameObject);
        int height = 2;
        for (int x = 0; x < width; x++)
        {
            if (Random.value < 0.2f) height = Mathf.Clamp(height + Random.Range(-1, 2), 1, 6);
            for (int y = 0; y < height; y++) map.SetTile(x, y, ground);
        }
    }
}
```

## Example: Is the player standing on ground?
file: Scripts/TileGrounded.cs
```csharp
using Elintria;

public class TileGrounded : ElintriaScript
{
    public GameObject level;

    public bool Grounded()
    {
        Tilemap map = Tilemap.Get(level);
        return map != null && map.HasTile(transform.position + Vector3.down * 0.55f);
    }
}
```

# NavAgent
summary: Move a character along the nav mesh: set a destination, read its path, speed and status.
see: NavMeshRuntime, NavCrowd, NavLinkKind

Bake a navigation mesh, add a **Nav Mesh Agent** to a character, and `NavAgent.Of(obj).SetDestination(point)` walks it there around obstacles. The static methods query the nav mesh directly, without an agent: snap a point onto it, test a straight line, or calculate a path.

## Members
currentLinkKind: The kind of off-mesh link being crossed now (None while walking), for choosing an animation.
hasMesh: True when the scene has a baked nav mesh.
SamplePosition: Finds the nearest point on the nav mesh within `maxDistance` of `source`.
@source: The point to start from.
@maxDistance: How far to search.
CalculatePath: Calculates a path between two points on the mesh, without moving anything.
Of: The agent on `go`. Always returns an object; check `exists` to see whether `go` really has a Nav Mesh Agent.
exists: Whether the object has a Nav Mesh Agent.
SetDestination: Starts walking to `target`. Returns false when no path is possible.
destination: Where it is going.
position: Where it is.
velocity: How fast and which way it is moving.
speed: Its top walking speed.
isStopped: Pauses it on its path when true.
remainingDistance: How far is left along the path.
hasPath: Whether it has a path.
isOnNavMesh: Whether it is standing on the mesh.
isOnOffMeshLink: Whether it is crossing a link (a jump, a ladder, a door).
Warp: Teleports it to `p` on the mesh.
ResetPath: Forgets the path and stops.

## Example: Click to move
file: Scripts/ClickToMove.cs
```csharp
using Elintria;

public class ClickToMove : ElintriaScript
{
    public GameObject cameraObject;

    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Transform cam = cameraObject.transform;
        if (Physics.Raycast(cam.position, -cam.forward, out RaycastHit hit, 200f))
            NavAgent.Of(gameObject).SetDestination(hit.point);
    }
}
```

## Example: A guard patrol
file: Scripts/GuardPatrol.cs
```csharp
using System.Collections.Generic;
using Elintria;

public class GuardPatrol : ElintriaScript
{
    public List<GameObject> points = new List<GameObject>();
    int next;
    NavAgent agent;

    public override void Start()
    {
        agent = NavAgent.Of(gameObject);
        agent.speed = 2f;
        if (points.Count > 0) agent.SetDestination(points[0].transform.position);
    }

    public override void Update()
    {
        if (!agent.exists || points.Count == 0) return;
        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            next = (next + 1) % points.Count;
            agent.SetDestination(points[next].transform.position);
        }
    }
}
```

## Example: Checking reachability before spawning
file: Scripts/ReachableSpawn.cs
```csharp
using Elintria;

public class ReachableSpawn : ElintriaScript
{
    public GameObject player;

    public bool CanReachPlayer(Vector3 from)
    {
        if (!NavAgent.hasMesh) return false;
        if (!NavAgent.SamplePosition(from, 2f, out Vector3 onMesh)) return false;
        return NavAgent.CalculatePath(onMesh, player.transform.position, out Vector3[] corners) && corners.Length > 0;
    }
}
```

# NavMeshRuntime
summary: Add obstacles and off-mesh links at run time: built walls, closed doors, ladders, jumps.
see: NavAgent, NavLinkKind, NavCrowd

Things players build or change while playing need to affect pathfinding. `AddObstacle` and `AddBoxObstacle` put obstacles on the nav mesh; with `carve` set, they cut a hole that paths go around. `AddLink` joins two points with a ladder, jump, drop, teleport or door, and doors can be opened, shut or locked. Each call returns an id you use to move or remove the thing later. Everything made here is removed when play stops.

## Members
MoveObstacle: Moves obstacle `id` to `feet`, turned `yaw` degrees.
RemoveObstacle: Removes obstacle `id`.
RemoveLink: Removes link `id`.
IsLinkOpen: Whether door link `id` is open.

## Example: A player-built wall
file: Scripts/BuildWall.cs
```csharp
using Elintria;

public class BuildWall : ElintriaScript
{
    int obstacle = -1;

    public override void Start()
    {
        obstacle = NavMeshRuntime.AddBoxObstacle(transform.position, new Vector3(4f, 2.5f, 0.3f), transform.eulerAngles.y, true);
    }

    public override void OnDestroy()
    {
        if (obstacle >= 0) NavMeshRuntime.RemoveObstacle(obstacle);
    }
}
```

## Example: A ladder link
file: Scripts/LadderLink.cs
```csharp
using Elintria;

public class LadderLink : ElintriaScript
{
    public GameObject bottom;
    public GameObject top;
    int link = -1;

    public override void Start()
    {
        link = NavMeshRuntime.AddLink(bottom.transform.position, top.transform.position, NavLinkKind.Climb, true, 3f);
        if (link < 0) Debug.LogWarning("A ladder end is off the nav mesh");
    }

    public override void OnDestroy() { if (link >= 0) NavMeshRuntime.RemoveLink(link); }
}
```

## Example: A lockable door
file: Scripts/NavDoor.cs
```csharp
using Elintria;

public class NavDoor : ElintriaScript
{
    public GameObject sideA;
    public GameObject sideB;
    int link;

    public override void Start() { link = NavMeshRuntime.AddLink(sideA.transform.position, sideB.transform.position, NavLinkKind.Door); }

    public void Open() { NavMeshRuntime.SetLinkOpen(link, true); }
    public void Close() { NavMeshRuntime.SetLinkOpen(link, false); }
    public void Lock(bool locked) { NavMeshRuntime.SetLinkEnabled(link, !locked); }
}
```

# NavCrowd
summary: Settings and statistics for the crowd every Nav Mesh Agent belongs to.
see: NavAgent

All agents move as one crowd: they avoid each other, share flow fields when many head for the same goal, and update less often when far from the player. `Configure` tunes this; pass -1 to leave a setting as it is.

## Members
Configure: Changes crowd settings. Any value left at -1 keeps its current setting.
SetFocus: Where the player is, so agents far away can update less often.
@p: The player's position.
Stats: Crowd statistics: step time in ms, agents, skipped by distance, flow fields, flow paths, A* paths, doors opened, links crossed.

## Example: Lots of zombies
file: Scripts/HordeSettings.cs
```csharp
using Elintria;

public class HordeSettings : ElintriaScript
{
    public override void Start()
    {
        NavCrowd.Configure(maxNeighbors: 6, quality: 1, flowFields: 1, lodDistance: 40f);
    }
}
```

## Example: Keeping the focus on the player
file: Scripts/CrowdFocus.cs
```csharp
using Elintria;

public class CrowdFocus : ElintriaScript
{
    public override void Update() { NavCrowd.SetFocus(transform.position); }
}
```

## Example: Showing crowd statistics
file: Scripts/CrowdStats.cs
```csharp
using Elintria;

public class CrowdStats : ElintriaScript
{
    public override void Update()
    {
        float[] s = NavCrowd.Stats();
        ScreenGUI.Text(20, 20, "Crowd: " + s[1] + " agents, " + s[0].ToString("0.00") + " ms, " + s[2] + " skipped");
    }
}
```

# NavLinkKind
summary: How an off-mesh link is crossed: walk, jump, drop, climb, teleport or door.
see: NavMeshRuntime, NavAgent

Agents report the kind of link they are crossing in [[NavAgent.currentLinkKind]], so you can play the right animation.

## Members
None: Not on a link (walking normally).
Auto: Chosen from the shape of the gap.
Walk: Walked across.
Jump: Jumped across.
Drop: Dropped down.
Climb: Climbed (a ladder).
Teleport: Instantly moved across.
Door: Walked through a door, which may need opening first.

## Example: Animations for links
file: Scripts/LinkAnims.cs
```csharp
using Elintria;

public class LinkAnims : ElintriaScript
{
    public override void Update()
    {
        NavAgent a = NavAgent.Of(gameObject);
        Animator anim = GetComponent<Animator>();
        if (anim == null) return;
        anim.SetBool("Climbing", a.currentLinkKind == NavLinkKind.Climb);
        anim.SetBool("Jumping", a.currentLinkKind == NavLinkKind.Jump || a.currentLinkKind == NavLinkKind.Drop);
    }
}
```

## Example: A jump link over a gap
file: Scripts/GapJump.cs
```csharp
using Elintria;

public class GapJump : ElintriaScript
{
    public GameObject landing;

    public override void Start()
    {
        NavMeshRuntime.AddLink(transform.position, landing.transform.position, NavLinkKind.Jump, false);
    }
}
```

## Example: Waiting at doors
file: Scripts/DoorWait.cs
```csharp
using Elintria;

public class DoorWait : ElintriaScript
{
    public override void Update()
    {
        NavAgent a = NavAgent.Of(gameObject);
        if (a.isWaitingForDoor) Debug.Log(name + " is waiting at a door");
        else if (a.currentLinkKind == NavLinkKind.Door) Debug.Log(name + " is going through a door (" + Mathf.RoundToInt(a.linkProgress * 100f) + "%)");
    }
}
```
