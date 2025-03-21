using Vint.Core.Battle.Player;
using Vint.Core.ECS.Components.Battle.Weapon.Types.Hammer;
using Vint.Core.ECS.Entities;
using Vint.Core.Server.Game.Protocol.Attributes;

namespace Vint.Core.ECS.Templates.Battle.Weapon;

[ProtocolId(4939169559170921259)]
public class HammerBattleItemTemplate : DiscreteWeaponTemplate {
    public IEntity Create(IEntity tank, Tanker tanker) {
        const string configPath = "garage/weapon/hammer";
        IEntity entity = Create(configPath, tank, tanker);

        entity.AddComponent<HammerComponent>();
        entity.AddComponent<MagazineReadyStateComponent>();
        entity.AddComponent<MagazineWeaponComponent>(configPath);
        entity.AddComponent<HammerPelletConeComponent>(configPath.Replace("garage", "battle"));
        return entity;
    }
}
