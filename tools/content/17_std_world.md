# PhysicsGrabber
summary: Picks up Rigidbodies in front of the player: G to grab and drop, left click to throw, the mouse wheel to turn what is held.
see: Pushable, Rigidbody

## Members
key: The key, by name (default G).
reach: Metres it reaches.
holdDistance: Metres in front it holds things.
maxMass: The heaviest thing it lifts.
throwSpeed: Metres a second a throw gives.
follow: How hard the held thing is pulled to the hold point.
held: What it is holding (null: nothing).
TryGrab: Grabs the Rigidbody in front; false when there is none (or it is too heavy).
Drop: Lets go, with a velocity.
@velocity: The velocity it leaves with.

## Example: Stronger with an upgrade
file: Scripts/StrongArms.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StrongArms : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id == "exo_arms") GetComponent<PhysicsGrabber>().maxMass = 200f;
    }
}
```

## Example: Thrown things hurt
file: Scripts/ThrownDamage.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ThrownDamage : ElintriaScript
{
    private GameObject m_last;

    public override void Update()
    {
        PhysicsGrabber g = GetComponent<PhysicsGrabber>();
        if (g.held != null) m_last = g.held;
        else if (m_last != null && Input.GetMouseButtonDown(0))
        {
            foreach (BasicEnemy e in FindObjectsOfType<BasicEnemy>())
                if (Vector3.Distance(e.transform.position, m_last.transform.position) < 1.5f) StdDamage.Apply(e.gameObject, 25f, gameObject, e.transform.position, Vector3.up, 4f);
            m_last = null;
        }
    }
}
```

## Example: A prompt while holding
file: Scripts/HoldPrompt.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HoldPrompt : ElintriaScript
{
    public override void Update()
    {
        if (GetComponent<PhysicsGrabber>().held != null) ScreenGUI.Text(20f, 240f, "Left click: throw   G: drop   Wheel: turn");
    }
}
```

# Pushable
summary: A crate the player pushes along the floor by walking into it, one axis at a time; with a Rigidbody it presses Pressure Plates too.
see: PressurePlate, PhysicsGrabber

## Members
size: The box's size, metres (world axes).
pushSpeed: Metres a second it moves.
pusherTag: Who can push it.

## Example: A heavy block
file: Scripts/HeavyBlock.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HeavyBlock : ElintriaScript
{
    public override void Start() { GetComponent<Pushable>().pushSpeed = 0.6f; }
}
```

## Example: Scraping sound
file: Scripts/ScrapeSound.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ScrapeSound : ElintriaScript
{
    private Vector3 m_last;

    public override void Update()
    {
        if ((transform.position - m_last).sqrMagnitude > 1e-6f) StdNoise.Make(transform.position, 6f, gameObject);
        m_last = transform.position;
    }
}
```

## Example: Solved when on the mark
file: Scripts/BlockOnMark.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BlockOnMark : ElintriaScript
{
    public GameObject mark;

    public override void Update()
    {
        if (mark != null && Vector3.Distance(transform.position, mark.transform.position) < 0.3f) StdVars.Set("block_puzzle", "1");
    }
}
```

# Teleporter
summary: A pad that sends the player to its partner (or any target object) when stepped on, with a flash.
see: LevelExit

## Members
target: Where it sends to (its position and facing).
radius: Metres from its middle that count as standing on it.
cooldown: Seconds before any pad works again after a jump (no ping-pong).
requiredTag: Who it sends.
fade: Flash the screen.
Send: Sends |who| to the target now.
@who: The object.

## Example: A one-way exit
file: Scripts/OneWay.cs
```csharp
using Elintria;
using Elintria.Standard;

public class OneWay : ElintriaScript
{
    public Teleporter backPad;

    public override void Start() { if (backPad != null) backPad.enabled = false; }
}
```

## Example: Powered teleporters
file: Scripts/PoweredPad.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PoweredPad : ElintriaScript
{
    public override void Update() { GetComponent<Teleporter>().enabled = StdVars.Is("power", "on"); }
}
```

## Example: Sending enemies too
file: Scripts/EnemyPad.cs
```csharp
using Elintria;
using Elintria.Standard;

