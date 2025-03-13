﻿using Serilog;
using Vint.Core.Database;
using Vint.Core.Database.Models;
using Vint.Core.ECS.Entities;
using Vint.Core.ECS.Events.Entrance.Validation;
using Vint.Core.Server.Game;
using Vint.Core.Server.Game.Protocol.Attributes;
using Vint.Core.Utils;

namespace Vint.Core.ECS.Events.Entrance.Login;

[ProtocolId(1439375251389)]
public class IntroduceUserByUidEvent : IntroduceUserEvent {
    [ProtocolName("Uid")] public string Username { get; private set; } = null!;

    public override async Task Execute(IPlayerConnection connection, IEntity[] entities) {
        ILogger logger = connection.Logger.ForType<IntroduceUserByUidEvent>();

        logger.Information("Login by username '{Username}'", Username);

        await using DbConnection db = new();

        Player? player = await db.GetSelfPlayerByUsername(Username);

        if (player == null) {
            connection.Player = null!;
            await connection.Send(new UidInvalidEvent());
            await connection.Send(new LoginFailedEvent());
            return;
        }

        connection.Player = player;
        await connection.Send(new PersonalPasscodeEvent());
    }
}
