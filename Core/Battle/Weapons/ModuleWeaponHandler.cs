using Vint.Core.Battle.Damage.Calculator;
using Vint.Core.Battle.Tank;
using Vint.Core.ECS.Entities;
using Vint.Core.ECS.Events.Battle.Weapon.Hit;

namespace Vint.Core.Battle.Weapons;

public abstract class ModuleWeaponHandler(
    BattleTank tank,
    IDamageCalculator damageCalculator,
    TimeSpan cooldown,
    IEntity marketEntity,
    IEntity battleEntity,
    bool damageWeakeningByDistance,
    float maxDamageDistance,
    float minDamageDistance,
    float minDamagePercent,
    float maxDamage,
    float minDamage,
    int maxHitTargets
) : IWeaponHandler {
    public IEntity MarketEntity { get; } = marketEntity;
    public IEntity BattleEntity { get; } = battleEntity;
    public float MinDamage { get; } = minDamage;
    public float MaxDamage { get; } = maxDamage;
    public IDamageCalculator DamageCalculator { get; } = damageCalculator;
    public BattleTank BattleTank { get; } = tank;
    public TimeSpan Cooldown { get; } = cooldown;
    public bool DamageWeakeningByDistance { get; } = damageWeakeningByDistance;
    public float MaxDamageDistance { get; } = maxDamageDistance;
    public float MinDamageDistance { get; } = minDamageDistance;
    public float MinDamagePercent { get; } = minDamagePercent;
    public int MaxHitTargets { get; } = maxHitTargets;

    public abstract Task Fire(HitTarget target, int targetIndex);

    public virtual Task OnTankEnable() => Task.CompletedTask;

    public virtual Task OnTankDisable() => Task.CompletedTask;

    public virtual Task Tick(TimeSpan deltaTime) => Task.CompletedTask;
}
