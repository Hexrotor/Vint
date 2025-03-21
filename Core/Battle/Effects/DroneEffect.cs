using Vint.Core.Battle.Tank;
using Vint.Core.Battle.Weapons;
using Vint.Core.ECS.Components.Battle.Effect;
using Vint.Core.ECS.Components.Battle.Weapon;
using Vint.Core.ECS.Entities;
using Vint.Core.ECS.Templates.Battle.Effect;
using Vint.Core.ECS.Templates.Battle.Weapon;

namespace Vint.Core.Battle.Effects;

public class DroneEffect : WeaponEffect {
    public DroneEffect(
        int index,
        IEntity marketEntity,
        TimeSpan duration,
        TimeSpan activationTime,
        float targetingDistance,
        float damage,
        BattleTank tank,
        int level) : base(tank, level) {
        Index = index;
        MarketEntity = marketEntity;
        ActivationTime = activationTime;
        TargetingDistance = targetingDistance;
        Damage = damage;
        Duration = duration;
    }

    public override ModuleWeaponHandler WeaponHandler { get; protected set; } = null!;

    public int Index { get; }
    IEntity MarketEntity { get; }
    TimeSpan ActivationTime { get; }
    float TargetingDistance { get; }
    float Damage { get; }

    public override async Task Activate() {
        if (IsActive) return;

        Tank.Effects.Add(this);

        WeaponEntity = new DroneWeaponTemplate().Create(Tank.Tanker);
        Entity = new DroneEffectTemplate().Create(Tank.Tanker, Duration, WeaponEntity, TargetingDistance);

        TimeSpan cooldown = TimeSpan.FromSeconds(WeaponEntity.GetComponent<WeaponCooldownComponent>().CooldownIntervalSec);
        WeaponHandler = new DroneWeaponHandler(Tank, Round.DamageCalculator, cooldown, MarketEntity, WeaponEntity, Damage);

        await ShareToAllPlayers();

        Schedule(ActivationTime,
            async () => {
                await Entity.AddComponent<EffectActiveComponent>();
                await WeaponEntity.AddComponent<ShootableComponent>();
            });

        Schedule(Duration, DeactivateInternal);
        CanBeDeactivated = false;
    }

    public override async Task Deactivate() {
        if (!CanBeDeactivated || !IsActive)
            return;

        Tank.Effects.TryRemove(this);

        await UnshareFromAllPlayers();
        Entity = null;
    }

    async Task DeactivateInternal() {
        CanBeDeactivated = true;
        await Deactivate();
    }
}
