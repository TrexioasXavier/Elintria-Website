# Inventory
summary: An inventory of item stacks, with a hotbar, a selected slot, stacking, moving, splitting and using items.
see: InventoryUI, ItemDefinition, ItemDatabase, ItemStack, Pickup

`Inventory` holds `slotCount` slots of [[ItemStack]]s. The first `hotbarSize` slots are the hotbar, and `selected` is the one in hand. Items are described once, by id, in `Assets/Items/*.json` (see [[ItemDefinition]]); an id nobody described still works. On every change, `"OnInventoryChanged"` is sent to the object and the `onChanged` event fires. Using the selected item sends `"OnUseItem"` with its id.

## Members
slotCount: How many slots the inventory has.
hotbarSize: How many of the first slots form the hotbar.
startingItems: What it starts with: "id" or "id:count" each.
onChangedTargets: Objects told when the contents change.
onChangedMessage: The message they get.
slots: The slots.
selected: The hotbar slot in hand.
version: Goes up on every change, so a UI can redraw only when needed.
Add: Adds `count` of `id`, topping up existing stacks first, then empty slots. Returns how many did NOT fit.
@id: The item id.
Remove: Takes up to `count` of `id`. Returns how many it took.
Count: How many of `id` it holds in total.
Has: True when it holds at least `count` of `id`.
HasRoomFor: True when `count` of `id` would fit.
Move: Drag and drop: moves slot `from` onto `to`, merging the same item up to its stack size, otherwise swapping.
@from: The slot dragged from.
@to: The slot dropped on.
Split: Moves half of slot `i`'s stack into the first empty slot.
TakeSlot: Empties slot `i` and returns what was there.
Selected: The stack in hand.
Select: Picks hotbar slot `hotbarIndex`.
UseSelected: Uses the item in hand: "OnUseItem" goes to this object with its id. A script that consumes it calls Remove.
Clear: Empties every slot.
Describe: A summary such as "coin x3, potion x1", for logs and tests.

## Example: Using potions
file: Scripts/PotionUser.cs
`OnUseItem` arrives with the id when the player uses the item in hand (F by default).
```csharp
using Elintria;
using Elintria.Standard;

public class PotionUser : ElintriaScript
{
    public void OnUseItem(string id)
    {
        Inventory inv = GetComponent<Inventory>();
        Health h = GetComponent<Health>();
        if (id == "potion" && h.health < h.maxHealth && inv.Remove("potion") == 1)
            h.Heal(25f);
    }
}
```

## Example: Crafting from ingredients
file: Scripts/Workbench.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Workbench : Interactable
{
    public override string Prompt(Interactor who) => "Craft Torch (2 wood, 1 cloth)";

    public override bool CanInteract(Interactor who, out string reason)
    {
        Inventory inv = who.inventory;
        bool ok = inv.Has("wood", 2) && inv.Has("cloth", 1) && inv.HasRoomFor("torch");
        reason = ok ? "" : "Missing materials";
        return ok;
    }

    public override void Interact(Interactor who)
    {
        who.inventory.Remove("wood", 2);
        who.inventory.Remove("cloth", 1);
        who.inventory.Add("torch");
        who.Say("Crafted a torch");
    }
}
```

## Example: Starting kit and a save summary
file: Scripts/StartingKit.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StartingKit : ElintriaScript
{
    public override void Start()
    {
        Inventory inv = GetComponent<Inventory>();
        int leftOver = inv.Add("arrow", 150);   // stacks of maxStack each
        if (leftOver > 0) Debug.Log(leftOver + " arrows didn't fit");
        inv.Select(0);
        Debug.Log("Carrying: " + inv.Describe() + "; in hand: " + inv.Selected);
    }

    public void OnInventoryChanged() { Debug.Log("Inventory changed (version " + GetComponent<Inventory>().version + ")"); }
}
```

# InventoryUI
summary: Draws the hotbar and the full bag, with drag-and-drop, stack splitting and dropping items.
see: Inventory

Shows the hotbar at the bottom of the screen. Tab or I opens the whole bag, where you drag an item onto another slot to move, swap or merge it, right-click to split a stack, and drag it out of the bag to drop it. The 1-9 keys and the mouse wheel pick the hotbar slot, and F uses it.

## Members
inventoryObject: The inventory to show (empty: the one on this object).
slotSize: Each slot's size in pixels.
spacing: The gap between slots.
columns: Slots per row in the bag.
showHotbar: Draw the hotbar.
dropTemplate: A scene object to copy for dropped items (it gets a Pickup). Empty: dropping is off.
open: Whether the bag is open.
dragFrom: The slot being dragged, or -1.
Toggle: Opens or closes the bag.
SlotRect: Where slot `i` is drawn now, in game-image pixels.
Drop: Drops slot `i`'s stack in front of the player as a pickup.

## Example: Opening the bag from your own button
file: Scripts/BagButton.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BagButton : ElintriaScript
{
    public override void Update()
    {
        InventoryUI ui = GetComponent<InventoryUI>();
        if (ScreenGUI.Button(new Rect(ScreenGUI.screenWidth - 110, 10, 100, 36), ui.open ? "Close" : "Bag")) ui.Toggle();
    }
}
```

## Example: Pausing while the bag is open
file: Scripts/PauseInBag.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PauseInBag : ElintriaScript
{
    public override void Update()
    {
        Time.timeScale = GetComponent<InventoryUI>().open ? 0f : 1f;
    }
}
```

## Example: Tooltips over slots
file: Scripts/SlotTooltips.cs
`SlotRect` tells you where each slot is drawn, so you can add your own overlays.
```csharp
using Elintria;
using Elintria.Standard;

public class SlotTooltips : ElintriaScript
{
    public override void Update()
    {
        InventoryUI ui = GetComponent<InventoryUI>();
        Inventory inv = GetComponent<Inventory>();
        if (!ui.open) return;
        for (int i = 0; i < inv.slots.Length; i++)
        {
            Rect r = ui.SlotRect(i);
            if (inv.slots[i].IsEmpty || !ScreenGUI.Hover(r)) continue;
            ItemDefinition def = ItemDatabase.Get(inv.slots[i].id);
            ScreenGUI.Label(new Rect(r.x, r.y - 26, 260, 24), def.displayName + ": " + def.description, null, Color.white, TextAnchor.MiddleLeft, false, Color.black);
        }
    }
}
```

# ItemDefinition
summary: What an item is: its id, name, description, stack size, icon, category and colour.
see: ItemDatabase, Inventory

Items are usually described in JSON files under `Assets/Items/`:

```json
[ { "id": "potion", "name": "Health Potion", "maxStack": 10,
    "icon": "Assets/Items/potion.png", "description": "Heals 25.",
    "category": "Consumable", "color": "#d04040" } ]
```

or registered from code with [[ItemDatabase.Register]].

## Members
id: The id inventories and pickups use.
displayName: The name shown to players.
description: A line describing it.
maxStack: How many fit in one slot.
icon: An image path for the UI.
category: A group name ("Weapon", "Consumable").
color: A colour for the UI when there is no icon.

## Example: Registering items from code
file: Scripts/RegisterItems.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RegisterItems : ElintriaScript
{
    public override void Awake()
    {
        ItemDatabase.Register(new ItemDefinition { id = "gem", displayName = "Ruby", description = "Sells well.", maxStack = 20, category = "Treasure", color = Color.red });
        ItemDatabase.Register(new ItemDefinition { id = "key_gold", displayName = "Gold Key", maxStack = 1, category = "Key", color = Color.yellow });
    }
}
```

## Example: Showing an item's details
file: Scripts/ItemCard.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ItemCard : ElintriaScript
{
    public string itemId = "potion";

    public override void Update()
    {
        ItemDefinition d = ItemDatabase.Get(itemId);
        if (d == null) return;
        ScreenGUI.Fill(new Rect(20, 20, 260, 70), new Color(0f, 0f, 0f, 0.6f));
        ScreenGUI.Text(30, 26, d.displayName + "  (" + d.category + ")", null, d.color);
        ScreenGUI.Text(30, 50, d.description + "  Stack " + d.maxStack);
    }
}
```

## Example: Sorting a bag by category
file: Scripts/BagSorter.cs
```csharp
using System.Linq;
using Elintria;
using Elintria.Standard;

public class BagSorter : ElintriaScript
{
    public void Sort()
    {
        Inventory inv = GetComponent<Inventory>();
        var stacks = inv.slots.Where(s => !s.IsEmpty).OrderBy(s => ItemDatabase.Get(s.id)?.category ?? "").ThenBy(s => s.id).ToList();
        inv.Clear();
        foreach (ItemStack s in stacks) inv.Add(s.id, s.count);
    }
}
```

