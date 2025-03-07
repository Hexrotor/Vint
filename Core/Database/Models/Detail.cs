using LinqToDB.Mapping;

namespace Vint.Core.Database.Models;

[Table(DbConstants.Details)]
public class Detail {
    [PrimaryKey(0)] public required long Id { get; init; }
    [PrimaryKey(1)] public required long PlayerId { get; init; }

    [Column] public int Count { get; set; }
}
