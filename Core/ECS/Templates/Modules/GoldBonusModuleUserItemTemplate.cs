﻿using Vint.Core.Server.Game.Protocol.Attributes;

namespace Vint.Core.ECS.Templates.Modules;

[ProtocolId(1531929899999)]
public class GoldBonusModuleUserItemTemplate : UserEntityTemplate {
    public override MarketEntityTemplate MarketTemplate => new GoldBonusModuleMarketItemTemplate();
}
