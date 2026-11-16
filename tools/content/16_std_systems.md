# SaveSystem
summary: Saves and loads the game in numbered slots on disk: every Saveable Object's place, whether it is on, the Standard components' state and every [Saveable] field, plus game-wide values (quests, Std Vars).
see: SaveableObject, SaveableAttribute, SavePoint, AutoSave

Saves go to your user folder (`folder`), one file per slot. A load puts the scene that is open back as it was: objects destroyed since the scene started (taken pickups, broken crates) are destroyed again.

## Members
lastError: Why the last Save or Load failed.
PathOf: The file a slot is kept in.
@slot: The slot number.
Exists: Whether a slot has a save.
Delete: Deletes a slot's save.
Info: A slot's summary: when it was saved, its label, the play time.

## Example: A save menu
file: Scripts/SaveMenu.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SaveMenu : ElintriaScript
{
    public bool open;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) open = !open;
        if (!open) return;
        for (int i = 1; i <= 3; ++i)
        {
            SaveSlotInfo info = SaveSystem.Info(i);
            string label = "Slot " + i + (info.exists ? "  " + info.time.ToString("g") : "  empty");
            if (ScreenGUI.Button(new Rect(40f, 40f + i * 40f, 260f, 32f), label)) SaveSystem.Save(i, "Manual");
            if (info.exists && ScreenGUI.Button(new Rect(310f, 40f + i * 40f, 80f, 32f), "Load")) SaveSystem.Load(i);
        }
    }
}
```

## Example: Saving your own values
file: Scripts/SaveScore.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SaveScore : ElintriaScript
{
    public static int score;

    public override void Start()
    {
        SaveSystem.Saving += d => d["score"] = score.ToString();
        SaveSystem.Loaded += d => { string s; if (d.TryGetValue("score", out s)) score = int.Parse(s); };
    }
}
```

## Example: Continue from the last save
file: Scripts/ContinueButton.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ContinueButton : ElintriaScript
{
    public override void Update()
    {
        if (!SaveSystem.Exists(0)) return;
        if (ScreenGUI.Button(new Rect(40f, 40f, 200f, 40f), "Continue") && !SaveSystem.Load(0)) Debug.LogWarning(SaveSystem.lastError);
    }
}
```

# SaveableObject
summary: Marks an object to be saved by the Save System, under an id that stays the same between runs.
see: SaveSystem, SaveableAttribute

Leave `id` empty to make one from the object's name and starting position; give it one yourself when the object moves before saving or has a twin of the same name.

## Members
id: The id it is saved under (empty: made from its name and position).
saveTransform: Save where it is and how it is turned.
saveActive: Save whether it is on.

## Example: Marking every crate
file: Scripts/MarkCrates.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MarkCrates : ElintriaScript
{
    public override void Start()
    {
        foreach (Breakable b in FindObjectsOfType<Breakable>())
            if (b.GetComponent<SaveableObject>() == null) b.gameObject.AddComponent<SaveableObject>();
    }
}
```

## Example: A fixed id
file: Scripts/PlayerId.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PlayerId : ElintriaScript
{
    public override void Awake()
    {
        SaveableObject s = GetComponent<SaveableObject>();
        if (s != null) s.id = "player";
    }
}
```

## Example: Saving state but not position
file: Scripts/StaticSave.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StaticSave : ElintriaScript
{
    public override void Start()
    {
        SaveableObject s = GetComponent<SaveableObject>();
        s.saveTransform = false;
        s.saveActive = true;
    }
}
```

# SaveableAttribute
summary: Marks a script field the Save System writes and reads back (numbers, bools, text, vectors, colours, enums, lists of text or numbers).
see: SaveSystem, SaveableObject

The object needs a Saveable Object.

## Example: A saved counter
file: Scripts/Shrine.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Shrine : ElintriaScript
{
    [Saveable] public int prayers;
    [Saveable] public bool blessed;

    public void Pray() { ++prayers; if (prayers >= 3) blessed = true; }
}
```

## Example: A saved list
file: Scripts/VisitedPlaces.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class VisitedPlaces : ElintriaScript
{
    [Saveable] public List<string> visited = new List<string>();

    public void Visit(string place) { if (!visited.Contains(place)) visited.Add(place); }
}
```

## Example: A saved colour
file: Scripts/PaintedDoor.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PaintedDoor : ElintriaScript
{
    [Saveable] public Color paint = Color.white;
    [Saveable] public Vector3 lastPainter;
}
```

# SavePoint
summary: A place to save: E saves the game to a slot, heals the player and makes it the respawn point.
see: SaveSystem, PlayerRespawn, LevelCheckpoint

## Members
slot: The slot it saves to.
heal: Also heals the player.

## Example: A different slot per save point
file: Scripts/SlotPerPoint.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SlotPerPoint : ElintriaScript
{
    public override void Start()
    {
        int i = 1;
        foreach (SavePoint p in FindObjectsOfType<SavePoint>()) p.slot = i++;
    }
}
```

