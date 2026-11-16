# BasicEnemy
summary: A ready enemy: patrols (or wanders), sees in a cone that walls block and hears noises, investigates, chases, attacks in melee or at range, and flees when badly hurt.
see: StdNoise, StealthPlayer, WorldHealthBar, Health

Give it a Character Controller and a Health. It uses a Nav Mesh Agent when the object has one and the scene has a baked nav mesh, else it steers straight and steps round what is in front. Being hurt sends it after whoever did it. With `stealth` on it grows suspicious first (the "?" over its head) before it is sure (the "!").

## Members
walkSpeed: Metres a second patrolling.
runSpeed: Metres a second chasing.
turnSpeed: Degrees a second it turns.
sightRange: Metres it sees (less for a Stealth Player in the dark).
attackRange: Metres it attacks from.
attackDamage: Damage an attack.
attackCooldown: Seconds between attacks.
projectileSpeed: Ranged: metres a second its shots fly.
targetTag: What it hunts.
patrolPoints: Objects it walks between in turn (empty: it wanders round where it started).
patrolWait: Seconds it waits at each.
model: The rigged child its clips play on (empty: this object).
idleClip: The clip standing still.
walkClip: The clip walking.
runClip: The clip running.
attackClip: The clip attacking.
onAlertTargets: Objects told when it starts chasing.
onAlertMessage: The message they get.
target: What it is after now.
lastSeen: Where it last saw its target.
CanSee: Whether it can see |g| now: in range, in the cone, nothing in the way.
@g: The object.
Attack: Strikes |t| now (melee) or fires at it (ranged).
@t: The target.

## Example: Raising the alarm
file: Scripts/AlarmOnSight.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AlarmOnSight : ElintriaScript
{
    public override void Update()
    {
        BasicEnemy me = GetComponent<BasicEnemy>();
        if (me.state != "Chase") return;
        foreach (BasicEnemy other in FindObjectsOfType<BasicEnemy>())
            if (other != me && other.state == "Patrol" && Vector3.Distance(other.transform.position, transform.position) < 20f)
                StdNoise.Make(transform.position, 25f, gameObject);
    }
}
```

## Example: Tougher as the night goes on
file: Scripts/NightBuff.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NightBuff : ElintriaScript
{
    public override void Update()
    {
        DayNightCycle d = DayNightCycle.current;
        BasicEnemy e = GetComponent<BasicEnemy>();
        e.runSpeed = d != null && !d.isDay ? 6f : 4.5f;
    }
}
```

## Example: Spawning a squad in code
file: Scripts/Squad.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Squad : ElintriaScript
{
    public GameObject enemyTemplate;

    public override void Start()
    {
        for (int i = 0; i < 4; ++i)
        {
            GameObject g = Instantiate(enemyTemplate, transform.position + new Vector3(i * 2f, 0f, 0f), Quaternion.identity);
            BasicEnemy e = g.GetComponent<BasicEnemy>();
            e.attackMode = i % 2;
            e.patrolPoints.Add(gameObject);
        }
    }
}
```

# StdNoise
summary: Sounds Basic Enemies can hear: footsteps, gunshots, a thrown bottle. Make one where it happens, with how far it carries.
see: BasicEnemy, StealthPlayer

Weapons make a noise of `noiseRadius` when they fire; Stealth Players make footstep noises.

## Members
Make: A noise at |position| heard |radius| metres away.
@position: Where it happened.
@radius: Metres it carries.
@source: What made it (an enemy does not react to its own).
Heard: The loudest noise from the last |seconds| heard at |listener|; false when none.
@listener: Where the listener is.
@hearing: Multiplies how far noises carry to it.
@seconds: How far back to listen.
@heard: Set to the noise.

## Example: An explosion everyone hears
file: Scripts/LoudBang.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LoudBang : ElintriaScript
{
    public void OnDeath() { StdNoise.Make(transform.position, 60f, gameObject); }
}
```

