using EmbedIO;
using EmbedIO.WebApi;
using LinqToDB;
using Newtonsoft.Json;
using Vint.Core.Database;
using Vint.Core.Database.Models;
using Vint.Core.Discord;
using Vint.Core.Server.Common.Attributes.Methods;

namespace Vint.Core.Server.Static.Controllers;

public class LoggingController(
    DiscordBot discordBot
) : WebApiController {
    [Post("/")]
    public async Task ReceiveLog() {
        if (!Request.HasEntityBody)
            throw HttpException.BadRequest();

        string log = await HttpContext.GetRequestBodyAsStringAsync();

        if (string.IsNullOrWhiteSpace(log))
            throw HttpException.BadRequest();

        int startIndex = log.LastIndexOf('{');

        if (startIndex == -1)
            throw HttpException.BadRequest();

        string json = log[startIndex..];
        ClientLogDTO dto = JsonConvert.DeserializeObject<ClientLogDTO>(json);

        if (dto == default)
            throw HttpException.BadRequest();

        ClientLog clientLog = new() {
            Timestamp = DateTimeOffset.UtcNow,
            LogLevel = dto.Level,
            Username = dto.Username,
            Hostname = dto.Host,
            DeviceId = dto.DeviceId,
            OperatingSystem = dto.OS,
            ClientVersion = dto.ClientVersion,
            InitUrl = dto.InitUrl,
            SessionId = dto.SessionId,
            Message = dto.Message,
            ExceptionMessage = dto.Exception,
            RawLog = log
        };

        await using (DbConnection db = new())
            clientLog.Id = await db.InsertWithInt64IdentityAsync(clientLog);

        await discordBot.SendReport($"New client log. Id: {clientLog.Id}", clientLog.Username);
    }

    readonly record struct ClientLogDTO(
        ClientLogLevel Level,
        string Username,
        string Host,
        string DeviceId,
        string OS,
        string ClientVersion,
        string InitUrl,
        long SessionId,
        string Message,
        string Exception
    );
}