## Example: A save point that costs money
file: Scripts/PaidSave.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PaidSave : SavePoint
{
    public int cost = 10;

    public override bool CanInteract(Interactor who, out string reason)
    {
        Wallet w = who != null ? who.GetComponent<Wallet>() : null;
        reason = "Costs " + cost;
        return w != null && w.money >= cost;
    }

    public override void Interact(Interactor who)
    {
        who.GetComponent<Wallet>().Spend(cost);
        base.Interact(who);
    }
}
```

## Example: A save point that only heals
file: Scripts/HealOnly.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HealOnly : ElintriaScript
{
    public override void Start() { GetComponent<SavePoint>().heal = true; }
}
```

# AutoSave
summary: Saves at every Level Checkpoint and every few minutes; F5 saves to a quick slot and F9 loads it.
see: SaveSystem, LevelCheckpoint

## Members
slot: The slot autosaves go to.
onCheckpoints: Save when a new checkpoint is reached.
everyMinutes: Minutes between timed saves (0: none).
quickKeys: F5 quick save, F9 quick load.
quickSlot: The quick save slot.

## Example: Autosave every five minutes
file: Scripts/TimedSaves.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TimedSaves : ElintriaScript
{
    public override void Start() { GetComponent<AutoSave>().everyMinutes = 5f; }
}
```

## Example: No quick load in hardcore mode
file: Scripts/Hardcore.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Hardcore : ElintriaScript
{
    public bool hardcore;

    public override void Start() { GetComponent<AutoSave>().quickKeys = !hardcore; }
}
```

## Example: Saving before a boss
file: Scripts/BossDoorSave.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BossDoorSave : ElintriaScript
{
    public void OnTriggered() { SaveSystem.Save(GetComponent<AutoSave>().slot, "Before the boss"); }
}
```

# StdVars
summary: Game-wide named values ("door_open" = "1") that dialogue, quests and your scripts read and set; saved with the game.
see: DialogueNpc, QuestGiver, SaveSystem

Conditions (`Check`) and actions (`Do`) are the same strings dialogue scripts and quest rewards use.

## Members
Get: A value (|fallback| when unset).
@name: The value's name.
@fallback: What to return when it is unset.
Set: Sets a value.
@value: The new value.
GetInt: A value as a whole number.
Add: Adds to a whole-number value.
@amount: How much.
Is: Whether a value equals |value| (ignoring case).
Clear: Forgets every value.
All: Every value.
Check: A condition: "var=value", "var!=value", "has:item[:n]", "quest:id:active|done|none", "money>=n", "level>=n"; "&" joins, "!" negates.
@cond: The condition.
@player: The player (for has, money and level).
Do: Actions: "give:item:n", "take:item:n", "set:var=value", "add:var:n", "money:n", "xp:n", "quest:id", "heal:n".
@actions: The actions, comma or space separated.

## Example: A lever that remembers
file: Scripts/RememberedLever.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RememberedLever : ElintriaScript
{
    public override void Start()
    {
        if (StdVars.Is("bridge_down", "1")) GetComponent<Lever>().Toggle();
    }

    public void Pulled() { StdVars.Set("bridge_down", "1"); }
}
```

## Example: A gate for the worthy
file: Scripts/WorthyGate.cs
```csharp
using Elintria;
using Elintria.Standard;

public class WorthyGate : ElintriaScript
{
    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (StdVars.Check("level>=5&has:sigil", p)) GetComponent<Door>().Unlock();
    }
}
```

## Example: Rewards in one line
file: Scripts/Treasure.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Treasure : ElintriaScript
{
    public void Claim()
    {
        StdVars.Do("give:gem:2, money:100, xp:50, set:treasure_found=1", GameObject.FindWithTag("Player"));
    }
}
```

# QuestGiver
summary: Offers a quest made of steps - collect, go to, kill, talk, variable - and pays a reward when the last step is done. E to accept, or a Dialogue NPC's "quest" action.
see: QuestLog, QuestTracker, DialogueNpc, StdVars

Steps are "kind:target:count:text" separated by "|": `collect:fuse:3:Find three fuses|goto:Generator:3:Bring them to the generator|kill:Zombie:5:Clear the yard|talk:Mara::Tell Mara|var:power=on::Turn the power on`. A goto step's count is the radius in metres; a kill step counts deaths of objects whose name contains the target (or tagged with it).

## Members
questId: The quest's id (conditions and the log use it).
title: Its name.
description: A line about it for the log.
Build: A new Quest from these settings.
Offered: Whether the quest has been taken.

## Example: A quest made in code
file: Scripts/BountyBoard.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BountyBoard : ElintriaScript
{
    public override void Start()
    {
        QuestGiver g = gameObject.AddComponent<QuestGiver>();
        g.questId = "wolves";
        g.title = "Wolf Bounty";
        g.steps = "kill:Wolf:5:Hunt five wolves|talk:Hunter::Claim the bounty";
        g.reward = "money:80, xp:150";
    }
}
```

