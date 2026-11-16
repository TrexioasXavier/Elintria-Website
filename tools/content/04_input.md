# Input
summary: Read the keyboard, mouse buttons, pointer position, scroll wheel and Input Manager axes.
see: KeyCode, Mouse, GameWindow

`Input` is how scripts read the player. `GetKey` is true for every frame a key is held. `GetKeyDown` and `GetKeyUp` are true only on the frame it went down or came up. Mouse buttons work the same way (0 left, 1 right, 2 middle). Axes from **Project Settings > Input Manager**, such as `"Horizontal"`, `"Vertical"`, `"Mouse X"` and `"Jump"`, are read with `GetAxis` and `GetButton`.

> note: In the editor, the game only gets input while the Game view has focus. Click it after pressing Play.

## Members
GetMouseButtonDown: True on the frame the mouse button went down.
GetMouseButtonUp: True on the frame the mouse button came up.
mouseScrollDelta: The scroll wheel movement this frame (positive away from you).
@key: The key to test.
@axisName: The name of an axis in the Input Manager, such as "Horizontal" or "Mouse X".
@buttonName: The name of a button axis in the Input Manager, such as "Jump" or "Fire1".

## Example: WASD movement with axes
file: Scripts/AxisMover.cs
`GetAxis` is smoothed and returns -1..1, so movement starts and stops gently. `GetAxisRaw` would snap.
```csharp
using Elintria;

public class AxisMover : ElintriaScript
{
    public float speed = 6f;

    public override void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0f, v);
        if (move.sqrMagnitude > 1f) move = move.normalized;
        transform.position += move * speed * Time.deltaTime;

        if (Input.GetButtonDown("Jump")) Debug.Log("Jump!");
    }
}
```

## Example: Keys, edges and modifiers
file: Scripts/Hotkeys.cs
`GetKeyDown` fires once per press, so it suits toggles and actions; `GetKey` suits anything held.
```csharp
using Elintria;

public class Hotkeys : ElintriaScript
{
    public GameObject map;
    float charge;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && map != null) map.SetActive(!map.activeSelf);

        if (Input.GetKey(KeyCode.Control) && Input.GetKeyDown(KeyCode.S)) Debug.Log("Quick save");

        if (Input.GetKey(KeyCode.F)) charge += Time.deltaTime;          // held
        if (Input.GetKeyUp(KeyCode.F)) { Debug.Log("Threw with power " + charge); charge = 0f; }

        if (Input.GetKey("left shift")) Debug.Log("Sprinting");          // by Unity key name
    }
}
```

## Example: Mouse look and clicking
file: Scripts/MouseLook.cs
Holding the right button turns the view with `mouseDelta`; a left click raycasts from the screen centre.
```csharp
using Elintria;

public class MouseLook : ElintriaScript
{
    public float sensitivity = 0.15f;
    float yaw, pitch;

    public override void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector2 d = Input.mouseDelta;
            yaw += d.x * sensitivity;
            pitch = Mathf.Clamp(pitch - d.y * sensitivity, -80f, 80f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 50f))
            Debug.Log("Clicked " + hit.gameObject.name + " at pointer " + Input.mousePosition);
    }
}
```

# KeyCode
summary: Every keyboard key, for Input.GetKey, GetKeyDown and GetKeyUp.
see: Input

`KeyCode` names the keys. Its values are Windows virtual-key codes, the same ones C++ scripts use, so both languages read the same key.

