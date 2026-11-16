# Agent
summary: A script that learns by reinforcement: it observes, acts, and is rewarded.
see: Observations, ActionBuffers, Brain, Academy

Derive from `Agent` to make something learn a behaviour instead of you coding it. Override `CollectObservations` to describe what it sees, `OnActionReceived` to act on what it decided, and call `AddReward` and `EndEpisode` to tell it how well it did. `Heuristic` lets you drive it yourself with the keyboard to check everything works.

Train it in **Window > AI > ML Training**. Hidden copies of the game run your scene side by side, and the trained brain is saved as `Assets/ML/<behaviour>.elbrain`. Set the agent's `brain` field to that file and it plays by itself, in the editor and in a built game.

> note: Decisions happen in `FixedUpdate`. If your agent overrides `FixedUpdate`, call `base.FixedUpdate()`.

## Members
ContinuousActions: Override to say how many continuous actions (each -1..1) the agent takes.
DiscreteBranches: Override to say how many choices each discrete action branch has, e.g. `new[] { 3 }` for left/none/right.
OnEpisodeBegin: Called when an episode starts: put the agent and its world back to a starting state.
CollectObservations: Add everything the agent should see to `o`, as numbers. Keep them roughly between -10 and 10.
@o: Add the agent's observations to this.
OnActionReceived: Act on the decided actions: move, turn, shoot. Usually also gives rewards.
@actions: The actions chosen.
Heuristic: Fill `actions` from the keyboard so you can play as the agent (used when `heuristicOnly` is on, or there is no brain).
CumulativeReward: The total reward this episode.
StepCount: Steps taken this episode.
CompletedEpisodes: How many episodes have finished.
Mode: How it is deciding now: "training", "brain" or "heuristic".
BehaviourName: The behaviour name in use (agents with the same name share a brain).
AddReward: Adds `r` to this step's reward: positive for good, negative for bad.
@r: The reward.
SetReward: Replaces this step's reward with `r`.

## Example: Rolling to a target
file: Scripts/Roller.cs
The classic first agent: it sees where the target is and learns to push itself there.
```csharp
using Elintria;
using Elintria.ML;

public class Roller : Agent
{
    public GameObject target;
    Rigidbody rb;

    public override int ContinuousActions => 2;

    public override void Start() { rb = GetComponent<Rigidbody>(); }

    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(0f, 0.5f, 0f);
        target.transform.position = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
    }

    public override void CollectObservations(Observations o)
    {
        o.Add(target.transform.position - transform.position);   // 3 numbers
        o.Add(rb.velocity.x);
        o.Add(rb.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers a)
    {
        rb.AddForce(new Vector3(a.continuous[0], 0f, a.continuous[1]) * 10f);
        float d = Vector3.Distance(transform.position, target.transform.position);
        if (d < 1f) { AddReward(1f); EndEpisode(); }
        else if (transform.position.y < -1f) { AddReward(-1f); EndEpisode(); }
        else AddReward(-0.001f);   // hurry up
    }

    public override void Heuristic(ActionBuffers a)
    {
        a.continuous[0] = Input.GetAxisHorizontal();
        a.continuous[1] = Input.GetAxisVertical();
    }
}
```

## Example: Discrete choices
file: Scripts/LaneRunner.cs
A runner that picks left, stay or right each decision, using one discrete branch of 3 choices.
```csharp
using Elintria;
using Elintria.ML;

public class LaneRunner : Agent
{
    public GameObject obstacle;
    int lane;

    public override int ContinuousActions => 0;
    public override int[] DiscreteBranches => new[] { 3 };   // 0 left, 1 stay, 2 right

    public override void OnEpisodeBegin() { lane = 0; obstacle.transform.position = new Vector3(Random.Range(-1, 2) * 2f, 0.5f, 20f); }

    public override void CollectObservations(Observations o)
    {
        o.AddOneHot(lane + 1, 3);
        o.Add(obstacle.transform.position.x / 2f);
        o.Add(obstacle.transform.position.z / 20f);
    }

    public override void OnActionReceived(ActionBuffers a)
    {
        lane = Mathf.Clamp(lane + a.discrete[0] - 1, -1, 1);
        transform.position = new Vector3(lane * 2f, 0.5f, 0f);
        obstacle.transform.position += Vector3.back * 0.5f;
        if (obstacle.transform.position.z < 0.5f)
        {
            bool hit = Mathf.Abs(obstacle.transform.position.x - transform.position.x) < 1f;
            AddReward(hit ? -1f : 1f);
            EndEpisode();
        }
    }
}
```

## Example: Using a trained brain
file: Scripts/BrainSwitch.cs
After training, set `brain` to the `.elbrain` file; `heuristicOnly` lets you take over for comparison.
```csharp
using Elintria;
using Elintria.ML;

public class BrainSwitch : ElintriaScript
{
    public GameObject agentObject;

    public override void Update()
    {
        Roller r = agentObject.GetComponent<Roller>();
        if (r == null) return;
        if (Input.GetKeyDown(KeyCode.B)) r.brain = "Assets/ML/Roller.elbrain";
        if (Input.GetKeyDown(KeyCode.H)) r.heuristicOnly = !r.heuristicOnly;
        ScreenGUI.Text(20, 20, "Mode: " + r.Mode + "  reward " + r.CumulativeReward.ToString("0.00") + "  episodes " + r.CompletedEpisodes);
    }
}
```

# Observations
summary: What an agent sees, written as numbers in CollectObservations.
see: Agent

Add numbers, bools, vectors and rotations in a fixed order. The order and count must stay the same every step. Scale values so they sit roughly between -10 and 10.

## Members
Count: How many numbers have been added so far.
Add: Adds a value: a float, int, bool (0 or 1), Vector2, Vector3 or Quaternion (its components).
@v: The value to add.
@q: A rotation (4 numbers).
AddOneHot: Adds `count` numbers, all 0 except a 1 at `index`. Use it for categories such as a lane, a weapon or a state.
@index: Which category is on.
@count: How many categories there are.

## Example: Normalised positions
file: Scripts/ArenaAgent.cs
```csharp
using Elintria;
using Elintria.ML;

public class ArenaAgent : Agent
{
    public GameObject enemy;
    public float arenaHalfSize = 10f;

    public override int ContinuousActions => 2;

    public override void CollectObservations(Observations o)
    {
        o.Add(transform.position / arenaHalfSize);                         // roughly -1..1
        o.Add((enemy.transform.position - transform.position) / arenaHalfSize);
        o.Add(transform.forward);
    }

    public override void OnActionReceived(ActionBuffers a)
    {
        transform.position += new Vector3(a.continuous[0], 0f, a.continuous[1]) * 0.1f;
    }
}
```

## Example: Rays as eyes
file: Scripts/RayEyes.cs
Several raycasts in a fan give the agent a sense of walls around it.
```csharp
using Elintria;
using Elintria.ML;

public class RayEyes : Agent
{
    public int rays = 7;
    public float range = 10f;

    public override int ContinuousActions => 2;

    public override void CollectObservations(Observations o)
    {
        for (int i = 0; i < rays; i++)
        {
            Vector3 dir = Quaternion.Euler(0f, -60f + 120f * i / (rays - 1), 0f) * transform.forward;
            bool hit = Physics.Raycast(transform.position, dir, out RaycastHit h, range);
            o.Add(hit ? h.distance / range : 1f);
        }
    }

    public override void OnActionReceived(ActionBuffers a)
    {
        transform.Rotate(0f, a.continuous[0] * 5f, 0f);
        transform.position += transform.forward * Mathf.Max(0f, a.continuous[1]) * 0.1f;
    }
}
```

## Example: States as one-hot and bools
file: Scripts/StateAgent.cs
```csharp
using Elintria;
using Elintria.ML;

public class StateAgent : Agent
{
    public int weapon;          // 0 sword, 1 bow, 2 staff
    public bool hasKey;
    public float health = 1f;

    public override int[] DiscreteBranches => new[] { 4 };
    public override int ContinuousActions => 0;

    public override void CollectObservations(Observations o)
    {
        o.AddOneHot(weapon, 3);
        o.Add(hasKey);
        o.Add(health);
        Debug.Assert(o.Count == 5);
    }

    public override void OnActionReceived(ActionBuffers a) { weapon = a.discrete[0] % 3; }
}
```

