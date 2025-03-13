using LinqToDB;
using Vint.Core.Database;
using Vint.Core.ECS.Entities;
using Vint.Core.Server.Game;
using Vint.Core.Server.Game.Protocol.Attributes;

namespace Vint.Core.ECS.Events.User.Friends;

[ProtocolId(1450263956353)]
public class RevokeFriendEvent(
    GameServer server
) : FriendBaseEvent, IServerEvent {
    public async Task Execute(IPlayerConnection connection, IEntity[] entities) {
        long senderId = connection.UserContainer.Id;
        await using DbConnection db = new();

        bool success = await db.FriendRequests
            .Where(request => request.SenderId == senderId && request.FriendId == UserId)
            .DeleteAsync() > 0;

        if (!success) return;

        await connection.Send(new OutgoingFriendRemovedEvent(UserId), connection.UserContainer.Entity);

        IPlayerConnection? targetConnection = server.PlayerConnections.Values
            .Where(conn => conn.IsLoggedIn)
            .SingleOrDefault(conn => conn.Player.Id == UserId);

        if (targetConnection != null)
            await targetConnection.Send(new IncomingFriendRemovedEvent(senderId), UserContainer.Entity);
    }
}