public class EnemyPad : ElintriaScript
{
    public override void Update()
    {
        Teleporter t = GetComponent<Teleporter>();
        foreach (BasicEnemy e in FindObjectsOfType<BasicEnemy>())
            if (Vector3.Distance(e.transform.position, transform.position) < t.radius) e.transform.position = t.target.transform.position;
    }
}
```

# LevelExit
summary: A zone that loads another scene when the player walks in - optionally needing an item or a condition, and saving first.
see: SceneManager, SaveSystem, StdVars

## Members
sceneName: The scene to load, as in the Build Settings.
size: The zone, metres (world axes; standing on this object).
requiredItem: An item the player must carry (empty: none).
requires: A StdVars condition that must hold (empty: none).
saveSlot: Save to this slot first (-1: no).
lockedMessage: What it says when the player cannot leave yet.

## Example: Leave once the objective is done
file: Scripts/ExitAfterObjective.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ExitAfterObjective : ElintriaScript
{
    public override void Start()
    {
        LevelExit e = GetComponent<LevelExit>();
        e.requires = "quest:escape:done";
        e.lockedMessage = "Find the fuses first";
    }
}
```

## Example: Choosing the next level
file: Scripts/NextLevel.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NextLevel : ElintriaScript
{
    public override void Start()
    {
        int n = StdVars.GetInt("level_number", 1);
        GetComponent<LevelExit>().sceneName = "Scenes/Level" + (n + 1) + ".scene";
        StdVars.Set("level_number", (n + 1).ToString());
    }
}
```

## Example: Autosave on the way out
file: Scripts/ExitSave.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ExitSave : ElintriaScript
{
    public override void Start() { GetComponent<LevelExit>().saveSlot = 0; }
}
```

# LootTable
summary: Weighted loot: "item:weight:min:max|..." rolled for items and counts, and dropped as pickups on the ground.
see: Breakable, LootChest, MoneyPickup

## Members
Roll: Rolls |rolls| times on |table|; "none" entries give nothing.
@table: The table.
@rolls: How many rolls.
Drop: A pickup on the ground: a small box taken by walking over it ("money" and "gold" go to the Wallet).
@item: The item.
@count: How many.
@at: Where.

## Example: Enemy drops
file: Scripts/EnemyDrops.cs
```csharp
using Elintria;
using Elintria.Standard;

public class EnemyDrops : ElintriaScript
{
    public string table = "ammo:5:5:15|potion:2:1:1|money:4:2:8|none:5";

    public void OnDeath()
    {
        foreach (ItemStack s in LootTable.Roll(table, 2)) LootTable.Drop(s.id, s.count, transform.position);
    }
}
```

## Example: A daily reward
file: Scripts/DailyReward.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DailyReward : ElintriaScript
{
    public void Claim(Inventory inv)
    {
        foreach (ItemStack s in LootTable.Roll("gem:1:1:1|potion:3:1:3|coin:6:10:30")) inv.Add(s.id, s.count);
    }
}
```

## Example: Rarer loot deeper down
file: Scripts/DepthLoot.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DepthLoot : ElintriaScript
{
    public override void Start()
    {
        bool deep = transform.position.y < -20f;
        GetComponent<LootChest>().loot = deep ? "gem:4:1:2|relic:1:1:1" : "coin:6:3:10|potion:3:1:2";
    }
}
```

# MoneyPickup
summary: Money on the ground: walking over it adds it to the player's Wallet.
see: Wallet, LootTable

## Members
amount: How much.
radius: Metres the player must come within.

## Example: A coin trail
file: Scripts/CoinTrail.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CoinTrail : ElintriaScript
{
    public override void Start()
    {
        for (int i = 0; i < 10; ++i)
        {
            GameObject c = StdBoxVisual.Make("Coin", transform.position + transform.forward * i * 1.5f + Vector3.up * 0.4f, new Vector3(0.2f, 0.2f, 0.05f), new Color(1f, 0.8f, 0.2f, 1f));
            c.AddComponent<MoneyPickup>().amount = 1;
        }
    }
}
```

## Example: A magnet
file: Scripts/CoinMagnet.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CoinMagnet : ElintriaScript
{
    public override void Start()
    {
        foreach (MoneyPickup m in FindObjectsOfType<MoneyPickup>()) m.radius = 4f;
    }
}
```

