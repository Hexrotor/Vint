using EmbedIO;
using EmbedIO.WebApi;
using Vint.Core.Config;
using Vint.Core.Discord;
using Vint.Core.Server.Common.Attributes.Deserialization;
using Vint.Core.Server.Common.Attributes.Methods;

namespace Vint.Core.Server.Static.Controllers;

public class DiscordController(
    DiscordBot discordBot
) : WebApiController {
    [Get("/auth")]
    public async Task<string> Authorize([FromQuery] string state, [FromQuery] string code) {
        DiscordLinkRequest linkRequest = ConfigManager.DiscordLinkRequests.SingleOrDefault(req => req.State == state);

        if (linkRequest == default)
            throw HttpException.BadRequest();

        ConfigManager.DiscordLinkRequests.TryRemove(linkRequest);
        bool success = await discordBot.NewLinkRequest(code, linkRequest.UserId);

        return success
            ? "Your Discord account is successfully linked!"
            : "Account is not linked, contact the administrators for support";
    }
}