## Members
*: The {name} key.
None: No key.
Return: The Enter key (the same value as Enter).
Enter: The Enter key.
Shift: Either Shift key.
Control: Either Ctrl key.
Ctrl: Either Ctrl key (the same as Control).
Alt: Either Alt key.
LeftShift: The left Shift key. Windows usually reports Shift without saying which side, so prefer `Shift`.
RightShift: The right Shift key. Prefer `Shift`.
LeftControl: The left Ctrl key. Prefer `Control`.
RightControl: The right Ctrl key. Prefer `Control`.
LeftAlt: The left Alt key. Prefer `Alt`.
RightAlt: The right Alt key. Prefer `Alt`.
Alpha0: The 0 key on the main keyboard (not the number pad).
Alpha1: The 1 key on the main keyboard.
Alpha2: The 2 key on the main keyboard.
Alpha3: The 3 key on the main keyboard.
Alpha4: The 4 key on the main keyboard.
Alpha5: The 5 key on the main keyboard.
Alpha6: The 6 key on the main keyboard.
Alpha7: The 7 key on the main keyboard.
Alpha8: The 8 key on the main keyboard.
Alpha9: The 9 key on the main keyboard.
LeftArrow: The left arrow key.
RightArrow: The right arrow key.
UpArrow: The up arrow key.
DownArrow: The down arrow key.
PageUp: The Page Up key.
PageDown: The Page Down key.
CapsLock: The Caps Lock key.

## Example: Number keys for a hotbar
file: Scripts/HotbarKeys.cs
`Alpha1`..`Alpha9` are consecutive, so arithmetic on the enum works.
```csharp
using Elintria;

public class HotbarKeys : ElintriaScript
{
    public int selected;

    public override void Update()
    {
        for (int i = 0; i < 9; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) { selected = i; Debug.Log("Slot " + (i + 1)); }
    }
}
```

## Example: Rebindable keys from a string
file: Scripts/Rebind.cs
Inspector fields cannot hold an enum, so the key is stored as text and parsed.
```csharp
using System;
using Elintria;

public class Rebind : ElintriaScript
{
    public string jumpKey = "Space";
    KeyCode jump = KeyCode.Space;

    public override void Start()
    {
        if (!Enum.TryParse(jumpKey, true, out jump)) jump = KeyCode.Space;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(jump)) Debug.Log("Jump (" + jump + ")");
    }
}
```

## Example: Arrow-key menu navigation
file: Scripts/ArrowMenu.cs
```csharp
using Elintria;

public class ArrowMenu : ElintriaScript
{
    string[] items = { "Continue", "Options", "Quit" };
    int index;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) index = (index + 1) % items.Length;
        if (Input.GetKeyDown(KeyCode.UpArrow)) index = (index + items.Length - 1) % items.Length;
        if (Input.GetKeyDown(KeyCode.Return)) Debug.Log("Chose " + items[index]);

        for (int i = 0; i < items.Length; i++)
            ScreenGUI.Text(60, 100 + i * 30, (i == index ? "> " : "  ") + items[i], null, i == index ? Color.yellow : Color.white);
    }
}
```

# Mouse
summary: Own the mouse cursor: hide or lock it and read movement for first-person look.
see: Input, GameWindow

`Mouse` controls the operating system cursor for first-person games. `DisableCursor` hides it and keeps it in the window. `RecenterAndGetDelta` snaps it back to the centre every frame and returns how far it moved, which is the classic mouse-look technique.

## Members
DisableCursor: Hides the cursor and confines it to the game window, for mouse look.
EnableCursor: Gives the cursor back: visible and free to leave the window.
ShowCursor: Makes the cursor visible.
HideCursor: Hides the cursor without confining it.

## Example: First-person mouse look
file: Scripts/FpsLook.cs
```csharp
using Elintria;

public class FpsLook : ElintriaScript
{
    public float sensitivity = 0.12f;
    float yaw, pitch;

    public override void Start() { Mouse.DisableCursor(); }

    public override void Update()
    {
        Vector2 d = Mouse.RecenterAndGetDelta();
        yaw += d.x * sensitivity;
        pitch = Mathf.Clamp(pitch - d.y * sensitivity, -85f, 85f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        if (Input.GetKeyDown(KeyCode.Escape)) Mouse.EnableCursor();
    }

    public override void OnDisable() { Mouse.EnableCursor(); }
}
```