# ItemDatabase
summary: Every known item by id, read from Assets/Items/*.json or registered from code.
see: ItemDefinition, Inventory

## Members
Register: Adds or replaces an item definition.
@def: The item's definition.
Get: The item with this id (an id nobody described gets a plain default definition).
All: Every registered item.
Reload: Reads Assets/Items/*.json again, after you edit them.

## Example: Listing every item
file: Scripts/ItemList.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ItemList : ElintriaScript
{
    public override void Start()
    {
        foreach (ItemDefinition d in ItemDatabase.All) Debug.Log(d.id + ": " + d.displayName + " (stacks to " + d.maxStack + ")");
    }
}
```

## Example: A cheat to give every item
file: Scripts/GiveAll.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GiveAll : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F9)) return;
        Inventory inv = GetComponent<Inventory>();
        foreach (ItemDefinition d in ItemDatabase.All) inv.Add(d.id, 1);
    }
}
```

## Example: Hot-reloading item data
file: Scripts/ItemReload.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ItemReload : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) { ItemDatabase.Reload(); Debug.Log("Item files reloaded"); }
    }
}
```

# ItemStack
summary: A number of one item in a slot.
see: Inventory

## Members
id: The item id ("" or null when empty).
count: How many.
IsEmpty: True for an empty slot.
ItemStack: Makes a stack of `count` of `id`.

## Example: Counting full slots
file: Scripts/FullSlots.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FullSlots : ElintriaScript
{
    public override void Update()
    {
        int used = 0;
        foreach (ItemStack s in GetComponent<Inventory>().slots) if (!s.IsEmpty) used++;
        ScreenGUI.Text(20, 20, "Bag: " + used + " slots used");
    }
}
```

## Example: Taking a slot and giving it to another bag
file: Scripts/Trade.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Trade : ElintriaScript
{
    public static void GiveSlot(Inventory from, int slot, Inventory to)
    {
        ItemStack s = from.TakeSlot(slot);
        if (s.IsEmpty) return;
        int left = to.Add(s.id, s.count);
        if (left > 0) from.Add(s.id, left);   // give back what didn't fit
    }
}
```

## Example: Making stacks
file: Scripts/StackDemo.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StackDemo : ElintriaScript
{
    public override void Start()
    {
        ItemStack s = new ItemStack("arrow", 24);
        Debug.Log(s + " empty? " + s.IsEmpty);
    }
}
```

# Weapon
summary: The shared base of guns: damage, fire rate, ammo, magazines, reloading, spread and aiming.
see: HitscanWeapon, ProjectileWeapon, MeleeWeapon, Health

`Weapon` is abstract. Add a [[HitscanWeapon]] or a [[ProjectileWeapon]], which share everything here. Guns have a magazine and a reserve. R reloads, and an empty gun reloads itself on the next trigger pull. With `aimFromCamera`, shots fly from the muzzle toward whatever is under the crosshair.

## Members
Fire: What each derived weapon does on a shot: a ray for HitscanWeapon, a launched Projectile for ProjectileWeapon. Override it to make a new kind of gun.
damage: Damage per hit.
fireRate: Shots a second.
automatic: Keep firing while the trigger is held.
magazineSize: Rounds per magazine.
reserveAmmo: Spare rounds.
infiniteAmmo: Never needs ammo.
reloadTime: Seconds a reload takes.
spread: How many degrees off the aim a shot may stray.
impactForce: How hard a hit pushes rigidbodies.
muzzle: Where shots come from (empty: this object).
aimFromCamera: Aim at what is under the crosshair.
cameraObject: The camera to aim with (empty: the scene's camera).
owner: Who is firing, kept out of the shots (empty: the root above this).
showHud: Draw the ammo counter and crosshair.
onFireTargets: Objects told on every shot.
onFireMessage: The message they get.
input: The input it reads; override it to fire from code.
ammo: Rounds in the magazine.
reloading: Whether it is reloading.
shotsFired: Shots fired so far.
hits: Shots that hit something.
TryFire: Fires if it can (rate, ammo, not reloading). Returns whether it fired.
Reload: Starts reloading.
Aim: Where a shot starts and which way it goes, before spread.
@origin: Filled in with where the shot starts.
@dir: Filled in with the shot's direction.
CameraForward: A camera object's forward direction (cameras look down -Z).
@cam: The camera object.

## Example: Ammo pickups refill the reserve
file: Scripts/AmmoBox.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AmmoBox : ElintriaScript
{
    public int rounds = 60;

    public override void OnTriggerEnter(Collider other)
    {
        Weapon w = other.gameObject.GetComponentInChildren<HitscanWeapon>();
        if (w == null) return;
        w.reserveAmmo += rounds;
        Destroy(gameObject);
    }
}
```

## Example: Switching weapons
file: Scripts/WeaponSwitch.cs
Several weapons on child objects; only the selected one is active.
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class WeaponSwitch : ElintriaScript
{
    public List<GameObject> weapons = new List<GameObject>();
    int current;

    public override void Start() { Equip(0); }

    public override void Update()
    {
        for (int i = 0; i < weapons.Count && i < 9; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) Equip(i);
    }

    void Equip(int i)
    {
        current = i;
        for (int k = 0; k < weapons.Count; k++) weapons[k].SetActive(k == i);
    }
}
```

## Example: Accuracy stats
file: Scripts/Accuracy.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Accuracy : ElintriaScript
{
    public override void Update()
    {
        Weapon w = GetComponent<HitscanWeapon>();
        if (w == null || w.shotsFired == 0) return;
        ScreenGUI.Text(20, 60, "Accuracy " + Mathf.RoundToInt(100f * w.hits / w.shotsFired) + "%  " + (w.reloading ? "RELOADING" : w.ammo + "/" + w.reserveAmmo));
    }
}
```

# HitscanWeapon
summary: A gun whose shots hit instantly along a ray: rifles, pistols, and shotguns with pellets.
see: Weapon, ProjectileWeapon

## Members
range: How far shots reach.
pellets: Rays per shot (8 or so for a shotgun, with more spread).
showTracers: Draw a tracer line for each shot.
impactTemplate: An object copied where shots land (sparks, a decal), then removed.
impactLifetime: How long impact copies stay.
lastHitPoint: Where the last shot landed.
lastHitObject: What the last shot hit.

## Example: A shotgun
file: Scripts/ShotgunSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ShotgunSetup : ElintriaScript
{
    public override void Start()
    {
        HitscanWeapon g = GetComponent<HitscanWeapon>();
        g.pellets = 8;
        g.spread = 6f;
        g.damage = 9f;          // per pellet
        g.fireRate = 1.2f;
        g.magazineSize = 6;
        g.range = 30f;
        g.automatic = false;
    }
}
```

## Example: A sniper rifle with impact sparks
file: Scripts/SniperSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SniperSetup : ElintriaScript
{
    public GameObject sparks;   // an inactive effect object

    public override void Start()
    {
        HitscanWeapon g = GetComponent<HitscanWeapon>();
        g.damage = 90f;
        g.range = 300f;
        g.spread = 0f;
        g.fireRate = 0.8f;
        g.impactTemplate = sparks;
        g.impactLifetime = 1.5f;
    }
}
```

## Example: Reacting to the last hit
file: Scripts/HitMarker.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HitMarker : ElintriaScript
{
    int lastHits;
    float showUntil;

    public override void Update()
    {
        HitscanWeapon g = GetComponent<HitscanWeapon>();
        if (g.hits != lastHits) { lastHits = g.hits; showUntil = Time.time + 0.15f; Debug.Log("Hit " + g.lastHitObject.name + " at " + g.lastHitPoint); }
        if (Time.time < showUntil) ScreenGUI.Label(new Rect(0, 0, ScreenGUI.screenWidth, ScreenGUI.screenHeight), "X", null, Color.red, TextAnchor.MiddleCenter);
    }
}
```

# ProjectileWeapon
summary: A weapon that launches Projectiles: rockets, arrows, grenades and fireballs.
see: Projectile, Weapon

## Members
projectileTemplate: An inactive scene object with a Projectile on it, copied for each shot. Empty: a small ball is made for each shot.
projectileSpeed: Launch speed.
projectileGravity: How much the projectile falls (0 for rockets, more for grenades).
explosionRadius: Damage radius on impact (0: only what it hits).
lastProjectile: The last projectile fired.

## Example: A rocket launcher
file: Scripts/RocketLauncherSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RocketLauncherSetup : ElintriaScript
{
    public override void Start()
    {
        ProjectileWeapon w = GetComponent<ProjectileWeapon>();
        w.projectileSpeed = 25f;
        w.projectileGravity = 0f;
        w.explosionRadius = 4f;
        w.damage = 100f;
        w.magazineSize = 1;
        w.reloadTime = 2f;
    }
}
```

## Example: A grenade launcher
file: Scripts/GrenadeLauncherSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GrenadeLauncherSetup : ElintriaScript
{
    public override void Start()
    {
        ProjectileWeapon w = GetComponent<ProjectileWeapon>();
        w.projectileSpeed = 15f;
        w.projectileGravity = 9.8f;   // arcs
        w.explosionRadius = 5f;
        w.fireRate = 1f;
    }
}
```

## Example: Following the last projectile with the camera
file: Scripts/ArrowCam.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ArrowCam : ElintriaScript
{
    public override void LateUpdate()
    {
        GameObject p = GetComponent<ProjectileWeapon>().lastProjectile;
        if (p != null && p.IsAlive && Input.GetKey(KeyCode.V))
            ScriptCamera.Set(p.transform.position - p.transform.forward * 2f + Vector3.up * 0.5f, p.transform.forward, Vector3.up, 60f, 0.1f, 300f);
        else ScriptCamera.Clear();
    }
}
```

# Projectile
summary: A flying shot: moves, falls, hits what it sweeps into, and can explode.
see: ProjectileWeapon, Health, StdDamage

## Members
speed: Speed in metres a second.
gravity: How fast it falls.
lifetime: Seconds before it removes itself.
damage: Damage to what it hits.
radius: Its size when sweeping for hits.
impactForce: How hard it pushes what it hits.
explosionRadius: Damages everything this close when it lands (0: only what it hits).
impactTemplate: An object copied where it lands (an explosion), then removed.
velocity: Its velocity now.
owner: Who fired it (it never hits its owner).
weapon: The weapon that fired it.
exploded: Whether it has exploded.
Explode: Explodes now at `at`, hurting what is in range.
@hitObject: What it hit directly, or null.
@at: Where it explodes.

## Example: A homing missile
file: Scripts/Homing2.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Homing2 : ElintriaScript
{
    public float turnRate = 90f;

    public override void Update()
    {
        Projectile p = GetComponent<Projectile>();
        GameObject target = GameObject.FindWithTag("Enemy");
        if (p == null || target == null) return;
        Vector3 want = (target.transform.position - transform.position).normalized * p.velocity.magnitude;
        p.velocity = Vector3.RotateTowards(p.velocity, want, turnRate * Mathf.Deg2Rad * Time.deltaTime, 0f);
    }
}
```

## Example: A sticky bomb
file: Scripts/StickyBomb.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StickyBomb : ElintriaScript
{
    float stuckAt = -1f;

    public override void Update()
    {
        Projectile p = GetComponent<Projectile>();
        if (stuckAt < 0f && Physics.Raycast(transform.position, p.velocity, out RaycastHit hit, p.velocity.magnitude * Time.deltaTime + 0.1f))
        {
            p.velocity = Vector3.zero;
            p.gravity = 0f;
            transform.position = hit.point;
            stuckAt = Time.time;
        }
        if (stuckAt >= 0f && Time.time - stuckAt > 2f && !p.exploded) p.Explode(null, transform.position, Vector3.up);
    }
}
```

## Example: Making a projectile template
file: Scripts/FireballTemplate.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FireballTemplate : ElintriaScript
{
    public override void Start()
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "Fireball Template";
        ball.transform.localScale = Vector3.one * 0.4f;
        Projectile p = ball.AddComponent<Projectile>();
        p.speed = 18f;
        p.damage = 35f;
        p.explosionRadius = 2f;
        p.lifetime = 4f;
        ball.SetActive(false);
        GetComponent<ProjectileWeapon>().projectileTemplate = ball;
    }
}
```

# MeleeWeapon
summary: Swings that hit everything in an arc in front, after a short wind-up.
see: Weapon, Health

## Members
damage: Damage per hit.
range: How far the swing reaches.
arc: How wide the swing is, in degrees.
cooldown: Seconds between swings.
windup: Seconds from the press to the hit.
knockback: How hard hits push.
owner: Who swings (never hit).
swingClip: A clip the owner's model plays as it swings.
onHitTargets: Objects told for each hit.
onHitMessage: The message they get.
input: The input it reads.
swings: Swings so far.
hits: Hits so far.
swinging: Whether a swing is under way.
Swing: Starts a swing if the cooldown allows. Returns whether it started.
Strike: Hits everything in the arc right now (without the wind-up). Returns how many were hit.

## Example: A heavy hammer
file: Scripts/HammerSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HammerSetup : ElintriaScript
{
    public override void Start()
    {
        MeleeWeapon m = GetComponent<MeleeWeapon>();
        m.damage = 60f;
        m.range = 2.5f;
        m.arc = 120f;
        m.windup = 0.45f;
        m.cooldown = 1.2f;
        m.knockback = 12f;
        m.swingClip = "attack_heavy";
    }
}
```

## Example: An enemy that swings when close
file: Scripts/BruteAttack.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BruteAttack : ElintriaScript
{
    public GameObject player;

    public override void Update()
    {
        MeleeWeapon m = GetComponent<MeleeWeapon>();
        if (player != null && Vector3.Distance(player.transform.position, transform.position) < m.range)
            m.Swing();
    }
}
```

## Example: A spin attack
file: Scripts/SpinAttack.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SpinAttack : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Q)) return;
        MeleeWeapon m = GetComponent<MeleeWeapon>();
        float oldArc = m.arc;
        m.arc = 360f;
        Debug.Log("Spin hit " + m.Strike() + " enemies");
        m.arc = oldArc;
    }
}
```

# Spawner
summary: Keeps copies of a template coming: up to a number alive, at an interval, with an optional total limit.
see: WaveSpawner, Health

Copies a template object (usually kept inactive out of sight) at its spawn points, or at its own position, within `radius`. It keeps up to `maxAlive` copies alive, one every `interval` seconds, `totalLimit` in all (0: no end). A copy counts as gone once destroyed or dead (by its [[Health]]). Each copy is sent `"OnSpawned"` with the spawner.

## Members
template: The object to copy.
spawnPoints: Positions to spawn at (empty: the spawner's own).
radius: How far from a spawn point a copy may appear.
interval: Seconds between spawns.
maxAlive: The most copies alive at once.
totalLimit: How many in all (0: no end).
spawnOnStart: How many appear at once when play starts.
spawnOnAwake: Start spawning at once.
randomYaw: Turn each copy to face a random way.
onSpawnTargets: Objects told for each spawn.
onSpawnMessage: The message they get (with the copy).
spawning: Whether it is spawning.
spawned: How many it has spawned in all.
alive: The copies alive now.
aliveCount: How many are alive.
StartSpawning: Starts spawning.
StopSpawning: Stops spawning (copies already out stay).
SpawnOne: Spawns one copy now (within the limits). Returns it, or null.
KillAll: Removes every live copy.

## Example: Ambient wildlife
file: Scripts/DeerSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DeerSetup : ElintriaScript
{
    public GameObject deerTemplate;

    public override void Start()
    {
        Spawner s = GetComponent<Spawner>();
        s.template = deerTemplate;
        s.maxAlive = 5;
        s.interval = 20f;
        s.radius = 25f;
        s.randomYaw = true;
        s.spawnOnStart = 3;
        s.StartSpawning();
    }
}
```

## Example: Spawns that react to the player
file: Scripts/AmbushSpawner.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AmbushSpawner : ElintriaScript
{
    public override void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        Spawner s = GetComponent<Spawner>();
        for (int i = 0; i < 4; i++) s.SpawnOne();
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) GetComponent<Spawner>().StopSpawning();
    }
}
```

## Example: Setting up each copy
file: Scripts/SpawnedEnemy.cs
`"OnSpawned"` arrives on the copy's scripts with the spawner.
```csharp
using Elintria;
using Elintria.Standard;

public class SpawnedEnemy : ElintriaScript
{
    public void OnSpawned(Spawner from)
    {
        Health h = GetComponent<Health>();
        if (h != null) h.maxHealth = h.health = 50f + from.spawned * 5f;   // later ones are tougher
    }
}
```

# WaveSpawner
summary: Enemy waves: each wave larger than the last, with pauses, events, and an on-screen wave counter.
see: Spawner, Health

Wave *n* spawns `firstWaveCount + (n-1) * countIncrease` enemies, using the `templates` in turn, `spawnInterval` apart. The next wave starts `timeBetweenWaves` after every enemy of the last one is dead. Events fire at the start and end of each wave and when every wave is done.

## Members
templates: The kinds of enemy, used in turn.
spawnPoints: Where they appear (empty: the spawner's position).
radius: How far from a spawn point an enemy may appear.
waveCount: How many waves (0: endless).
firstWaveCount: How many enemies in wave 1.
countIncrease: How many more each wave.
spawnInterval: Seconds between enemies within a wave.
timeBetweenWaves: The pause after a wave is cleared.
autoStart: Begin when play starts.
showHud: Show the wave number and enemies left.
onWaveStartTargets: Objects told when a wave starts (with its number).
onWaveStartMessage: The message they get.
onWaveClearedTargets: Objects told when a wave is cleared.
onWaveClearedMessage: The message they get.
onAllClearedTargets: Objects told when every wave is done.
onAllClearedMessage: The message they get.
wave: The wave running (1-based; 0 before the first).
toSpawn: Enemies of this wave still to appear.
between: Whether it is in the pause between waves.
finished: Whether every wave is done.
running: Whether it is running.
alive: The enemies alive now.
aliveCount: How many enemies are alive.
WaveSize: How many enemies wave `n` has.
countdown: Seconds until the next wave, during the pause.
Begin: Starts wave 1.
NextWaveNow: Skips the pause: the next wave starts now.
KillAll: Clears the wave at once (for testing).

## Example: An arena with a reward at the end
file: Scripts/ArenaSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ArenaSetup : ElintriaScript
{
    public GameObject rewardChest;

    public override void Start()
    {
        WaveSpawner w = GetComponent<WaveSpawner>();
        w.waveCount = 5;
        w.firstWaveCount = 3;
        w.countIncrease = 2;
        w.timeBetweenWaves = 8f;
        w.onAllClearedTargets.Add(rewardChest);
        w.onAllClearedMessage = "SetActive:true";
        w.Begin();
    }
}
```

## Example: Your own wave UI
file: Scripts/WaveBanner.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WaveBanner : ElintriaScript
{
    public override void Update()
    {
        WaveSpawner w = GetComponent<WaveSpawner>();
        string text = w.finished ? "VICTORY" : w.between ? "Next wave in " + Mathf.CeilToInt(w.countdown) : "Wave " + w.wave + "  -  " + (w.aliveCount + w.toSpawn) + " left";
        ScreenGUI.Label(new Rect(0, 20, ScreenGUI.screenWidth, 40), text, GuiFont.Get("", 26, true), Color.white, TextAnchor.MiddleCenter);
        if (w.between && Input.GetKeyDown(KeyCode.N)) w.NextWaveNow();
    }
}
```

## Example: Planning wave sizes
file: Scripts/WavePlan.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WavePlan : ElintriaScript
{
    public override void Start()
    {
        WaveSpawner w = GetComponent<WaveSpawner>();
        int total = 0;
        for (int n = 1; n <= w.waveCount; n++) total += w.WaveSize(n);
        Debug.Log(w.waveCount + " waves, " + total + " enemies in all");
    }
}
```

# MovingPlatform
summary: A platform that travels between waypoints, waits at each stop, and carries what stands on it.
see: Elevator, Lever, PressurePlate

## Members
waypoints: Stops, in order (their positions). Empty: it goes to `endOffset` and back.
endOffset: Where it goes, relative to its start, when there are no waypoints.
speed: Speed in metres a second.
waitTime: Seconds it waits at each stop.
pingPong: Go back the way it came at the end, instead of looping round.
waitForActivate: Stay still until something sends it "Activate".
carryRiders: Carry whatever stands on it.
target: The waypoint it is heading for.
moving: Whether it is moving.
Activate: Starts it moving.
Deactivate: Stops it.
ToggleActive: Starts it if stopped, stops it if moving.

## Example: A platform started by a pressure plate
file: Scripts/PlatePlatformSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PlatePlatformSetup : ElintriaScript
{
    public GameObject platform;

    public override void Start()
    {
        platform.GetComponent<MovingPlatform>().waitForActivate = true;
        PressurePlate plate = GetComponent<PressurePlate>();
        plate.onPressedTargets.Add(platform);
        plate.onPressedMessage = "Activate";
    }
}
```

## Example: A simple up-and-down platform
file: Scripts/BobPlatform.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BobPlatform : ElintriaScript
{
    public override void Start()
    {
        MovingPlatform m = GetComponent<MovingPlatform>();
        m.endOffset = new Vector3(0f, 4f, 0f);
        m.speed = 2f;
        m.waitTime = 1f;
        m.pingPong = true;
    }
}
```

## Example: A ferry that stops while the player is off
file: Scripts/Ferry.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Ferry : ElintriaScript
{
    public GameObject player;

    public override void Update()
    {
        MovingPlatform m = GetComponent<MovingPlatform>();
        bool aboard = player != null && Vector3.Distance(player.transform.position, transform.position) < 3f;
        if (aboard && !m.moving) m.Activate();
        else if (!aboard && m.moving) m.Deactivate();
    }
}
```

# Elevator
summary: A lift with floors: go to a floor, next or previous, carrying riders, with an arrival event.
see: Lever, MovingPlatform, StdEvent

## Members
floorHeights: Floor heights above where it starts. Floor 0 is the starting height and is added for you.
speed: Speed in metres a second.
startFloor: The floor it starts on.
returnAfter: Goes back to floor 0 by itself after waiting this long (0: stays).
carryRiders: Carry whatever stands in it.
onArriveTargets: Objects told when it arrives (with the floor number).
onArriveMessage: The message they get.
floor: The floor it is at (or last left).
targetFloor: The floor it is heading for.
moving: Whether it is moving.
GoToFloor: Sends it to floor `n`.
NextFloor: Up one floor.
PreviousFloor: Down one floor.
Cycle: To the top floor if on the ground, else back to the ground (for a single button).

## Example: Call buttons by event
file: Scripts/ElevatorButtons.cs
Each lever's event sends "GoToFloor:N"; no code needed in practice.
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class ElevatorButtons : ElintriaScript
{
    public GameObject elevator;
    public List<GameObject> floorLevers = new List<GameObject>();

    public override void Start()
    {
        for (int i = 0; i < floorLevers.Count; i++)
        {
            Lever l = floorLevers[i].GetComponent<Lever>();
            l.onOnTargets.Add(elevator);
            l.onOnMessage = "GoToFloor:" + i;
        }
    }
}
```

## Example: Floors set from code
file: Scripts/TowerLift.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TowerLift : ElintriaScript
{
    public override void Start()
    {
        Elevator e = GetComponent<Elevator>();
        e.floorHeights.Clear();
        e.floorHeights.Add(0f);
        for (int i = 1; i <= 5; i++) e.floorHeights.Add(i * 4f);   // five floors, 4 m apart
        e.speed = 3f;
        e.returnAfter = 20f;
    }
}
```

## Example: Floor display and arrival chime
file: Scripts/FloorDisplay.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FloorDisplay : ElintriaScript
{
    public GameObject elevator;

    public void Arrived(int floor) { gameObject.audioSource.PlayOneShot("Audio/ding.wav"); }   // set as the onArrive message

    public override void Update()
    {
        Elevator e = elevator.GetComponent<Elevator>();
        ScreenGUI.Text(20, 20, "Floor " + e.floor + (e.moving ? " -> " + e.targetFloor : ""));
    }
}
```

# PressurePlate
summary: A plate pressed while something stands on it: it sinks and fires pressed and released events.
see: TriggerZone, Door, MovingPlatform

## Members
requiredTag: Only objects with this tag press it (empty: any character or rigidbody).
requiredCount: How many things must stand on it.
latch: Stay pressed once pressed.
sinkDepth: How far it sinks when pressed.
onPressedTargets: Objects told when it is pressed.
onPressedMessage: The message they get.
onReleasedTargets: Objects told when it is released.
onReleasedMessage: The message they get.
pressed: Whether it is pressed.
count: How many things are on it.

## Example: Hold the door open
file: Scripts/PlateDoorSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PlateDoorSetup : ElintriaScript
{
    public GameObject door;

    public override void Start()
    {
        PressurePlate p = GetComponent<PressurePlate>();
        p.onPressedTargets.Add(door);  p.onPressedMessage = "Open";
        p.onReleasedTargets.Add(door); p.onReleasedMessage = "Close";
    }
}
```

## Example: A crate puzzle
file: Scripts/CratePlate.cs
Only crates press it, and two are needed.
```csharp
using Elintria;
using Elintria.Standard;

public class CratePlate : ElintriaScript
{
    public override void Start()
    {
        PressurePlate p = GetComponent<PressurePlate>();
        p.requiredTag = "Crate";
        p.requiredCount = 2;
        p.latch = true;
    }

    public override void Update()
    {
        PressurePlate p = GetComponent<PressurePlate>();
        ScreenGUI.Text(20, 20, "Crates on the plate: " + p.count + "/" + p.requiredCount + (p.pressed ? "  SOLVED" : ""));
    }
}
```

## Example: A trap plate
file: Scripts/TrapPlate.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TrapPlate : ElintriaScript
{
    public GameObject arrowShooter;   // a ProjectileWeapon

    public void Fire() { arrowShooter.GetComponent<ProjectileWeapon>().TryFire(); }   // set as the onPressed message, targeting this object
}
```

# TriggerZone
summary: A box that fires events when things enter or leave, and can send messages to whatever is inside.
see: PressurePlate, StdEvent

The box is the object's Box Collider, or `size` when it has none. `requiredTag` filters what counts. `enterMessage` is sent to the thing that came in, and `stayMessage` is sent to it every `stayInterval` seconds while it is inside, which suits hazards like `"ApplyDamage:5"`.

## Members
requiredTag: What counts (empty: anything with a collider).
size: The box, when the object has no Box Collider.
once: Fire the first time only.
onEnterTargets: Objects told when something comes in (with what came in).
onEnterMessage: The message they get.
onExitTargets: Objects told when something leaves.
onExitMessage: The message they get.
enterMessage: Sent to the thing that came in (e.g. "ApplyDamage:10").
stayMessage: Sent to each thing inside every `stayInterval` seconds.
stayInterval: Seconds between stay messages.
inside: How many things are inside now.
enterCount: How many times something has come in.
Box: The zone's box in world space.

## Example: A poison cloud
file: Scripts/PoisonCloud.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PoisonCloud : ElintriaScript
{
    public override void Start()
    {
        TriggerZone z = GetComponent<TriggerZone>();
        z.size = new Vector3(6f, 3f, 6f);
        z.requiredTag = "Player";
        z.stayMessage = "ApplyDamage:4";
        z.stayInterval = 0.5f;
    }
}
```

## Example: A checkpoint
file: Scripts/Checkpoint2.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Checkpoint2 : ElintriaScript
{
    public static Vector3 lastCheckpoint;

    public override void Start()
    {
        TriggerZone z = GetComponent<TriggerZone>();
        z.requiredTag = "Player";
        z.once = true;
        z.onEnterTargets.Add(gameObject);
        z.onEnterMessage = "Reached";
    }

    public void Reached(GameObject player)
    {
        lastCheckpoint = transform.position;
        player.GetComponent<Interactor>()?.Say("Checkpoint reached");
    }
}
```

## Example: Music zones
file: Scripts/MusicZone.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MusicZone : ElintriaScript
{
    public string track = "Audio/cave_theme.ogg";

    public override void Start()
    {
        TriggerZone z = GetComponent<TriggerZone>();
        z.requiredTag = "Player";
        z.onEnterTargets.Add(gameObject); z.onEnterMessage = "Enter";
        z.onExitTargets.Add(gameObject);  z.onExitMessage = "Leave";
    }

    public void Enter() { AudioSource s = gameObject.audioSource; s.clip = track; s.loop = true; s.Play(); }
    public void Leave() { gameObject.audioSource.Stop(); }
}
```

# LookAtIK
summary: Turns a character's head (with some neck and spine) toward a target, within a limit.
see: TwoBoneIK, FootIK, SkinnedModel

Put it on the object with the rigged model (or point `model` at it). Bone names are found automatically on Mixamo-style and most humanoid rigs. Set them by hand if your rig names them differently.

## Members
model: The rigged model (empty: this object).
target: The object to look at.
targetPoint: A point to look at when there is no target object.
useTargetPoint: Look at `targetPoint` instead of `target`.
headBone: The head bone's name (found automatically when empty).
neckBone: The neck bone's name.
spineBone: The upper spine bone's name.
maxAngle: The most the head turns, in degrees.
weight: How much it applies, 0..1.
spineShare: The share of the turn the spine takes.
neckShare: The share of the turn the neck takes.
smoothing: How quickly it follows (higher: snappier).
appliedAngle: How far it is turned now.
Release: Hands the bones back to the animation at once.

## Example: NPCs watch the player when near
file: Scripts/WatchPlayer.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WatchPlayer : ElintriaScript
{
    public GameObject player;
    public float range = 6f;

    public override void Update()
    {
        LookAtIK ik = GetComponent<LookAtIK>();
        bool near = Vector3.Distance(player.transform.position, transform.position) < range;
        ik.target = player;
        ik.weight = Mathf.MoveTowards(ik.weight, near ? 1f : 0f, Time.deltaTime * 2f);
    }
}
```

## Example: Looking at a point of interest
file: Scripts/LookAtPoint.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LookAtPoint : ElintriaScript
{
    public override void Start()
    {
        LookAtIK ik = GetComponent<LookAtIK>();
        ik.useTargetPoint = true;
        ik.targetPoint = new Vector3(10f, 2f, 5f);
        ik.maxAngle = 60f;
        ik.spineShare = 0.3f;
        ik.neckShare = 0.3f;
    }
}
```

## Example: Letting go during a cutscene
file: Scripts/IkOff.cs
```csharp
using Elintria;
using Elintria.Standard;

public class IkOff : ElintriaScript
{
    public void CutsceneStarted()
    {
        LookAtIK ik = GetComponent<LookAtIK>();
        ik.Release();
        ik.enabled = false;
    }
}
```

# TwoBoneIK
summary: Makes an arm or leg reach a target, bending the elbow or knee toward a hint.
see: LookAtIK, FootIK, TwoBoneSolver

## Members
model: The rigged model (empty: this object).
upperBone: The upper bone (shoulder or hip).
middleBone: The middle bone (elbow or knee).
endBone: The end bone (hand or foot).
target: Where the hand or foot should go.
hint: Where the elbow or knee should point.
matchRotation: Also turn the hand or foot to match the target's rotation.
weight: How much it applies, 0..1.
reachError: How far the end bone still is from the target (when out of reach).

## Example: A hand on a door handle
file: Scripts/HandOnHandle.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HandOnHandle : ElintriaScript
{
    public GameObject handle;

    public override void Start()
    {
        TwoBoneIK ik = GetComponent<TwoBoneIK>();
        ik.upperBone = "mixamorig:RightArm";
        ik.middleBone = "mixamorig:RightForeArm";
        ik.endBone = "mixamorig:RightHand";
        ik.target = handle;
        ik.matchRotation = true;
    }
}
```

## Example: Reaching only when close
file: Scripts/ReachWhenNear.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ReachWhenNear : ElintriaScript
{
    public override void Update()
    {
        TwoBoneIK ik = GetComponent<TwoBoneIK>();
        if (ik.target == null) return;
        float d = Vector3.Distance(ik.target.transform.position, transform.position + Vector3.up * 1.3f);
        ik.weight = Mathf.MoveTowards(ik.weight, d < 0.8f ? 1f : 0f, Time.deltaTime * 3f);
    }
}
```

## Example: Checking whether it can reach
file: Scripts/ReachCheck.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ReachCheck : ElintriaScript
{
    public override void Update()
    {
        TwoBoneIK ik = GetComponent<TwoBoneIK>();
        if (ik.weight > 0.9f && ik.reachError > 0.1f) Debug.Log("Target is " + ik.reachError.ToString("0.00") + " m out of reach");
    }
}
```

# FootIK
summary: Plants a character's feet on uneven ground, tilting them to the slope and lowering the hips.
see: TwoBoneIK, Ragdoll

Each foot is placed where a ray finds the ground under it and tilted to match the slope. The hips drop so the lower foot can still reach.

## Members
leftGround: Where the ground under the left foot was found.
rightGround: Where the ground under the right foot was found.
leftHit: Whether ground was found under the left foot.
rightHit: Whether ground was found under the right foot.
model: The rigged model (empty: this object).
hipsBone: The hips bone.
leftUpperLeg: The left thigh bone.
leftLowerLeg: The left shin bone.
leftFoot: The left foot bone.
rightUpperLeg: The right thigh bone.
rightLowerLeg: The right shin bone.
rightFoot: The right foot bone.
footHeight: How high the ankle is above the sole.
rayHeight: How far above the foot to start looking for ground.
maxStepDown: How far below the foot the ground may be and still be reached.
alignToSlope: Tilt the feet to the ground's slope.
weight: How much it applies, 0..1.
smoothing: How quickly the feet follow the ground.
pelvisOffset: How far the hips are lowered now.
leftGround: Where the left foot's ground is.
rightGround: Where the right foot's ground is.
leftHit: Whether ground was found under the left foot.
rightHit: Whether ground was found under the right foot.

## Example: Turning foot IK off while running
file: Scripts/FootIkBySpeed.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FootIkBySpeed : ElintriaScript
{
    public override void Update()
    {
        FootIK ik = GetComponent<FootIK>();
        ThirdPersonController tpc = GetComponent<ThirdPersonController>();
        float speed = tpc != null ? new Vector3(tpc.velocity.x, 0f, tpc.velocity.z).magnitude : 0f;
        ik.weight = Mathf.MoveTowards(ik.weight, speed < 1f && tpc.isGrounded ? 1f : 0f, Time.deltaTime * 4f);
    }
}
```

## Example: Debug lines to the ground
file: Scripts/FootDebug.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FootDebug : ElintriaScript
{
    public override void Update()
    {
        FootIK ik = GetComponent<FootIK>();
        if (ik.leftHit) Debug.DrawRay(ik.leftGround, Vector3.up * 0.3f, Color.green);
        if (ik.rightHit) Debug.DrawRay(ik.rightGround, Vector3.up * 0.3f, Color.cyan);
        ScreenGUI.Text(20, 20, "Hips lowered " + ik.pelvisOffset.ToString("0.00") + " m");
    }
}
```

## Example: Tuning for a small creature
file: Scripts/SmallFeet.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SmallFeet : ElintriaScript
{
    public override void Start()
    {
        FootIK ik = GetComponent<FootIK>();
        ik.footHeight = 0.04f;
        ik.rayHeight = 0.3f;
        ik.maxStepDown = 0.25f;
        ik.smoothing = 20f;
    }
}
```

# Ragdoll
summary: Makes a character go limp and tumble, push it with impulses, and get it back up.
see: Health, FootIK, SkinnedModel

When activated, every main bone becomes a point mass held to its parent. It falls and tumbles against the world, starting from the animated pose and keeping its motion. With `ragdollOnDeath`, it activates when the object's [[Health]] runs out. `GetUp` blends back to the animation where the body lies.

## Members
model: The rigged model (empty: this object).
ragdollOnDeath: Go limp when the object dies.
gravity: How strongly it falls.
damping: The share of speed kept each step (air drag and joint friction), 0..1.
partRadius: Each body part's size against the world.
friction: How much parts grip the ground.
iterations: Constraint passes per frame (more: stiffer joints).
blendTime: Seconds to go limp and to get back up.
getUpTime: Seconds the get-up takes.
disableWhileRagdoll: Objects switched off while limp (your controller, its collider).
active: Whether it is limp.
blend: How limp it is, 0..1.
settled: Whether the body has come to rest.
partCount: How many body parts it simulates.
OnDeath: Called on death; goes limp when ragdollOnDeath is set.
Activate: Goes limp now, from the pose it is in.
AddImpulse: Pushes the part nearest `point` by `impulse`, for hits and explosions.
@impulse: The push.
@point: Where the hit landed.
GetUp: Blends back to the animation, standing where the hips lie.
hipsPosition: Where the hips are now.

## Example: Knocked down by a big hit
file: Scripts/Knockdown.cs
```csharp
using System.Collections;
using Elintria;
using Elintria.Standard;

public class Knockdown : ElintriaScript
{
    public void HeavyHit(Vector3 dir) { StartCoroutine(Down(dir)); }

    IEnumerator Down(Vector3 dir)
    {
        Ragdoll r = GetComponent<Ragdoll>();
        r.Activate();
        r.AddImpulse(dir.normalized * 8f + Vector3.up * 3f, transform.position + Vector3.up);
        yield return new WaitUntil(() => r.settled);
        yield return new WaitForSeconds(1f);
        r.GetUp();
    }
}
```

## Example: Ragdoll on death, pushed by the killing blow
file: Scripts/DeathPush.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DeathPush : ElintriaScript
{
    public override void Start()
    {
        GetComponent<Ragdoll>().ragdollOnDeath = true;
    }

    public void OnDeath()
    {
        Health h = GetComponent<Health>();
        if (h.lastAttacker != null)
            GetComponent<Ragdoll>().AddImpulse((transform.position - h.lastAttacker.transform.position).normalized * 6f, transform.position + Vector3.up);
    }
}
```

## Example: Following the body with the camera
file: Scripts/RagdollCam.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RagdollCam : ElintriaScript
{
    public override void LateUpdate()
    {
        Ragdoll r = GetComponent<Ragdoll>();
        if (!r.active) return;
        Vector3 hips = r.hipsPosition;
        ScriptCamera.Set(hips + new Vector3(0f, 3f, -4f), (hips - (hips + new Vector3(0f, 3f, -4f))).normalized, Vector3.up, 60f, 0.1f, 200f);
    }
}
```

# TwoBoneSolver
summary: The two-bone IK solve on its own, for your own IK code.
see: TwoBoneIK

## Members
Solve: Turns the upper and middle bones so the end reaches `target` as nearly as the bone lengths allow, bending toward `hint`. Positions and rotations are in world space.
@a: The upper joint's position.
@b: The middle joint's position.
@c: The end joint's position.
@ra: The upper bone's rotation.
@rb: The middle bone's rotation.
@target: Where the end should go.
@hint: Where the middle joint should point.
@newA: Filled in with the upper bone's new rotation.
@newB: Filled in with the middle bone's new rotation.

## Example: Solving a robot arm made of objects
file: Scripts/RobotArm.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RobotArm : ElintriaScript
{
    public GameObject shoulder, elbow, hand, goal, pole;

    public override void LateUpdate()
    {
        TwoBoneSolver.Solve(shoulder.transform.position, elbow.transform.position, hand.transform.position,
                            shoulder.transform.rotation, elbow.transform.rotation,
                            goal.transform.position, pole.transform.position,
                            out Quaternion newShoulder, out Quaternion newElbow);
        shoulder.transform.rotation = newShoulder;
        elbow.transform.rotation = newElbow;
    }
}
```

## Example: A leg on a skinned model
file: Scripts/LegSolve.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LegSolve : ElintriaScript
{
    public Vector3 footTarget;

    public override void LateUpdate()
    {
        SkinnedModel m = SkinnedModel.Get(gameObject);
        if (!m.AnimatedBonePose("LeftUpLeg", out Vector3 a, out Quaternion ra)) return;
        m.AnimatedBonePose("LeftLeg", out Vector3 b, out Quaternion rb);
        m.AnimatedBonePose("LeftFoot", out Vector3 c, out Quaternion _);
        TwoBoneSolver.Solve(a, b, c, ra, rb, footTarget, b + transform.forward, out Quaternion na, out Quaternion nb);
        m.SetBoneRotation("LeftUpLeg", na);
        m.SetBoneRotation("LeftLeg", nb);
    }
}
```

## Example: Visualising the solve
file: Scripts/SolveDebug.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SolveDebug : ElintriaScript
{
    public override void Update()
    {
        Vector3 a = Vector3.zero, b = new Vector3(0f, -1f, 0f), c = new Vector3(0f, -2f, 0f);
        TwoBoneSolver.Solve(a, b, c, Quaternion.identity, Quaternion.identity, new Vector3(1f, -1.2f, 0f), new Vector3(0f, -1f, 1f), out Quaternion na, out Quaternion nb);
        Vector3 nb_pos = a + na * (b - a);
        Debug.DrawLine(a, nb_pos, Color.yellow);
        Debug.DrawLine(nb_pos, nb_pos + nb * (c - b), Color.cyan);
    }
}
```

# HumanoidBones
summary: Finds humanoid bones (hips, legs, arms, head) by their usual names on most rigs.
see: SkinnedModel, FootIK

## Members
Parts: The part names it looks for ("Hips", "LeftUpperLeg", "Head", ...).
Map: Each part name it could find, mapped to the model's own bone name.
Find: The model's bone name for `part`, or null.
@m: The model.
@part: A part name such as "LeftFoot".

## Example: Printing a rig's mapping
file: Scripts/RigReport.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RigReport : ElintriaScript
{
    public override void Start()
    {
        var map = HumanoidBones.Map(SkinnedModel.Get(gameObject));
        foreach (string part in HumanoidBones.Parts)
            Debug.Log(part + " -> " + (map.TryGetValue(part, out string bone) ? bone : "(missing)"));
    }
}
```

## Example: A hat on any rig
file: Scripts/AnyRigHat.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AnyRigHat : ElintriaScript
{
    public GameObject character;

    public override void LateUpdate()
    {
        SkinnedModel m = SkinnedModel.Get(character);
        string head = HumanoidBones.Find(m, "Head");
        if (head != null && m.BonePose(head, out Vector3 p, out Quaternion r)) { transform.position = p + r * Vector3.up * 0.2f; transform.rotation = r; }
    }
}
```

## Example: Checking a rig is humanoid
file: Scripts/IsHumanoid.cs
```csharp
using Elintria;
using Elintria.Standard;

public static class IsHumanoid
{
    public static bool Check(SkinnedModel m) =>
        HumanoidBones.Find(m, "Hips") != null && HumanoidBones.Find(m, "Head") != null && HumanoidBones.Find(m, "LeftFoot") != null;
}
```

# StdRot
summary: Small rotation helpers: the shortest turn between two directions, and clamping a turn.
see: Quaternion

## Members
FromTo: The shortest rotation that turns direction `from` into `to`.
@from: The starting direction.
@to: The ending direction.
Clamp: The same turn as `q`, cut down to at most `maxDegrees`.
@q: The rotation to limit.
@maxDegrees: The largest turn allowed.

## Example: Aligning an object to a surface
file: Scripts/AlignToGround.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AlignToGround : ElintriaScript
{
    public override void Update()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 3f))
            transform.rotation = StdRot.FromTo(transform.up, hit.normal) * transform.rotation;
    }
}
```

## Example: A turret with a turn limit
file: Scripts/LimitedTurret.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LimitedTurret : ElintriaScript
{
    public GameObject target;
    Quaternion rest;

    public override void Start() { rest = transform.rotation; }

    public override void Update()
    {
        Quaternion want = Quaternion.LookRotation(target.transform.position - transform.position);
        Quaternion turn = want * Quaternion.Inverse(rest);
        transform.rotation = StdRot.Clamp(turn, 45f) * rest;   // never more than 45 degrees off rest
    }
}
```

## Example: Tilting a boat to the wave
file: Scripts/TiltBoat.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TiltBoat : ElintriaScript
{
    public override void Update()
    {
        if (Water.SurfaceAt(transform.position, out float h, out Vector3 n))
            transform.rotation = StdRot.Clamp(StdRot.FromTo(Vector3.up, n), 20f) * Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }
}
```

# Cloth
summary: A simulated sheet of cloth: curtains, flags and tablecloths that hang, blow in the wind and can tear.
see: Destructible, Water

Cloth simulates a sheet on its object. It can be a curtain or flag hanging from its pinned edge, or a tablecloth lying flat. It blows in `wind`, falls onto the ground, and is pushed aside by characters and by the spheres listed in `colliders`. Pins can also be objects, and the corner nearest each one follows it. It draws with the object's own material. Over-stretched cloth can tear.

## Members
width: The sheet's width in metres.
height: The sheet's height (or length) in metres.
resolution: Squares along each side (more: smoother, slower).
horizontal: Lie flat (a tablecloth) instead of hanging (a curtain).
pinMode: Which edge is fixed: "Top", "TopCorners", "Left", "Corners" or "None".
pins: Objects the nearest cloth point is tied to.
gravity: How strongly it falls.
damping: How quickly motion dies away.
iterations: Constraint passes per frame (more: stiffer).
wind: The wind's direction and strength.
turbulence: How gusty the wind is.
colliders: Objects it is pushed out of, as spheres (each object's largest scale is the diameter).
collideWithCharacters: Pushed aside by character controllers.
collideWithGround: Rests on the ground.
tearFactor: Tears where a link stretches past this many times its length (0: never).
thickness: How far it keeps from what it touches.
friction: How much cloth touching something sticks to it, 0..1.
particleCount: How many points it simulates.
brokenLinks: How many links have torn.
lowestY: The lowest point of the cloth.
ReleasePins: Lets go of every pin, so it falls.
Particle: Where point (x, y) of the grid is now.
gridSize: Points along each side.

## Example: A flag in gusty wind
file: Scripts/WindyFlag.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WindyFlag : ElintriaScript
{
    public override void Start()
    {
        Cloth c = GetComponent<Cloth>();
        c.pinMode = "Left";
        c.width = 2f;
        c.height = 1.2f;
        c.wind = new Vector3(6f, 0f, 1f);
        c.turbulence = 0.5f;
    }

    public override void Update()
    {
        GetComponent<Cloth>().wind = new Vector3(6f + Mathf.Sin(Time.time * 0.7f) * 3f, 0f, Mathf.Cos(Time.time * 0.4f));
    }
}
```

## Example: A curtain that tears when shot
file: Scripts/TearableCurtain.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TearableCurtain : ElintriaScript
{
    public override void Start()
    {
        Cloth c = GetComponent<Cloth>();
        c.pinMode = "Top";
        c.tearFactor = 2.5f;
        c.collideWithCharacters = true;
    }

    public void ApplyDamage(float amount)
    {
        Cloth c = GetComponent<Cloth>();
        if (amount > 30f) c.ReleasePins();   // a big hit tears it down
        Debug.Log("Torn links: " + c.brokenLinks);
    }
}
```

## Example: A tablecloth pulled off by an object
file: Scripts/TableclothTrick.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TableclothTrick : ElintriaScript
{
    public GameObject hand;   // move this object to drag a corner

    public override void Start()
    {
        Cloth c = GetComponent<Cloth>();
        c.horizontal = true;
        c.pinMode = "None";
        c.pins.Add(hand);
        c.friction = 0.6f;
    }

    public override void Update()
    {
        if (GetComponent<Cloth>().lowestY < -5f) Debug.Log("The tablecloth fell on the floor");
    }
}
```

# Destructible
summary: Breaks an object into physics pieces when it takes enough damage: crates, walls, statues.
see: Health, SliceMesh, Weapon

A Destructible breaks when it takes enough damage. Damage can come from its [[Health]], a weapon's `ApplyDamage`, or `Break()` called from a script. Its shape (a cube, sphere, cylinder or any .obj model) is cut by random planes into `pieces` chunks. Each chunk is a physics body in the object's material that flies apart and then fades.

## Members
health: Damage it takes to break (when it has no Health of its own).
pieces: How many pieces it breaks into.
explosionForce: How hard the pieces fly apart.
pieceLifetime: Seconds the pieces stay before they go (0: they stay).
minPieceSize: Pieces smaller than this get no collider of their own.
pieceMass: Each piece's mass.
onBreakTargets: Objects told when it breaks.
onBreakMessage: The message they get.
broken: Whether it has broken.
brokenPieces: The pieces it broke into.
ApplyDamage: Takes damage; breaks when it runs out.
OnDeath: Breaks when its Health runs out.
Break: Breaks now, the pieces flying outward.
BreakFrom: Breaks now, the pieces flying away from `hitDirection`.

## Example: A crate with loot inside
file: Scripts/LootCrate.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LootCrate : ElintriaScript
{
    public GameObject lootTemplate;

    public override void Start()
    {
        Destructible d = GetComponent<Destructible>();
        d.health = 30f;
        d.pieces = 8;
        d.pieceLifetime = 6f;
        d.onBreakTargets.Add(gameObject);
        d.onBreakMessage = "DropLoot";
    }

    public void DropLoot()
    {
        Instantiate(lootTemplate, transform.position + Vector3.up * 0.3f, Quaternion.identity).SetActive(true);
    }
}
```

## Example: A wall broken by a battering ram
file: Scripts/BreakableWall.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BreakableWall : ElintriaScript
{
    public override void OnCollisionEnter(Collision c)
    {
        if (c.relativeVelocity.magnitude > 6f && c.gameObject.CompareTag("Ram"))
            GetComponent<Destructible>().BreakFrom(c.relativeVelocity.normalized);
    }
}
```

## Example: Breaking a whole row of statues
file: Scripts/ChainBreak.cs
```csharp
using System.Collections;
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class ChainBreak : ElintriaScript
{
    public List<GameObject> statues = new List<GameObject>();

    public void Go() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        foreach (GameObject s in statues)
        {
            Destructible d = s.GetComponent<Destructible>();
            if (d != null && !d.broken) d.Break();
            yield return new WaitForSeconds(0.25f);
        }
    }
}
```

# SliceMesh
summary: A triangle mesh in plain lists, cut by planes into closed pieces.
see: Destructible, RuntimeMesh

The mesh code behind [[Destructible]]. Make one from a shape (`Cube`, `Sphere`, `Cylinder`) or an .obj file, then `Split` it by a plane into two closed halves. Show the results with [[RuntimeMesh]].

## Members
v: The vertex positions.
uv: The texture coordinates.
tri: The triangles, three indices each.
Centre: The middle of the vertices.
Box: The bounding box.
Split: Cuts along the plane through `p` facing `n`: `front` is the part in front, `back` the part behind, each capped over the cut.
@p: A point on the cutting plane.
@n: The plane's normal.
@front: Filled in with the part in front of the plane.
@back: Filled in with the part behind it.
Cube: A unit cube.
Sphere: A unit sphere with `seg` segments.
@seg: How many segments around.
Cylinder: A unit cylinder with `seg` segments.
FromObj: The triangles of an .obj file (read through Content, so it works in a built game).

## Example: Cutting a cube in half and showing both halves
file: Scripts/HalveCube.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HalveCube : ElintriaScript
{
    public override void Start()
    {
        SliceMesh.Cube().Split(Vector3.zero, new Vector3(1f, 1f, 0f).normalized, out SliceMesh a, out SliceMesh b);
        Show(a, new Vector3(-1f, 1f, 0f));
        Show(b, new Vector3(1f, 1f, 0f));
    }

    static void Show(SliceMesh m, Vector3 at)
    {
        GameObject g = new GameObject("Half");
        g.transform.position = at;
        var rm = new RuntimeMesh();
        rm.SetVertices(m.v.ToArray());
        rm.SetUVs(m.uv.ToArray());
        rm.SetTriangles(m.tri.ToArray());
        RuntimeMesh.Assign(g, rm);
    }
}
```

## Example: Slicing a model with a sword stroke
file: Scripts/SwordSlice.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SwordSlice : ElintriaScript
{
    public string modelPath = "Assets/Models/melon.obj";

    public void Slice(Vector3 swingDirection)
    {
        SliceMesh melon = SliceMesh.FromObj(modelPath);
        Vector3 normal = Vector3.Cross(swingDirection, Vector3.up).normalized;
        melon.Split(melon.Centre(), normal, out SliceMesh left, out SliceMesh right);
        Debug.Log("Pieces: " + left.tri.Count / 3 + " and " + right.tri.Count / 3 + " triangles");
    }
}
```

## Example: Measuring pieces
file: Scripts/PieceSizes.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PieceSizes : ElintriaScript
{
    public override void Start()
    {
        SliceMesh s = SliceMesh.Sphere(16);
        s.Split(new Vector3(0f, 0.3f, 0f), Vector3.up, out SliceMesh cap, out SliceMesh rest);
        Debug.Log("Cap size " + cap.Box().size + ", rest size " + rest.Box().size);
    }
}
```

# Buoyancy
summary: Makes an object float on Water: it bobs, rolls and pitches on the waves.
see: Water, Rigidbody

Points on the object (its corners, or the ones you list) are pushed up according to how deep they are under the waves, so it bobs, rolls and pitches. In the water it is slowed by drag. With a [[Rigidbody]] it floats by forces; without one it rides the surface directly.

## Members
floatPoints: Points that float, in the object's own space (empty: the eight corners and the middle of a unit cube, scaled).
buoyancy: The upward push per metre under water, as a multiple of gravity.
maxDepth: The depth past which a point pushes no harder.
waterDrag: Drag on movement in the water.
waterAngularDrag: Drag on spinning in the water.
draft: Without a Rigidbody, how far the object sits in the water.
tilt: Without a Rigidbody, how much it tilts with the waves (0..1).
pointsUnderwater: How many float points are under water.
inWater: Whether any part is in the water.

## Example: A heavy barge
file: Scripts/Barge.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Barge : ElintriaScript
{
    public override void Start()
    {
        Buoyancy b = GetComponent<Buoyancy>();
        b.floatPoints.Clear();
        for (int x = -1; x <= 1; x += 2)
            for (int z = -2; z <= 2; z += 2)
                b.floatPoints.Add(new Vector3(x * 0.45f, -0.4f, z * 0.2f));
        b.buoyancy = 3f;
        b.waterDrag = 1.5f;
    }
}
```

## Example: A buoy without a Rigidbody
file: Scripts/BuoySetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BuoySetup : ElintriaScript
{
    public override void Start()
    {
        Buoyancy b = GetComponent<Buoyancy>();
        b.draft = 0.4f;   // sits 0.4 m deep
        b.tilt = 0.8f;    // follows the waves' slope
    }
}
```

## Example: Splash when an object lands in water
file: Scripts/LandingSplash.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LandingSplash : ElintriaScript
{
    bool was;

    public override void Update()
    {
        bool now = GetComponent<Buoyancy>().inWater;
        if (now && !was) Audio.PlayClipAtPoint("Audio/splash.wav", transform.position);
        was = now;
    }
}
```

# GrassBender
summary: Makes painted grass bend away from this object as it moves through.
see: Foliage

Add it to the player, cars or animals. Each bender in the scene needs its own `slot`, from 0 to 7.

## Members
slot: Which of the 8 bender slots this object uses.
radius: How far around the object grass bends.
offset: Where the bending centre is, relative to the object.

## Example: Benders for a player and a car
file: Scripts/BenderSetup.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BenderSetup : ElintriaScript
{
    public GameObject player;
    public GameObject car;

    public override void Start()
    {
        GrassBender p = player.AddComponent<GrassBender>();
        p.slot = 0; p.radius = 0.7f;
        GrassBender c = car.AddComponent<GrassBender>();
        c.slot = 1; c.radius = 2f; c.offset = new Vector3(0f, 0f, -0.5f);
    }
}
```

## Example: A bigger radius while running
file: Scripts/RunBend.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RunBend : ElintriaScript
{
    public override void Update()
    {
        GetComponent<GrassBender>().radius = Input.GetKey(KeyCode.Shift) ? 1.2f : 0.7f;
    }
}
```

## Example: Giving each animal its own slot
file: Scripts/HerdBenders.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class HerdBenders : ElintriaScript
{
    public List<GameObject> animals = new List<GameObject>();

    public override void Start()
    {
        for (int i = 0; i < animals.Count && i < Foliage.MaxBenders - 1; i++)
            animals[i].AddComponent<GrassBender>().slot = i + 1;   // slot 0 is the player's
    }
}
```

# SplineFollower
summary: Moves an object along a Spline at a set speed: trains, camera dollies, patrols and moving platforms.
see: Spline, MovingPlatform

## Members
spline: The object with the Spline component.
speed: Metres a second along it.
mode: 0 loops, 1 ping-pongs, 2 stops at the end.
faceForward: Turn to face along the curve.
offset: Added on top of the curve (to lift it, say).
startDistance: Where on the spline it starts, in metres from the start.
playOnStart: Start moving when play starts.
onEndTargets: Objects told when it reaches the end (once mode).
onEndMessage: The message they get.
distance: How far along the spline it is, in metres.
playing: Whether it is moving.
finished: Whether it has reached the end (once mode).
Play: Starts moving.
Stop: Stops.

## Example: A train with stations
file: Scripts/TrainStops.cs
```csharp
using System.Collections;
using Elintria;
using Elintria.Standard;

public class TrainStops : ElintriaScript
{
    public float[] stations = { 0f, 120f, 260f };

    public override void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        SplineFollower f = GetComponent<SplineFollower>();
        int next = 1;
        while (true)
        {
            f.Play();
            yield return new WaitUntil(() => Mathf.Abs(f.distance - stations[next]) < 1f);
            f.Stop();
            yield return new WaitForSeconds(5f);
            next = (next + 1) % stations.Length;
        }
    }
}
```

## Example: A camera dolly that ends a cutscene
file: Scripts/DollyCutscene.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DollyCutscene : ElintriaScript
{
    public GameObject player;

    public override void Start()
    {
        SplineFollower f = GetComponent<SplineFollower>();
        f.mode = 2;            // once
        f.speed = 3f;
        f.faceForward = true;
        f.onEndTargets.Add(player);
        f.onEndMessage = "SetActive:true";   // the player appears when the shot ends
        f.Play();
    }
}
```

## Example: A guard on a path, speeding up when alerted
file: Scripts/PathGuard.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PathGuard : ElintriaScript
{
    public bool alerted;

    public override void Start()
    {
        SplineFollower f = GetComponent<SplineFollower>();
        f.mode = 1;   // ping-pong along the wall
        f.offset = new Vector3(0f, 0.05f, 0f);
    }

    public override void Update()
    {
        GetComponent<SplineFollower>().speed = alerted ? 4f : 1.5f;
    }
}
```