# ActionBuffers
summary: What an agent decided this step: continuous values in -1..1 and one choice per discrete branch.
see: Agent

## Members
continuous: One value per continuous action, each -1..1.
discrete: One chosen index per discrete branch.
ActionBuffers: Makes buffers for `cont` continuous actions and `branches` discrete branches.

## Example: Steering and throttle
file: Scripts/KartAgent.cs
```csharp
using Elintria;
using Elintria.ML;

public class KartAgent : Agent
{
    public override int ContinuousActions => 2;

    public override void OnActionReceived(ActionBuffers a)
    {
        float steer = a.continuous[0], throttle = a.continuous[1];
        transform.Rotate(0f, steer * 3f, 0f);
        transform.position += transform.forward * throttle * 0.2f;
    }

    public override void CollectObservations(Observations o) { o.Add(transform.forward); }
}
```

## Example: Two discrete branches
file: Scripts/TankAgent.cs
```csharp
using Elintria;
using Elintria.ML;

public class TankAgent : Agent
{
    public override int ContinuousActions => 0;
    public override int[] DiscreteBranches => new[] { 3, 2 };   // turn: left/none/right, fire: no/yes

    public override void OnActionReceived(ActionBuffers a)
    {
        transform.Rotate(0f, (a.discrete[0] - 1) * 4f, 0f);
        if (a.discrete[1] == 1) Debug.Log("Bang");
    }

    public override void CollectObservations(Observations o) { o.Add(transform.forward); }

    public override void Heuristic(ActionBuffers a)
    {
        a.discrete[0] = Input.GetKey(KeyCode.A) ? 0 : Input.GetKey(KeyCode.D) ? 2 : 1;
        a.discrete[1] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}
```

## Example: Asking a brain directly
file: Scripts/BrainQuery.cs
You can make your own buffers and ask a [[Brain]] what it would do.
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.ML;

public class BrainQuery : ElintriaScript
{
    public override void Start()
    {
        Brain b = Brain.Load("Assets/ML/Roller.elbrain");
        if (b == null) return;
        var act = new ActionBuffers(b.continuous, b.branches.Length);
        b.Act(new List<float> { 1f, 0f, 2f, 0f, 0f }, act);
        Debug.Log("It would push " + act.continuous[0] + ", " + act.continuous[1]);
    }
}
```

# Brain
summary: A trained policy loaded from an .elbrain file: what an agent does with what it sees.
see: Agent, ActionBuffers

Training writes a `Brain` to `Assets/ML/<behaviour>.elbrain`. Agents use one automatically through their `brain` field. You can also load one yourself to inspect it, or ask it for actions directly.

## Members
behaviour: The behaviour name it was trained for.
observations: How many numbers it expects to see.
continuous: How many continuous actions it outputs.
branches: The size of each discrete branch.
steps: How many training steps it has had.
meanReward: The average episode reward at the end of training.
Load: The brain at a project path (cached), or null.
Forget: Clears the cache, so the next Load reads the files again.
Act: Fills `act` with the brain's best action for `obs` (without exploring).
ToJson: The brain as JSON text.
Parse: A brain from JSON text.

## Example: Showing training results
file: Scripts/BrainInfo.cs
```csharp
using Elintria;
using Elintria.ML;

public class BrainInfo : ElintriaScript
{
    public string brainPath = "Assets/ML/Roller.elbrain";

    public override void Start()
    {
        Brain b = Brain.Load(brainPath);
        if (b == null) { Debug.Log("Not trained yet"); return; }
        Debug.Log(b.behaviour + ": " + b.steps + " steps, mean reward " + b.meanReward.ToString("0.00") +
                  ", sees " + b.observations + " numbers, " + b.continuous + " continuous actions");
    }
}
```

## Example: Picking a difficulty by brain
file: Scripts/DifficultyBrains.cs
Train the same behaviour for different lengths and keep each brain as a difficulty level.
```csharp
using Elintria;
using Elintria.ML;

public class DifficultyBrains : ElintriaScript
{
    public int difficulty = 1;   // 0 easy, 1 normal, 2 hard
    public GameObject enemy;

    public override void Start()
    {
        string[] files = { "Assets/ML/Enemy_easy.elbrain", "Assets/ML/Enemy.elbrain", "Assets/ML/Enemy_hard.elbrain" };
        Agent a = enemy.GetComponent<Agent>();
        if (a != null) a.brain = files[Mathf.Clamp(difficulty, 0, 2)];
    }
}
```

## Example: Reloading after retraining
file: Scripts/BrainReload.cs
```csharp
using Elintria;
using Elintria.ML;

public class BrainReload : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6)) { Brain.Forget(); Debug.Log("Brains will be read again"); }
    }
}
```

# Academy
summary: Tells a script whether this copy of the game is being trained.
see: Agent

During training, hidden copies of your game run and talk to the trainer. `Academy.Training` is true in those copies, so you can skip things that only matter to a human player: music, menus, cutscenes and slow effects.

## Members
Training: True when this copy of the game is a training copy.

## Example: Skipping the intro while training
file: Scripts/Intro.cs
```csharp
using Elintria;
using Elintria.ML;

public class Intro : ElintriaScript
{
    public GameObject introCutscene;

    public override void Start()
    {
        if (Academy.Training && introCutscene != null) introCutscene.SetActive(false);
    }
}
```

## Example: Faster episodes in training
file: Scripts/TrainingSpeed.cs
```csharp
using Elintria;
using Elintria.ML;

public class TrainingSpeed : ElintriaScript
{
    public float respawnDelay = 3f;

    public float Delay() => Academy.Training ? 0f : respawnDelay;
}
```

## Example: Muting training copies
file: Scripts/QuietTraining.cs
```csharp
using Elintria;
using Elintria.ML;

public class QuietTraining : ElintriaScript
{
    public override void Start()
    {
        if (Academy.Training) Audio.masterVolume = 0f;
    }
}
```

# NetworkManager
summary: Starts and stops multiplayer sessions, spawns shared objects and reports who is connected.
see: NetworkManagerComponent, NetworkBehaviour, NetworkObject, INetTransport

`NetworkManager` runs Elintria's built-in netcode. One machine **hosts** (or runs a dedicated **server**) and others join as **clients**. The host is authoritative: it runs the game and sends state to everyone. Clients send their input and predict their own movement.

Usually a [[NetworkManagerComponent]] in the scene starts things from its settings and the command line. From code you can start a host or client, spawn objects for everyone from registered templates, and react to players joining and leaving.

## Members
RegisterTemplate: Registers a scene object (left inactive) that Spawn may copy, by its name. Register it on every machine.
Mode: Offline, Host, Server or Client.
IsRunning: True while a session is running.
IsServer: True on the host or a dedicated server: the machine that runs the game.
IsHost: True on a host (a server that also plays).
LocalClientId: This machine's peer id (1 for the host).
Transport: The transport the session uses.
Config: The settings the session started with.
TickRate: Network ticks a second.
TickInterval: Seconds between ticks.
Tick: The current tick number.
InterpolationDelay: How far in the past remote objects are drawn, in seconds, so movement is smooth.
PlayerTemplate: The name of the scene object copied for each player who joins.
LastError: Why the last start failed, or "".
OnClientConnected: Raised on the server when a client joins, with its id.
OnClientDisconnected: Raised on the server when a client leaves, with its id.
OnStarted: Raised when the session starts.
OnStopped: Raised when the session stops, with the reason.
ConnectedClients: The ids of everyone connected.
Objects: Every shared object, by network id.
NameOf: The player name of `client`.
Log: Writes to the network log.
@m: The message.
MakeTransport: Makes a transport by name: "lan", "relay" or "steam".
@kind: "lan", "relay" or "steam".
StartHost: Starts hosting: this machine runs the game and plays too.
@t: The transport to use.
StartClient: Joins the session described by `c`.
Stop: Ends the session.
Despawn: Server: removes a shared object from everyone.
Update: Runs the network loop; the NetworkManagerComponent calls it every frame.
@pos: Where to spawn it.
@rot: Its rotation.

## Example: Hosting and joining from a menu
file: Scripts/NetMenu.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class NetMenu : ElintriaScript
{
    string address = "127.0.0.1";

    public override void Update()
    {
        if (NetworkManager.IsRunning)
        {
            ScreenGUI.Text(20, 20, NetworkManager.Mode + "  players " + NetworkManager.ConnectedClients.Count + "  ping " + NetworkManager.RttMs.ToString("0") + " ms");
            if (ScreenGUI.Button(new Rect(20, 50, 140, 36), "Leave")) NetworkManager.Stop("left");
            return;
        }
        var cfg = new NetworkConfig { address = address, port = 7777, maxPlayers = 4, name = "Player" };
        if (ScreenGUI.Button(new Rect(20, 50, 140, 36), "Host"))
            NetworkManager.StartHost(NetworkManager.MakeTransport("lan"), cfg);
        if (ScreenGUI.Button(new Rect(20, 92, 140, 36), "Join " + address))
            if (!NetworkManager.StartClient(NetworkManager.MakeTransport("lan"), cfg)) Debug.LogWarning(NetworkManager.LastError);
    }
}
```

