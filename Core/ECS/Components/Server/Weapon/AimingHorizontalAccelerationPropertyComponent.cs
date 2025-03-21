using Vint.Core.ECS.Components.Battle.Weapon.Types.Shaft;
using Vint.Core.ECS.Components.Server.Common;

namespace Vint.Core.ECS.Components.Server.Weapon;

public class AimingHorizontalAccelerationPropertyComponent : RangedComponent, IConvertible<ShaftAimingSpeedComponent> {
    public void Convert(ShaftAimingSpeedComponent component) =>
        component.HorizontalAcceleration = FinalValue;
}