## Example: Offered only after another quest
file: Scripts/ChainedQuest.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ChainedQuest : ElintriaScript
{
    public override void Start() { GetComponent<QuestGiver>().requires = "quest:wolves:done"; }
}
```

## Example: Starting it from a trigger
file: Scripts/QuestTrigger.cs
```csharp
using Elintria;
using Elintria.Standard;

public class QuestTrigger : ElintriaScript
{
    public QuestGiver giver;

    public void OnTriggered() { if (!giver.Offered) giver.Offer(); }
}
```

# QuestLog
summary: The quests the player has taken: their steps and progress, and events when they start, move on and finish.
see: QuestGiver, QuestTracker

## Members
quests: Every quest taken.
Started: A quest was taken.
StepDone: A step was finished.
Completed: A quest was finished.
Find: The quest with an id (null: not taken).
@id: The quest's id.
Start: Takes a quest; false when it was already taken.
@q: The quest.
Talked: Somebody was talked to (Dialogue NPC calls it).
@name: Their name.
Active: The first unfinished quest.
Tick: Checks every step (the tracker and givers call it each frame).
@player: The player.
Clear: Forgets every quest.

## Example: A sound when a quest is done
file: Scripts/QuestFanfare.cs
```csharp
using Elintria;
using Elintria.Standard;

public class QuestFanfare : ElintriaScript
{
    public AudioSource fanfare;

    public override void Start() { QuestLog.Completed += q => { if (fanfare != null) fanfare.Play(); }; }
}
```

## Example: Counting finished quests
file: Scripts/QuestCount.cs
```csharp
using Elintria;
using Elintria.Standard;

public class QuestCount : ElintriaScript
{
    public override void Update()
    {
        int done = 0;
        foreach (Quest q in QuestLog.quests) if (q.done) ++done;
        ScreenGUI.Text(20f, 60f, "Quests done: " + done);
    }
}
```

## Example: Opening a door on a step
file: Scripts/StepDoor.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StepDoor : ElintriaScript
{
    public string questId = "power";

    public override void Start()
    {
        QuestLog.StepDone += (q, s) => { if (q.id == questId && s.kind == "goto") GetComponent<Door>().Open(); };
    }
}
```

# Quest
summary: A quest taken: its steps, which one is in hand, and whether it is done.
see: QuestLog, QuestStep, QuestGiver

## Members
id: Its id.
title: Its name.
description: A line about it.
reward: What it pays (StdVars actions).
steps: Its steps.
current: The step in hand.
done: Finished.
Step: The step in hand (null when done).

## Example: Showing the current step
file: Scripts/CurrentStep.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CurrentStep : ElintriaScript
{
    public override void Update()
    {
        Quest q = QuestLog.Active;
        if (q != null && q.Step != null) ScreenGUI.Text(20f, 80f, q.title + ": " + q.Step.text);
    }
}
```

## Example: Skipping a step (a cheat)
file: Scripts/SkipStep.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SkipStep : ElintriaScript
{
    public override void Update()
    {
        Quest q = QuestLog.Active;
        if (q != null && Input.GetKeyDown(KeyCode.F8)) ++q.current;
    }
}
```

## Example: A quest's progress
file: Scripts/QuestProgress.cs
```csharp
using Elintria;
using Elintria.Standard;

public class QuestProgress : ElintriaScript
{
    public float Share(string id)
    {
        Quest q = QuestLog.Find(id);
        if (q == null || q.steps.Count == 0) return 0f;
        return q.done ? 1f : q.current / (float)q.steps.Count;
    }
}
```

# QuestStep
summary: One step of a quest: what to do (collect, goto, kill, talk, var), to what, how many, and how far along it is.
see: Quest, QuestGiver

## Members
kind: collect, goto, kill, talk or var.
target: The item, object name, enemy name or tag, person, or condition.
count: How many (goto: the radius in metres).
text: What the tracker shows.
progress: How many so far.
done: Finished.