## Example: Spawning shared objects on the server
file: Scripts/CrateDropper.cs
Templates are inactive scene objects with a [[NetworkObject]]. Only the server spawns; everyone sees the copy.
```csharp
using Elintria;
using Elintria.Netcode;

public class CrateDropper : ElintriaScript
{
    public GameObject crateTemplate;   // inactive, with a NetworkObject

    public override void Start() { NetworkManager.RegisterTemplate(crateTemplate); }

    public override void Update()
    {
        if (!NetworkManager.IsServer || !Input.GetKeyDown(KeyCode.C)) return;
        Vector3 at = new Vector3(Random.Range(-5f, 5f), 10f, Random.Range(-5f, 5f));
        NetworkObject crate = NetworkManager.Spawn(crateTemplate.name, at, Quaternion.identity);
        Debug.Log("Spawned crate #" + crate.NetId);
    }
}
```

## Example: Join and leave messages
file: Scripts/JoinFeed.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class JoinFeed : ElintriaScript
{
    public override void Start()
    {
        NetworkManager.OnClientConnected += OnJoin;
        NetworkManager.OnClientDisconnected += OnLeave;
        NetworkManager.OnStopped += why => Debug.Log("Session ended: " + why);
    }

    void OnJoin(int id) { Debug.Log(NetworkManager.NameOf(id) + " joined"); }
    void OnLeave(int id) { Debug.Log("Player " + id + " left"); }

    public override void OnDestroy()
    {
        NetworkManager.OnClientConnected -= OnJoin;
        NetworkManager.OnClientDisconnected -= OnLeave;
    }
}
```

# NetworkManagerComponent
summary: The scene's network settings: transport, address, port, player template, and automatic start.
see: NetworkManager

Put one in a multiplayer scene. It holds the settings, reads the command line (`-host`, `-server`, `-connect ADDRESS`, `-port N`, `-relay URL -room CODE`, `-steam`, `-name NAME`), and runs [[NetworkManager.Update]] every frame. `autoStart` can start a host, server or client by itself; leave it empty to start from your menu.

## Members
transport: "lan", "relay" or "steam".
address: The host's address to join.
port: The port to host on or join.
maxPlayers: The most players allowed.
tickRate: Network ticks a second.
playerTemplate: The scene object copied for each player who joins (it needs a NetworkObject).
relayUrl: The relay server's address (for "relay").
roomCode: The relay room to host or join.
playerName: This player's name.
autoStart: "" (your scripts decide), "host", "server" or "client".
MakeConfig: A NetworkConfig filled from these settings.
StartHost: Starts hosting with these settings.
StartServer: Starts a dedicated server with these settings.
StartClient: Joins `addr` with these settings.
@addr: The host's address.

## Example: Buttons using the component's settings
file: Scripts/SimpleLobby.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class SimpleLobby : ElintriaScript
{
    public GameObject networkManagerObject;

    public override void Update()
    {
        if (NetworkManager.IsRunning) return;
        var net = networkManagerObject.GetComponent<NetworkManagerComponent>();
        if (ScreenGUI.Button(new Rect(40, 40, 160, 40), "Host")) net.StartHost();
        if (ScreenGUI.Button(new Rect(40, 90, 160, 40), "Join")) net.StartClient(net.address);
    }
}
```

## Example: Hosting over a relay room
file: Scripts/RelayHost.cs
No port forwarding: everyone connects to the relay with the same room code.
```csharp
using Elintria;
using Elintria.Netcode;

public class RelayHost : ElintriaScript
{
    public override void Start()
    {
        var net = GetComponent<NetworkManagerComponent>();
        net.transport = "relay";
        net.roomCode = "ROOM" + Random.Range(1000, 9999);
        Debug.Log("Tell your friends the room code: " + net.roomCode);
        net.StartHost();
    }
}
```

## Example: A headless dedicated server
file: Scripts/ServerBoot.cs
Run `Game.exe -server` and set `autoStart` to "server", or start it from code:
```csharp
using Elintria;
using Elintria.Netcode;

public class ServerBoot : ElintriaScript
{
    public override void Start()
    {
        bool dedicated = System.Environment.CommandLine.Contains("-server");
        if (dedicated && !NetworkManager.IsRunning)
            GetComponent<NetworkManagerComponent>().StartServer();
    }
}
```

# NetworkObject
summary: Marks a GameObject as shared over the network, with an id and an owner.
see: NetworkBehaviour, NetworkManager

Every object that exists on all machines needs a `NetworkObject`. It is given a network id when spawned and has an **owner**: the peer allowed to drive it (1 is the host). Its [[NetworkBehaviour]] scripts can then sync variables and call RPCs.

## Members
NetId: The object's network id, the same on every machine.
OwnerId: The peer that owns it (1 = the host).
IsOwner: True on the machine that owns it.
IsSpawned: True once it has been spawned on the network.

## Example: Handing ownership to a player
file: Scripts/PickupOwnership.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class PickupOwnership : NetworkBehaviour
{
    public override void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        NetworkObject player = other.gameObject.GetComponent<NetworkObject>();
        if (player != null) NetworkManager.SetOwner(NetworkObject, player.OwnerId);   // now that player carries it
    }
}
```

## Example: Coloring your own objects
file: Scripts/MineMarker.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class MineMarker : ElintriaScript
{
    public override void Update()
    {
        NetworkObject n = GetComponent<NetworkObject>();
        if (n != null && n.IsSpawned) ScreenGUI.Text(20, 20, "Object #" + n.NetId + (n.IsOwner ? " (yours)" : " owned by " + n.OwnerId));
    }
}
```

## Example: Despawning on the server
file: Scripts/Expire.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class Expire : NetworkBehaviour
{
    public float lifetime = 20f;
    float born;

    public override void Start() { born = Time.time; }

    public override void Update()
    {
        if (IsServer && Time.time - born > lifetime) NetworkManager.Despawn(NetworkObject);
    }
}
```

# NetworkBehaviour
summary: The base for multiplayer scripts: know your role, sync variables and call RPCs.
see: NetworkObject, NetworkVariable, ServerRpcAttribute, ClientRpcAttribute

Derive gameplay scripts on shared objects from `NetworkBehaviour` instead of [[ElintriaScript]]. It tells you where the code is running (`IsServer`, `IsClient`, `IsOwner`, `IsLocalPlayer`). Public [[NetworkVariable]] fields sync from the server to everyone. RPCs run methods across the network: `CallServer` runs a `[ServerRpc]` on the server, and `CallClients` runs a `[ClientRpc]` everywhere.

## Members
NetworkObject: The NetworkObject on this object.
IsServer: True on the host or dedicated server.
IsClient: True on machines that play (the host and clients).
IsHost: True on the host.
IsOwner: True on the machine that owns this object.
OwnerId: The owner's peer id.
CallServer: Asks the server to run the `[ServerRpc]` method `method` with `args` (on the host it runs at once).
@method: The RPC method's name; use `nameof(MyRpc)`.
CallClients: Server only: runs the `[ClientRpc]` method `method` on every client and the host.
CallClient: Server only: runs the `[ClientRpc]` method `method` on one client.

## Example: Shooting with a server RPC
file: Scripts/NetShooter.cs
The owner asks the server to shoot; the server checks and applies damage, then tells everyone to show the effect.
```csharp
using Elintria;
using Elintria.Netcode;