## Example: Your own listener
file: Scripts/GuardDog.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GuardDog : ElintriaScript
{
    public override void Update()
    {
        StdNoise.NoiseEvent n;
        if (StdNoise.Heard(transform.position, 2f, 0.5f, out n)) Debug.Log("Woof! Something at " + n.position);
    }
}
```

## Example: A noisy floor
file: Scripts/CreakyFloor.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CreakyFloor : ElintriaScript
{
    private float m_next;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null || Time.time < m_next) return;
        if (Vector3.Distance(p.transform.position, transform.position) < 1.5f) { StdNoise.Make(transform.position, 10f, p); m_next = Time.time + 1f; }
    }
}
```

# DamageType
summary: Kinds of damage. A Damage Resistance scales each; fire, poison, shock and cold also start a Status Effect.
see: StdDamage, DamageResistance, StatusEffects

## Members
Physical: Ordinary damage (bullets, blades, fists).
Fire: Burns: damage over time.
Poison: Poisons: slower damage over time.
Shock: Stuns for a moment.
Cold: Chills: slows.
Explosive: Explosions (armour counts in full).

## Example: A flamethrower
file: Scripts/Flamethrower.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Flamethrower : ElintriaScript
{
    public override void Start() { GetComponent<HitscanWeapon>().damageType = (int)DamageType.Fire; }
}
```

## Example: Typed damage from your own code
file: Scripts/IceSpike.cs
```csharp
using Elintria;
using Elintria.Standard;

public class IceSpike : ElintriaScript
{
    public void Hit(GameObject target)
    {
        StdDamage.Apply(target, 15f, gameObject, target.transform.position, Vector3.up, 2f, DamageType.Cold);
    }
}
```

## Example: Reacting to the type
file: Scripts/TypeReaction.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TypeReaction : ElintriaScript
{
    public void OnDamaged(float amount)
    {
        if (StdEvents.currentType == DamageType.Fire) StdToast.Show("Burning!");
    }
}
```

# DamageResistance
summary: Scales damage by type (1 normal, 0.5 half, 0 immune, 2 double) and takes armour off every hit.
see: DamageType, StdDamage, Equipment

Armour counts in full against physical and explosive damage and at 30% against the rest. Equipment adds `bonusArmor`.

## Members
physical: Multiplier for physical damage.
fire: Multiplier for fire.
poison: Multiplier for poison.
shock: Multiplier for shock.
cold: Multiplier for cold.
explosive: Multiplier for explosions.
bonusArmor: Armour from gear (Equipment sets it).
Multiplier: The multiplier for a type.
@t: The type.
Modify: |amount| after armour and the type's multiplier.
@amount: The damage before.

## Example: A fire elemental
file: Scripts/FireElemental.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FireElemental : ElintriaScript
{
    public override void Start()
    {
        DamageResistance r = gameObject.AddComponent<DamageResistance>();
        r.fire = 0f;
        r.cold = 2f;
    }
}
```

## Example: Armour that breaks
file: Scripts/BreakingArmour.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BreakingArmour : ElintriaScript
{
    public void OnDamaged(float amount)
    {
        DamageResistance r = GetComponent<DamageResistance>();
        r.armor = Mathf.Max(0f, r.armor - 1f);
    }
}
```

## Example: Showing the damage a hit would do
file: Scripts/DamagePreview.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DamagePreview : ElintriaScript
{
    public DamageResistance target;

    public override void Update()
    {
        if (target != null) ScreenGUI.Text(20f, 180f, "Fire hit: " + target.Modify(20f, DamageType.Fire));
    }
}
```

# StatusEffects
summary: Burning, poisoned, shocked (stunned) and chilled (slowed). Typed damage starts them (the component is added when needed); they tick down by themselves.
see: DamageType, DamageResistance, BasicEnemy

## Members
burnDamagePerSecond: Damage a second while burning.
burnSeconds: Seconds a burn lasts.
poisonDamagePerSecond: Damage a second while poisoned.
poisonSeconds: Seconds poison lasts.
stunSeconds: Seconds a shock stuns for.
chillSlow: Speed multiplier while chilled.
chillSeconds: Seconds a chill lasts.
showOnScreen: The player sees its effects written on screen.
burningUntil: When the burn ends (Time.time).
poisonedUntil: When the poison ends.
stunnedUntil: When the stun ends.
chilledUntil: When the chill ends.
burning: Burning now.
poisoned: Poisoned now.
stunned: Stunned now.
chilled: Chilled now.
Apply: Starts the effect a damage type brings.
@t: The damage type.
@source: Who caused it (gets the credit for kills).
ClearAll: Ends every effect (an antidote, a cleanse).

## Example: An antidote
file: Scripts/Antidote.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Antidote : ElintriaScript
{
    public void OnUseItem(string id)
    {
        StatusEffects fx = GetComponent<StatusEffects>();
        if (id == "antidote" && fx != null) fx.poisonedUntil = 0f;
    }
}
```

