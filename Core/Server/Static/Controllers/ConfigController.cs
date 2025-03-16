using EmbedIO;
using EmbedIO.WebApi;
using Vint.Core.Config;
using Vint.Core.Server.Common.Attributes.Methods;

namespace Vint.Core.Server.Static.Controllers;

public class ConfigController : WebApiController {
    [Get("/{version}/{locale}", true)]
    public byte[] SendConfig(string version, string locale) =>
        ConfigManager.TryGetConfig(locale, out byte[]? config)
            ? config
            : throw HttpException.NotFound();
}