## Example: A money bag
file: Scripts/MoneyBag.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MoneyBag : ElintriaScript
{
    public override void Start() { gameObject.AddComponent<MoneyPickup>().amount = 100; }
}
```

# Breakable
summary: Pots and crates: when its Health runs out it breaks (a Destructible on it shatters it), makes a noise and drops loot from a weighted table.
see: LootTable, Destructible, Health

## Members
loot: The loot table ("item:weight:min:max|...", "none" for nothing).
rolls: How many rolls on it.
removeWhenBroken: Remove the object when it breaks (when it has no Destructible).
onBreakTargets: Objects told when it breaks.
onBreakMessage: The message they get.
broken: It has broken.
Break: Breaks it now.

## Example: A secret behind a crate
file: Scripts/SecretCrate.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SecretCrate : ElintriaScript
{
    public GameObject secretDoor;

    public override void Update()
    {
        if (GetComponent<Breakable>().broken && secretDoor != null) secretDoor.SetActive(false);
    }
}
```

## Example: Explosive barrels
file: Scripts/ExplosiveBarrel.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ExplosiveBarrel : ElintriaScript
{
    public void OnDeath()
    {
        foreach (Health h in FindObjectsOfType<Health>())
            if (h.gameObject != gameObject && Vector3.Distance(h.transform.position, transform.position) < 4f)
                StdDamage.Apply(h.gameObject, 50f, gameObject, transform.position, h.transform.position - transform.position, 8f, DamageType.Explosive);
        StdCameraShake.Shake(0.3f, 0.5f);
    }
}
```

## Example: Breaking with a kick
file: Scripts/KickCrate.cs
```csharp
using Elintria;
using Elintria.Standard;

public class KickCrate : Interactable
{
    public override string Prompt(Interactor who) => "Kick";

    public override void Interact(Interactor who) { GetComponent<Breakable>().Break(); }
}
```

# LootChest
summary: A chest: E opens it once and its loot goes into the Inventory (or spills on the ground); it can need a key, and its lid swings open.
see: LootTable, Breakable

## Members
loot: The loot table.
rolls: How many rolls on it.
intoInventory: Straight into the Inventory instead of onto the ground.
lid: The lid object (turns open).
requiredItem: An item needed to open it (a key; empty: none).
opened: It has been opened (saved with a Saveable Object).

## Example: A locked chest
file: Scripts/LockedChest.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LockedChest : ElintriaScript
{
    public override void Start()
    {
        LootChest c = GetComponent<LootChest>();
        c.requiredItem = "chest_key";
        c.loot = "gem:1:2:3|money:1:50:100";
    }
}
```

## Example: A mimic
file: Scripts/Mimic.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Mimic : ElintriaScript
{
    public override void Update()
    {
        if (!GetComponent<LootChest>().opened) return;
        BasicEnemy e = GetComponent<BasicEnemy>();
        if (e != null && !e.enabled) { e.enabled = true; StdToast.Show("It's a mimic!"); }
    }
}
```

## Example: Refilling chests
file: Scripts/RefillChest.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RefillChest : ElintriaScript
{
    public override void Update()
    {
        DayNightCycle d = DayNightCycle.current;
        if (d != null && d.hour < 0.05f) GetComponent<LootChest>().opened = false;
    }
}
```

# SwingingBlade
summary: A blade that swings like a pendulum and hurts and knocks back whatever it hits. Put the blade model in `blade`, hanging down from this pivot.
see: RollingBoulder, LevelHazard

## Members
blade: The swinging part (empty: this object).
angle: Degrees either side.
period: Seconds for a whole swing there and back.
length: Metres from the pivot to the blade's edge.
damage: Damage a hit.
knockback: Metres a second a hit pushes.
phase: Seconds into the swing it starts (so blades in a row take turns).

