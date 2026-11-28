"""Builds assets/data/api.js, the data behind the Scripting Documentation.

    python extract_api.py        # re-read the engine's C# sources (after API changes)
    python build_docs.py         # merge with tools/content/*.md and write assets/data/api.js

The extracted API supplies every type, member and signature; the hand-written
content files supply descriptions, parameter notes and examples. The build
reports anything that still lacks a description or has fewer than three examples.
"""
import os, re, sys, json, html, glob, datetime

HERE = os.path.dirname(os.path.abspath(__file__))
SITE = os.path.normpath(os.path.join(HERE, ".."))
MIN_EXAMPLES = 3

# ── What is documented, and where it sits in the sidebar ─────────────────────
EXCLUDE = {"Bootstrap", "EditorWindows", "BTDebug", "ScriptDebugger", "Installer", "Trainer", "TrainerConfig",
           "ComponentProviders", "NetworkVariableBase", "GUILayoutOption"}

SECTIONS = [
    ("manual", "Manual", "Start here: how scripts work in Elintria, from your first component to building a game."),
    ("engine", "Engine API", "The <code>Elintria</code> namespaces: the classes, structs and enums every script is written against."),
    ("standard", "Elintria Standard Package", "Ready-made gameplay components. Enable the <b>Standard</b> package in the Plugins window, then use <b>Add Component &gt; Elintria Standard</b>, or derive your own scripts from them."),
]