public class NetShooter : NetworkBehaviour
{
    public override void Update()
    {
        if (IsOwner && Input.GetMouseButtonDown(0))
            CallServer(nameof(Shoot), transform.position, transform.forward);
    }

    [ServerRpc] void Shoot(Vector3 from, Vector3 dir)
    {
        if (Physics.Raycast(from, dir, out RaycastHit hit, 100f))
        {
            hit.gameObject.SendMessage("ApplyDamage", 20f);
            CallClients(nameof(ShowHit), hit.point);
        }
    }

    [ClientRpc] void ShowHit(Vector3 at) { Debug.DrawRay(at, Vector3.up, Color.red, 0.5f); }
}
```

## Example: Owner-only input
file: Scripts/NetMover.cs
Only the owner reads the keyboard; everyone else sees it through a [[NetworkTransform]].
```csharp
using Elintria;
using Elintria.Netcode;

public class NetMover : NetworkBehaviour
{
    public float speed = 5f;

    public override void Update()
    {
        if (!IsOwner) return;
        Vector3 move = new Vector3(Input.GetAxisHorizontal(), 0f, Input.GetAxisVertical());
        transform.position += move * speed * Time.deltaTime;
    }
}
```

## Example: A private message to one client
file: Scripts/Whisper.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class Whisper : NetworkBehaviour
{
    public void TellOwner(string text)
    {
        if (IsServer) CallClient(OwnerId, nameof(ShowMessage), text);
    }

    [ClientRpc] void ShowMessage(string text) { if (IsLocalPlayer) Debug.Log("Server says: " + text); }
}
```

# NetworkVariable
summary: A value the server owns and every client sees, with a change event.
see: NetworkBehaviour

Declare one as a public field on a [[NetworkBehaviour]]. The server sets `Value`; clients receive it and raise `OnValueChanged(old, new)`. It converts to its value implicitly, so `if (health > 0)` works.

## Members
OnValueChanged: Raised on every machine when the value changes, with the old and new values.
NetworkVariable: Makes a variable, optionally with a starting value.
@initial: The starting value.
Value: The current value. Set it on the server only.
operator T: Reads the value directly.

## Example: Synced health with a change event
file: Scripts/NetHealth.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class NetHealth : NetworkBehaviour
{
    public NetworkVariable<float> health = new NetworkVariable<float>(100f);

    public override void Start()
    {
        health.OnValueChanged += (oldValue, newValue) =>
        {
            if (newValue < oldValue) Debug.Log(name + " took " + (oldValue - newValue) + " damage");
        };
    }

    public void ApplyDamage(float amount)
    {
        if (IsServer) health.Value = Mathf.Max(0f, health.Value - amount);
    }
}
```

## Example: A shared match timer
file: Scripts/MatchTimer.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class MatchTimer : NetworkBehaviour
{
    public NetworkVariable<int> secondsLeft = new NetworkVariable<int>(300);
    float acc;

    public override void Update()
    {
        if (IsServer && secondsLeft > 0)
        {
            acc += Time.deltaTime;
            if (acc >= 1f) { acc -= 1f; secondsLeft.Value -= 1; }
        }
        ScreenGUI.Text(ScreenGUI.screenWidth / 2 - 30, 10, (secondsLeft / 60) + ":" + (secondsLeft % 60).ToString("00"));
    }
}
```

## Example: Team scores
file: Scripts/TeamScore.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class TeamScore : NetworkBehaviour
{
    public NetworkVariable<int> red = new NetworkVariable<int>();
    public NetworkVariable<int> blue = new NetworkVariable<int>();

    public void Score(bool redTeam)
    {
        if (!IsServer) return;
        if (redTeam) red.Value++; else blue.Value++;
    }

    public override void Update() { ScreenGUI.Text(20, 20, "Red " + red + " - " + blue + " Blue"); }
}
```

# PredictedBehaviour
summary: Client-side prediction: the owner moves at once on its own input, and the server corrects it.
see: NetworkBehaviour, NetworkTransform

For responsive player movement, derive from `PredictedBehaviour<TInput, TState>` and describe your movement as pure functions:

- `GatherInput`: read this frame's input into a `TInput` struct.
- `Simulate`: apply one input for `dt` seconds. It runs on both the owner and the server, so keep it deterministic.
- `CaptureState` and `ApplyState`: turn the object's state into a `TState` struct and back.

The owner predicts immediately. When the server's state disagrees, it rewinds to the server's state and replays the inputs that were not answered yet. Other players see a smooth interpolation (override `Interpolate` to blend states).

## Members
GatherInput: Reads this frame's input (the owner only).
Simulate: Moves the object by one input over `dt` seconds. Must give the same result on every machine.
CaptureState: The object's state as a struct.
ApplyState: Puts the object into `state`.
Interpolate: Blends two states for remote players (default: snap at halfway).
@a: The earlier state.
@b: The later state.
@t: How far between them, 0..1.
Mispredictions: How many times the server's state differed from the prediction.
PendingInputs: Inputs sent but not answered yet.
LastAckedInput: The last input number the server confirmed.

## Example: A predicted character
file: Scripts/NetPlayer.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class NetPlayer : PredictedBehaviour<NetPlayer.In, NetPlayer.St>
{
    public struct In { public float x, z; public bool jump; }
    public struct St { public Vector3 pos; public float vy; }

    public float speed = 5f;
    float vy;

    protected override In GatherInput() =>
        new In { x = Input.GetAxisHorizontal(), z = Input.GetAxisVertical(), jump = Input.GetKey(KeyCode.Space) };

    protected override void Simulate(In i, float dt)
    {
        bool grounded = transform.position.y <= 0.001f;
        if (grounded && i.jump) vy = 6f;
        vy -= 20f * dt;
        Vector3 p = transform.position + new Vector3(i.x, 0f, i.z) * speed * dt + Vector3.up * vy * dt;
        if (p.y < 0f) { p.y = 0f; vy = 0f; }
        transform.position = p;
    }

    protected override St CaptureState() => new St { pos = transform.position, vy = vy };
    protected override void ApplyState(St s) { transform.position = s.pos; vy = s.vy; }
}
```

## Example: Smooth remote players
file: Scripts/SmoothRemote.cs
Override `Interpolate` so other players glide between the states they receive.
```csharp
using Elintria;
using Elintria.Netcode;

public class SmoothRemote : PredictedBehaviour<SmoothRemote.In, SmoothRemote.St>
{
    public struct In { public float turn, move; }
    public struct St { public Vector3 pos; public float yaw; }

    protected override In GatherInput() => new In { turn = Input.GetAxisHorizontal(), move = Input.GetAxisVertical() };

    protected override void Simulate(In i, float dt)
    {
        transform.Rotate(0f, i.turn * 120f * dt, 0f);
        transform.position += transform.forward * i.move * 4f * dt;
    }

    protected override St CaptureState() => new St { pos = transform.position, yaw = transform.eulerAngles.y };
    protected override void ApplyState(St s) { transform.position = s.pos; transform.rotation = Quaternion.Euler(0f, s.yaw, 0f); }

    protected override St Interpolate(St a, St b, float t) =>
        new St { pos = Vector3.Lerp(a.pos, b.pos, t), yaw = Mathf.LerpAngle(a.yaw, b.yaw, t) };
}
```

