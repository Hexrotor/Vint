using Vint.Core.ECS.Components.Battle.Team;
using Vint.Core.ECS.Components.Group;
using Vint.Core.ECS.Entities;
using Vint.Core.ECS.Enums;
using Vint.Core.Server.Game.Protocol.Attributes;

namespace Vint.Core.ECS.Templates.Battle;

[ProtocolId(1429761302402)]
public class TeamTemplate : EntityTemplate {
    public IEntity Create(TeamColor teamColor) => Entity(null,
        builder => builder
            .AddComponent<TeamComponent>()
            .AddComponent(new TeamScoreComponent())
            .AddComponent(new TeamColorComponent(teamColor))
            .AddGroupComponent<TeamGroupComponent>());
}
