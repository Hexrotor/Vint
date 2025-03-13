using LinqToDB.Mapping;

namespace Vint.Core.Database.Models;

[Table(DbConstants.FriendRequests)]
public class FriendRequest {
    [PrimaryKey(0), Identity] public int Id { get; set; }

    [Column] public required long SenderId { get; init; }
    [Column] public required long FriendId { get; init; }

    [Column] public required DateTimeOffset CreatedAt { get; init; }

    [Association(ThisKey = nameof(SenderId), OtherKey = nameof(Player.Id))]
    public Player SenderPlayer { get; private set; } = null!;

    [Association(ThisKey = nameof(FriendId), OtherKey = nameof(Player.Id))]
    public Player FriendPlayer { get; private set; } = null!;
}