## Example: A row of blades
file: Scripts/BladeRow.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BladeRow : ElintriaScript
{
    public override void Start()
    {
        int i = 0;
        foreach (SwingingBlade b in FindObjectsOfType<SwingingBlade>()) b.phase = (i++) * 0.6f;
    }
}
```

## Example: Faster when the alarm is on
file: Scripts/AlarmBlades.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AlarmBlades : ElintriaScript
{
    public override void Update() { GetComponent<SwingingBlade>().period = StdVars.Is("alarm", "1") ? 1.2f : 2.4f; }
}
```

## Example: Switched off by a lever
file: Scripts/BladeSwitch.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BladeSwitch : ElintriaScript
{
    public void Stop() { GetComponent<SwingingBlade>().enabled = false; }
}
```

# RollingBoulder
summary: A boulder that rolls along `direction` when the player comes near (or when told "Roll"), crushing what is in its way, until it hits a wall or goes far enough.
see: SwingingBlade, TriggerZone

## Members
direction: Which way it rolls (world).
speed: Metres a second.
radius: The boulder's radius, metres.
damage: Damage to what it runs over.
triggerDistance: Starts rolling when the player is this close (0: only when told "Roll").
maxDistance: Stops after this many metres.
rolling: Rolling now.
Roll: Starts it rolling.

## Example: Released by a pressure plate
file: Scripts/BoulderPlate.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BoulderPlate : ElintriaScript
{
    public RollingBoulder boulder;

    public void Pressed() { if (boulder != null) boulder.Roll(); }
}
```

## Example: Only when told
file: Scripts/ManualBoulder.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ManualBoulder : ElintriaScript
{
    public override void Start() { GetComponent<RollingBoulder>().triggerDistance = 0f; }
}
```

## Example: Running from it
file: Scripts/RunWarning.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RunWarning : ElintriaScript
{
    private bool m_said;

    public override void Update()
    {
        if (GetComponent<RollingBoulder>().rolling && !m_said) { StdToast.Show("RUN!", 2f, new Color(1f, 0.3f, 0.2f, 1f)); m_said = true; }
    }
}
```

# CollapsingFloor
summary: A floor tile that shakes, then drops, a moment after the player stands on it; it comes back after a while.
see: SwingingBlade, MovingPlatform

## Members
size: The tile, metres (world axes).
delay: Seconds between stepping on it and it falling.
shake: Metres it shakes before falling.
respawnTime: Seconds before it is back (0: never).
fallen: It has fallen.

## Example: A crumbling bridge
file: Scripts/CrumblingBridge.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CrumblingBridge : ElintriaScript
{
    public override void Start()
    {
        foreach (CollapsingFloor f in FindObjectsOfType<CollapsingFloor>()) { f.delay = 0.4f; f.respawnTime = 0f; }
    }
}
```

## Example: Counting fallen tiles
file: Scripts/FallenCount.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FallenCount : ElintriaScript
{
    public override void Update()
    {
        int n = 0;
        foreach (CollapsingFloor f in FindObjectsOfType<CollapsingFloor>()) if (f.fallen) ++n;
        ScreenGUI.Text(20f, 280f, n + " tiles fallen");
    }
}
```

## Example: Harder each try
file: Scripts/HarderFloors.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HarderFloors : ElintriaScript
{
    public void OnRespawn()
    {
        foreach (CollapsingFloor f in FindObjectsOfType<CollapsingFloor>()) f.delay = Mathf.Max(0.2f, f.delay - 0.1f);
    }
}
```

# ArrowTrap
summary: Shoots arrows (Projectiles) along its forward: when the player crosses in front, or on a timer.
see: Projectile, DamageType

## Members
interval: Seconds between arrows.
triggerRange: Fires only when the player is this close in front (0: on the timer).
speed: Metres a second an arrow flies.
damage: Damage an arrow does.
damageType: 0 physical, 1 fire, 2 poison...
arrowTemplate: An object copied for each arrow (empty: a plain arrow).
Fire: Fires an arrow now.

## Example: Poison darts
file: Scripts/PoisonDarts.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PoisonDarts : ElintriaScript
{
    public override void Start()
    {
        ArrowTrap t = GetComponent<ArrowTrap>();
        t.damageType = (int)DamageType.Poison;
        t.damage = 4f;
        t.interval = 1f;
    }
}
```

