using EmbedIO.WebApi;
using Vint.Core.Server.Common.Attributes.Methods;

namespace Vint.Core.Server.Static.Controllers;

public class StateController : WebApiController {
    const byte State = 0; // 0 - ok, all other values - error

    [Get("/", true)]
    public string SendState() => $"state: {State}";
}