CATEGORIES = [
    # (section, title, description, [type names])
    ("manual", "Manual", "Guides to scripting in Elintria.", []),
    ("engine", "Scripting Core", "Scripts, objects, components and the services every game uses.",
     ["ElintriaScript", "GameObject", "Transform", "Component", "ComponentBase", "Behaviour", "ManagedComponent",
      "SerializeFieldAttribute", "HideInInspectorAttribute", "Debug", "Time", "Random", "Project", "Content", "Json",
      "SceneManager", "Frame", "HostCommands", "IComponentProvider", "Space", "PrimitiveType", "FindObjectsSortMode", "FindObjectsInactive"]),
    ("engine", "Coroutines", "Code that runs over several frames.",
     ["Coroutine", "WaitForSeconds", "WaitForSecondsRealtime", "WaitForEndOfFrame", "WaitForFixedUpdate", "WaitUntil", "WaitWhile", "CustomYieldInstruction"]),
    ("engine", "Math", "Vectors, rotations, colours, shapes and maths helpers.",
     ["Vector2", "Vector3", "Vector4", "Quaternion", "Mathf", "Matrix4x4", "Color", "Color32", "Rect", "Bounds", "Ray", "Plane"]),
    ("engine", "Input", "Keyboard, mouse and the game window.",
     ["Input", "KeyCode", "Mouse", "GameWindow"]),
    ("engine", "Physics", "Bodies, colliders (convex and compound too), joints, ropes, soft bodies, the character controller and physics queries.",
     ["Physics", "Rigidbody", "Collider", "BoxCollider", "SphereCollider", "MeshCollider", "CharacterController", "ControllerColliderHit",
      "CollisionFlags", "Collision", "ContactPoint", "RaycastHit", "ForceMode", "RigidbodyConstraints", "RigidbodyInterpolation",
      "CollisionDetectionMode", "QueryTriggerInteraction", "LayerMask", "ConvexCollider", "ConvexShape", "CompoundCollider",
      "Joint", "FixedJoint", "HingeJoint", "BallJoint", "SpringJoint", "SliderJoint", "JointLimits", "JointMotor", "JointSpring",
      "Rope", "SoftBody", "SoftBodyShape"]),
    ("engine", "Rendering", "Lights, meshes, textures, cameras, sprites, particles and graphics settings.",
     ["Light", "LightType", "RuntimeMesh", "ObjectLook", "SpriteRenderer", "ParticleEmitter", "ParticleShape", "ParticleRenderMode", "ParticleBlend",
      "GpuMesh", "GpuTexture", "MeshInstance", "MaterialFlags", "TextureFlags", "RenderCamera", "ScriptCamera", "RenderEnvironment",
      "RenderZones", "Overlay2D", "Lines3D", "Quality"]),
    ("engine", "Animation and Audio", "Animators, skinned models and sound.",
     ["Animator", "AnimatorStateInfo", "SkinnedModel", "AudioSource", "Audio", "AudioRolloffMode"]),
    ("engine", "User Interface", "Immediate-mode game UI, and the UI Designer's screens: driving them from scripts and the no-code bindings.",
     ["ScreenGUI", "GuiFont", "TextAnchor", "UIRuntime", "UIBridge"]),
    ("engine", "World", "Terrain, water, foliage, splines and tilemaps.",
     ["Terrain", "TerrainData", "TerrainCollider", "TerrainLayer", "Water", "Foliage", "Spline", "Tilemap"]),
    ("engine", "Navigation", "Nav mesh agents, run-time obstacles and links, crowds.",
     ["NavAgent", "NavMeshRuntime", "NavCrowd", "NavLinkKind"]),
    ("engine", "AI: Behaviour Trees", "Behaviour trees, blackboards, utility AI and tree nodes (Elintria.AI).",
     ["BehaviourTree", "BTInstance", "BehaviourTreeRunner", "BehaviourTreeExtensions", "Blackboard", "BTContext", "BTNode", "BTStatus",
      "BTActionAttribute", "BTConditionAttribute", "BTScoreAttribute", "Utility", "NodeState",
      "Selector", "Sequence", "Parallel", "RandomSelector", "UtilitySelector",
      "Decorator", "Inverter", "Succeeder", "Failer", "Repeat", "UntilFail", "Cooldown", "TimeLimit", "If",
      "ActionNode", "ConditionNode", "Wait", "SetValue", "LogNode", "SubTree"]),
    ("engine", "Machine Learning", "Agents that learn by reinforcement (Elintria.ML).",
     ["Agent", "Observations", "ActionBuffers", "Brain", "Academy"]),
    ("engine", "Netcode", "Host-authoritative multiplayer with prediction (Elintria.Netcode).",
     ["NetworkManager", "NetworkManagerComponent", "NetworkObject", "NetworkBehaviour", "NetworkTransform", "NetworkVariable",
      "PredictedBehaviour", "ServerRpcAttribute", "ClientRpcAttribute", "NetworkMode", "NetworkConfig", "INetTransport",
      "LanTransport", "RelayTransport", "SteamTransport", "NetEvent", "NetEventKind", "DiscoveredGame", "NetWriter", "NetReader"]),
    ("engine", "Network Session", "The engine's built-in session layer and the UI runtime it shares a file with.",
     ["NetworkSession", "NetRole", "DiscoveredServer", "NetworkIdentity"]),
    ("engine", "Procedural Generation", "Random numbers, noise, dungeons, WFC and L-systems (Elintria.Procedural).",
     ["Rng", "Noise", "Poisson", "Cell", "Room", "Grid", "Dungeon", "DungeonResult", "Wfc", "LSystem", "ProcBuild"]),
    ("engine", "Level Generator", "The editor's Level Generator from a running game: generate, draw, collide and fill levels.",
     ["LevelGenerator", "LevelGenSettings", "GeneratedLevel", "LevelMarker", "LevelGenUtil", "LevelLightAnimator"]),
    ("engine", "Editor Scripting", "Your own editor windows and menu items (scripts in Scripts/Editor).",
     ["EditorWindow", "EditorGUILayout", "EditorGUI", "GUILayout", "MenuItemAttribute", "MessageType", "EditorApplication", "EditorUtility", "AssetFiles"]),
    ("standard", "Gameplay Core", "Shared pieces every Standard component uses: events, input, health, damage, the camera, the screen.",
     ["StdEvent", "StdInput", "Health", "StdDamage", "StdCamera", "StdEvents", "StdCameraShake", "StdScreen", "StdTime", "StdToast", "StdBoxVisual", "StdLine", "StdExtensions"]),
    ("standard", "Characters", "First-person, third-person and top-down controllers, respawning, stealth, companions and mounts.",
     ["ThirdPersonController", "LockOnTarget", "FirstPersonController", "PlayerRespawn", "TopDownController", "StealthPlayer", "HidingSpot", "Companion", "Mount"]),
    ("standard", "Movement", "Dash, double jump, slide, mantle, wall run, grapple; swimming and climbing; stamina and needs.",
     ["StdMotor", "DashAbility", "DoubleJumpAbility", "SlideAbility", "MantleAbility", "WallRunAbility", "GrappleHook",
      "Swimmer", "WaterVolume", "Climber", "ClimbableSurface", "Stamina", "Needs", "HeatSource"]),
    ("standard", "Combat", "Enemies that see, hear, chase and attack; turrets; grenades, smoke, molotovs, flashbangs and decoys; damage types, status effects and hit feedback; crosshairs, recoil and cover.",
     ["BasicEnemy", "StdNoise", "NoiseEvent", "DamageType", "DamageResistance", "StatusEffects", "DamageNumbers", "HitFeedback", "Crosshair", "WeaponRecoil", "CoverSystem",
      "Turret", "ThrowableUser", "Throwable", "ThrowEffect", "StdThrowKinds"]),
    ("standard", "Saving", "Save slots: positions, component state and [Saveable] fields; save points and autosave.",
     ["SaveSystem", "SaveSlotInfo", "SaveableObject", "SaveableAttribute", "SavePoint", "AutoSave"]),
    ("standard", "Quests and Dialogue", "Quests with steps and rewards, a tracker, branching dialogue, and game-wide variables.",
     ["StdVars", "QuestGiver", "QuestLog", "Quest", "QuestStep", "QuestTracker", "DialogueNpc"]),
    ("standard", "Economy and Stats", "Crafting, money and shops, levels and stats, and equipment.",
     ["CraftingBench", "Recipe", "Wallet", "Vendor", "CharacterStats", "XpReward", "Equipment", "StdModifiers"]),
    ("standard", "HUD", "Bars, a compass, markers, and world-space health bars, names and icons.",
     ["StdHud", "HudMarker", "WorldHealthBar", "NameTag", "InteractIcon", "CompassBar"]),
    ("standard", "Vehicles", "An arcade car and the seat to get into it.",
     ["VehicleController", "VehicleEntry"]),
    ("standard", "Interaction", "Things the player uses with a key press: doors, levers, pickups.",
     ["Interactable", "Interactor", "Door", "Lever", "Pickup"]),
    ("standard", "Inventory", "Items, stacks, a bag with a hotbar, and its drag-and-drop UI.",
     ["Inventory", "InventoryUI", "ItemDefinition", "ItemDatabase", "ItemStack"]),
    ("standard", "Weapons", "Guns, projectiles and melee attacks.",
     ["Weapon", "HitscanWeapon", "ProjectileWeapon", "Projectile", "MeleeWeapon"]),
    ("standard", "Spawning", "Spawners and enemy waves.",
     ["Spawner", "WaveSpawner"]),
    ("standard", "Mechanisms", "Platforms, elevators, pressure plates and trigger zones.",
     ["MovingPlatform", "Elevator", "PressurePlate", "TriggerZone"]),
    ("standard", "Level", "Patrols, objectives, flickering lights, ladders, streaming, traps, puzzles, checkpoints, notes and a minimap - what the Level Generator wires up.",
     ["StdPatrol", "LevelObjective", "LightFlicker", "Ladder", "LevelStreamer", "LevelHazard", "LevelPuzzle", "LevelCheckpoint", "Readable", "LevelMinimap"]),
    ("standard", "Animation and IK", "Look-at, two-bone and foot IK, and ragdolls.",
     ["LookAtIK", "TwoBoneIK", "FootIK", "Ragdoll", "TwoBoneSolver", "HumanoidBones", "StdRot"]),
    ("standard", "Cloth and Destruction", "Cloth simulation and objects that break into pieces.",
     ["Cloth", "Destructible", "SliceMesh"]),
    ("standard", "Environment", "Floating objects, grass benders, spline followers, day and night, and weather.",
     ["Buoyancy", "GrassBender", "SplineFollower", "DayNightCycle", "Weather"]),
    ("standard", "World Interaction", "Grabbing and pushing, teleporters, level exits, loot, breakables, chests and traps.",
     ["PhysicsGrabber", "Pushable", "Teleporter", "LevelExit", "LootTable", "MoneyPickup", "Breakable", "LootChest",
      "SwingingBlade", "RollingBoulder", "CollapsingFloor", "ArrowTrap"]),
    ("standard", "Traversal", "Ziplines, rope swings, gliders, jetpacks, jump pads, boost rings, gravity and wind zones, conveyor belts and fall damage.",
     ["Zipline", "RopeSwing", "Glider", "Jetpack", "FallDamage", "JumpPad", "BoostRing", "GravityZone", "WindZone", "ConveyorBelt", "BallisticFlight"]),
    ("standard", "Building", "Build mode for the player: pieces on a grid, paid for with items or money, saved with the game.",
     ["BuildingPlacer", "BuildPiece"]),
    ("standard", "Farming", "Plots to plant, water and harvest, crops that grow with the game clock, and sprinklers.",
     ["FarmPlot", "CropInfo", "Sprinkler"]),
    ("standard", "Tower Defense", "A whole tower defense match: a path, build spots, towers that upgrade, waves of enemies, money and lives.",
     ["TdGame", "TdPath", "TdBuildSpot", "TdTower", "TdEnemy", "TdShot"]),
    ("standard", "Debug", "An in-game console with cheats and your own commands, and a frame-rate counter.",
     ["DebugConsole"]),
]

