using EmbedIO;
using EmbedIO.Routing;
using JetBrains.Annotations;

namespace Vint.Core.Server.Common.Attributes.Methods;

[MeansImplicitUse(ImplicitUseKindFlags.Access)]
public class AnyAttribute(
    string route,
    bool isBaseRoute = false
) : RouteAttribute(HttpVerbs.Any, route, isBaseRoute);
