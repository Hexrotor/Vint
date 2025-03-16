using EmbedIO;
using EmbedIO.Routing;
using JetBrains.Annotations;

namespace Vint.Core.Server.Common.Attributes.Methods;

[MeansImplicitUse(ImplicitUseKindFlags.Access)]
public class PutAttribute(
    string route,
    bool isBaseRoute = false
) : RouteAttribute(HttpVerbs.Put, route, isBaseRoute);