STANDARD_MENU = {"Animation.cs": "Animation", "ClothAndDestruction.cs": "Physics", "Environment.cs": "Environment",
                 "Interaction.cs": "Interaction", "Inventory.cs": "Inventory", "Mechanisms.cs": "Mechanisms",
                 "Spawner.cs": "Spawning", "StdCore.cs": "Gameplay", "ThirdPersonController.cs": "Characters", "Player.cs": "Characters",
                 "Movement.cs": "Movement", "Combat.cs": "Combat", "Saving.cs": "Saving", "Quests.cs": "Quests", "Economy.cs": "Economy",
                 "HUD.cs": "HUD", "WorldInteraction.cs": "World", "Companions.cs": "Characters", "TimeAndWeather.cs": "Environment",
                 "VehicleController.cs": "Vehicles", "Weapons.cs": "Weapons", "Traversal.cs": "Movement", "Ordnance.cs": "Combat",
                 "TowerDefense.cs": "Tower Defense", "Building.cs": "Building", "Farming.cs": "Farming", "DebugConsole.cs": "Debug"}

LIFECYCLE = {"Start", "Update", "FixedUpdate", "LateUpdate", "Awake", "OnDestroy", "OnEnable", "OnDisable",
             "OnTriggerEnter", "OnTriggerStay", "OnTriggerExit", "OnCollisionEnter", "OnCollisionStay", "OnCollisionExit",
             "OnControllerColliderHit"}

# ── Shared descriptions for members many types have ─────────────────────────
COMMON = {
    "gameObject": "The GameObject this {T} is attached to.",
    "transform": "The Transform of the object this {T} is on: its position, rotation, scale and place in the hierarchy.",
    "name": "The name of the object, as shown in the Hierarchy. Setting it renames the object.",
    "tag": "The object's tag, used to find and tell apart groups of objects (\"Player\", \"Enemy\").",
    "CompareTag": "True when the object's tag is `t`. Cheaper and clearer than comparing `tag` with `==`.",
    "GetComponent": "The first component of type `T` on the same object, or null when it has none. `T` can be an engine component (Rigidbody, Light...) or one of your scripts.",
    "GetComponents": "Every component of type `T` on the same object.",
    "GetComponentInChildren": "The first component of type `T` on this object or any object below it in the hierarchy. Pass `includeInactive` to also search switched-off children.",
    "GetComponentsInChildren": "Every component of type `T` on this object and all objects below it.",
    "GetComponentInParent": "The first component of type `T` on this object or the nearest object above it.",
    "GetComponentsInParent": "Every component of type `T` on this object and the objects above it.",
    "TryGetComponent": "Looks for a component of type `T` on the object. Returns true and fills `component` when one exists; otherwise returns false.",
    "AddComponent": "Adds a new component of type `T` to the object and returns it. Engine components start working at once; scripts get Awake and Start like any other.",
    "SendMessage": "Calls the method named `methodName` on every script on the object, passing `value` when given. Objects without such a method are skipped silently.",
    "BroadcastMessage": "Calls `methodName` on every script on this object and every object below it.",
    "ToString": "A readable form of the value, for logs and debugging.",
    "Equals": "True when `o` is the same value or refers to the same object.",
    "GetHashCode": "A hash for use as a dictionary key.",
    "operator bool": "Lets you write `if (thing)`: true while it exists and has not been destroyed.",
    "operator ==": "True when the two values are equal.",
    "operator !=": "True when the two values differ.",
    "IsAlive": "False once the object or component has been destroyed. Check it before using a reference you kept from an earlier frame.",
    "enabled": "Whether the component is switched on. A disabled script keeps its state but stops receiving Update calls.",
    "isActiveAndEnabled": "True when the component is enabled and its object is active in the hierarchy.",
    "Destroy": "Removes `obj` (a GameObject or component) from the scene at the end of the frame, or after `delay` seconds.",
    "DestroyImmediate": "Removes `obj` at once instead of at the end of the frame. Prefer Destroy during play.",
    "DontDestroyOnLoad": "Kept for Unity code. Elintria never unloads a scene from under an object, so there is nothing to protect.",
    "Instantiate": "Makes a copy of `original` (a GameObject, a component's object, or a plain object), optionally at `position` with `rotation` and under `parent`. Returns the copy.",
    "FindObjectOfType": "The first live object of type `T` in the scene (a script or an engine component), or null.",
    "FindObjectsOfType": "Every live object of type `T` in the scene.",
    "FindFirstObjectByType": "The first live object of type `T`, or null.",
    "FindAnyObjectByType": "Any one live object of type `T`, or null. Use it when you do not care which one.",
    "FindObjectsByType": "Every live object of type `T`, in the order given by `sortMode`.",
    "Get": "The {T} on the GameObject `go`, or null when it has none.",
    "id": "The engine handle of this {T}.",
    "this[int i]": "Reads or writes a component by index: 0 is x, 1 is y, and so on.",
}

