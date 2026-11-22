# page: manual-ui-designer | The UI Designer
summary: Build menus and HUDs in the editor, wire them to your scripts without code, and drive them from C# when you need to.

**Window > UI Designer** (or the toolbar's UI Designer button) opens the game's UI in a dock tab. The canvas in the middle shows the UI at a chosen resolution, with the safe area of a phone or TV if you like; the Hierarchy on the left lists every element, and the Inspector on the right edits the selected one. Everything you make is saved with the scene and drawn by the game exactly as the designer shows it.

## Building screens

- **Widgets**: drag from the Widgets tab or click to add: Panel, Text, Button, Image, Slider, Input Field, Toggle, Scroll View, Dropdown, Progress Bar, Tab Bar, Tooltip, Radial Menu, Minimap and Inventory Grid.
- **Templates**: the Templates tab builds a whole screen in one click: main menu, pause, settings, HUD, dialogue, inventory, loading and game over. Their buttons are already wired.
- **Layout**: anchors and pivots (the Rect Transform), horizontal, vertical and grid layout groups, align and distribute, snapping with smart guides, and nine-slice images for panels that stretch.
- **Text**: rich-text tags (`<b>`, `<i>`, `<u>`, `<color=#ffcc33>`, `<size=40>`, `<br>`), any font in your project, outline, shadow and auto-size.
- **Themes**: colour and font *tokens* (Primary, Background, Heading...). An element that uses a token changes when the token does.
- **Prefabs**: save an element and its children as a UI prefab, place copies, apply or revert changes, and make variants.
- **Undo**: Ctrl+Z / Ctrl+Y undo anything done in the designer. While the game plays, edits change the running UI and are thrown away on Stop unless **Keep play changes** is on.

## Events without code

Each element's **Events** section lists actions to run **On click** and **On value changed** (sliders, toggles, dropdowns). An action is one of:

- `show:Name`, `hide:Name`, `toggle:Name`: an element by name.
- `showgroup:prefix`, `hidegroup:prefix`: every element whose name starts with `prefix` (a whole menu, such as `pause.`).
- `settext:Name=new text` and `anim:Name` (play an element's show animation).
- `scene:Scenes/Level2`: load a scene. `quit`: close the game.
- `msg:Player.OnButton`: [[GameObject.SendMessage]] on an object.
- `call:Player/PlayerScript.Jump(arg)`: a public method on a script, with an optional argument. On value changed, a `call:` without an argument passes the new value: a slider's value, a toggle's 1 or 0, a dropdown's, tab bar's or radial menu's index, an input field's text.

The `msg:` and `call:` actions reach your scripts through [[UIBridge]].

```csharp
using Elintria;

// "call:Settings/GameSettings.SetMusicVolume" on a slider's On value changed.
public class GameSettings : ElintriaScript
{
    public float music = 0.8f;

    public void SetMusicVolume(float value)
    {
        music = value;
        Debug.Log("Music volume " + value);
    }
}
```

## Data binding

The **Data Binding** section shows a script's value without code: set **Value** to `GameObject/Script.field` (properties and dotted paths work too: `Player/Stats.armor.current`). A Text shows the value; a Progress Bar or Slider fills to it, out of **Maximum** (a number, or another path such as `Player/Health.maxHealth`). **Format** shapes the text: `{0}` is the value and `{1}` the maximum, as in `HP {0} / {1}`.

## Animation and states

- **When shown**: fade, slide in from a side, scale or pop, with a time and a delay. Showing an element again plays it again.
- **Loop**: pulse, bob, float, spin or blink.
- **Hover scale** grows a button under the pointer; buttons ease between their normal, hover, pressed and disabled colours.

## From C#

[[UIRuntime]] reads and changes the live UI by element name: text, visibility, clicks, fills, and properties such as a dropdown's selection (`sel`), a minimap's centre (`mapx`, `mapy`, `maprot`) or an inventory slot's picture (`slot3`) and count (`count3`).

```csharp
using Elintria;

public class HudDriver : ElintriaScript
{
    public float ammo = 30f;

    public override void Update()
    {
        UIRuntime.SetText("hud.ammo", ((int)ammo).ToString() + " / 90");
        if (UIRuntime.WasClicked("pause.Resume")) UIRuntime.SetGroupVisible("pause.", false);
    }
}
```

> tip: The **Stats** button shows what the UI costs each frame (elements, glyphs, draw calls, milliseconds). Turn on **Stats in Game** to see it over the Game view while playing.

# UIBridge
summary: How the UI Designer's no-code bindings and actions reach your scripts: read a value by path, run an action string.
see: UIRuntime, GameObject

The engine calls `UIBridge` for every **Data Binding** once a frame and for every `call:` / `msg:` / `scene:` / `quit` action an element runs. You can call it yourself too: to read a value the same way a binding does, or to run an action string kept in data (a dialogue choice, a menu built from a file).

A path is `GameObject/Component.field`; fields, properties and dotted paths into them work (`Player/Stats.armor.current`), and `GameObject/field` looks in every script on the object. An action is `call:GameObject/Component.Method(arg)`, `msg:GameObject.Method`, `scene:Path` or `quit`.

## Members
Read: The value at a binding path, as text ("" when there is none).
@path: "GameObject/Component.field", or "GameObject/field".
ReadAll: Several paths at once, one per line; the values come back one per line in the same order.
@paths: Paths separated by newlines.
Run: Runs one action string; returns "" when it worked, else what went wrong.
@action: "call:Player/Shop.Buy(sword)", "msg:Player.OnButton", "scene:Scenes/Level2" or "quit".

## Example: Reading a bound value yourself
file: Scripts/HealthReadout.cs
```csharp
using Elintria;

public class HealthReadout : ElintriaScript
{
    public override void Update()
    {
        // The same path a Text element's Data Binding would use.
        string hp = UIBridge.Read("Player/Health.health");
        if (hp != "") UIRuntime.SetText("HealthLabel", "HP " + hp);
    }
}
```

## Example: Dialogue choices that run actions
file: Scripts/DialogueChoices.cs
```csharp
using Elintria;

public class DialogueChoices : ElintriaScript
{
    // Each choice: the button's name and the action it runs.
    public string[] buttons = { "choice1", "choice2", "choice3" };
    public string[] actions = { "msg:Shopkeeper.OpenShop", "scene:Scenes/Town", "quit" };

    public override void Update()
    {
        for (int i = 0; i < buttons.Length && i < actions.Length; ++i)
        {
            if (!UIRuntime.WasClicked(buttons[i])) continue;
            string error = UIBridge.Run(actions[i]);
            if (error != "") Debug.LogWarning("Choice " + i + ": " + error);
        }
    }
}
```

## Example: Several values in one call
file: Scripts/StatusPanel.cs
```csharp
using Elintria;

public class StatusPanel : ElintriaScript
{
    public override void Update()
    {
        string[] v = UIBridge.ReadAll("Player/Health.health\nPlayer/Health.maxHealth\nPlayer/Wallet.money").Split('\n');
        if (v.Length < 3) return;
        UIRuntime.SetText("StatusText", "HP " + v[0] + " / " + v[1] + "   Coins " + v[2]);
    }
}
```

# NoiseEvent
summary: One noise made with StdNoise.Make: where it was, how far it carries, when, and who made it.
see: StdNoise, BasicEnemy

Enemies and your own listeners get one back from `StdNoise.Heard`: the loudest recent noise they could hear.

## Members
position: Where the noise was made.
radius: How far it carries, in metres, before a listener's hearing multiplies it.
time: When it was made (Time.time).
source: The object that made it, or null.

## Example: A guard that turns toward noises
file: Scripts/NoiseListener.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NoiseListener : ElintriaScript
{
    public float hearing = 1f;

    public override void Update()
    {
        StdNoise.NoiseEvent n;
        if (StdNoise.Heard(transform.position, hearing, 2f, out n))
        {
            Vector3 dir = n.position - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.01f) transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
```

## Example: Footsteps that carry further when running
file: Scripts/Footsteps.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Footsteps : ElintriaScript
{
    public float stepEvery = 0.45f;
    float timer;

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer < stepEvery) return;
        timer = 0f;
        bool running = Input.GetKey(KeyCode.LeftShift);
        StdNoise.Make(transform.position, running ? 12f : 4f, gameObject);
    }
}
```

## Example: Who was heard
file: Scripts/NoiseAlarm.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NoiseAlarm : ElintriaScript
{
    public override void Update()
    {
        StdNoise.NoiseEvent n;
        if (!StdNoise.Heard(transform.position, 1.5f, 1f, out n)) return;
        string who = n.source != null ? n.source.name : "something";
        float age = Time.time - n.time;
        Debug.Log("Heard " + who + " " + age.ToString("0.0") + " s ago, " + n.radius + " m noise");
    }
}
```

# StdModifiers
summary: What multiplies the damage a character deals: its Character Stats and its Equipment together.
see: CharacterStats, Equipment, StdDamage

The Standard weapons multiply their damage by this, so strength and gear count everywhere. Use it in your own attacks to match them.

## Members
DamageMultiplier: The attacker's damage multiplier: Character Stats' damage multiplier times one plus the Equipment's damage bonus. 1 for an object with neither, or null.
@attacker: The object dealing the damage.

## Example: A custom attack that respects stats and gear
file: Scripts/PunchAttack.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PunchAttack : ElintriaScript
{
    public float baseDamage = 10f;
    public float reach = 1.5f;

    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.forward, out hit, reach)) return;
        Health h = hit.collider.gameObject.GetComponent<Health>();
        if (h != null) h.Damage(baseDamage * StdModifiers.DamageMultiplier(gameObject), gameObject);
    }
}
```

## Example: Showing the multiplier on a character sheet
file: Scripts/DamageSheet.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DamageSheet : ElintriaScript
{
    public override void Update()
    {
        float k = StdModifiers.DamageMultiplier(gameObject);
        UIRuntime.SetText("DamageText", "Damage x" + k.ToString("0.00"));
    }
}
```

## Example: Comparing two fighters
file: Scripts/FighterCompare.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FighterCompare : ElintriaScript
{
    public GameObject a;
    public GameObject b;

    public override void Start()
    {
        float ka = StdModifiers.DamageMultiplier(a), kb = StdModifiers.DamageMultiplier(b);
        Debug.Log((ka >= kb ? a.name : b.name) + " hits harder (" + ka.ToString("0.00") + " vs " + kb.ToString("0.00") + ")");
    }
}
```

# StdExtensions
summary: Small helpers the Standard components use, available to your scripts too.
see: Transform

## Members
IsChildOf: Whether a transform is the given parent or anywhere below it.
@t: The transform to test.
@parent: The possible parent (or the same transform).

## Example: Ignoring hits on yourself
file: Scripts/SelfSafeTrigger.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SelfSafeTrigger : ElintriaScript
{
    public GameObject owner;

    public override void OnTriggerEnter(Collider other)
    {
        // A collider anywhere on the owner (an arm, a weapon) does not count.
        if (owner != null && other.transform.IsChildOf(owner.transform)) return;
        Debug.Log("Hit " + other.gameObject.name);
    }
}
```

## Example: Is the player standing on this platform's part?
file: Scripts/PlatformCheck.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PlatformCheck : ElintriaScript
{
    public Transform player;

    public override void Update()
    {
        RaycastHit hit;
        if (player != null && Physics.Raycast(player.position, Vector3.down, out hit, 1.5f))
            if (hit.collider.transform.IsChildOf(transform)) Debug.Log("Player is on " + name);
    }
}
```

## Example: Counting children under a group
file: Scripts/GroupCounter.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GroupCounter : ElintriaScript
{
    public override void Start()
    {
        int n = 0;
        foreach (GameObject g in GameObject.FindObjectsOfType<GameObject>())
            if (g != gameObject && g.transform.IsChildOf(transform)) ++n;
        Debug.Log(name + " holds " + n + " objects");
    }
}
```

# SaveSlotInfo
summary: One save slot's summary, from SaveSystem.Info: whether it has a save, when, its label and the play time.
see: SaveSystem

## Members
slot: The slot number.
exists: Whether the slot has a save.
time: When it was saved (local time).
label: The label given to SaveSystem.Save.
playSeconds: How long the game had been played when it was saved, in seconds.

## Example: Slot buttons with their details
file: Scripts/SlotList.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SlotList : ElintriaScript
{
    public override void Start()
    {
        for (int i = 1; i <= 3; ++i)
        {
            SaveSlotInfo s = SaveSystem.Info(i);
            string text = s.exists ? s.label + "  " + s.time.ToString("g") : "Empty";
            UIRuntime.SetText("slot" + i + "Text", text);
        }
    }
}
```

## Example: Showing play time
file: Scripts/PlayTime.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PlayTime : ElintriaScript
{
    public int slot = 1;

    public override void Start()
    {
        SaveSlotInfo s = SaveSystem.Info(slot);
        if (!s.exists) return;
        int minutes = (int)(s.playSeconds / 60f);
        Debug.Log("Slot " + s.slot + ": " + minutes / 60 + " h " + minutes % 60 + " min played");
    }
}
```

## Example: Continue from the newest save
file: Scripts/ContinueButton.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ContinueButton : ElintriaScript
{
    public override void Update()
    {
        if (!UIRuntime.WasClicked("ContinueButton")) return;
        SaveSlotInfo newest = null;
        for (int i = 1; i <= 5; ++i)
        {
            SaveSlotInfo s = SaveSystem.Info(i);
            if (s.exists && (newest == null || s.time > newest.time)) newest = s;
        }
        if (newest != null) SaveSystem.Load(newest.slot);
    }
}
```