## Example: Progress bars for every step
file: Scripts/StepBars.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StepBars : ElintriaScript
{
    public override void Update()
    {
        Quest q = QuestLog.Active;
        if (q == null) return;
        float y = 260f;
        foreach (QuestStep s in q.steps)
        {
            ScreenGUI.Bar(new Rect(20f, y, 150f, 6f), s.done ? 1f : s.progress / (float)Mathf.Max(1, s.count), Color.green, Color.black);
            y += 10f;
        }
    }
}
```

## Example: Kill steps counted by your own rule
file: Scripts/CountBosses.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CountBosses : ElintriaScript
{
    public override void Start()
    {
        StdEvents.Died += (h, killer) =>
        {
            Quest q = QuestLog.Active;
            if (q != null && q.Step != null && q.Step.kind == "kill" && h.maxHealth >= 500f) ++q.Step.progress;
        };
    }
}
```

## Example: Listing what is left
file: Scripts/StepsLeft.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StepsLeft : ElintriaScript
{
    public override void Start()
    {
        QuestLog.Started += q => { foreach (QuestStep s in q.steps) Debug.Log(q.title + ": " + s.text); };
    }
}
```

# QuestTracker
summary: The quest HUD on the player: the step in hand with its progress, a marker to where it is, and J for the whole log.
see: QuestLog, StdHud

## Members
showMarker: A marker to a goto or talk step's object.
logKey: The key for the quest log (default J).
logOpen: The log is open.

## Example: The log on L
file: Scripts/LogOnL.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LogOnL : ElintriaScript
{
    public override void Awake() { GetComponent<QuestTracker>().logKey = "L"; }
}
```

## Example: No markers in hard mode
file: Scripts/NoMarkers.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NoMarkers : ElintriaScript
{
    public bool hard;

    public override void Start() { GetComponent<QuestTracker>().showMarker = !hard; }
}
```

## Example: Pausing while the log is open
file: Scripts/PauseOnLog.cs
```csharp
using Elintria;
using Elintria.Standard;

public class PauseOnLog : ElintriaScript
{
    public override void Update() { Time.timeScale = GetComponent<QuestTracker>().logOpen ? 0f : 1f; }
}
```

# DialogueNpc
summary: A conversation from a short script: lines, choices with conditions, and actions (give items, start quests, set values). Typewriter text; Space continues, 1-9 or a click picks a choice.
see: StdVars, QuestGiver, NameTag

A script is nodes ("# name"), lines ("Name: text", or just text for this NPC), choices ("> text -> node ?condition") and actions in brackets ("[give:key]", "[quest:power]", "[set:met=1]", "[end]"). Write it in `script` with "|" between lines, or in a text file named by `scriptFile`. Several "# start" nodes with conditions pick the first that holds.

## Members
npcName: The name shown (and what "talk" quest steps match).
typeSpeed: Letters a second (0: all at once).
talkDistance: Metres away before the talk ends by itself.
talking: Talking now.
Begin: Starts the conversation.

## Example: A shopkeeper's greeting
file: Scripts/Greeting.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Greeting : ElintriaScript
{
    public override void Start()
    {
        DialogueNpc d = GetComponent<DialogueNpc>();
        d.script = "# start ?met=1|Welcome back!|> Bye -> end" +
                   "|# start|Ah, a new face. [set:met=1]|> Hello -> end";
    }
}
```

## Example: Dialogue from a file
file: Scripts/FileDialogue.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FileDialogue : ElintriaScript
{
    public override void Start() { GetComponent<DialogueNpc>().scriptFile = "Assets/Dialogue/elder.txt"; }
}
```

## Example: Talking when the player comes close
file: Scripts/Ambush.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Ambush : ElintriaScript
{
    private bool m_done;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (m_done || p == null || Vector3.Distance(p.transform.position, transform.position) > 3f) return;
        m_done = true;
        GetComponent<DialogueNpc>().Begin();
    }
}
```

# CraftingBench
summary: A workbench: E opens its recipes; click one (or press its number) to make it from what is in the Inventory.
see: Recipe, Inventory

Recipes are "result:count=ingredient:n+ingredient:n" separated by "|".

## Members
benchName: The name shown.
recipes: The recipes.
craftTime: Seconds to make one.
open: The recipe window is open.
Close: Closes the window.