## Example: Showing the cursor in menus only
file: Scripts/CursorForMenus.cs
```csharp
using Elintria;

public class CursorForMenus : ElintriaScript
{
    public bool menuOpen;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menuOpen = !menuOpen;
            if (menuOpen) Mouse.ShowCursor(); else Mouse.HideCursor();
        }
    }
}
```

## Example: Mouse-driven turret with a dead zone
file: Scripts/MouseTurret.cs
```csharp
using Elintria;

public class MouseTurret : ElintriaScript
{
    public float speed = 0.2f;
    float yaw;

    public override void Start() { Mouse.DisableCursor(); }

    public override void Update()
    {
        Vector2 d = Mouse.RecenterAndGetDelta();
        if (Mathf.Abs(d.x) > 1f) yaw += d.x * speed;   // ignore tiny jitter
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
}
```

# GameWindow
summary: The window the game runs in: its size, focus, raw OS input, cursor locking, screenshots and quitting.
see: Input, Mouse, ScreenGUI

`GameWindow` is the same in the editor's Game view and in a built game. It reports the game image's size, lets you capture screenshots, quit, and choose whether Escape closes the game. It can also read input straight from the operating system with Windows virtual-key codes.

## Members
SetCursorLocked: Locks the cursor to the middle of the game image and hides it (true), or frees it (false). Read movement with LockedDelta.
TakeWheel: Mouse wheel notches since the last call (positive away from you).
LockedDelta: Mouse movement since the last call while the cursor is locked.
GetViewport: The game image's rectangle inside the window, in pixels.
@x: Filled in with the left edge.
@y: Filled in with the top edge.
@w: Filled in with the width.
@h: Filled in with the height.
width: The game image's width in pixels.
height: The game image's height in pixels.
Quit: Ends the game (in the editor, stops Play mode).
@quits: True for the default (Escape closes the game), false when your game has its own pause menu.
@path: Where to save the .bmp file.
@virtualKey: A Windows virtual-key code (65 for A, 1 for the left mouse button).
@locked: True to lock and hide the cursor in the middle of the game image.

## Example: A pause menu that owns Escape
file: Scripts/EscapeMenu.cs
```csharp
using Elintria;

public class EscapeMenu : ElintriaScript
{
    bool open;

    public override void Start() { GameWindow.SetEscapeQuits(false); }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { open = !open; Time.timeScale = open ? 0f : 1f; }
        if (!open) return;
        if (ScreenGUI.Button(new Rect(GameWindow.width / 2 - 100, 200, 200, 48), "Resume")) { open = false; Time.timeScale = 1f; }
        if (ScreenGUI.Button(new Rect(GameWindow.width / 2 - 100, 260, 200, 48), "Quit")) GameWindow.Quit();
    }
}
```

## Example: Locked cursor look and focus
file: Scripts/LockedLook.cs
```csharp
using Elintria;

public class LockedLook : ElintriaScript
{
    float yaw;

    public override void Update()
    {
        if (!GameWindow.hasFocus) return;   // don't steal the mouse when alt-tabbed
        GameWindow.SetCursorLocked(true);
        yaw += GameWindow.LockedDelta().x * 0.15f;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    public override void OnDisable() { GameWindow.SetCursorLocked(false); }
}
```

## Example: Screenshots and raw keys
file: Scripts/PhotoKey.cs
```csharp
using Elintria;

public class PhotoKey : ElintriaScript
{
    bool wasDown;

    public override void Update()
    {
        bool down = GameWindow.KeyDown(0x2C);   // VK_SNAPSHOT (Print Screen)
        if (down && !wasDown)
        {
            GameWindow.Capture(Project.Resolve("capture_" + System.DateTime.Now.ToString("HHmmss") + ".bmp"));
            Debug.Log("Saved a " + GameWindow.width + "x" + GameWindow.height + " screenshot");
        }
        wasDown = down;
        if (GameWindow.MousePosition(out float mx, out float my)) ScreenGUI.Text(10, 10, "Pointer " + mx + ", " + my);
    }
}
```
