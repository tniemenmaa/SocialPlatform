using SocialPlatform.GroupRegistry.Shared;
using SocialPlatform.GroupRegistry.Shared.Messages;
using SocialPlatform.GroupRegistry.Shared.Messages.ClientToServer;
using SocialPlatform.GroupRegistry.Shared.Messages.ServerToClient;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SocialPlatform.Groups.Client
{
    public class DummyClient
    {

        private const string DEFAULT_WS_ADDRESS = "ws://localhost:8080/ws";

        public async Task RunAsync()
        {
            _playerId = Guid.NewGuid();
            Console.WriteLine("This is simple test client for using the groups functionality.");
            Console.WriteLine("Input server websocket address, leave empty to default to 'ws://localhost:8080/ws'");
            string address = Console.ReadLine();

            if (string.IsNullOrEmpty(address))
            {
                address = DEFAULT_WS_ADDRESS;
            }


            ClientWebSocket socket = new ClientWebSocket();
            socket.Options.SetRequestHeader("playerId", _playerId.ToString());
            await socket.ConnectAsync(new Uri(address), CancellationToken.None);

            if (socket.State != WebSocketState.Open)
            {
                Console.WriteLine("Failed to connect to the server");
            }
            else
            {
                Console.WriteLine($"Connected to {address} successfully.");
            }

            Console.WriteLine("What is your name?");
            string playerName = Console.ReadLine();

            Console.WriteLine($"Hello {playerName}, you have been assigned id {_playerId}");
            Task receiveAsync = Task.Run( () => ReceiveAsync(socket, CancellationToken.None));
            PrintHelp();

            INetworkMessage message;
            byte[] data;

            while (true)
            {
                string[] input = Console.ReadLine().Trim().ToLower().Split(" ", 2);

                // If player inputs /exit then 
                switch (input[0])
                {
                    case "/exit":
                        {
                            Environment.Exit(0);
                            break;
                        }
                    case "/list":
                        {
                            message = new GetGroupsRequest();
                            data = NetworkMessageWriter.Write(message, _outgoingSequenceNumber++);
                            await socket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, CancellationToken.None);
                            break;
                        }
                    case "/create":
                        {
                            if (input.Length < 2)
                            {
                                Console.WriteLine("Group name is missing.");
                                break;
                            }
                            message = new CreateGroupRequest(input[1], new GroupMember(_playerId, playerName));
                            data = NetworkMessageWriter.Write(message, _outgoingSequenceNumber++);
                            await socket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, CancellationToken.None);
                            break;
                        }
                    case "/join":
                        {
                            if (input.Length < 2)
                            {
                                Console.WriteLine("Group id is missing.");
                                break;
                            }

                            Guid groupId;
                            if (!Guid.TryParse(input[1], out groupId))
                            {
                                Console.WriteLine("Group id is not a valid guid.");
                                break;
                            }

                            message = new JoinGroupRequest(groupId, new GroupMember(_playerId, playerName));
                            data = NetworkMessageWriter.Write(message, _outgoingSequenceNumber++);
                            await socket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, CancellationToken.None);
                            break;
                        }
                    case "/leave":
                        {
                            if (_currentGroup == null)
                            {
                                Console.WriteLine("You are not in a group.");
                                break;
                            }

                            message = new LeaveGroupRequest(_currentGroup.Id, _playerId);
                            data = NetworkMessageWriter.Write(message, _outgoingSequenceNumber++);
                            await socket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, CancellationToken.None);
                            break;
                        }
                    case "/send":
                        {
                            if (_currentGroup == null)
                            {
                                Console.WriteLine("You are not in a group.");
                                break;
                            }

                            if (input.Length < 2)
                            {
                                Console.WriteLine("Message text is missing.");
                                break;
                            }

                            

                            message = new SendGroupMessage(_currentGroup.Id, new PlayerGroupMessage(_playerId, input[1]));
                            data = NetworkMessageWriter.Write(message, _outgoingSequenceNumber++);
                            await socket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, CancellationToken.None);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Invalid command");
                            break;
                        }

                }
            }
        }

        private async Task ReceiveAsync(ClientWebSocket socket, CancellationToken ct)
        {
            try
            {
                byte[] buffer = new byte[4096];
                while (!socket.CloseStatus.HasValue)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                    var (networkMessage, sequenceNumber) = NetworkMessageReader.Read(buffer, 0, result.Count);

                    if (networkMessage == null) continue;

                    switch (networkMessage.MessageType)
                    {
                        case ServerToClientMessageTypes.GetGroupsResponse:
                            {
                                var response = (GetGroupsResponse)networkMessage;
                                foreach (var group in response.Groups)
                                {
                                    Console.WriteLine($"{group.Id} - {group.Name} - {group.Members.Length}/20");
                                }
                                break;
                            }
                        case ServerToClientMessageTypes.CreateGroupResponse:
                            {
                                var response = (CreateGroupResponse)networkMessage;
                                if (response.Success)
                                {
                                    Console.WriteLine($"Group {response.Group.Name} with id {response.Group.Id} created");
                                    _currentGroup = response.Group;
                                }
                                else
                                {
                                    Console.WriteLine("Failed to create group");
                                }
                                break;
                            }
                        case ServerToClientMessageTypes.JoinGroupResponse:
                            {
                                var response = (JoinGroupResponse)networkMessage;
                                if (response.Success)
                                {
                                    Console.WriteLine($"Joined group {response.Group.Name} successfully.");
                                    _currentGroup = response.Group;
                                    foreach (var msg in _currentGroup.Messages)
                                    {
                                        Console.WriteLine(msg.ToConsoleString(_currentGroup));
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Failed to join the group.");
                                }
                                break;
                            }
                        case ServerToClientMessageTypes.LeaveGroupResponse:
                            {
                                var response = (LeaveGroupResponse)networkMessage;
                                if (response.Success)
                                {
                                    Console.WriteLine($"Successfully left the group.");
                                    _currentGroup = null;
                                }
                                else
                                {
                                    Console.WriteLine($"There was error while trying to leave the group.");
                                }
                                break;
                            }
                        case ServerToClientMessageTypes.ReceiveGroupMessage:
                            {
                                var message = (ReceiveGroupMessage)networkMessage;
                                // Display message only if it is for the players current group. 
                                if (_currentGroup != null && _currentGroup.Id == message.GroupId)
                                {
                                    Console.WriteLine(message.Message.ToConsoleString(_currentGroup));
                                }
                                break;
                            }
                        case ServerToClientMessageTypes.GroupUpdatedMessage:
                            {
                                var message = (GroupUpdatedMessage)networkMessage;
                                _currentGroup = message.Group;
                                break;
                            }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR:\n"+ e.Message+"\n"+e.StackTrace);
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("'/list' - lists all existing groups");
            Console.WriteLine("'/create <group name>' - creates a group");
            Console.WriteLine("'/join <group id>' - adds player to the group.");
            Console.WriteLine("'/leave' - leaves players current group.");
            Console.WriteLine("'/send <message>' - sends message to the players current group.");
            Console.WriteLine("'/exit' - to close the application");
        }

        private Guid _playerId;
        private int _outgoingSequenceNumber = 0;
        private Group _currentGroup = null;
    }
}