## Example: A volley on a lever
file: Scripts/Volley.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Volley : ElintriaScript
{
    public void Fire()
    {
        foreach (ArrowTrap t in FindObjectsOfType<ArrowTrap>()) t.Fire();
    }
}
```

## Example: A turret that turns
file: Scripts/TurningTrap.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TurningTrap : ElintriaScript
{
    public override void Update()
    {
        transform.eulerAngles = new Vector3(0f, Mathf.Sin(Time.time * 0.5f) * 45f, 0f);
    }
}
```

# DayNightCycle
summary: Turns the sun (a Directional Light) through the day, colours the sky, dims the ambient light at night, warms and cools the air for Needs, and fires events at dawn and dusk.
see: Weather, Needs, Light

## Members
dayMinutes: Real minutes for a whole day.
hour: The hour now, 0..24 (saved with a Saveable Object).
running: The clock moves.
sun: The sun (empty: the scene's first Directional Light).
sunIntensity: The sun's brightness at noon.
moonIntensity: Its brightness at night.
sunYaw: Degrees the sun's path is turned.
daySky: The sky by day.
duskSky: The sky at dawn and dusk.
nightSky: The sky by night.
dayTemperature: Air temperature at noon.
nightTemperature: Air temperature at night.
showClock: Draw the time.
onDawnTargets: Objects told at dawn.
onDawnMessage: The message they get.
onDuskTargets: Objects told at dusk.
onDuskMessage: The message they get.
current: The cycle in the scene.
daylight: 0 at night .. 1 at noon.
overcast: Weather dims the sun with this (1 clear).
isDay: Between 6 and 18.

## Example: Sleeping until morning
file: Scripts/Bed.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Bed : Interactable
{
    public override string Prompt(Interactor who) => "Sleep until morning";

    public override void Interact(Interactor who)
    {
        DayNightCycle d = DayNightCycle.current;
        if (d != null) d.hour = 6.5f;
        StdToast.Show("You wake up rested");
    }
}
```

## Example: Street lamps at night
file: Scripts/StreetLamp.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StreetLamp : ElintriaScript
{
    public override void Update()
    {
        DayNightCycle d = DayNightCycle.current;
        Light l = GetComponent<Light>();
        if (d != null && l != null) l.intensity = d.isDay ? 0f : 1.5f;
    }
}
```

## Example: A longer day
file: Scripts/LongDay.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LongDay : ElintriaScript
{
    public override void Start() { GetComponent<DayNightCycle>().dayMinutes = 40f; }
}
```

# Weather
summary: Clear, rain, snow, storm or fog: falling streaks or flakes round the camera, fog, lightning in storms. It changes by itself every few minutes (by the chances you give), or when told.
see: DayNightCycle, RenderEnvironment

## Members
weather: 0 clear, 1 rain, 2 snow, 3 storm, 4 fog.
changeMinutes: Change by itself every this many minutes (0: never).
chances: Chances of each when it changes: "clear,rain,snow,storm,fog".
particles: Streaks or flakes round the camera.
area: Metres across the area they fall in.
fogColor: The fog's colour.
strength: How far it has come in, 0..1.
Set: Changes the weather now.
@kind: 0 clear, 1 rain, 2 snow, 3 storm, 4 fog.

## Example: Snow in the mountains
file: Scripts/MountainSnow.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MountainSnow : ElintriaScript
{
    public Weather weather;

    public override void Update()
    {
        if (weather != null) weather.Set(transform.position.y > 40f ? 2 : 0);
    }
}
```

## Example: A storm for the finale
file: Scripts/FinaleStorm.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FinaleStorm : ElintriaScript
{
    public void Begin()
    {
        Weather w = FindObjectOfType<Weather>();
        if (w == null) return;
        w.changeMinutes = 0f;
        w.Set(3);
    }
}
```

## Example: Rain puts out fires
file: Scripts/RainPutsOutFires.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RainPutsOutFires : ElintriaScript
{
    public override void Update()
    {
        Weather w = FindObjectOfType<Weather>();
        bool raining = w != null && (w.weather == 1 || w.weather == 3) && w.strength > 0.5f;
        GetComponent<HeatSource>().enabled = !raining;
    }
}
```