## Example: A forge
file: Scripts/Forge.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Forge : ElintriaScript
{
    public override void Start()
    {
        CraftingBench b = GetComponent<CraftingBench>();
        b.benchName = "Forge";
        b.recipes = "sword:1=iron:3+wood:1|helmet:1=iron:4|nails:10=iron:1";
        b.craftTime = 2f;
    }
}
```

## Example: Unlocking a recipe
file: Scripts/LearnRecipe.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LearnRecipe : ElintriaScript
{
    public CraftingBench bench;

    public void OnUseItem(string id)
    {
        if (id == "recipe_bomb" && !bench.recipes.Contains("bomb")) bench.recipes += "|bomb:1=powder:2+cloth:1";
    }
}
```

## Example: Closing on damage
file: Scripts/CloseOnHit.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CloseOnHit : ElintriaScript
{
    public CraftingBench bench;

    public void OnDamaged(float amount) { if (bench != null && bench.open) bench.Close(); }
}
```

# Recipe
summary: One crafting recipe: what it makes, how many, and from what.
see: CraftingBench, Inventory

## Members
result: The item made.
count: How many.
ingredients: What it takes.
Parse: Recipes from text ("result:count=ingredient:n+ingredient:n|...").
@text: The recipes.
CanMake: Whether |inv| has the ingredients (and room).
@inv: The inventory.
Make: Takes the ingredients and adds the result; false when it cannot.
Describe: The ingredients in words ("2 Cloth, 1 Herb").

## Example: Crafting from code
file: Scripts/QuickCraft.cs
```csharp
using Elintria;
using Elintria.Standard;

public class QuickCraft : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.H)) return;
        foreach (Recipe r in Recipe.Parse("medkit:1=cloth:2+herb:1"))
            if (r.Make(GetComponent<Inventory>())) StdToast.Show("Made a medkit");
    }
}
```

## Example: What can I make?
file: Scripts/CraftableList.cs
```csharp
using Elintria;
using Elintria.Standard;

public class CraftableList : ElintriaScript
{
    public CraftingBench bench;

    public override void Update()
    {
        float y = 300f;
        foreach (Recipe r in Recipe.Parse(bench.recipes))
            if (r.CanMake(GetComponent<Inventory>())) { ScreenGUI.Text(20f, y, r.result + " (" + r.Describe() + ")"); y += 20f; }
    }
}
```

## Example: Doubling the output
file: Scripts/MasterCrafter.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MasterCrafter : ElintriaScript
{
    public bool Craft(Recipe r)
    {
        Inventory inv = GetComponent<Inventory>();
        if (!r.Make(inv)) return false;
        inv.Add(r.result, r.count);      // a second batch for free
        return true;
    }
}
```

# Wallet
summary: The player's money, shown in the top right.
see: Vendor, MoneyPickup, StdVars

## Members
money: How much there is.
currency: Its name ("gold").
showOnScreen: Draw it.
Add: Adds (or takes away) money.
@amount: How much.
Spend: Takes |amount|; false (and nothing taken) when there is not enough.

## Example: Paying a toll
file: Scripts/Toll.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Toll : Interactable
{
    public int price = 5;

    public override string Prompt(Interactor who) => "Pay " + price + " to pass";

    public override void Interact(Interactor who)
    {
        Wallet w = who.GetComponent<Wallet>();
        if (w != null && w.Spend(price)) GetComponent<Door>().Open();
        else who.Say("Not enough money");
    }
}
```

## Example: Credits instead of gold
file: Scripts/Credits.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Credits : ElintriaScript
{
    public override void Start() { GetComponent<Wallet>().currency = "credits"; }
}
```

## Example: Interest
file: Scripts/Interest.cs
```csharp
using Elintria;
using Elintria.Standard;

public class Interest : ElintriaScript
{
    private float m_next = 60f;

    public override void Update()
    {
        if (Time.time < m_next) return;
        m_next = Time.time + 60f;
        Wallet w = GetComponent<Wallet>();
        w.Add(w.money / 20);
    }
}
```

# Vendor
summary: A shopkeeper: E opens the shop - buy from its stock with the Wallet's money, sell what it buys.
see: Wallet, Inventory, ItemDatabase

Stock is "item:price|item:price". With `buys` empty it buys back what it sells at `sellShare` of the price.

## Members
shopName: The name shown.
stock: What it sells and for how much.
buys: What it pays for, "item:price|..." (empty: what it sells).
sellShare: Share of the price it pays when buying back.
stockCount: How many of each it has (0: endless).
open: The shop is open.
Buy: Buys one |item| for |price| for the player; false when it cannot.
@item: The item.
@price: What it costs.
Sell: Sells one |item| for |price|; false when the player has none.