PARAMS = {
    "go": "The GameObject to act on.", "gameObject": "The GameObject to act on.", "obj": "The object to act on.",
    "t": "The interpolation factor or value, usually 0..1.", "a": "The first value.", "b": "The second value.",
    "x": "The x coordinate.", "y": "The y coordinate.", "z": "The z coordinate.", "w": "The w component, or a width.",
    "position": "A position in world space.", "pos": "A position in world space.", "point": "A point in world space.", "p": "A point or position.",
    "rotation": "A rotation.", "rot": "A rotation.", "q": "A rotation.",
    "direction": "The direction to cast or move in; it does not need to be normalized.", "dir": "A direction.",
    "origin": "Where the ray or cast starts, in world space.", "maxDistance": "How far to look, in world units.",
    "hitInfo": "Filled in with what was hit: its point, normal, distance and collider.",
    "radius": "The radius, in world units.", "layerMask": "Which layers to consider (carried for Unity code; Elintria does not filter by layer yet).",
    "q": "Whether triggers count (carried for Unity code).", "name": "The name to use.", "value": "The value to set.",
    "target": "The target to act on.", "seconds": "A time in seconds.", "delay": "How long to wait first, in seconds.",
    "duration": "How long it lasts, in seconds (0: this frame only).", "color": "The colour to draw with.",
    "path": "A project-relative path such as \"Assets/Data/file.json\".", "key": "The name the value is stored under.",
    "def": "What to return when there is no value.", "message": "The message to log or show.", "methodName": "The name of the method to call.",
    "method": "The name of the method to call.", "speed": "A speed, in units a second.", "amount": "How much.",
    "min": "The smallest value allowed.", "max": "The largest value allowed.", "seed": "A seed; the same seed gives the same result.",
    "original": "The object to copy.", "parent": "The Transform the result is put under.", "index": "A zero-based index.",
    "i": "A zero-based index.", "count": "How many.", "width": "The width.", "height": "The height.",
    "r": "A rectangle on screen, in pixels.", "text": "The text to show.", "label": "The label shown next to the control.",
    "mode": "How it is applied.", "force": "The force vector.", "clip": "The audio clip's file name or path.",
    "volume": "The volume, 0..1.", "includeInactive": "Also search objects that are switched off.", "component": "Filled in with the component when found.",
    "worldPositionStays": "When true the object keeps its world position; when false it keeps its local one.",
    "maxDelta": "The most it may change in one call.", "current": "The current value.", "currentVelocity": "The velocity, kept between calls and updated by the function.",
    "smoothTime": "Roughly how long to reach the target, in seconds.", "deltaTime": "The time step; defaults to Time.deltaTime.",
    "maxSpeed": "The highest speed allowed.", "from": "The starting value or point.", "to": "The end value or point.",
    "axis": "The axis to turn about.", "angle": "An angle in degrees.", "up": "Which way is up.", "forward": "Which way is forward.",
    "eulers": "Euler angles in degrees.", "euler": "Euler angles in degrees.", "relativeTo": "Whether the values are in world or local space.",
    "space": "Whether the values are in world or local space.", "button": "The mouse button: 0 left, 1 right, 2 middle.",
    "id": "The handle returned when the thing was made.", "agent": "The script whose methods the tree calls.",
    "owner": "The object or peer that owns it.", "args": "Values passed to the method.", "data": "The text to send.", "peer": "The peer to send to.",
    "c": "The settings to start with.", "e": "Filled in with the next event.", "list": "The list to use.", "weights": "One weight per choice.",
    "font": "The font (null for the default).", "anchor": "Where the text sits in the rectangle.", "tint": "A colour to multiply by.",
    "texture": "A texture id from GpuTexture.", "fov": "Field of view in degrees.", "fovDegrees": "Field of view in degrees.",
    "nearClip": "The near clip distance.", "farClip": "The far clip distance.", "flags": "Option flags; combine with |.",
    "template": "The name of a scene object to copy.", "templates": "Cell value to the name of the scene object to copy for it.",
    "cellSize": "The size of one cell in world units.", "scale": "A scale or feature size.", "octaves": "How many layers of detail to add.",
    "lacunarity": "How much the frequency grows each octave.", "gain": "How much each octave's strength shrinks.",
    "iterations": "How many times to apply it.", "rules": "Each character's replacement.", "axiom": "The starting string.",
    "inventory": "The inventory to use.", "inv": "The inventory to put it in.", "who": "The Interactor (the player) doing it.",
    "attacker": "Who caused it; kept out of hits and reported to the victim.", "hitDirection": "The direction the hit came from.",
    "player": "The player's GameObject.", "n": "A number or index.", "hotbarIndex": "The hotbar slot, 0-based.",
    "reason": "Filled in with why it was refused.", "stateName": "The name of the Animator state.", "layer": "The Animator layer (-1: the first that has the state).",
    "normalizedTime": "Where in the state to start, 0..1.", "bone": "The bone's name in the model.", "weight": "How much it applies, 0..1.",
    "worldRotation": "The rotation in world space.", "worldPosition": "The position in world space.", "slot": "Which slot, 0..7.",
    "metres": "A distance along the curve in metres.", "samples": "How many points to test.", "world": "A point in world space.",
    "tile": "The tile index (-1 for empty).", "prefix": "The start the names must match.", "visible": "Whether it shows.",
    "fraction01": "A fraction from 0 to 1.", "sortMode": "How the results are sorted.", "findInactive": "Whether to include inactive objects.",
    "allowDestroyingAssets": "Ignored; kept for Unity code.", "routine": "The IEnumerator a coroutine method returns.",
    "time": "A time in seconds.", "repeatRate": "Seconds between calls.", "s": "A value in seconds, or a vector.",
    "v": "The value.", "m": "The matrix.", "lo": "The lowest value allowed.", "hi": "The highest value allowed.",
    "length": "The length of the range.", "cur": "The current value.", "condition": "The condition that must hold.",
    "format": "A composite format string such as \"{0} hit {1}\".", "context": "The object the message is about.",
    "start": "Where it starts.", "end": "Where it ends.", "depthTest": "Whether nearer geometry hides the line.",
    "e1": "", "carve": "Whether the obstacle cuts the nav mesh once it stands still.", "feet": "The point on the ground it stands on.",
    "yaw": "A turn about the up axis, in degrees.", "size": "Its size.", "kind": "How the link is crossed.",
    "biDirectional": "Whether agents may cross both ways.", "cost": "How much the path finder dislikes using it (1 = like walking).",
    "open": "Whether it is open.", "enabled": "Whether it is in use.", "maxNeighbors": "How many neighbours each agent avoids.",
    "quality": "Avoidance quality 0..3.", "flowFields": "Whether agents share flow fields to common goals.", "lodDistance": "Agents further than this from the focus update less often.",
    "horizon": "How far ahead agents look when avoiding each other.", "source": "The point to start from.", "hit": "Filled in with the result.",
    "corners": "Filled in with the path's corners.", "dt": "The time step in seconds.", "state": "A captured state.", "input": "One frame's input.",
    "client": "A client's peer id.", "o": "The object.", "transport": "The transport to use.", "why": "Why it stopped (logged).",
    "ms": "How long to listen, in milliseconds.", "obs": "The observations.", "act": "Filled in with the chosen actions.",
    "cont": "How many continuous actions.", "branches": "How many choices each discrete branch has.", "observations": "Filled in with what the agent sees.",
    "actions": "The actions chosen this decision.", "json": "JSON text.", "pretty": "Indent the output for people to read.",
    "fallback": "What to return when the text is not JSON.", "d": "The dictionary to read from.", "tokens": "The message split into tokens.",
    "assetPath": "A project-relative image path.", "options": "The choices.", "selected": "Whether the row is selected.",
    "extensions": "File extensions such as \".png\".", "itemName": "The menu path, such as \"Window/My Tools/Item Editor\".",
    "isValidateFunction": "True for the method that decides whether the item is enabled.", "priority": "Lower numbers come first.",
    "title": "The window or dialog title.", "ok": "The confirm button's label.", "cancel": "The cancel button's label (empty for none).",
    "px": "A size in pixels.", "type": "The kind of message.", "bold": "Whether the text is bold.", "style": "One of the style constants.",
    "dim": "Whether the text is greyed out.", "align": "0 left, 1 centre, 2 right.", "open_": "",
    "g": "Green, 0..1.", "h": "The height.", "tag": "The tag to look for, such as \"Enemy\".",
    "recursive": "Also list files in sub-folders.", "relative": "A project-relative path such as \"Assets/Levels/1.json\".",
    "shadow": "A colour to draw a one-pixel drop shadow in, or null for none.", "mesh": "The mesh to use (null clears it).",
    "nx": "The new x.", "ny": "The new y.", "nz": "The new z.", "map": "The layer weights, as [y, x, layer].",
    "heights": "The heights, 0..1, as [y, x].", "xBase": "The first column of samples.", "yBase": "The first row of samples.",
}