## Example: Watching prediction quality
file: Scripts/NetStats.cs
Test with simulated lag (`-netlag 120` or `NetworkManager.SimulatedLatencyMs`) and watch the counters.
```csharp
using Elintria;
using Elintria.Netcode;

public class NetStats : ElintriaScript
{
    public override void Start() { NetworkManager.SimulatedLatencyMs = 120; }

    public override void Update()
    {
        NetPlayer p = GetComponent<NetPlayer>();
        if (p != null && p.IsOwner)
            ScreenGUI.Text(20, 40, "pending " + p.PendingInputs + "  mispredictions " + p.Mispredictions + "  rtt " + NetworkManager.RttMs.ToString("0"));
    }
}
```

# NetworkTransform
summary: Syncs an object's position and rotation to everyone, from the server or from its owner.
see: NetworkBehaviour, PredictedBehaviour

Add it to a networked object that should move the same for everyone. By default the server is in charge. With `clientAuthority` on, the owner sends its own transform instead, which is simpler but trusts the client. `interpolate` smooths movement between updates.

## Members
clientAuthority: The owner sends its transform instead of the server (simple games; no protection against cheating).
interpolate: Smooth movement between updates.

## Example: Server-moved platform
file: Scripts/NetPlatform.cs
Only the server moves it; the NetworkTransform shows it to everyone.
```csharp
using Elintria;
using Elintria.Netcode;

public class NetPlatform : NetworkBehaviour
{
    Vector3 start;
    public override void Start() { start = transform.position; }

    public override void Update()
    {
        if (IsServer) transform.position = start + Vector3.right * Mathf.Sin(Time.time) * 4f;
    }
}
```

## Example: A client-authoritative pet
file: Scripts/PetFollow.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class PetFollow : NetworkBehaviour
{
    public override void Start()
    {
        NetworkTransform nt = GetComponent<NetworkTransform>();
        if (nt != null) nt.clientAuthority = true;   // the owner moves it
    }

    public override void Update()
    {
        if (!IsOwner || NetworkManager.LocalPlayer == null) return;
        Vector3 target = NetworkManager.LocalPlayer.transform.position + Vector3.back * 2f;
        transform.position = Vector3.Lerp(transform.position, target, 3f * Time.deltaTime);
    }
}
```

## Example: Turning interpolation off for teleports
file: Scripts/NetTeleport.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class NetTeleport : NetworkBehaviour
{
    public void TeleportTo(Vector3 p)
    {
        if (!IsServer) return;
        NetworkTransform nt = GetComponent<NetworkTransform>();
        nt.interpolate = false;
        transform.position = p;
        Invoke("SmoothAgain", 0.2f);
    }

    void SmoothAgain() { GetComponent<NetworkTransform>().interpolate = true; }
}
```

# ServerRpcAttribute
summary: Marks a method that clients can ask the server to run, with CallServer.
see: ClientRpcAttribute, NetworkBehaviour

By default, only the object's owner may call it. Set `RequireOwnership = false` for things anyone may use, such as a door or a shop.

## Members
RequireOwnership: When true (the default), only the object's owner may call it.

## Example: An owner-only RPC
file: Scripts/NetJump.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class NetJump : NetworkBehaviour
{
    public override void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.Space)) CallServer(nameof(Jump));
    }

    [ServerRpc] void Jump() { GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse); }
}
```

## Example: An RPC anyone can call
file: Scripts/NetDoor.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class NetDoor : NetworkBehaviour
{
    public NetworkVariable<bool> open = new NetworkVariable<bool>();

    public void Use() { CallServer(nameof(Toggle)); }

    [ServerRpc(RequireOwnership = false)] void Toggle() { open.Value = !open.Value; }

    public override void Update()
    {
        transform.localEulerAngles = new Vector3(0f, open ? 90f : 0f, 0f);
    }
}
```

## Example: Validating on the server
file: Scripts/NetShop.cs
The server checks before granting anything, so clients cannot cheat by calling the RPC.
```csharp
using Elintria;
using Elintria.Netcode;

public class NetShop : NetworkBehaviour
{
    public NetworkVariable<int> gold = new NetworkVariable<int>(100);

    [ServerRpc] void Buy(string item, int price)
    {
        if (gold.Value < price) return;   // refuse
        gold.Value -= price;
        CallClient(OwnerId, nameof(Bought), item);
    }

    [ClientRpc] void Bought(string item) { Debug.Log("You bought " + item); }
}
```

# ClientRpcAttribute
summary: Marks a method the server can run on clients, with CallClients or CallClient.
see: ServerRpcAttribute, NetworkBehaviour

Use client RPCs for things everyone should *see* but that don't need to be stored: effects, sounds and announcements. State that has to stay correct belongs in a [[NetworkVariable]].

## Example: An explosion everyone sees
file: Scripts/NetExplosion.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class NetExplosion : NetworkBehaviour
{
    public void Explode()
    {
        if (IsServer) CallClients(nameof(PlayEffect), transform.position);
    }

    [ClientRpc] void PlayEffect(Vector3 at) { Audio.PlayClipAtPoint("Audio/boom.wav", at); }
}
```

## Example: An announcement
file: Scripts/Announcer2.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class Announcer2 : NetworkBehaviour
{
    string banner;
    float until;

    public void Announce(string text) { if (IsServer) CallClients(nameof(Show), text); }

    [ClientRpc] void Show(string text) { banner = text; until = Time.time + 3f; }

    public override void Update()
    {
        if (Time.time < until) ScreenGUI.Label(new Rect(0, 80, ScreenGUI.screenWidth, 40), banner, null, Color.yellow, TextAnchor.MiddleCenter);
    }
}
```

## Example: A message to one player
file: Scripts/Kick.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class Kick : NetworkBehaviour
{
    public void Warn(int client) { CallClient(client, nameof(Warning), "Stop that!"); }
    [ClientRpc] void Warning(string text) { Debug.LogWarning(text); }
}
```

# NetworkMode
summary: The role this machine plays in a session.
see: NetworkManager

## Members
Offline: No session is running.
Host: This machine runs the game and plays.
Server: A dedicated server: runs the game, nobody plays on it.
Client: Joined someone else's game.

## Example: Showing the role
file: Scripts/RoleBadge.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class RoleBadge : ElintriaScript
{
    public override void Update() { ScreenGUI.Text(10, 10, "Mode: " + NetworkManager.Mode); }
}
```

## Example: Server-only logic
file: Scripts/ServerLogic.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class ServerLogic : ElintriaScript
{
    public override void Update()
    {
        NetworkMode m = NetworkManager.Mode;
        if (m == NetworkMode.Host || m == NetworkMode.Server) { /* spawn enemies, run AI */ }
    }
}
```

## Example: Hiding the HUD on a dedicated server
file: Scripts/NoHudOnServer.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class NoHudOnServer : ElintriaScript
{
    public override void Update()
    {
        if (NetworkManager.Mode == NetworkMode.Server) return;   // nothing to draw
        ScreenGUI.Text(20, 20, "HUD");
    }
}
```

# NetworkConfig
summary: The settings a session starts with: address, port, player limit, names, relay and Steam details.
see: NetworkManager, NetworkManagerComponent

## Members
address: The host's address (clients).
port: The port.
maxPlayers: The most players allowed (host).
name: This player's name.
gameName: The game's name shown to players finding it on the LAN.
relayUrl: The relay server's URL (relay transport).
room: The relay room code.
steamHost: The host's SteamID to join (Steam transport).

## Example: A LAN config
file: Scripts/LanConfig.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class LanConfig : ElintriaScript
{
    public static NetworkConfig Make(string playerName) =>
        new NetworkConfig { address = "192.168.1.20", port = 7777, maxPlayers = 8, name = playerName, gameName = playerName + "'s game" };
}
```

## Example: A relay config
file: Scripts/RelayConfig.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class RelayConfig : ElintriaScript
{
    public static NetworkConfig Make(string room) =>
        new NetworkConfig { relayUrl = "wss://my-relay.example.com", room = room, name = "Player" };
}
```