## Example: Prices that rise
file: Scripts/RisingPrices.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RisingPrices : ElintriaScript
{
    public override void Update()
    {
        DayNightCycle d = DayNightCycle.current;
        GetComponent<Vendor>().stock = d != null && !d.isDay ? "potion:40|torch:16" : "potion:20|torch:8";
    }
}
```

## Example: A collector who buys gems
file: Scripts/GemCollector.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GemCollector : ElintriaScript
{
    public override void Start()
    {
        Vendor v = GetComponent<Vendor>();
        v.stock = "";
        v.buys = "gem:50|relic:200";
    }
}
```

## Example: Limited stock
file: Scripts/LimitedStock.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LimitedStock : ElintriaScript
{
    public override void Start() { GetComponent<Vendor>().stockCount = 3; }
}
```

# CharacterStats
summary: Level, experience and stat points. Kills (an Xp Reward on the victim) and quests give XP; each level gives points to spend on Strength (damage), Agility (speed), Vitality (health) and Endurance (stamina) - K opens the page.
see: XpReward, Equipment, StdDamage

## Members
level: The level.
xp: Experience toward the next level.
statPoints: Points to spend.
strength: Each point over 5 adds 5% damage.
agility: Each point over 5 adds 2% speed.
vitality: Each point adds `healthPerVitality` health.
endurance: Each point adds `staminaPerEndurance` stamina.
pointsPerLevel: Points each level gives.
baseHealth: Health before Vitality.
healthPerVitality: Health per point of Vitality.
staminaPerEndurance: Stamina per point of Endurance.
pageKey: The key for the stats page (default K).
showXpBar: Draw the XP bar.
pageOpen: The stats page is open.
damageMultiplier: What Strength multiplies damage by.
speedMultiplier: What Agility multiplies speed by.
xpToNext: XP the next level needs.
AddXp: Gives experience (levelling up as it fills).
@amount: How much.
Raise: Spends a point on a stat ("strength", "agility", "vitality", "endurance"); false when there are none.
@stat: The stat.
Apply: Puts the stats into Health, Stamina and speed.

## Example: XP for exploring
file: Scripts/DiscoveryXp.cs
```csharp
using Elintria;
using Elintria.Standard;

public class DiscoveryXp : ElintriaScript
{
    private bool m_found;

    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (m_found || p == null || Vector3.Distance(p.transform.position, transform.position) > 5f) return;
        m_found = true;
        p.GetComponent<CharacterStats>().AddXp(40);
        StdToast.Show("Discovered: " + gameObject.name);
    }
}
```

## Example: A faster level curve
file: Scripts/FastLevels.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FastLevels : ElintriaScript
{
    public override void Start()
    {
        CharacterStats s = GetComponent<CharacterStats>();
        s.xpPerLevel = 50;
        s.pointsPerLevel = 2;
    }
}
```

## Example: A level gate
file: Scripts/LevelGate.cs
```csharp
using Elintria;
using Elintria.Standard;

public class LevelGate : Interactable
{
    public int needed = 5;

    public override bool CanInteract(Interactor who, out string reason)
    {
        CharacterStats s = who.GetComponent<CharacterStats>();
        reason = "Level " + needed + " needed";
        return s != null && s.level >= needed;
    }
}
```

# XpReward
summary: Experience (and money) given to whoever kills this.
see: CharacterStats, BasicEnemy

## Members
xp: Experience given.
money: Money given (to a Wallet).

## Example: Bosses give more
file: Scripts/BossReward.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BossReward : ElintriaScript
{
    public override void Start()
    {
        XpReward r = GetComponent<XpReward>();
        r.xp = 500;
        r.money = 200;
    }
}
```

## Example: Reward by health
file: Scripts/RewardByHealth.cs
```csharp
using Elintria;
using Elintria.Standard;

public class RewardByHealth : ElintriaScript
{
    public override void Start()
    {
        XpReward r = gameObject.AddComponent<XpReward>();
        r.xp = (int)(GetComponent<Health>().maxHealth / 4f);
    }
}
```

## Example: No XP for farmed enemies
file: Scripts/NoFarming.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NoFarming : ElintriaScript
{
    public override void Start()
    {
        if (StdVars.GetInt("farm_kills") > 50) GetComponent<XpReward>().xp = 0;
        StdVars.Add("farm_kills", 1);
    }
}
```

# Equipment
summary: Gear slots - head, body, hands, feet and weapon. Using an equippable item from the Inventory wears it (the old piece goes back); gear adds armour, speed and damage, and the weapon slot switches on the child object named after the item. U opens the page.
see: CharacterStats, DamageResistance, Inventory

Items are "item:slot:armor=5,speed=-0.05,damage=0.1" separated by "|".

