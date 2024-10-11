using Chat.Common.Net;
using Chat.Common.Net.Packet;
using Chat.Common.Net.Packet.Header;
using Chat.Common.Packet.Data.Client;
using Chat.Common.Packet.Data.Server;

namespace Chat.Server.Net.Handlers;

[PacketHandler(ClientHeader.ClientSyncMusic)]
public class SyncMusicHandler : AbstractHandler
{
    internal override Task Handle(ChatSession session, InPacket inPacket)
    {
        var data = inPacket.Decode<ClientSyncMusic>();
        var channel = ChatServer.Instance.GetChannel(data.Channel);
        var user = channel?.GetUser(session.Client.Id);

        if (channel != null && user != null)
        {
            var packet = new OutPacket(ServerHeader.ServerSyncMusic);
            var serverSyncMusic = new ServerSyncMusic
            {
                Channel = data.Channel,
                Data = data.Data
            };
            packet.Encode(serverSyncMusic);

            channel.Broadcast(packet, user);
        }

        return Task.CompletedTask;
    }
}