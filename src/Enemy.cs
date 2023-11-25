using System.Collections.Generic;
using battleships.ShipParts;
using Godot;
using Turret = battleships.ShipParts.Turret;

namespace battleships;

public partial class Enemy : Ship
{
    private readonly Ship _target;
    private int _health = 10;

    public Enemy(Vector2 position, Ship target)
    {
        Position = position;
        _target = target;

        for (var x = 0; x <= 2; x++)
        {
            HullGrid.Add(new List<Hull> 
            {
                new(this), 
                new(this),
                new(this)
            });
        }
        
        HullGrid.Add(new List<Hull> 
        {
            new(this), 
            new Turret(this, "railgun", .75),
            new(this)
        });
        
        HullGrid.Add(new List<Hull> 
        {
            new(this), 
            new Bridge(this),
            new(this)
        });
        
        HullGrid.Add(new List<Hull> 
        {
            new(this), 
            new Turret(this, "railgun", .75),
            new(this)
        });
        
        for (var x = 0; x <= 2; x++)
        {
            HullGrid.Add(new List<Hull> 
            {
                new(this), 
                new(this),
                new(this)
            });
        }

        HullGrid.Add(new List<Hull> 
        {
            null, 
            new ShipEngine(this), 
            null
        });
    }

    public override Vector2 GetTargetPosition()
    {
        if (!IsInstanceValid(_target)) return Vector2.Zero;
        // Calculate a leading position for the target for the railgun.
        var targetPosition = _target.GlobalPosition;
        var targetVelocity = _target.Velocity;
        var distance = (targetPosition - GlobalPosition).Length();
        var timeToHit = distance / 1000;
        var targetPositionAtTimeToHit = targetPosition + targetVelocity * (float) timeToHit;
        return targetPositionAtTimeToHit;
    }

    public override bool IsPlayer()
    {
        return false;
    }
    
    public override bool ShouldFire()
    {
        return true;
    }
}