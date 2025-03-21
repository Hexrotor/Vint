﻿using Vint.Core.ECS.Entities;
using Vint.Core.Server.Game;
using Vint.Core.Server.Game.Protocol.Attributes;

namespace Vint.Core.ECS.Events.Entrance.Registration;

[ProtocolId(1453881244963)]
public class IncrementRegistrationNicksEvent : IServerEvent { // TODO statistics?
    [ProtocolName("Nick")] public string Nickname { get; private set; } = null!;

    public Task Execute(IPlayerConnection connection, IEntity[] entities) => Task.CompletedTask;
}
