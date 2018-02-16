
The framework is, generally, trying to preserve Unity functions, and to build stuff on top of Unity, without modifying it's behaviour.
This will make possible to extend the framework easily, to integrate other toolkits with no trouble, or to add your own assets without changing
them at all.


***


--------------------------------------
Singleton objects
--------------------------------------

Most of the manager objects are singleton objects. These objects are preserved across scenes.

All singleton objects should be placed in startup scene. This scene should be loaded only on startup, and never again, otherwise you may get
unwanted behaviour. This way, singleton objects will be created only once during application lifetime.

--------------------------------------
Scenes
--------------------------------------

First scene in the project should be the startup scene with singleton objects. This scene should, generally, 
contain only singleton objects. Next scene should be the offline scene in NetworkManager.

--------------------------------------
Scene changing
--------------------------------------

If you ever need to change map, use SceneChanger. Why ? Well, because some features require to know if any
scene is being loaded, and SceneChanger provides this information.

--------------------------------------
Players
--------------------------------------

Every player has a dedicated game object which represents that player. It is used to, for example, log in to server, send commands,
send rpcs, and in general interact with the player. You can attach any component to this game object to sync information between client
and server. This game object is preserved across scenes, so that he would not be disconnected when the scene changes.

Each player has 1 object which he controls.
He will be respawned every time the round ends.
When playing object gets destroyed, the player is treated as dead.
While a player is dead, he can spectate other players.

This kind of setup, where player controls only 1 object, is ideal for, lets say, first person shooter game. But what about games where 
a player can control multiple objects, even up to several hundreds of them, like in some strategy games ? It's not a problem, you can 
assign a dummy object to act as a player's controlling object, and you keep him alive as long as all player's objects are alive.


--------------------------------------
Teams
--------------------------------------

Each player can choose a team upon connecting.
Round will end when there is only 1 team left standing.
There is FFA (free for all) mode, in which each player plays for himself.



----------------------------------------------------------------------------------

--------------------------------------
Input controls in demo scene
--------------------------------------

Space - shoot bullets


----------------------------------------------------------------------------------


--------------------------------------
Creating your own playing object
--------------------------------------

What is the playing object ? It's simply a game object which the player controls (it can be character, car, etc).

It takes only few steps to create your own playing object.

All you have to do is add some components to it.

If you don't already have your playing game object, then you should probably copy the integrated 
CapsulePlayer.prefab, and modify it as you wish (add mesh renderers, colliders, animators, etc).

If you already have a playing object, then you will have to ensure that he has the following components:

- NetworkIdentity - you need to check 'local player authority' checkbox
- NetworkTransform - if you want to sync player position
- ControllableObject - used to determine which player owns the object, among other things.
- script which controls camera - anything that inherits from CameraController - you can use built-in BasicCameraController - this component is not required, but spectating will not work without it

Now, if you have a damaging sytem (player can inflict damage to another player), you can use the built-in 
Damagable script (together with ProjectileDamageHandler and Bullet scripts), which notifies other components when damage is inflicted, and when player is killed.
Script which catches these messages is InflictedDamageReporter, which in turn, notifies owning player about this.

It's important that you notify owning player about damage and kill, or otherwise some things will not work.
So, if you have your own damaging system, you should see how the things are done in above scripts, and adapt it.

--------------------------------------
Creating menu
--------------------------------------

- Create a copy of existing menu, such as main menu.

- If it's not visible, enable it's canvas component.

- Assign menu's name, and parent name ( menu name should be unique ).

- Add your own controls as desired.

- If you don't know how to handle button click events to, for example, exit current menu (go up),
then you should check how it's done in other buttons (it's all in UI system, no scripts required, of course).

- That's it. Now you can create a prefab out of your menu, and distribute it as a module :D .

--------------------------------------
Creating/Adapting scenes
--------------------------------------

Every scene should have at least 1 spawn position.
This is necessary so that NetworkManager could create player (from Player.prefab), and also for 
framework to spawn playing objects.
Spawn position can be created in the scene from SpawnPosition.prefab .

To set specific playing object for the scene, use PlayingObjectSetter.prefab .

And don't forget to add new scenes to map cycle.

If you find something unclear, then just see how it's done in demo scene.


--------------------------------------
Adding cvars for settings menu
--------------------------------------

Check TeamSettings script for how to do this.

--------------------------------------
Adding commands to console
--------------------------------------

Check any of command scripts, such as DefaultCommands, for how to do this.

--------------------------------------
Scoreboard
--------------------------------------

To show/hide scoreboard press Tab.

You can customize the scoreboard by assigning delegates.

--------------------------------------
Console
--------------------------------------

To show/hide console press ` key.



--------------------------------------
Messages
--------------------------------------

Here is a list of messages sent:

Messages sent to all scripts:
OnSceneChanged (SceneChangedInfo)
OnRoundStarted
OnRoundFinished (string)

Messages broadcasted to game object:

sender - LocalNetworkEventsDispatcher:
OnServerStarted
OnServerStopped
OnClientConnected
OnClientDisconnected
OnClientStartedConnecting

sender - Player:
OnLoggedIn
OnDisconnectedByServer (string)

sender - PlayerTeamChooser:
OnReceivedChooseTeamMessage (string[])
OnPlayerChoosedTeam (string)

sender - Damagable:
OnDamaged (InflictedDamageInfo)
OnKilled (InflictedDamageInfo)

sender - InflictedDamageReporter:
OnEarnedKill (InflictedDamageInfo)
OnDied (Player)
OnInflictedDamage (InflictedDamageInfo)

sender - Bullet:
OnProjectileHit (ProjectileHitInfo)

sender - Menu:
OnMenuOpened
OnMenuClosed

