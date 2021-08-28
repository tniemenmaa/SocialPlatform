using MessagePack;
using SocialPlatform.Groups.Shared.Messages.ClientToServer;
using SocialPlatform.Groups.Shared.Messages.ServerToClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.Groups.Shared.Messages
{
    public static class NetworkMessageSerializer
    {
        public static byte[] Serialize(INetworkMessage message)
        {
            return message.Serialize();
        }
        public static INetworkMessage Deserialize( byte messageType, byte[] buffer, int offset, int length )
        {
            switch (messageType)
            {
                // Client to Server
                case ClientToServerMessageTypes.GetGroupsRequest:
                    return MessagePackSerializer.Deserialize<GetGroupsRequest>(new ReadOnlyMemory<byte>(buffer, offset, length));

                case ClientToServerMessageTypes.CreateGroupRequest:
                    return MessagePackSerializer.Deserialize<CreateGroupRequest>(new ReadOnlyMemory<byte>(buffer, offset, length));

                case ClientToServerMessageTypes.JoinGroupRequest:
                    return MessagePackSerializer.Deserialize<JoinGroupRequest>(new ReadOnlyMemory<byte>(buffer, offset, length));

                case ClientToServerMessageTypes.LeaveGroupRequest:
                    return MessagePackSerializer.Deserialize<LeaveGroupRequest>(new ReadOnlyMemory<byte>(buffer, offset, length));

                case ClientToServerMessageTypes.SendGroupMessage:
                    return MessagePackSerializer.Deserialize<SendGroupMessage>(new ReadOnlyMemory<byte>(buffer, offset, length));


                // Server to Client
                case ServerToClientMessageTypes.GetGroupsResponse:
                    return MessagePackSerializer.Deserialize<GetGroupsResponse>(new ReadOnlyMemory<byte>(buffer, offset, length));

                case ServerToClientMessageTypes.CreateGroupResponse:
                    return MessagePackSerializer.Deserialize<CreateGroupResponse>(new ReadOnlyMemory<byte>(buffer, offset, length));

                case ServerToClientMessageTypes.JoinGroupResponse:
                    return MessagePackSerializer.Deserialize<JoinGroupResponse>(new ReadOnlyMemory<byte>(buffer, offset, length));

                case ServerToClientMessageTypes.LeaveGroupResponse:
                    return MessagePackSerializer.Deserialize<LeaveGroupResponse>(new ReadOnlyMemory<byte>(buffer, offset, length));

                case ServerToClientMessageTypes.ReceiveGroupMessage:
                    return MessagePackSerializer.Deserialize<ReceiveGroupMessage>(new ReadOnlyMemory<byte>(buffer, offset, length));

                case ServerToClientMessageTypes.GroupUpdatedMessage:
                    return MessagePackSerializer.Deserialize<GroupUpdatedMessage>(new ReadOnlyMemory<byte>(buffer, offset, length));

                default:
                    throw new ArgumentException($"Invalid argument value {messageType}", nameof(messageType));
            }
        }
    }
}
