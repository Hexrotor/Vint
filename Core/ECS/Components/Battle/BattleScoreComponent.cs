using Vint.Core.Server.Game.Protocol.Attributes;

namespace Vint.Core.ECS.Components.Battle;

[ProtocolId(1436532217083)]
public class BattleScoreComponent : IComponent {
    public int ScoreRed { get; private set; }
    public int ScoreBlue { get; private set; }
}
