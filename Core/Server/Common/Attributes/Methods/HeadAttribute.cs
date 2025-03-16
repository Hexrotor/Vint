using EmbedIO;
using EmbedIO.Routing;
using JetBrains.Annotations;

namespace Vint.Core.Server.Common.Attributes.Methods;

[MeansImplicitUse(ImplicitUseKindFlags.Access)]
public class HeadAttribute(
    string route,
    bool isBaseRoute = false
) : RouteAttribute(HttpVerbs.Head, route, isBaseRoute);