## Members
items: The gear and what each piece does.
pageKey: The key for the equipment page (default U).
head: What is worn on the head.
body: On the body.
hands: On the hands.
feet: On the feet.
weapon: In hand (the child object of this name is switched on).
armorBonus: Armour from the gear.
speedBonus: Speed from the gear (0.1: 10% faster).
damageBonus: Damage from the gear (0.1: 10% more).
pageOpen: The page is open.
IsEquippable: Whether an item is gear.
@id: The item.
SlotOf: The slot an item goes in ("" when it is not gear).
Equip: Wears |id| from the Inventory; false when it cannot.
Unequip: Takes off what is in |slot| (back into the Inventory).
@slot: The slot.
Apply: Totals the gear's stats and switches the weapon object.

## Example: Your own gear list
file: Scripts/GearList.cs
```csharp
using Elintria;
using Elintria.Standard;

public class GearList : ElintriaScript
{
    public override void Awake()
    {
        GetComponent<Equipment>().items = "hood:head:armor=1|chainmail:body:armor=12,speed=-0.1|bow:weapon|axe:weapon";
    }
}
```

## Example: Starting equipped
file: Scripts/StartingGear.cs
```csharp
using Elintria;
using Elintria.Standard;

public class StartingGear : ElintriaScript
{
    public override void Start()
    {
        GetComponent<Inventory>().Add("rifle", 1);
        GetComponent<Equipment>().Equip("rifle");
    }
}
```

## Example: A set bonus
file: Scripts/SetBonus.cs
```csharp
using Elintria;
using Elintria.Standard;

public class SetBonus : ElintriaScript
{
    public override void Update()
    {
        Equipment e = GetComponent<Equipment>();
        bool set = e.head == "knight_helm" && e.body == "knight_plate";
        GetComponent<Health>().maxHealth = set ? 150f : 100f;
    }
}
```

# StdHud
summary: The player's HUD: health, stamina and ammo bars, a compass with markers, markers that point off screen, which way the last hit came from, and the toast messages. Put it on the player.
see: HudMarker, StdToast, HitFeedback

## Members
showHealth: Draw the health bar.
showStamina: Draw the stamina bar.
showAmmo: Draw the ammo count (for weapons that do not draw their own).
showCompass: Draw the compass.
showMarkers: Draw Hud Markers on the screen.
showDamageDirection: Show where the last hit came from (needs Hit Feedback).
healthColor: The health bar's colour.
Marker: A marker for this frame only (a quest step uses it).
@world: Where.
@color: Its colour.
@distance: Metres away to write (-1: none).
DrawMarker: Draws a marker on the point, or at the screen's edge pointing to it.
@label: A label.

## Example: A compass-only HUD
file: Scripts/MinimalHud.cs
```csharp
using Elintria;
using Elintria.Standard;

public class MinimalHud : ElintriaScript
{
    public override void Start()
    {
        StdHud h = GetComponent<StdHud>();
        h.showHealth = false;
        h.showStamina = false;
        h.showAmmo = false;
    }
}
```

## Example: Marking the nearest enemy
file: Scripts/NearestEnemyMarker.cs
```csharp
using Elintria;
using Elintria.Standard;

public class NearestEnemyMarker : ElintriaScript
{
    public override void Update()
    {
        BasicEnemy best = null; float d = 30f;
        foreach (BasicEnemy e in FindObjectsOfType<BasicEnemy>())
        {
            float x = Vector3.Distance(e.transform.position, transform.position);
            if (x < d) { d = x; best = e; }
        }
        if (best != null) StdHud.Marker(best.transform.position + Vector3.up * 2f, new Color(1f, 0.3f, 0.3f, 1f), d);
    }
}
```

## Example: Hiding the HUD in cutscenes
file: Scripts/HudOff.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HudOff : ElintriaScript
{
    public bool cutscene;

    public override void Update() { GetComponent<StdHud>().enabled = !cutscene; }
}
```

# HudMarker
summary: Puts this object on the HUD's compass and on the screen (pointing from the edge when off screen), with its distance.
see: StdHud, QuestTracker

## Members
label: Text beside the marker.
color: Its colour.
onCompass: Show it on the compass.
onScreen: Show it on the screen.
showDistance: Write the distance.
height: Metres above the object.
maxDistance: Hidden beyond this (0: always shown).

## Example: Marking the exit once the key is found
file: Scripts/ExitMarker.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ExitMarker : ElintriaScript
{
    public override void Update()
    {
        GameObject p = GameObject.FindWithTag("Player");
        bool hasKey = p != null && p.GetComponent<Inventory>().Has("exit_key");
        GetComponent<HudMarker>().enabled = hasKey;
    }
}
```