## Example: Joining a Steam friend
file: Scripts/SteamJoin.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class SteamJoin : ElintriaScript
{
    public void Join(ulong friendSteamId)
    {
        var cfg = new NetworkConfig { steamHost = friendSteamId, name = "Player" };
        NetworkManager.StartClient(NetworkManager.MakeTransport("steam"), cfg);
    }
}
```

# INetTransport
summary: The interface every network transport implements: start, send, poll and stop.
see: LanTransport, RelayTransport, SteamTransport

A transport moves text messages between peers. The host is peer 1 and clients are 2, 3 and so on. Elintria includes [[LanTransport]], [[RelayTransport]] and [[SteamTransport]]. You can write your own (WebRTC, a platform SDK) by implementing this interface and passing it to [[NetworkManager.StartHost]].

## Members
Name: The transport's name.
Unavailable: Why it cannot run here, or "" when it can.
StartHost: Starts listening as the host. Returns false on failure.
StartClient: Connects to the host described by `c`.
Stop: Disconnects and stops.
Send: Sends `data`. From the host, `peer` 0 means every client; from a client, it always goes to the host.
Poll: Takes the next event (connected, disconnected, data, error). Returns false when there is none.
LocalId: This machine's peer id once connected (host 1), else 0.

## Example: Choosing an available transport
file: Scripts/PickTransport.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class PickTransport : ElintriaScript
{
    public static INetTransport Best()
    {
        INetTransport steam = NetworkManager.MakeTransport("steam");
        if (steam.Unavailable == "") return steam;
        Debug.Log("Steam unavailable: " + steam.Unavailable);
        return NetworkManager.MakeTransport("lan");
    }
}
```

## Example: A loopback transport for tests
file: Scripts/LoopbackTransport.cs
```csharp
using System.Collections.Generic;
using Elintria.Netcode;

public class LoopbackTransport : INetTransport
{
    readonly Queue<NetEvent> inbox = new Queue<NetEvent>();
    public string Name => "loopback";
    public string Unavailable => "";
    public int LocalId { get; private set; }

    public bool StartHost(NetworkConfig c) { LocalId = 1; return true; }
    public bool StartClient(NetworkConfig c) { return false; }   // single player only
    public void Stop() { LocalId = 0; inbox.Clear(); }
    public void Send(int peer, string data) { inbox.Enqueue(new NetEvent { kind = NetEventKind.Data, peer = 1, data = data }); }
    public bool Poll(out NetEvent e) { if (inbox.Count > 0) { e = inbox.Dequeue(); return true; } e = default(NetEvent); return false; }
}
```

## Example: Low-level messaging without NetworkManager
file: Scripts/RawChat.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class RawChat : ElintriaScript
{
    INetTransport t;

    public override void Start()
    {
        t = new LanTransport();
        t.StartHost(new NetworkConfig { port = 7790, maxPlayers = 4 });
    }

    public override void Update()
    {
        while (t.Poll(out NetEvent e))
            if (e.kind == NetEventKind.Data) { Debug.Log("peer " + e.peer + ": " + e.data); t.Send(0, e.data); }
    }

    public override void OnDestroy() { t.Stop(); }
}
```

# LanTransport
summary: Networking on a local network: TCP to the host, and finding games by UDP broadcast.
see: INetTransport, DiscoveredGame

## Members
DiscoveryPort: The UDP port games answer discovery on (47900).
Name: "lan".
Unavailable: Why it cannot run, or "".
LocalId: This machine's peer id once connected.
StartHost: Starts hosting and answering discovery.
StartClient: Connects to `c.address`:`c.port`.
Discover: The games answering on this network within `ms` milliseconds.
Send: Sends text to a peer.
Poll: Takes the next event.
Stop: Disconnects.

## Example: A server browser
file: Scripts/LanBrowser.cs
```csharp
using System.Collections.Generic;
using Elintria;
using Elintria.Netcode;

public class LanBrowser : ElintriaScript
{
    List<DiscoveredGame> games = new List<DiscoveredGame>();

    public override void Update()
    {
        if (ScreenGUI.Button(new Rect(20, 20, 160, 36), "Refresh")) games = LanTransport.Discover(800);
        for (int i = 0; i < games.Count; i++)
        {
            DiscoveredGame g = games[i];
            if (ScreenGUI.Button(new Rect(20, 70 + i * 40, 360, 36), g.name + "  " + g.players + "/" + g.maxPlayers))
                NetworkManager.StartClient(new LanTransport(), new NetworkConfig { address = g.address, port = g.port, name = "Player" });
        }
    }
}
```

## Example: Hosting on LAN
file: Scripts/LanHost.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class LanHost : ElintriaScript
{
    public override void Start()
    {
        bool ok = NetworkManager.StartHost(new LanTransport(), new NetworkConfig { port = 7777, maxPlayers = 6, gameName = "Friday Night" });
        Debug.Log(ok ? "Hosting on LAN" : "Failed: " + NetworkManager.LastError);
    }
}
```

## Example: Joining by typed address
file: Scripts/LanJoin.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class LanJoin : ElintriaScript
{
    public string address = "192.168.1.50";

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            NetworkManager.StartClient(new LanTransport(), new NetworkConfig { address = address, port = 7777, name = "Guest" });
    }
}
```

# DiscoveredGame
summary: A game found on the local network by LanTransport.Discover.
see: LanTransport

## Members
name: The game's advertised name.
address: The host's address.
port: The port to join on.
players: How many players are in it.
maxPlayers: Its player limit.

## Example: Only games with room
file: Scripts/OpenGames.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class OpenGames : ElintriaScript
{
    public override void Start()
    {
        foreach (DiscoveredGame g in LanTransport.Discover())
            if (g.players < g.maxPlayers) Debug.Log("Open: " + g.name + " at " + g.address + ":" + g.port);
    }
}
```

## Example: Auto-join the first game
file: Scripts/QuickJoin.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class QuickJoin : ElintriaScript
{
    public override void Start()
    {
        var games = LanTransport.Discover(1000);
        if (games.Count == 0) { NetworkManager.StartHost(new LanTransport(), new NetworkConfig { port = 7777 }); return; }
        NetworkManager.StartClient(new LanTransport(), new NetworkConfig { address = games[0].address, port = games[0].port });
    }
}
```

## Example: Sorting by population
file: Scripts/BusiestGame.cs
```csharp
using System.Linq;
using Elintria;
using Elintria.Netcode;

public class BusiestGame : ElintriaScript
{
    public override void Start()
    {
        var best = LanTransport.Discover().OrderByDescending(g => g.players).FirstOrDefault();
        if (best.name != null) Debug.Log("Busiest: " + best.name);
    }
}
```

# RelayTransport
summary: Networking through a relay server room, with no port forwarding.
see: INetTransport, NetworkConfig

Everyone connects out to a relay (Elintria's `Tools/CollabServer`, for example hosted on Render) and meets in a room by code. It works through home routers without forwarding ports.

## Members
Name: "relay".
Unavailable: Why it cannot run, or "".
LocalId: This machine's peer id once connected.
StartHost: Opens the room as host.
StartClient: Joins the room.
Send: Sends text to a peer.
Poll: Takes the next event.
Stop: Leaves the room.

## Example: Host a room
file: Scripts/RelayRoomHost.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class RelayRoomHost : ElintriaScript
{
    public string relayUrl = "wss://your-relay.onrender.com";

    public override void Start()
    {
        string room = "R" + Random.Range(10000, 99999);
        NetworkManager.StartHost(new RelayTransport(), new NetworkConfig { relayUrl = relayUrl, room = room, name = "Host" });
        Debug.Log("Room code: " + room);
    }
}
```

