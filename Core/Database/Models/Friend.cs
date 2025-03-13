using LinqToDB.Mapping;

namespace Vint.Core.Database.Models;

[Table(DbConstants.Friends)]
public class Friend {
    [PrimaryKey(0)] public required long UserId { get; init; }
    [PrimaryKey(1)] public required long FriendId { get; init; }

    [Column] public required DateTimeOffset RequestedAt { get; init; }
    [Column] public required DateTimeOffset AcceptedAt { get; init; }

    [Association(ThisKey = nameof(FriendId), OtherKey = nameof(Player.Id))]
    public Player FriendPlayer { get; private set; } = null!;
}
