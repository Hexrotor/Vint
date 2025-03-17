using System.Text.RegularExpressions;
using EmbedIO;
using EmbedIO.WebApi;
using LinqToDB;
using Newtonsoft.Json;
using Serilog;
using Vint.Core.Database;
using Vint.Core.Database.Models;
using Vint.Core.Discord;
using Vint.Core.Server.Common.Attributes.Methods;
using Vint.Core.Utils;

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
        int endIndex = log.LastIndexOf('}');
        int length = endIndex - startIndex + 1;

        if (startIndex == -1 || endIndex == -1 || length < 0)
            throw HttpException.BadRequest();

        string json = log.Substring(startIndex, length);

        try {
            ClientLogDTO dto = JsonConvert.DeserializeObject<ClientLogDTO>(json);

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
        } catch (Exception e) {
            Log.Logger.ForType<LoggingController>().WithEndPoint(Request).Error(e, "Failed to deserialize client log");

            string filePath = await SaveLogOnDisk(log);
            await discordBot.SendReport($"New client log. Failed to deserialize. File: {filePath}", "");
        }
    }

    readonly record struct ClientLogDTO(
        ClientLogLevel Level = ClientLogLevel.All,
        string Username = "",
        string Host = "",
        string DeviceId = "",
        string OS = "",
        string ClientVersion = "",
        string InitUrl = "",
        long SessionId = 0,
        string Message = "",
        string Exception = ""
    );

    static async Task<string> SaveLogOnDisk(string log) {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "ClientLogs");

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string fileName = $"{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss-fffffff}.log";
        string filePath = Path.Combine(path, fileName);
        await File.WriteAllTextAsync(filePath, log);
        return filePath;
    }
}