## Example: Join a room by code
file: Scripts/RelayRoomJoin.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class RelayRoomJoin : ElintriaScript
{
    public string relayUrl = "wss://your-relay.onrender.com";
    public string room = "R12345";

    public override void Start()
    {
        if (!NetworkManager.StartClient(new RelayTransport(), new NetworkConfig { relayUrl = relayUrl, room = room, name = "Guest" }))
            Debug.LogWarning(NetworkManager.LastError);
    }
}
```

## Example: Fallback from LAN to relay
file: Scripts/AnyJoin.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class AnyJoin : ElintriaScript
{
    public string relayUrl = "wss://your-relay.onrender.com";

    public void Join(string room)
    {
        var lan = LanTransport.Discover(500);
        if (lan.Count > 0) NetworkManager.StartClient(new LanTransport(), new NetworkConfig { address = lan[0].address, port = lan[0].port });
        else NetworkManager.StartClient(new RelayTransport(), new NetworkConfig { relayUrl = relayUrl, room = room });
    }
}
```

# SteamTransport
summary: Networking through Steam: players join each other by SteamID.
see: INetTransport

Needs the Steamworks redistributable `steam_api64.dll` next to the game (and `steam_appid.txt` while developing), and Steam running. Without them the game still runs. The transport just reports why it is unavailable.

## Members
Name: "steam".
LocalId: This machine's peer id once connected.
Unavailable: Why Steam cannot be used, or "".
MySteamId: This player's SteamID, for friends to join by.
StartHost: Starts hosting over Steam.
StartClient: Joins the SteamID in `c.steamHost`.
Send: Sends text to a peer.
Poll: Takes the next event.
Stop: Disconnects.

## Example: Show your SteamID for friends
file: Scripts/SteamId.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class SteamId : ElintriaScript
{
    SteamTransport steam = new SteamTransport();

    public override void Update()
    {
        ScreenGUI.Text(20, 20, steam.Unavailable == "" ? "Your SteamID: " + steam.MySteamId : "Steam: " + steam.Unavailable);
    }
}
```

## Example: Hosting over Steam
file: Scripts/SteamHost.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class SteamHost : ElintriaScript
{
    public override void Start()
    {
        var steam = new SteamTransport();
        if (steam.Unavailable != "") { Debug.LogWarning(steam.Unavailable); return; }
        NetworkManager.StartHost(steam, new NetworkConfig { maxPlayers = 4, name = "Host" });
    }
}
```

## Example: Joining a friend's SteamID
file: Scripts/SteamFriendJoin.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class SteamFriendJoin : ElintriaScript
{
    public string friendId = "76561198000000000";

    public override void Start()
    {
        if (ulong.TryParse(friendId, out ulong id))
            NetworkManager.StartClient(new SteamTransport(), new NetworkConfig { steamHost = id });
    }
}
```

# NetEvent
summary: One thing that happened on a transport: a connection, a disconnection, a message or an error.
see: INetTransport, NetEventKind

## Members
kind: What happened.
peer: Which peer it was about (host 1, clients 2 and up).
data: The message text (for Data) or the error (for Error).

## Example: Handling every kind
file: Scripts/EventPump.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class EventPump : ElintriaScript
{
    public INetTransport transport;

    public override void Update()
    {
        if (transport == null) return;
        while (transport.Poll(out NetEvent e))
        {
            switch (e.kind)
            {
                case NetEventKind.Connected: Debug.Log("peer " + e.peer + " connected"); break;
                case NetEventKind.Disconnected: Debug.Log("peer " + e.peer + " left"); break;
                case NetEventKind.Data: Debug.Log("peer " + e.peer + ": " + e.data); break;
                case NetEventKind.Error: Debug.LogError(e.data); break;
            }
        }
    }
}
```

## Example: Making events in a custom transport
file: Scripts/FakeEvents.cs
```csharp
using Elintria.Netcode;

public static class FakeEvents
{
    public static NetEvent Joined(int peer) => new NetEvent { kind = NetEventKind.Connected, peer = peer };
    public static NetEvent Said(int peer, string text) => new NetEvent { kind = NetEventKind.Data, peer = peer, data = text };
}
```

## Example: Counting traffic
file: Scripts/Traffic.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class Traffic : ElintriaScript
{
    public int messages, bytes;

    public void Count(NetEvent e)
    {
        if (e.kind != NetEventKind.Data) return;
        messages++;
        bytes += e.data.Length;
    }
}
```

# NetEventKind
summary: What a NetEvent is: Connected, Disconnected, Data or Error.
see: NetEvent

## Members
Connected: A peer connected.
Disconnected: A peer left or the connection dropped.
Data: A message arrived.
Error: Something went wrong; the event's data says what.

## Example: Logging connections only
file: Scripts/ConnLog.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class ConnLog : ElintriaScript
{
    public static void Log(NetEvent e)
    {
        if (e.kind == NetEventKind.Connected || e.kind == NetEventKind.Disconnected)
            Debug.Log(e.kind + ": peer " + e.peer);
    }
}
```

## Example: Treating errors as disconnects
file: Scripts/ErrorPolicy.cs
```csharp
using Elintria.Netcode;

public static class ErrorPolicy
{
    public static bool Lost(NetEvent e) => e.kind == NetEventKind.Disconnected || e.kind == NetEventKind.Error;
}
```

## Example: A switch on the kind
file: Scripts/KindName.cs
```csharp
using Elintria.Netcode;

public static class KindName
{
    public static string Of(NetEventKind k)
    {
        switch (k)
        {
            case NetEventKind.Connected: return "joined";
            case NetEventKind.Disconnected: return "left";
            case NetEventKind.Data: return "message";
            default: return "error";
        }
    }
}
```

# NetWriter
summary: Writes values into a network message as text tokens.
see: NetReader

Netcode sends RPC arguments and state as text tokens joined with `|`. `NetWriter` builds a message (strings are escaped, so they may contain spaces and `|`), and [[NetReader]] reads it back in the same order: split the text on `'|'` and read the values in the order you wrote them. Structs are written field by field. Use them for your own custom messages.

## Members
Put: Adds a string, int or float.
@s: A string.
@v: A number.
Raw: Adds text as it is, without escaping.
Value: Adds any supported value (numbers, strings, vectors, structs field by field).
@t: The value's type.

## Example: Writing and reading a custom message
file: Scripts/CustomMessage.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class CustomMessage : ElintriaScript
{
    public override void Start()
    {
        string msg = new NetWriter().Put("chat").Put(3).Value(new Vector3(1f, 2f, 3f)).Put("hello there").ToString();
        NetReader r = new NetReader(msg.Split('|'), 0);
        Debug.Log(r.Str() + " from " + r.Int() + " at " + r.Value<Vector3>() + ": " + r.Str());
    }
}
```

## Example: Serialising a struct
file: Scripts/StructMessage.cs
```csharp
using Elintria;
using Elintria.Netcode;

public class StructMessage : ElintriaScript
{
    public struct Shot { public Vector3 from; public Vector3 dir; public float damage; }

    public static string Encode(Shot s) => new NetWriter().Value(s).ToString();
}
```

## Example: Chaining calls
file: Scripts/ScoreMessage.cs
Each call returns the writer, so a message reads in one line.
```csharp
using Elintria.Netcode;

public static class ScoreMessage
{
    public static string Make(int team, int score, float time) => new NetWriter().Put("score").Put(team).Put(score).Put(time).ToString();
}
```

# NetReader
summary: Reads values back out of a network message, in the order they were written.
see: NetWriter

## Members
NetReader: Starts reading `tokens` at index `start`.
@tokens: The message split into tokens.
@start: The first token to read.
More: True while there are tokens left.
Raw: The next token as it is.
Str: The next token as a string.
Int: The next token as an int.
Float: The next token as a float.
Value: The next value of type `t` (or `T`).
@t: The type to read.

## Example: Reading until the end
file: Scripts/ReadAll.cs
```csharp
using System.Collections.Generic;
using Elintria.Netcode;

public static class ReadAll
{
    public static List<int> Ints(string message)
    {
        var r = new NetReader(message.Split('|'), 0);
        var list = new List<int>();
        while (r.More) list.Add(r.Int());
        return list;
    }
}
```

## Example: Skipping a header token
file: Scripts/SkipHeader.cs
```csharp
using Elintria;
using Elintria.Netcode;

public static class SkipHeader
{
    public static Vector3 PositionOf(string message)
    {
        var r = new NetReader(message.Split('|'), 1);   // token 0 is the message type
        return r.Value<Vector3>();
    }
}
```

## Example: Decoding a score message
file: Scripts/ScoreDecode.cs
```csharp
using Elintria;
using Elintria.Netcode;

