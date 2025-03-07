using Vint.Core.Server.Game.Protocol.Attributes;

namespace Vint.Core.ECS.Components.DailyBonus;

[ProtocolId(636459174909060087)]
public class UserDailyBonusReceivedRewardsComponent(
    List<int> receivedRewards
) : IComponent {
    public List<int> ReceivedRewards { get; } = receivedRewards;
}
