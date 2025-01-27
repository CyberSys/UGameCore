using UGameCore.Net;
using UnityEngine;
using static UGameCore.CommandManager;

namespace UGameCore
{

    public class NetworkCommands : MonoBehaviour
    {

        public CommandManager commandManager;


        void Start()
        {

            string[] commands = new string[] { "client_cmd", "players", "kick", "kick_instantly",
                "startserver", "starthost", "connect", "stopnet"};

            foreach (var cmd in commands)
            {
                this.commandManager.RegisterCommand(cmd, ProcessCommand);
            }

        }

        ProcessCommandResult ProcessCommand(ProcessCommandContext context)
        {

            string command = context.command;
            string[] words = CommandManager.SplitSingleCommandIntoArguments(command);
            int numWords = words.Length;
            string restOfTheCommand = context.GetRestOfTheCommand();

            string response = "";


            //			else if (words [0] == "server_cmd") {
            //
            //				if (NetworkStatus.IsClientConnected ()) {
            //					if (numWords < 2)
            //						response += invalidSyntaxString;
            //					else
            //						Player.local.ExecuteCommandOnServer (restOfTheCommand);
            //				}
            //
            //			}
            if (words[0] == "client_cmd")
            {

                if (NetworkStatus.IsServerStarted)
                {
                    if (numWords < 2)
                    {
                        response += CommandManager.invalidSyntaxText;
                    }
                    else
                    {
                        foreach (var player in PlayerManager.players)
                        {
                            player.RpcExecuteCommandOnClient(restOfTheCommand, true);
                        }
                    }
                }

            }
            else if (words[0] == "players")
            {

                // list all players

                response += "name | net id";
                if (NetworkStatus.IsServerStarted)
                    response += " | ip";
                response += "\n";

                foreach (var player in PlayerManager.players)
                {
                    response += player.playerName + " | " + player.NetworkId;
                    if (NetworkStatus.IsServerStarted)
                        response += " | " + player.clientAddress;
                    response += "\n";
                }

            }
            else if (words[0] == "kick")
            {

                if (NetworkStatus.IsServerStarted)
                {
                    var p = PlayerManager.GetPlayerByName(restOfTheCommand);
                    if (null == p)
                    {
                        response += "There is no such player connected.";
                    }
                    else
                    {
                        p.DisconnectPlayer(3, "You are kicked from server.");
                    }

                }
                else
                {
                    response += "Only server can use this command.";
                }

            }
            else if (words[0] == "kick_instantly")
            {

                if (NetworkStatus.IsServerStarted)
                {
                    var p = PlayerManager.GetPlayerByName(restOfTheCommand);
                    if (null == p)
                    {
                        response += "There is no such player connected.";
                    }
                    else
                    {
                        p.DisconnectPlayer(0, "");
                    }

                }
                else
                {
                    response += "Only server can use this command.";
                }

            }
            else if (words[0] == "bot_add")
            {

                if (NetworkStatus.IsServerStarted)
                {

                    //					Player player = this.networkManager.AddBot ();
                    //					if (player != null)
                    //						response += "Added bot: " + player.playerName;
                    //					else
                    //						response += "Failed to add bot.";
                    //

                    /*	GameObject go = GameObject.Instantiate( this.playerObjectPrefab );
					if( go != null ) {
						go.GetComponent<NavMeshAgent>().enabled = true ;

						FPS_Character script = go.GetComponent<FPS_Character>();
						script.isBot = true ;
							//	script.playerName = this.networkManager.CheckPlayerNameAndChangeItIfItExists( "bot" );
						// find random waypoints
						GameObject[] waypoints = GameObject.FindGameObjectsWithTag( "Waypoint" );
						if( waypoints.Length > 0 ) {
							int index1 = Random.Range( 0, waypoints.Length );
							int index2 = Random.Range( 0, waypoints.Length );
							if( index1 == index2 ) {
								index2 ++ ;
								if( index2 >= waypoints.Length )
									index2 = 0 ;
							}

							script.botWaypoints.Add( waypoints[index1].transform );
							script.botWaypoints.Add( waypoints[index2].transform );

							script.botCurrentWaypointIndex = 0;
						}

					//	Player player = this.networkManager.AddLocalPlayer( go );

						// the above function assigns name
					//	script.playerName = player.playerName ;

						NetworkServer.Spawn( go );

						script.respawnOnStart = true;
					//	script.Respawn();

						response += "Added bot." ;

					} else {
						response += "Can't create object for bot." ;
					}
				*/

                }
                else
                {
                    response += "Only server can use this command.";
                }

            }
            else if (words[0] == "bot_add_multiple")
            {

                //				if (this.networkManager.IsServer () && NetworkStatus.IsServerStarted ()) {
                //
                //					int numBotsToAdd = 0;
                //					if (2 == numWords && int.TryParse (words [1], out numBotsToAdd)) {
                //
                //						int numBotsAdded = 0;
                //						for (int i = 0; i < numBotsToAdd; i++) {
                //							Player player = this.networkManager.AddBot ();
                //							if (player != null)
                //								numBotsAdded++;
                //						}
                //
                //						response += "Added " + numBotsAdded + " bots.";
                //
                //					} else {
                //						response += invalidSyntaxString;
                //					}
                //				}

            }
            else if (words[0] == "remove_all_bots")
            {

                if (NetworkStatus.IsServerStarted)
                {
                    int count = 0;
                    foreach (var p in PlayerManager.players)
                    {
                        if (p.IsBot())
                        {
                            p.DisconnectPlayer(0, "");
                            count++;
                        }
                    }
                    response += "Removed " + count + " bots.";
                }

            }
            else if (words[0] == "startserver" || words[0] == "starthost")
            {

                int portNumber = NetManager.defaultListenPortNumber;

                if (numWords > 1)
                    portNumber = int.Parse(words[1]);

                if (words[0] == "startserver")
                    NetManager.StartServer(portNumber);
                else
                    NetManager.StartHost(portNumber);

            }
            else if (words[0] == "connect")
            {

                if (numWords != 3)
                {
                    response += CommandManager.invalidSyntaxText;
                }
                else
                {
                    string ip = words[1];
                    int port = int.Parse(words[2]);

                    NetManager.StartClient(ip, port);
                }

            }
            else if (words[0] == "stopnet")
            {

                NetManager.StopNetwork();

            }

            return ProcessCommandResult.SuccessResponse(response);

        }

    }

}