public static class ScoreDecode
{
    public static void Apply(string message)
    {
        var r = new NetReader(message.Split('|'), 0);
        if (r.Str() != "score") return;
        int team = r.Int(), score = r.Int();
        float time = r.Float();
        Debug.Log("Team " + team + " has " + score + " at " + time + " s");
    }
}
```

# NetworkSession
summary: The engine's built-in session layer: a server and clients exchanging text messages, with LAN discovery.
see: NetRole, DiscoveredServer, NetworkIdentity

`NetworkSession` is the engine's older, lower-level networking, which C++ scripts also use. There is one session per machine: start a server or connect a client, send and receive text messages, and find games on the LAN. For new C# games, the higher-level [[NetworkManager]] is usually easier.

## Members
IsAvailable: Whether networking can run here.
UnavailableReason: Why not, when it cannot.
StartServer: Starts a server on `port` for up to `maxPeers` players. A non-empty `advertiseName` lets players on the same network find it.
@port: The port to listen on.
@maxPeers: The most players.
@advertiseName: The name shown to players searching the LAN ("" to stay hidden).
ConnectClient: Connects to `host`:`port`.
@host: The server's address.
Stop: Ends the session.
Role: Offline, Server or Client.
IsRunning: Whether a session is running.
IsServer: Whether this machine is the server.
IsClient: Whether this machine is a client.
Port: The port in use.
LocalPeerId: This machine's peer id.
PeerCount: How many peers are connected.
DiscoverBegin: Starts listening for servers on `port`.
DiscoverEnd: Stops listening.
IsDiscovering: Whether discovery is running.
SendToAll: Sends text to everyone (from a client it goes via the server to everyone else).
@reliable: Whether delivery is guaranteed.
SendToPeer: Sends text to one peer.
@peer: The peer id.
PopReceived: Takes one received message. Only one script should read these each frame.
@text: Filled in with the message.
@from: Filled in with the sender's peer id.

## Example: A tiny chat
file: Scripts/SessionChat.cs
```csharp
using Elintria;

public class SessionChat : ElintriaScript
{
    public override void Update()
    {
        if (!NetworkSession.IsRunning)
        {
            if (Input.GetKeyDown(KeyCode.H)) NetworkSession.StartServer(7800, 8, "Chat Room");
            if (Input.GetKeyDown(KeyCode.J)) NetworkSession.ConnectClient("127.0.0.1", 7800);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Return)) NetworkSession.SendToAll("Hello from " + NetworkSession.LocalPeerId);
        while (NetworkSession.PopReceived(out string text, out uint from)) Debug.Log(from + ": " + text);
    }
}
```

## Example: Finding servers on the LAN
file: Scripts/SessionFinder.cs
```csharp
using System.Collections.Generic;
using Elintria;

public class SessionFinder : ElintriaScript
{
    readonly List<DiscoveredServer> found = new List<DiscoveredServer>();

    public override void Start() { NetworkSession.DiscoverBegin(7800); }

    public override void Update()
    {
        found.AddRange(NetworkSession.DiscoverPoll());   // only new ones each call
        for (int i = 0; i < found.Count; i++)
            if (ScreenGUI.Button(new Rect(20, 20 + i * 40, 300, 36), found[i].name))
            {
                NetworkSession.DiscoverEnd();
                NetworkSession.ConnectClient(found[i].address, found[i].port);
            }
    }
}
```

## Example: Status line
file: Scripts/SessionStatus.cs
```csharp
using Elintria;

public class SessionStatus : ElintriaScript
{
    public override void Update()
    {
        if (!NetworkSession.IsAvailable) { ScreenGUI.Text(20, 20, "No networking: " + NetworkSession.UnavailableReason); return; }
        ScreenGUI.Text(20, 20, NetworkSession.Role + "  ready " + NetworkSession.IsReady + "  peers " + NetworkSession.PeerCount +
                               "  ping " + NetworkSession.PingMs + "  address " + NetworkSession.LocalAddress);
    }
}
```

# NetRole
summary: This machine's role in the NetworkSession.
see: NetworkSession

## Members
Offline: No session.
Server: Running the session.
Client: Connected to a server.

## Example: Role-based logic
file: Scripts/RoleLogic.cs
```csharp
using Elintria;

public class RoleLogic : ElintriaScript
{
    public override void Update()
    {
        if (NetworkSession.Role == NetRole.Server) { /* authoritative work */ }
        else if (NetworkSession.Role == NetRole.Client) { /* send input */ }
    }
}
```

## Example: Displaying it
file: Scripts/RoleText.cs
```csharp
using Elintria;

public class RoleText : ElintriaScript
{
    public override void Update() { ScreenGUI.Text(20, 40, "Role: " + NetworkSession.Role); }
}
```

## Example: Offline check
file: Scripts/OfflineOnly.cs
```csharp
using Elintria;

public class OfflineOnly : ElintriaScript
{
    public override void Update()
    {
        if (NetworkSession.Role == NetRole.Offline && Input.GetKeyDown(KeyCode.P)) Time.timeScale = Time.timeScale == 0f ? 1f : 0f;   // pausing only offline
    }
}
```

# DiscoveredServer
summary: A NetworkSession server found on the local network.
see: NetworkSession

## Members
name: The server's advertised name.
address: Its address.
port: Its port.
players: How many players are in it.
maxPlayers: Its player limit.

## Example: Listing servers
file: Scripts/ServerList.cs
```csharp
using Elintria;

public class ServerList : ElintriaScript
{
    public override void Update()
    {
        foreach (DiscoveredServer s in NetworkSession.DiscoverPoll())
            Debug.Log(s.name + " " + s.players + "/" + s.maxPlayers + " at " + s.address + ":" + s.port);
    }
}
```

## Example: Skipping full servers
file: Scripts/NotFull.cs
```csharp
using Elintria;

public static class NotFull
{
    public static bool CanJoin(DiscoveredServer s) => s.players < s.maxPlayers;
}
```

## Example: Joining one
file: Scripts/JoinServer.cs
```csharp
using Elintria;

public static class JoinServer
{
    public static bool Join(DiscoveredServer s) => NetworkSession.ConnectClient(s.address, s.port);
}
```

# NetworkIdentity
summary: A scene object's identity in the NetworkSession: its id and which machine owns it.
see: NetworkSession

Placed on objects in the editor, so the same object has the same id on every machine that loaded the scene. Scripts check `isMine` before reading input to move it.

## Members
OwnerServer: The owner value meaning "the server" (0).
networkId: The object's id, the same on every machine.
owner: The peer that controls it.
isMine: True on the machine that decides where this object is.
isPlayerSlot: Whether it is a player slot.

## Example: Only move your own character
file: Scripts/MyCharacter.cs
```csharp
using Elintria;

public class MyCharacter : ElintriaScript
{
    public override void Update()
    {
        NetworkIdentity id = GetComponent<NetworkIdentity>();
        if (id != null && !id.isMine) return;
        transform.position += new Vector3(Input.GetAxisHorizontal(), 0f, Input.GetAxisVertical()) * 4f * Time.deltaTime;
    }
}
```

## Example: Server-owned props
file: Scripts/ServerProp.cs
```csharp
using Elintria;

public class ServerProp : ElintriaScript
{
    public override void Start()
    {
        NetworkIdentity id = GetComponent<NetworkIdentity>();
        if (id != null && NetworkSession.IsServer) id.owner = NetworkIdentity.OwnerServer;
    }
}
```

## Example: Labelling objects with ids
file: Scripts/IdLabel.cs
```csharp
using Elintria;

public class IdLabel : ElintriaScript
{
    public override void Update()
    {
        NetworkIdentity id = GetComponent<NetworkIdentity>();
        if (id != null) ScreenGUI.Text(20, 60, name + " #" + id.networkId + " owner " + id.owner + (id.isPlayerSlot ? " (player)" : ""));
    }
}
```