## Example: Water puts fires out
file: Scripts/Douse.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Douse : ElintriaScript
{
    public override void Update()
    {
        StatusEffects fx = GetComponent<StatusEffects>();
        Swimmer s = GetComponent<Swimmer>();
        if (fx != null && s != null && s.swimming) fx.burningUntil = 0f;
    }
}
```

## Example: A tint while burning
file: Scripts/BurnTint.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BurnTint : ElintriaScript
{
    public override void Update()
    {
        StatusEffects fx = GetComponent<StatusEffects>();
        if (fx != null && fx.burning) ScreenGUI.Fill(new Rect(0f, 0f, GameWindow.width, GameWindow.height), new Color(1f, 0.4f, 0f, 0.15f));
    }
}
```

# DamageNumbers
summary: Numbers that float up from whatever is hurt, coloured by damage type. One in the scene is enough.
see: StdEvents, HitFeedback

## Members
showPlayerDamage: Also float numbers off the player.
riseSpeed: Metres a second they rise.
lifetime: Seconds each shows.

## Example: Adding it from code
file: Scripts/AddNumbers.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AddNumbers : ElintriaScript
{
    public override void Start()
    {
        if (FindObjectOfType<DamageNumbers>() == null) gameObject.AddComponent<DamageNumbers>();
    }
}
```

## Example: Off in the settings
file: Scripts/NumbersSetting.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NumbersSetting : ElintriaScript
{
    public bool show = true;

    public override void Update()
    {
        foreach (DamageNumbers d in FindObjectsOfType<DamageNumbers>()) d.enabled = show;
    }
}
```

## Example: Slower, longer numbers
file: Scripts/SlowNumbers.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SlowNumbers : ElintriaScript
{
    public override void Start()
    {
        DamageNumbers d = GetComponent<DamageNumbers>();
        d.riseSpeed = 0.6f;
        d.lifetime = 2f;
    }
}
```

# HitFeedback
summary: On the player: when it is hurt, the screen shakes, flashes red, freezes a moment on big hits, and the player is knocked back; when it hurts something, a smaller shake.
see: StdCameraShake, StdHud, DamageNumbers

## Members
shakePerDamage: Metres of shake for each point of damage.
maxShake: The most it shakes.
redFlash: Flash the screen red.
hitStop: Seconds the game nearly stops on a big hit (or a kill).
knockback: Metres a second the player is pushed.
onDealing: Feedback when the player hurts something too.
lastHitFrom: Where the last hit came from (the HUD's damage direction).
lastHitTime: When it came.

## Example: Less shake for comfort
file: Scripts/ComfortShake.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ComfortShake : ElintriaScript
{
    public bool reduceMotion = true;

    public override void Start()
    {
        HitFeedback h = GetComponent<HitFeedback>();
        if (reduceMotion) { h.maxShake = 0.05f; h.hitStop = 0f; }
    }
}
```

## Example: A sound on a hit
file: Scripts/HurtSound.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HurtSound : ElintriaScript
{
    public AudioSource grunt;

    public void OnDamaged(float amount)
    {
        if (grunt != null) grunt.Play();
    }
}
```

## Example: Was the player hit recently?
file: Scripts/InCombat.cs
```csharp
using Elintria;
using Elintria.Standard;