## Example: Adding markers to every chest
file: Scripts/ChestMarkers.cs
```csharp
using Elintria;
using Elintria.Standard;

public class ChestMarkers : ElintriaScript
{
    public override void Start()
    {
        foreach (LootChest c in FindObjectsOfType<LootChest>())
        {
            HudMarker m = c.gameObject.AddComponent<HudMarker>();
            m.label = "Chest";
            m.maxDistance = 25f;
        }
    }
}
```

## Example: A blinking marker
file: Scripts/BlinkMarker.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BlinkMarker : ElintriaScript
{
    public override void Update()
    {
        GetComponent<HudMarker>().onScreen = Mathf.Sin(Time.time * 6f) > 0f;
    }
}
```

# WorldHealthBar
summary: A health bar over this object's head while it is hurt, and a Basic Enemy's "?" (with how suspicious it is) and "!" when it spots you.
see: BasicEnemy, Health, StdScreen

## Members
height: Metres above the object.
width: Pixels wide.
hideWhenFull: Hidden at full health.
maxDistance: Hidden beyond this.
color: The bar's colour.
showAlert: Show a Basic Enemy's "?" and "!".

## Example: Always shown for bosses
file: Scripts/BossBar.cs
```csharp
using Elintria;
using Elintria.Standard;

public class BossBar : ElintriaScript
{
    public override void Start()
    {
        WorldHealthBar b = GetComponent<WorldHealthBar>();
        b.hideWhenFull = false;
        b.width = 140f;
        b.height = 4f;
    }
}
```

## Example: Bars on every enemy
file: Scripts/AllBars.cs
```csharp
using Elintria;
using Elintria.Standard;

public class AllBars : ElintriaScript
{
    public override void Start()
    {
        foreach (BasicEnemy e in FindObjectsOfType<BasicEnemy>())
            if (e.GetComponent<WorldHealthBar>() == null) e.gameObject.AddComponent<WorldHealthBar>();
    }
}
```

## Example: Friendly green bars
file: Scripts/FriendlyBar.cs
```csharp
using Elintria;
using Elintria.Standard;

public class FriendlyBar : ElintriaScript
{
    public override void Start() { GetComponent<WorldHealthBar>().color = new Color(0.3f, 0.9f, 0.4f, 1f); }
}
```

# NameTag
summary: A name over a character, fading out with distance.
see: WorldHealthBar, DialogueNpc

## Members
displayName: The name (empty: the object's name).
height: Metres above the object.
maxDistance: Hidden beyond this.
color: Its colour.

## Example: Names from dialogue
file: Scripts/TagFromDialogue.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TagFromDialogue : ElintriaScript
{
    public override void Start()
    {
        NameTag t = GetComponent<NameTag>();
        DialogueNpc d = GetComponent<DialogueNpc>();
        if (d != null) t.displayName = d.npcName;
    }
}
```

## Example: Hostile names in red
file: Scripts/HostileTag.cs
```csharp
using Elintria;
using Elintria.Standard;

public class HostileTag : ElintriaScript
{
    public override void Update()
    {
        BasicEnemy e = GetComponent<BasicEnemy>();
        GetComponent<NameTag>().color = e != null && e.state == "Chase" ? new Color(1f, 0.3f, 0.3f, 1f) : Color.white;
    }
}
```

## Example: Showing a title
file: Scripts/TitledName.cs
```csharp
using Elintria;
using Elintria.Standard;

public class TitledName : ElintriaScript
{
    public string title = "the Brave";

    public override void Start()
    {
        NameTag t = GetComponent<NameTag>();
        t.displayName = gameObject.name + " " + title;
    }
}
```

# InteractIcon
summary: On the player: an [E] over every Interactable in reach, highlighted on the one E would use.
see: Interactor, Interactable

## Members
range: Metres from the player.
color: The icon's letter colour.

## Example: A shorter reach
file: Scripts/IconReach.cs
```csharp
using Elintria;
using Elintria.Standard;

public class IconReach : ElintriaScript
{
    public override void Start() { GetComponent<InteractIcon>().range = 2.5f; }
}
```

## Example: Off while in combat
file: Scripts/IconsInCombat.cs
```csharp
using Elintria;
using Elintria.Standard;

public class IconsInCombat : ElintriaScript
{
    public override void Update()
    {
        HitFeedback h = GetComponent<HitFeedback>();
        GetComponent<InteractIcon>().enabled = h == null || Time.time - h.lastHitTime > 3f;
    }
}
```

## Example: Coloured icons
file: Scripts/IconColour.cs
```csharp
using Elintria;
using Elintria.Standard;

public class IconColour : ElintriaScript
{
    public override void Start() { GetComponent<InteractIcon>().color = new Color(1f, 0.9f, 0.4f, 1f); }
}
```