# ── Inline markup ───────────────────────────────────────────────────────────
LINKS = {}   # simple name -> slug, filled before rendering


def inline(s):
    s = html.escape(s, quote=False)
    def link(m):
        target = m.group(1)
        label = m.group(2) or target
        base, _, member = target.partition(".")
        slug = LINKS.get(base) or LINKS.get(target)
        if not slug:
            return "<code>" + label + "</code>"
        href = "#/" + slug + ("/" + member if member else "")
        return '<a href="' + href + '"><code>' + label + "</code></a>"
    s = re.sub(r'\[\[([\w\.<>]+)(?:\|([^\]]+))?\]\]', link, s)
    s = re.sub(r'`([^`]+)`', lambda m: "<code>" + m.group(1) + "</code>", s)
    # [text](https://...) - an outside link, opened in a new tab
    s = re.sub(r'(?<!\[)\[([^\[\]]+)\]\((https?://[^)\s]+)\)', r'<a href="\2" target="_blank" rel="noopener">\1</a>', s)
    s = re.sub(r'\*\*([^*]+)\*\*', r'<b>\1</b>', s)
    s = re.sub(r'(?<![\w*])\*([^*\s][^*]*)\*(?![\w*])', r'<i>\1</i>', s)
    # auto-link bare known type names written as `Name` already handled; link plain CamelCase type names inside <code>
    def code_link(m):
        inner = m.group(1)
        if inner in LINKS and not inner.startswith("<"):
            return '<a href="#/' + LINKS[inner] + '"><code>' + inner + "</code></a>"
        return m.group(0)
    s = re.sub(r'(?<!">)<code>([A-Z]\w+)</code>(?!</a>)', code_link, s)
    return s


ICON_INFO = '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="9"/><path d="M12 11v5M12 8h.01"/></svg>'
ICON_WARN = '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M10.3 3.9 1.8 18a2 2 0 0 0 1.7 3h17a2 2 0 0 0 1.7-3L13.7 3.9a2 2 0 0 0-3.4 0Z"/><path d="M12 9v4M12 17h.01"/></svg>'