public class InCombat : ElintriaScript
{
    public override void Update()
    {
        HitFeedback h = GetComponent<HitFeedback>();
        bool fighting = Time.time - h.lastHitTime < 5f;
        if (fighting) ScreenGUI.Text(20f, 200f, "In combat", null, new Color(1f, 0.4f, 0.4f, 1f));
    }
}
```

# Crosshair
summary: A crosshair in the middle of the screen that opens while moving and firing, with hit markers (red on a kill).
see: WeaponRecoil, HitscanWeapon

## Members
color: Its colour.
gap: Pixels between the lines when still.
length: Pixels each line is long.
thickness: Pixels each line is thick.
moveSpread: Extra gap while moving.
fireSpread: Extra gap per shot (it closes again).
dot: A dot in the middle.
hitMarkers: Show a mark when a shot hurts something.

## Example: A green dot sight
file: Scripts/DotSight.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DotSight : ElintriaScript
{
    public override void Start()
    {
        Crosshair c = GetComponent<Crosshair>();
        c.color = new Color(0.3f, 1f, 0.4f, 1f);
        c.length = 0f;
        c.dot = true;
    }
}
```

## Example: Hidden while aiming
file: Scripts/AimHidesCrosshair.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AimHidesCrosshair : ElintriaScript
{
    public override void Update()
    {
        GetComponent<Crosshair>().enabled = !Input.GetMouseButton(1);
    }
}
```

## Example: A wider crosshair for a shotgun
file: Scripts/ShotgunCrosshair.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ShotgunCrosshair : ElintriaScript
{
    public override void Update()
    {
        HitscanWeapon w = GetComponent<HitscanWeapon>();
        GetComponent<Crosshair>().gap = w != null && w.pellets > 1 ? 18f : 6f;
    }
}
```

# WeaponRecoil
summary: Kicks the view up as the player's weapons fire (and lets it settle), sways a weapon model with the mouse, and can nudge the aim toward enemies near the crosshair while aiming.
see: Crosshair, Weapon, FirstPersonController

## Members
kickUp: Degrees up per shot.
kickSide: Degrees either way per shot, at random.
recovery: Share of the kick that comes back down.
recoverySpeed: How fast it comes back.
swayObject: A weapon model that sways (a child near the camera).
swayAmount: How far it sways.
aimAssist: 0 off .. 1 strong: pulls toward an enemy within `assistAngle` while aiming (right mouse).
assistAngle: Degrees from the crosshair an enemy may be.
assistRange: Metres an enemy may be.
enemyTag: The enemies' tag.

## Example: Aim assist for gamepads
file: Scripts/PadAssist.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PadAssist : ElintriaScript
{
    public bool usingPad;

    public override void Update()
    {
        GetComponent<WeaponRecoil>().aimAssist = usingPad ? 0.6f : 0f;
    }
}
```

## Example: Less recoil crouched
file: Scripts/CrouchSteady.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CrouchSteady : ElintriaScript
{
    public override void Update()
    {
        FirstPersonController fp = GetComponent<FirstPersonController>();
        GetComponent<WeaponRecoil>().kickUp = fp.crouching ? 0.6f : 1.2f;
    }
}
```

## Example: A heavy machine gun
file: Scripts/HeavyGun.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HeavyGun : ElintriaScript
{
    public override void Start()
    {
        WeaponRecoil r = GetComponent<WeaponRecoil>();
        r.kickUp = 2f;
        r.kickSide = 1f;
        r.recovery = 0.4f;
    }
}
```

# CoverSystem
summary: Third-person cover: press the key by a wall to take cover; A/D move along it, right mouse peeks out to aim, firing without peeking is blind (wider spread). The key again (or S) leaves.
see: ThirdPersonController, Weapon

Walls lower than `lowCoverHeight` are low cover: the player crouches behind them.

## Members
key: The key, by name (default Control).
reach: Metres from the wall it can take cover.
moveSpeed: Metres a second along the wall.
peekOffset: Metres the camera leans out when peeking.
inCover: In cover now.
lowCover: Behind low cover.
peeking: Peeking out now.
wallNormal: The way the wall faces.
Leave: Leaves cover.
@tp: The Third Person Controller.

## Example: Healing slowly in cover
file: Scripts/CoverRest.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CoverRest : ElintriaScript
{
    public override void Update()
    {
        if (GetComponent<CoverSystem>().inCover) GetComponent<Health>().Heal(3f * Time.deltaTime);
    }
}
```

