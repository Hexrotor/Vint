using LinqToDB.Mapping;

namespace Vint.Core.Database.Models;

[Table(DbConstants.Blocks)]
public class Block {
    [PrimaryKey(0)] public required long BlockerId { get; init; }
    [PrimaryKey(1)] public required long BlockedId { get; init; }

    [Column] public required DateTimeOffset CreatedAt { get; init; }
}