def block(text):
    """Paragraphs, lists, callouts, code fences and ### headings -> HTML."""
    out, para, lst = [], [], []
    lines = text.split("\n")
    i = 0
    def flush():
        nonlocal para, lst
        if para:
            out.append("<p>" + inline(" ".join(para)) + "</p>"); para = []
        if lst:
            out.append("<ul>" + "".join("<li>" + inline(x) + "</li>" for x in lst) + "</ul>"); lst = []
    while i < len(lines):
        l = lines[i]
        s = l.strip()
        if s.startswith("```"):
            flush()
            lang = s[3:].strip() or "csharp"
            code = []
            i += 1
            while i < len(lines) and not lines[i].strip().startswith("```"):
                code.append(lines[i]); i += 1
            out.append('<div class="code-wrap"><div class="code-head"><span>' + ("JSON" if lang == "json" else "C#" if lang in ("csharp", "cs") else lang) +
                       '</span><button class="copy-btn" type="button"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="9" y="9" width="12" height="12" rx="2"/><path d="M5 15V5a2 2 0 0 1 2-2h10"/></svg><span>Copy</span></button></div>'
                       '<pre class="code" data-lang="' + lang + '">' + html.escape("\n".join(code).rstrip()) + "</pre></div>")
            i += 1
            continue
        if not s:
            flush(); i += 1; continue
        m = re.match(r'^>\s*(note|warning|tip):\s*(.*)$', s, re.I)
        if m:
            flush()
            warn = m.group(1).lower() == "warning"
            out.append('<div class="callout' + (" warn" if warn else "") + '">' + (ICON_WARN if warn else ICON_INFO) + "<p>" + inline(m.group(2)) + "</p></div>")
            i += 1; continue
        m = re.match(r'^!\[(.*?)\]\((\S+?)\)$', s)
        if m:
            # ![caption](../assets/img/x.jpg): a screenshot with its caption
            flush()
            out.append('<figure class="doc-shot"><img src="' + html.escape(m.group(2)) + '" alt="' + html.escape(m.group(1)) +
                       '" loading="lazy"><figcaption>' + inline(m.group(1)) + '</figcaption></figure>')
            i += 1; continue
        m = re.match(r'^(#{2,3})\s+(.*)$', s)
        if m:
            flush()
            lvl = len(m.group(1))
            hid = re.sub(r'[^a-z0-9]+', '-', m.group(2).lower()).strip('-')
            out.append("<h%d id=\"%s\">%s</h%d>" % (lvl, hid, inline(m.group(2)), lvl))
            i += 1; continue
        if re.match(r'^[-*]\s+', s):
            if para: out.append("<p>" + inline(" ".join(para)) + "</p>"); para = []
            lst.append(re.sub(r'^[-*]\s+', '', s))
            i += 1; continue
        if lst and l.startswith("  "):
            lst[-1] += " " + s; i += 1; continue
        if lst: flush()
        para.append(s)
        i += 1
    flush()
    return "\n".join(out)


def plain(htmltext):
    t = re.sub(r'<[^>]+>', '', htmltext)
    return html.unescape(t).strip()


# ── Content files ───────────────────────────────────────────────────────────
def parse_content():
    docs = {}
    for path in sorted(glob.glob(os.path.join(HERE, "content", "*.md"))):
        text = open(path, encoding="utf-8").read().replace("\r\n", "\n")
        chunks = re.split(r'^# ', text, flags=re.M)
        for ch in chunks[1:]:
            head, _, body = ch.partition("\n")
            head = head.strip()
            entry = {"file": os.path.basename(path), "summary": "", "see": [], "desc": "", "members": {}, "params": {},
                     "returns": {}, "examples": [], "page": None, "addcomponent": ""}
            if head.startswith("page:"):
                slug, _, title = head[5:].partition("|")
                entry["page"] = (slug.strip(), title.strip())
                key = "page:" + slug.strip()
            else:
                key = head
            parts = re.split(r'^## ', body, flags=re.M)
            intro = parts[0]
            meta_done = False
            desc_lines = []
            for l in intro.split("\n"):
                m = re.match(r'^(summary|see|addcomponent):\s*(.*)$', l)
                if m and not meta_done:
                    if m.group(1) == "see": entry["see"] = [x.strip() for x in m.group(2).split(",") if x.strip()]
                    else: entry[m.group(1)] = m.group(2).strip()
                    continue
                if l.strip(): meta_done = True
                desc_lines.append(l)
            entry["desc"] = "\n".join(desc_lines).strip()
            if entry["page"]:
                # a page keeps its ## headings in the body
                entry["desc"] = (entry["desc"] + "\n" + "\n".join("## " + p for p in parts[1:])).strip()
                docs[key] = entry
                continue
            for p in parts[1:]:
                title, _, pbody = p.partition("\n")
                tl = title.strip()
                if tl.lower() == "members":
                    last = None
                    for l in pbody.split("\n"):
                        if not l.strip(): continue
                        if l.startswith("@"):
                            pn, _, pd = l[1:].partition(":")
                            entry["params"].setdefault(last or "", {})[pn.strip()] = pd.strip()
                            continue
                        if l.startswith("=>"):
                            if last: entry["returns"][last] = l[2:].strip()
                            continue
                        if l.startswith("  ") and last:
                            entry["members"][last] += " " + l.strip(); continue
                        m = re.match(r'^([^:]+?):\s+(.*)$', l)
                        if m:
                            last = m.group(1).strip()
                            entry["members"][last] = m.group(2).strip()
                elif tl.lower().startswith("example"):
                    etitle = tl.split(":", 1)[1].strip() if ":" in tl else "Example"
                    fm = re.search(r'```(\w*)\n(.*?)```', pbody, re.S)
                    if not fm:
                        print("!! example without code:", key, etitle); continue
                    pre = pbody[:fm.start()]
                    efile = ""
                    pm = re.match(r'^\s*file:\s*(.+)\n', pre)
                    if pm: efile = pm.group(1).strip(); pre = pre[pm.end():]
                    entry["examples"].append({"title": etitle, "text": pre.strip(), "code": fm.group(2).rstrip(),
                                              "lang": fm.group(1) or "csharp", "file": efile,
                                              "after": pbody[fm.end():].strip()})
            if key in docs:
                print("!! duplicate content for", key, "in", entry["file"], "and", docs[key]["file"])
            docs[key] = entry
    return docs


def camel_words(name):
    w = re.sub(r'([a-z0-9])([A-Z])', r'\1 \2', name)
    return w.lower()


def fallback_member(m, tname):
    n = m["name"]
    if m["kind"] in ("property", "field", "constant"):
        return "The " + camel_words(n) + (" of this " + tname if not m["static"] else "") + "."
    if m["kind"] == "constructor":
        return "Makes a new " + tname + "."
    if m["kind"] == "event":
        return "Raised when " + camel_words(n.replace("On", "", 1)) + "."
    return camel_words(n).capitalize() + "."


