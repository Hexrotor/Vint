using Vint.Core.Battle.Player;
using Vint.Core.Config;
using Vint.Core.ECS.Components.Battle.Weapon;
using Vint.Core.ECS.Components.Battle.Weapon.Types;
using Vint.Core.ECS.Components.Server.Weapon;
using Vint.Core.ECS.Entities;
using Vint.Core.Server.Game.Protocol.Attributes;

namespace Vint.Core.ECS.Templates.Battle.Weapon;

[ProtocolId(-8939173357737272930)]
public class RicochetBattleItemTemplate : BulletWeaponTemplate {
    public IEntity Create(IEntity tank, Tanker tanker) {
        const string configPath = "garage/weapon/ricochet";
        IEntity entity = Create(configPath, tank, tanker);

        float energyChargePerShot = ConfigManager.GetComponent<EnergyChargePerShotPropertyComponent>(configPath).FinalValue;
        float energyRechargeSpeed = ConfigManager.GetComponent<EnergyRechargeSpeedPropertyComponent>(configPath).FinalValue;

        entity.AddComponent<RicochetComponent>();
        entity.AddComponent(new DiscreteWeaponEnergyComponent(energyRechargeSpeed, energyChargePerShot));
        return entity;
    }
}
