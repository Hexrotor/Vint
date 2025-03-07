using LinqToDB.Mapping;

namespace Vint.Core.Database.Models;

[Table(DbConstants.DailyBonusRedemptions)]
public class DailyBonusRedemption {
    [PrimaryKey(0), Identity] public long Id { get; set; }
    [PrimaryKey(1)] public required int Code { get; init; }
    [PrimaryKey(2)] public required long PlayerId { get; init; }

    [Column] public required DateTimeOffset RedeemedAt { get; init; }
    [Column] public required int Zone { get; init; }
    [Column] public required int Cycle { get; init; }
}