def main():
    raw = json.load(open(os.path.join(HERE, "api_raw.json"), encoding="utf-8"))
    raw = [t for t in raw if t["source"] in ("ElintriaSharp", "Package:Standard")]
    content = parse_content()

    cat_of = {}
    for ci, (sec, title, desc, names) in enumerate(CATEGORIES):
        for n in names: cat_of[n] = ci

    types = []
    seen_simple = {}
    for t in raw:
        simple = t["name"].split(".")[-1]
        base_simple = re.sub(r'<.*$', '', simple)
        if base_simple in EXCLUDE: continue
        if base_simple not in cat_of:
            print("?? no category for", t["namespace"], t["name"]); continue
        t["_simple"] = base_simple
        types.append(t)
        seen_simple.setdefault(base_simple, []).append(t)
    for n, lst in seen_simple.items():
        if len(lst) > 1: print("!! name used twice:", n, [x["namespace"] for x in lst])

    def display_name(t):
        if t["name"].startswith("BTInstance."): return t["name"]
        return re.sub(r'<.*$', '', t["name"].split(".")[-1]) + (re.search(r'<.*$', t["name"]).group(0) if '<' in t["name"] else "")

    for t in types:
        t["_slug"] = t["_simple"]
        LINKS[t["_simple"]] = t["_slug"]
        LINKS[display_name(t)] = t["_slug"]
    pages = [(k, v) for k, v in content.items() if v["page"]]
    for k, v in pages:
        LINKS[v["page"][0]] = v["page"][0]

    by_simple = {t["_simple"]: t for t in types}
    def is_script(t, depth=0):
        b = re.sub(r'<.*$', '', (t["base"] or "").split(",")[0].strip())
        if b == "ElintriaScript": return True
        if b in by_simple and depth < 8: return is_script(by_simple[b], depth + 1)
        return False

    derived = {}
    for t in types:
        for b in (t["base"] or "").split(","):
            b = re.sub(r'<.*$', '', b.strip())
            if b in by_simple: derived.setdefault(b, []).append(display_name(t))

    out_types, problems = [], []
    total_members = total_examples = 0
    for t in types:
        simple = t["_simple"]
        c = content.get(simple) or content.get(display_name(t)) or content.get(t["name"])
        if not c:
            problems.append("no content: " + simple)
            c = {"summary": t["summary"], "see": [], "desc": t["summary"], "members": {}, "params": {}, "returns": {}, "examples": [], "addcomponent": ""}
        tname = display_name(t)
        script = is_script(t)
        # members, overloads grouped by name
        groups, order = {}, []
        for m in t["members"]:
            sig = m["signature"]
            sig = re.sub(r'^(\[[^\]]*\]\s*)+', '', sig)
            sig = re.sub(r'^public\s+', '', sig)
            hidden = "[HideInInspector]" in m["signature"]
            if m["kind"] == "method" and m["name"] in LIFECYCLE and re.search(r'\boverride\b', sig): continue
            if m["name"] in ("Equals", "GetHashCode") and re.search(r'\boverride\b', sig): continue
            if m["kind"] == "method" and m["name"] in ("Prompt", "CanInteract", "Interact", "Point") and re.search(r'\boverride\b', sig) and simple != "Interactable":
                pass
            key = (m["name"], m["kind"] if m["kind"] != "field" else "field", m.get("protected", False), m["static"] and m["kind"] == "method")
            if key not in groups:
                groups[key] = {"n": m["name"], "k": m["kind"], "s": [], "st": m["static"], "prot": m.get("protected", False),
                               "ro": m["readonly"], "t": m["type"], "params": [], "summary": [], "hidden": hidden}
                order.append(key)
            g = groups[key]
            g["s"].append(sig)
            if m["summary"]: g["summary"].append((sig, m["summary"]))
            for p in m["params"]:
                if p["name"] not in [x["name"] for x in g["params"]]:
                    g["params"].append(p)
        members = []
        anchors = set()
        for key in order:
            g = groups[key]
            n = g["n"]
            # description: content by exact signature, then by name, then extracted, then common, then fallback
            cm = c["members"]
            extra = []
            for k in cm:
                if "(" in k and any(k.replace(" ", "") in sig.replace(" ", "") for sig in g["s"]):
                    extra.append(cm[k])
            extracted = " ".join(dict.fromkeys(s for _, s in g["summary"]))
            extracted = re.sub(r'\|([\w\.\[\]]+)\|', r'`\1`', extracted)   # source comments write names as |name|
            desc = cm.get(n) or extracted or None
            if desc is None and any(re.search(r'\boverride\b', s_) for s_ in g["s"]):
                b_ = re.sub(r'<.*$', '', (t["base"] or "").split(",")[0].strip())
                while desc is None and b_ in by_simple:
                    desc = ((content.get(b_) or {}).get("members") or {}).get(n)
                    b_ = re.sub(r'<.*$', '', (by_simple[b_]["base"] or "").split(",")[0].strip())
            if desc is None and n in COMMON: desc = COMMON[n].replace("{T}", tname)
            if desc is None and n.startswith("operator"):
                desc = "The `" + n.replace("operator ", "") + "` operator on " + tname + " values."
            if extra: desc = ((desc + " ") if desc else "") + " ".join(extra)
            if desc is None:
                desc = fallback_member({"name": n, "kind": g["k"], "static": g["st"]}, tname)
                problems.append("member without description: %s.%s" % (simple, n))
            desc = desc.lstrip("!")
            params = []
            pnotes = {}
            for k, v in c["params"].items():
                if k == n or ("(" in k and any(k.replace(" ", "") in s.replace(" ", "") for s in g["s"])):
                    pnotes.update(v)
            type_notes = {}
            for v in c["params"].values(): type_notes.update(v)
            for p in g["params"]:
                pd = pnotes.get(p["name"]) or type_notes.get(p["name"]) or PARAMS.get(p["name"]) or ""
                if not pd:
                    pd = "The " + camel_words(p["name"]) + "."
                    problems.append("param without description: %s.%s(%s)" % (simple, n, p["name"]))
                params.append({"n": p["name"], "t": p["type"], "def": p["default"], "d": inline(pd)})
            ret = c["returns"].get(n)
            anchor = n.replace("operator ", "op_").replace(" ", "").replace("[", "_").replace("]", "_").replace(",", "_")
            while anchor in anchors: anchor += "_"
            anchors.add(anchor)
            dhtml = inline(desc)
            short = re.split(r'(?<=[.!?])\s', plain(dhtml), 1)[0]
            members.append({"n": n, "k": g["k"], "s": g["s"], "st": g["st"], "prot": g["prot"], "ro": g["ro"], "t": g["t"],
                            "d": dhtml, "short": html.escape(short), "sText": short[:110], "p": params,
                            "r": inline(ret) if ret else "", "anchor": anchor})
        total_members += len(members)
        enum_vals = []
        for v in t["enumValues"] or []:
            vd = c["members"].get(v["name"]) or v["summary"] or ""
            if not vd and "*" in c["members"]:
                vd = c["members"]["*"].replace("{name}", v["name"]).replace("{words}", camel_words(v["name"]))
            if not vd: problems.append("enum value without description: %s.%s" % (simple, v["name"]))
            enum_vals.append({"n": v["name"], "v": v["value"], "d": inline(vd)})
        exs = []
        for e in c["examples"]:
            exs.append({"title": e["title"], "text": block(e["text"]) + (block(e["after"]) if e.get("after") else ""), "code": e["code"], "lang": e["lang"], "file": e["file"]})
        total_examples += len(exs)
        if len(exs) < MIN_EXAMPLES: problems.append("only %d examples: %s" % (len(exs), simple))
        kind = t["kind"]
        if kind == "class" and simple.endswith("Attribute"): kind = "attribute"
        ci = cat_of[simple]
        sec_id = CATEGORIES[ci][0]
        summary = c["summary"] or t["summary"]
        if not c["summary"]: problems.append("no summary: " + simple)
        inherited = []
        b = re.sub(r'<.*$', '', (t["base"] or "").split(",")[0].strip())
        while b in by_simple:
            inherited.append(display_name(by_simple[b]))
            b = re.sub(r'<.*$', '', (by_simple[b]["base"] or "").split(",")[0].strip())
        if b == "ElintriaScript" or (inherited and inherited[-1] != "ElintriaScript" and script): pass
        add = c.get("addcomponent") or ""
        if not add and t["source"] == "Package:Standard" and script and "abstract" not in t["modifiers"]:
            add = "Elintria Standard - " + STANDARD_MENU.get(t["file"], "")
        out_types.append({
            "slug": t["_slug"], "name": tname, "kind": kind, "namespace": t["namespace"], "category": CATEGORIES[ci][1],
            "section": [s[0] for s in SECTIONS].index(sec_id), "catIndex": ci,
            "base": ", ".join(x.strip() for x in (t["base"] or "").split(",") if x.strip()) if t["base"] else "",
            "derived": sorted(derived.get(simple, [])), "file": t["file"], "pkg": "Standard package" if t["source"] == "Package:Standard" else "",
            "isStatic": "static" in t["modifiers"], "isScript": script, "addComponent": add,
            "summary": inline(summary), "summaryText": plain(inline(summary))[:140], "desc": block(c["desc"] or summary),
            "members": members, "enumValues": enum_vals, "examples": exs,
            "seeAlso": [LINKS[s] for s in c["see"] if s in LINKS], "inherited": inherited,
        })
        for s in c["see"]:
            if s not in LINKS: problems.append("see-also target unknown: %s -> %s" % (simple, s))

    out_pages = []
    for k, v in pages:
        slug, title = v["page"]
        out_pages.append({"slug": slug, "title": title, "kind": "page", "section": 0, "summary": inline(v["summary"]),
                          "summaryText": plain(inline(v["summary"]))[:140], "html": block(v["desc"])})
    for key in content:
        if not key.startswith("page:") and key not in LINKS and key not in [display_name(t) for t in types]:
            problems.append("content for an unknown type: " + key)

    # sidebar sections
    sections = []
    for sid, stitle, sdesc in SECTIONS:
        cats = []
        for ci, (sec, title, desc, names) in enumerate(CATEGORIES):
            if sec != sid: continue
            if sid == "manual":
                items = [p["slug"] for p in out_pages]
            else:
                items = [t["slug"] for t in sorted([t for t in out_types if t["catIndex"] == ci], key=lambda t: names.index(t["slug"]) if t["slug"] in names else 999)]
            if items: cats.append({"title": title, "desc": desc, "items": items})
        sections.append({"id": sid, "title": stitle, "desc": sdesc, "categories": cats})
    for t in out_types: t.pop("catIndex", None)

    type_links = {}
    for t in out_types:
        type_links[t["name"]] = t["slug"]
        type_links[re.sub(r'<.*$', '', t["name"])] = t["slug"]
    data = {"generated": datetime.date.today().isoformat(), "sections": sections, "types": out_types, "pages": out_pages,
            "typeLinks": type_links,
            "stats": {"types": len(out_types), "members": total_members, "examples": total_examples}}
    os.makedirs(os.path.join(SITE, "assets", "data"), exist_ok=True)
    with open(os.path.join(SITE, "assets", "data", "api.js"), "w", encoding="utf-8", newline="\n") as f:
        f.write("/* Generated by tools/build_docs.py - do not edit by hand. */\nwindow.ELINTRIA_DOCS = ")
        json.dump(data, f, ensure_ascii=False, separators=(",", ":"))
        f.write(";\n")
    with open(os.path.join(HERE, "build_report.txt"), "w", encoding="utf-8") as f:
        f.write("\n".join(problems))
    kinds = {}
    for p in problems:
        k = p.split(":")[0]; kinds[k] = kinds.get(k, 0) + 1
    print("types %d, members %d, examples %d, pages %d" % (len(out_types), total_members, total_examples, len(out_pages)))
    print("problems:", kinds if kinds else "none")


if __name__ == "__main__":
    main()
