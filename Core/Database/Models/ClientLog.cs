using System.Diagnostics.CodeAnalysis;
using LinqToDB.Mapping;

namespace Vint.Core.Database.Models;

[Table(DbConstants.ClientLogs)]
public class ClientLog {
    [PrimaryKey, Identity] public long Id { get; set; }

    [Column] public required DateTimeOffset Timestamp { get; init; }
    [Column] public required ClientLogLevel LogLevel { get; init; }

    [Column] public required string Username { get; init; }
    [Column] public required string Hostname { get; init; }
    [Column] public required string DeviceId { get; init; }
    [Column] public required string OperatingSystem { get; init; }

    [Column] public required string ClientVersion { get; init; }
    [Column] public required string InitUrl { get; init; }
    [Column] public required long SessionId { get; init; }

    [Column] public required string Message { get; init; }
    [Column] public required string ExceptionMessage { get; init; }

    [Column] public required string RawLog { get; init; }
}

[SuppressMessage("Design", "CA1069:Значения перечислений не должны повторяться")]
public enum ClientLogLevel {
    Off = int.MaxValue,
    Emergency = 120000,
    Fatal = 110000,
    Alert = 100000,
    Critical = 90000,
    Severe = 80000,
    Error = 70000,
    Warn = 60000,
    Notice = 50000,
    Info = 40000,
    Debug = 30000,
    Fine = 30000,
    Trace = 20000,
    Finer = 20000,
    Verbose = 10000,
    Finest = 10000,
    All = int.MinValue
}
