using System.Net;
using System.Text;
using EmbedIO.WebApi;
using Vint.Core.Server.Common.Attributes.Methods;
using Vint.Core.Server.Game;

namespace Vint.Core.Server.Static.Controllers;

public class InitController : WebApiController {
    [Get("/init.yml")]
    public string SendInitConfiguration() =>
        GenerateInitConfig(Request.LocalEndPoint).Serialize();

    static IInitConfig GenerateInitConfig(IPEndPoint localEndPoint) {
        IPAddress host = localEndPoint.Address;
        const ushort acceptorPort = GameServer.Port;
        const string resourcesUrl = "https://cdn.vint.win/resources";
        string configsUrl = $"http://{localEndPoint}/config";
        const string updateConfigUrl = "https://cdn.vint.win/update/{BuildTarget}.yml";
        const string discordRpcResourcesUrl = "https://cdn.vint.win/resources/discord";
        string loggingUrl = $"http://{localEndPoint}/log";

        return new InitConfig(host, acceptorPort, resourcesUrl, configsUrl, updateConfigUrl, discordRpcResourcesUrl, loggingUrl);
    }

    readonly record struct InitConfig(
        IPAddress Host,
        ushort AcceptorPort,
        string ResourcesUrl,
        string ConfigsUrl,
        string UpdateConfigUrl,
        string DiscordRpcResourcesUrl,
        string LoggingUrl
    ) : IInitConfig {
        public string Serialize() {
            StringBuilder builder = new();

            builder.AppendLine($"host: {Host}");
            builder.AppendLine($"acceptorPort: {AcceptorPort}");
            builder.AppendLine($"resourcesUrl: {ResourcesUrl}");
            builder.AppendLine($"configsUrl: {ConfigsUrl}");
            builder.AppendLine($"updateConfigUrl: {UpdateConfigUrl}");
            builder.AppendLine($"discordRpcResourcesUrl: {DiscordRpcResourcesUrl}");
            builder.AppendLine($"loggingUrl: {LoggingUrl}");

            return builder.ToString();
        }
    }

    interface IInitConfig {
        string Serialize();
    }
}