## Example: Enemies lose sight of you in cover
file: Scripts/CoverHides.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CoverHides : ElintriaScript
{
    public override void Update()
    {
        CoverSystem c = GetComponent<CoverSystem>();
        StealthPlayer s = GetComponent<StealthPlayer>();
        if (s != null) s.hidden = c.inCover && c.lowCover && !c.peeking;
    }
}
```

## Example: A cover prompt
file: Scripts/CoverPrompt.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CoverPrompt : ElintriaScript
{
    public override void Update()
    {
        CoverSystem c = GetComponent<CoverSystem>();
        if (c.inCover) ScreenGUI.Text(20f, 220f, c.peeking ? "Peeking" : "In cover - right mouse to peek");
    }
}
```

# StdEvents
summary: Game-wide events from the Standard components: anything hurt or anything killed. Quests count kills, XP is given, damage numbers float up and the HUD shows hits through these.
see: Health, StdDamage, DamageNumbers

## Members
Damaged: (health, amount, attacker) - after damage is taken.
Died: (health, killer) - as something dies.
currentType: The type of the damage being dealt now (read it in a Damaged handler).
Clear: Forgets every handler.

## Example: A kill counter
file: Scripts/KillCounter.cs
```csharp
using Elintria;
using Elintria.Standard;

public class KillCounter : ElintriaScript
{
    public int kills;

    public override void Start()
    {
        StdEvents.Died += (h, killer) => { if (killer == gameObject) ++kills; };
    }
}
```

## Example: Achievement for damage dealt
file: Scripts/DamageTally.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DamageTally : ElintriaScript
{
    public float dealt;

    public override void Start()
    {
        StdEvents.Damaged += (h, amount, attacker) =>
        {
            if (attacker != gameObject) return;
            dealt += amount;
            if (dealt >= 1000f) StdToast.Show("1000 damage dealt!");
        };
    }
}
```

## Example: Boss music when the boss is hurt
file: Scripts/BossMusic.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BossMusic : ElintriaScript
{
    public GameObject boss;
    public AudioSource music;

    public override void Start()
    {
        StdEvents.Damaged += (h, amount, attacker) =>
        {
            if (h.gameObject == boss && music != null && !music.isPlaying) music.Play();
        };
    }
}
```

# StdCameraShake
summary: Shakes the view. Every Standard controller places its camera through StdCamera.Place, which adds this shake.
see: HitFeedback, StdCamera

## Members
Shake: Shakes by |amount| metres for |seconds|, fading out; a bigger shake overrides a smaller one.
@amount: Metres at the start.
@seconds: How long.
@frequency: How fast it shakes.
Offset: The shake's position offset now.
Tilt: The shake's look offset now.

## Example: An earthquake
file: Scripts/Earthquake.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Earthquake : ElintriaScript
{
    public void Begin() { StdCameraShake.Shake(0.4f, 6f, 10f); }
}
```

## Example: Footsteps of a giant
file: Scripts/GiantSteps.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GiantSteps : ElintriaScript
{
    private float m_next;

    public override void Update()
    {
        if (Time.time < m_next) return;
        m_next = Time.time + 1.2f;
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null) return;
        float d = Vector3.Distance(p.transform.position, transform.position);
        if (d < 40f) StdCameraShake.Shake(0.2f * (1f - d / 40f), 0.4f);
    }
}
```

