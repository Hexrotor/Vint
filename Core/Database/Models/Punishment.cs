using System.Text;
using LinqToDB.Mapping;

namespace Vint.Core.Database.Models;

[Table(DbConstants.Punishments)]
public class Punishment {
    [Association(ThisKey = nameof(PlayerId), OtherKey = nameof(Player.Id))] [field: NotColumn]
    public required Player Player {
        get;
        init {
            field = value;
            PlayerId = value.Id;
        }
    } = null!;

    [PrimaryKey(0), Identity] public long Id { get; set; }
    [PrimaryKey(1)] public long PlayerId { get; private set; }

    [Column] public string? IPAddress { get; init; }
    [Column] public required string HardwareFingerprint { get; init; }
    [Column] public required PunishmentType Type { get; init; }
    [Column] public required DateTimeOffset PunishTime { get; init; }
    [Column] public required TimeSpan? Duration { get; init; }

    [Column] public required string? Reason { get; init; }
    [Column] public required bool Active { get; set; }

    [NotColumn] public bool Permanent => Duration == null;
    [NotColumn] public DateTimeOffset? EndTime => PunishTime + Duration;

    public override string ToString() {
        string verb = Type switch {
            PunishmentType.Warn => "warned",
            PunishmentType.Mute => "muted",
            PunishmentType.Ban => "banned",
            _ => ""
        };

        StringBuilder builder = new(verb);

        if (Reason != null) {
            builder.Append(" for \"");
            builder.Append(Reason);
            builder.Append('"');
        }

        if (EndTime != null) {
            builder.Append(" until ");
            builder.Append(EndTime);
        }

        return builder.ToString();
    }
}

public enum PunishmentType {
    Warn,
    Mute,
    Ban
}
