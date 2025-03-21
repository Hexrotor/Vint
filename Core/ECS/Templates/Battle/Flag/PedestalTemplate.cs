using System.Numerics;
using Vint.Core.ECS.Components.Battle.Flag;
using Vint.Core.ECS.Components.Group;
using Vint.Core.ECS.Entities;
using Vint.Core.Server.Game.Protocol.Attributes;

namespace Vint.Core.ECS.Templates.Battle.Flag;

[ProtocolId(1431942342249)]
public class PedestalTemplate : EntityTemplate {
    public IEntity Create(Vector3 position, IEntity team, IEntity round) => Entity("battle/modes/ctf",
        builder => builder
            .AddComponent(new FlagPedestalComponent(position))
            .AddGroupComponent<TeamGroupComponent>(team)
            .AddGroupComponent<BattleGroupComponent>(round));
}
