using Vint.Core.Battle.Tank;
using Vint.Core.Battle.Weapons;

namespace Vint.Core.Battle.Effects;

public class SapperEffect(
    float resistance,
    BattleTank tank,
    int level
) : Effect(tank, level), IDamageMultiplierEffect {
    public float Multiplier { get; } = resistance;

    public float GetMultiplier(BattleTank source, BattleTank target, IWeaponHandler weaponHandler, bool isSplash, bool isBackHit, bool isTurretHit) {
        if (target != Tank || weaponHandler is not IMineWeaponHandler)
            return 1;

        Deactivate().GetAwaiter().GetResult();
        return Multiplier;
    }

    public override Task Activate() => Task.FromResult(Tank.Effects.Add(this));

    public override Task Deactivate() => Task.FromResult(Tank.Effects.TryRemove(this));
}
