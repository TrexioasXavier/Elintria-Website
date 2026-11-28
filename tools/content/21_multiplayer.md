# page: manual-multiplayer | Multiplayer: Two Players Across the World
summary: Set up a scene where two players, in different homes or different countries, see and move around each other, with the ENetworking package and an internet relay.

This tutorial makes a small scene that two people can play together over the internet. One player **hosts** and gets a six-letter **room code**. The other types that code and **joins**. Neither needs to touch their router.

It uses the **ENetworking** package: small components you add from **Add Component > Networking** and set up in the Inspector. No code is needed. The last section shows how to add your own networked scripts.

![The Connect Menu after hosting over the internet relay: the room code to read out to a friend.](../assets/img/screens/net_room_code.jpg)

## How the connection works

Two computers on different home networks usually cannot reach each other directly, because each sits behind its own router. ENetworking can connect them in three ways:

- **Internet relay** (this tutorial): both games dial out to a small relay server. The host opens a room, the friend joins it by its code, and the relay passes messages between them. Nobody forwards ports, and it works from anywhere.
- **LAN / direct IP**: the friend connects straight to the host's address. This is perfect on the same Wi-Fi. Over the internet, the host must forward the port (TCP 7777 by default) on their router.
- **Steam**: through Steam's own networking, joining by SteamID.

Whichever you pick, the host runs the game and every player's machine follows it. Each player moves their own character, and everyone sees everyone.

## Step 1: Put the relay online (once)

The relay is a tiny Node.js program that comes with the engine: `Tools/CollabServer/server.js`. It is the same relay the editor's live collaboration uses. You host it once and every game you make can share it. Render.com runs it for free:

- Make a new GitHub repository and copy in the two files from the engine's `Tools/CollabServer` folder: `server.js` and `package.json`.
- On [Render](https://render.com), choose **New > Web Service** and connect that repository.
- Set **Runtime** to Node, **Build Command** to `echo no build`, **Start Command** to `node server.js`, and pick the **Free** plan. Render sets the port itself.
- When it is live, open its address (`https://your-relay.onrender.com`) in a browser. It should say *Elintria collaboration relay - 0 session(s) open*.

Your relay address for the game is the same address with `wss://` in front instead of `https://`, for example `wss://your-relay.onrender.com`.

> note: A free Render service sleeps after a while with nobody connected. The first connection after that takes up to a minute while it wakes. If Join says it cannot reach the relay, wait a moment and try again. A paid plan, or any machine that runs Node 18 or newer, keeps it awake.

## Step 2: Install ENetworking

- Open **Window > Plugins**.
- Install **ENetworking** (Install from Folder, then pick `Packages/ENetworking` in the engine folder).
- Its components now appear under **Add Component > Networking**.

## Step 3: The Network object

- Create an empty object (**GameObject > Create Empty**) and name it `Network`.
- Add **Network Setup**, then set:
- **Start As**: `Manual`. The players choose Host or Join in the menu.
- **Connection**: `Internet Relay`.
- **Relay Url**: your relay address from step 1, such as `wss://your-relay.onrender.com`.
- **Random Room Code**: ticked. The host gets a fresh code each time.
- **Max Players**: `2` for this tutorial. Any number works.
- Add **Network Player Spawner**. You will give it the player in the next step.
- Add **Network Connect Menu**, which draws the Host / Join menu on screen. Untick **Show Connection Choice** if your game only ever uses the relay.
- Add **Network Status Display**, the small corner panel with the room code, the players and the ping.

![Network Setup and its neighbours in the Inspector. Start As, Connection, Order and Corner are dropdowns.](../assets/img/screens/net_inspector.jpg)

## Step 4: The player

Make one player in the scene. ENetworking copies it once for each person who joins.

- Create a **Capsule** (**GameObject > 3D Object > Capsule**) and name it `Player`.
- Add a **Character Controller**, so the player walks with gravity and bumps into walls.
- Add **Networked Object**. This makes it the same object on every machine, owned by one player.
- Add **Network Sync Transform** and set **Moved By** to `Owner`. Each player moves their own character, and the other screen follows smoothly.
- Add **Network Player Movement**: WASD or the arrow keys to walk, Shift to run, Space to jump. Its **Camera Mode** picks Third Person, Top Down or First Person. It only listens to the keyboard of the player who owns it, and the camera follows only your own character.
- Add **Network Name Tag**, so each player's name floats above the other player's head.
- Select `Network` again and drag `Player` from the Hierarchy into **Network Player Spawner > Player Template**.

> tip: The template is hidden when the game starts and only used as the pattern for the copies, so it does not matter where it stands in the scene.

Already have a character controller of your own, or one from the Standard package? Use it instead of Network Player Movement, and add **Network Owner Only** with your controller's class name in **Scripts**. It switches that script off on the other player's copy, so your keyboard only drives your character.

## Step 5: Spawn points, a floor and a camera

- Create two empty objects, `Spawn 1` and `Spawn 2`, a few metres apart, and add **Network Spawn Point** to each. Set **For Player** to `1` on the first (the host) and `2` on the second (the first player to join). Turn them to face the way players should start.
- Add a floor with a collider for the Character Controller to stand on, such as a Cube scaled to 40 × 1 × 40.
- Make sure the scene has a **Camera**. Network Player Movement moves it behind your character.
- Save the scene.

## Step 6: Try it on your own machine

Press **Play**. The Connect Menu appears:

![The Network Connect Menu: name, connection, address or room code, Host and Join.](../assets/img/screens/net_menu.jpg)

- Type a name and press **Host**. Your player appears at `Spawn 1`, and the menu shows the room code.
- To test the other side, build the game (**File > Build Settings**, add the scene, **Build**) and run `Game.exe` from the `0_BUILD_0` folder in your project, so you have two copies. In the second, type the code and press **Join**.
- Each window moves its own player with WASD and sees the other one move.

> tip: Network Setup's **Simulated Lag Ms** adds fake delay, for example `150`, so you can feel what playing with someone far away is like without leaving your desk.

## Step 7: Play with a friend anywhere

- Build the game and send your friend the build folder (zip `0_BUILD_0\YourGame`).
- You run `Game.exe`, type your name, press **Host**, and read out the six-letter room code.
- Your friend runs their copy, types their name and the code, and presses **Join**.

That is all: you are both in the same scene, each with your own character, and each sees the other's name above their head.

> warning: Both players must run the same build of the game. A copy built from a changed scene can disagree about which object is which.

## Other ways to connect

### On the same Wi-Fi

Set **Connection** to `LAN`. The host presses **Host**. The other player presses **Find LAN Games** and picks the game, or types the host's local address (such as `192.168.1.20`) and presses **Join**. A **LAN Game Finder** component does this automatically: it joins the first game on the network, or hosts one if there is none.

### Direct IP with port forwarding

Also with **Connection** set to `LAN`: the host forwards **Port** (TCP 7777 by default) on their router to their computer, and the friend joins with the host's public IP address. This needs no relay, but every host has to set up their router.

### A dedicated server

A server that stays up with no player of its own:

```text
Game.exe -server                                          on a LAN or forwarded port
Game.exe -server -relay wss://your-relay.onrender.com -room MYGAME   over the relay
```

Tick **Dedicated server launcher** in the Build window to get a `Start Dedicated Server.bat` beside the game. Players then join with the address, or with the room code you gave it.

### The command line

The command line overrides the Inspector, which is handy for testing and for shortcuts:

```text
Game.exe -host -name Ana
Game.exe -connect 203.0.113.7 -port 7777 -name Ben
Game.exe -relay wss://your-relay.onrender.com -join ABC234 -name Ben
-maxplayers 4    -netlag 150    -netlog net.txt
```

## When something goes wrong

- **"no game is open with room code ..."**: the code is wrong, or the host has closed the game. Codes avoid look-alike characters (no 0/O or 1/I).
- **"cannot reach the relay"**: check the **Relay Url** starts with `wss://`, and open the `https://` address in a browser to wake the relay.
- **Join does nothing on LAN**: allow the game through the Windows Firewall when it asks, and check both players use the same **Port**.
- **The other player does not move**: the player template needs **Network Sync Transform** with **Moved By** set to `Owner`.
- **Both keyboards move the same character**: use **Network Player Movement**, or add **Network Owner Only** for your own controller.

## Your own networked scripts

The components are built on [[NetworkManager]] and [[NetworkBehaviour]], and your scripts can use them too. A [[NetworkBehaviour]] on a networked object can tell whose it is (`IsOwner`), ask the host to do something (`[ServerRpc]`, called with `CallServer`), tell everyone (`[ClientRpc]`, called with `CallClients`), and share values ([[NetworkVariable]]):

```csharp
using Elintria;
using Elintria.Netcode;

public class Waver : NetworkBehaviour
{
    public NetworkVariable<int> waves = new NetworkVariable<int>();

    public override void Update()
    {
        // Only my own character listens to my keyboard.
        if (IsOwner && Input.GetKeyDown(KeyCode.E)) CallServer(nameof(Wave));
    }

    [ServerRpc] void Wave()
    {
        waves.Value += 1;                     // the host keeps the count; everyone sees it
        CallClients(nameof(ShowWave), NetworkManager.NameOf(OwnerId));
    }

    [ClientRpc] void ShowWave(string who)
    {
        Debug.Log(who + " waves! (" + waves.Value + ")");
    }
}
```

Put it on the player template next to the Networked Object. Press E in one window and both print the wave.