## Example: Shake on landing hard
file: Scripts/HardLanding.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HardLanding : ElintriaScript
{
    private float m_fall;

    public override void Update()
    {
        FirstPersonController fp = GetComponent<FirstPersonController>();
        if (!fp.isGrounded) m_fall = Mathf.Min(m_fall, fp.velocity.y);
        else { if (m_fall < -12f) StdCameraShake.Shake(0.15f, 0.3f); m_fall = 0f; }
    }
}
```

# StdScreen
summary: World points to screen pixels, and screen pixels to rays, for the view the controllers placed last - world-space health bars, name tags, markers and mouse picking.
see: StdHud, WorldHealthBar, StdCamera

## Members
cameraFieldOfView: The scene camera's vertical field of view, when a controller does not say.
View: The eye, the look direction and the field of view now.
@pos: Set to the eye.
@dir: Set to the look direction.
@fov: Set to the field of view.
WorldToScreen: Pixels from the game view's top left; false when the point is behind the camera.
@world: The point.
@screen: Set to the pixel.
ScreenRay: The ray through a pixel (from the top left).
@origin: Set to the ray's start.
@direction: Set to its direction.

## Example: A label over an object
file: Scripts/FloatingLabel.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FloatingLabel : ElintriaScript
{
    public string text = "Here";

    public override void Update()
    {
        Vector2 s;
        if (StdScreen.WorldToScreen(transform.position + Vector3.up * 2f, out s)) ScreenGUI.Text(s.x, s.y, text);
    }
}
```

## Example: Picking with the mouse
file: Scripts/MousePick.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MousePick : ElintriaScript
{
    public override void Update()
    {
        float x, y;
        if (!Input.GetMouseButtonDown(0) || !GameWindow.MousePosition(out x, out y)) return;
        Vector3 o, d;
        StdScreen.ScreenRay(new Vector2(x, y), out o, out d);
        RaycastHit hit;
        if (Physics.Raycast(o, d, out hit, 100f)) Debug.Log("Clicked " + hit.gameObject.name);
    }
}
```

## Example: Is it on screen?
file: Scripts/OnScreenCheck.cs
```csharp
using Elintria;
using Elintria.Standard;

public class OnScreenCheck : ElintriaScript
{
    public bool onScreen;

    public override void Update()
    {
        Vector2 s;
        onScreen = StdScreen.WorldToScreen(transform.position, out s) && s.x >= 0f && s.y >= 0f && s.x <= GameWindow.width && s.y <= GameWindow.height;
    }
}
```

# StdTime
summary: Real seconds, untouched by Time.timeScale: for hit-stop, pause menus and anything that must keep moving while the game is slowed or stopped.
see: HitFeedback, Time

## Members
real: Seconds since the program started, in real time.

## Example: A pause menu clock
file: Scripts/PauseClock.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PauseClock : ElintriaScript
{
    public override void Update()
    {
        if (Time.timeScale == 0f) ScreenGUI.Text(20f, 20f, "Paused for " + Mathf.Round(StdTime.real) + " s");
    }
}
```

## Example: Slow motion that ends on time
file: Scripts/SlowMo.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SlowMo : ElintriaScript
{
    private float m_until;

    public void Begin(float seconds) { Time.timeScale = 0.3f; m_until = StdTime.real + seconds; }

    public override void Update()
    {
        if (m_until > 0f && StdTime.real >= m_until) { Time.timeScale = 1f; m_until = 0f; }
    }
}
```

## Example: Measuring a frame
file: Scripts/FrameTimer.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FrameTimer : ElintriaScript
{
    private float m_last;

    public override void Update()
    {
        float now = StdTime.real;
        ScreenGUI.Text(20f, 40f, ((now - m_last) * 1000f).ToString("0.0") + " ms");
        m_last = now;
    }
}
```

# StdToast
summary: Short messages that stack up in the middle of the screen and fade ("Item picked up", "Game saved").
see: StdHud, Interactor

They are drawn by any Interactor, Std Hud or Quest Tracker in the scene.

## Members
Show: Shows |text| for |seconds| (in |color|).
@text: The message.
@seconds: How long.
@color: Its colour.
Draw: Draws them now (once a frame; the HUD components call it).

## Example: A welcome message
file: Scripts/Welcome.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Welcome : ElintriaScript
{
    public override void Start() { StdToast.Show("Welcome to the Facility", 4f); }
}
```

## Example: A red warning
file: Scripts/LowAmmoWarning.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LowAmmoWarning : ElintriaScript
{
    private bool m_said;

    public override void Update()
    {
        Weapon w = GetComponent<Weapon>();
        if (w == null) return;
        if (w.ammo <= 3 && !m_said) { StdToast.Show("Low ammo", 2f, new Color(1f, 0.3f, 0.3f, 1f)); m_said = true; }
        if (w.ammo > 3) m_said = false;
    }
}
```

