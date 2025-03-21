﻿using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Serilog;
using Vint.Core.ECS.Components;
using Vint.Core.ECS.Entities;
using Vint.Core.Server.Game.Protocol.Attributes;
using Vint.Core.Utils;

namespace Vint.Core.Server.Game.Protocol.Commands;

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
public class ComponentRemoveCommand : IServerCommand {
    [ProtocolPosition(0)] public required IEntity Entity { get; init; }
    [ProtocolVaried, ProtocolPosition(1)] public required Type Component { get; init; }

    public async Task Execute(IPlayerConnection connection, IServiceProvider serviceProvider) {
        ILogger logger = connection.Logger.ForType<ComponentRemoveCommand>();
        ClientRemovableAttribute? clientRemovable = Component.GetCustomAttribute<ClientRemovableAttribute>();

        if (clientRemovable == null) {
            logger.Error("{Component} is not in whitelist ({Entity})", Component.Name, Entity);
            /*ChatUtils.SendMessage($"ClientRemovable: {Component.Name}", ChatUtils.GetChat(connection), [connection], null);*/
            return; // maybe disconnect
        }

        IComponent component = Entity.GetComponent(Component);

        await Entity.RemoveComponent(Component, connection);
        await component.Removed(connection, Entity);

        logger.Debug("Removed {Component} from {Entity}", Component.Name, Entity);
    }

    public override string ToString() =>
        $"ComponentRemove command {{ Entity: {Entity}, Component: {Component.Name} }}";
}
