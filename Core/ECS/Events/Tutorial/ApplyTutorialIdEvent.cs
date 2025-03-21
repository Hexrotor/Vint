using Vint.Core.ECS.Entities;
using Vint.Core.Server.Game;
using Vint.Core.Server.Game.Protocol.Attributes;

namespace Vint.Core.ECS.Events.Tutorial;

[ProtocolId(1505212007257)]
public class ApplyTutorialIdEvent : IServerEvent { // todo
    public long Id { get; private set; }

    public Task Execute(IPlayerConnection connection, IEntity[] entities) => Task.CompletedTask;
}