## Example: Toasts without a HUD
file: Scripts/ToastDrawer.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ToastDrawer : ElintriaScript
{
    public override void Update() { StdToast.Draw(); }
}
```

# StdBoxVisual
summary: A box drawn where its object is, by the script renderer: what objects made in code show as while the game runs (loot, shots, arrows).
see: StdLine, LootTable

GameObject.CreatePrimitive draws nothing in a running game; make the object with `Make` instead (or add this to it).

## Members
size: The box, metres (z along the object's forward).
color: Its colour.
glow: Drawn additively (a glowing shot).
Make: A new object at |at| drawn as a box, with no collider.
@name: The object's name.
@at: Where.
@size: The box.
@color: Its colour.
@glow: Glowing.

## Example: A glowing orb
file: Scripts/SpawnOrb.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SpawnOrb : ElintriaScript
{
    public override void Start()
    {
        StdBoxVisual.Make("Orb", transform.position + Vector3.up, new Vector3(0.3f, 0.3f, 0.3f), new Color(0.4f, 0.8f, 1f, 1f), true);
    }
}
```

## Example: A dropped coin
file: Scripts/DropCoin.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DropCoin : ElintriaScript
{
    public void OnDeath()
    {
        GameObject coin = StdBoxVisual.Make("Coin", transform.position + Vector3.up * 0.3f, new Vector3(0.2f, 0.2f, 0.05f), new Color(1f, 0.8f, 0.2f, 1f));
        Pickup p = coin.AddComponent<Pickup>();
        p.itemId = "coin";
        p.autoPickup = true;
    }
}
```

## Example: A changing colour
file: Scripts/PulseBox.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PulseBox : ElintriaScript
{
    public override void Start()
    {
        StdBoxVisual v = gameObject.AddComponent<StdBoxVisual>();
        v.color = new Color(1f, 0.2f, 0.2f, 1f);
        v.size = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
```

# StdLine
summary: A stretched box drawn by the script renderer between two points: ropes, beams, laser sights, rain streaks.
see: StdBoxVisual, GrappleHook, Weather

## Members
StdLine: A new line in |color| (|glow|: drawn additively).
@color: Its colour.
@glow: Glowing.
Set: From |a| to |b|, |width| metres across.
@a: One end.
@b: The other end.
@width: Metres across.
visible: Drawn or not.
Destroy: Removes it (call it in OnDestroy).

## Example: A laser sight
file: Scripts/LaserSight.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LaserSight : ElintriaScript
{
    private StdLine m_line;

    public override void LateUpdate()
    {
        if (m_line == null) m_line = new StdLine(new Color(1f, 0.1f, 0.1f, 1f), true);
        Vector3 eye, dir; float fov;
        StdScreen.View(out eye, out dir, out fov);
        m_line.Set(transform.position + Vector3.up * 1.4f, eye + dir * 30f, 0.01f);
    }

    public override void OnDestroy() { if (m_line != null) m_line.Destroy(); }
}
```

## Example: A tether between two objects
file: Scripts/Tether.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Tether : ElintriaScript
{
    public GameObject other;
    private StdLine m_rope;

    public override void LateUpdate()
    {
        if (other == null) return;
        if (m_rope == null) m_rope = new StdLine(new Color(0.3f, 0.25f, 0.2f, 1f));
        m_rope.Set(transform.position, other.transform.position, 0.04f);
    }

    public override void OnDestroy() { if (m_rope != null) m_rope.Destroy(); }
}
```

## Example: Showing a path
file: Scripts/PathLine.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PathLine : ElintriaScript
{
    public GameObject target;
    private StdLine m_line;

    public override void LateUpdate()
    {
        if (target == null) return;
        if (m_line == null) m_line = new StdLine(new Color(0.3f, 1f, 0.5f, 1f), true);
        m_line.visible = Input.GetKey(KeyCode.Tab);
        if (m_line.visible) m_line.Set(transform.position + Vector3.up * 0.1f, target.transform.position + Vector3.up * 0.1f, 0.05f);
    }

    public override void OnDestroy() { if (m_line != null) m_line.Destroy(); }
}
```
