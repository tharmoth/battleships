using battleships.Utils;
using Godot;

namespace battleships.ShipParts;

public partial class ShipEngine : Hull
{
    private readonly Texture2D _engineOn = GD.Load<Texture2D>("res://res/parts/engine.png");
    private readonly Texture2D _engineOff = GD.Load<Texture2D>("res://res/parts/engine_off.png");
    
    public ShipEngine(Ship ship): base(ship)
    {

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (IsDestroyed) return;

        // If the ship is moving, turn the engine on.
        Sprite.Texture = Ship.Velocity.Length() > 0 ? _engineOn : _engineOff;
        
        Rotate(delta);
        Move(delta);
    }

    private void Rotate(double delta)
    {
        if (!Ship.IsPlayer())
        {
            IRotateable.RotateShip(delta, Ship);
        } 
        else if (Input.IsActionPressed("left") )
        {
            Ship.GlobalRotation -= (float)delta * Ship.GetRotationSpeed();
        } 
        else if (Input.IsActionPressed("right"))
        {
            Ship.GlobalRotation += (float)delta * Ship.GetRotationSpeed();
        }
    }
    
    private void Move(double delta)
    {
        var velocity = Ship.Velocity;
        if (!Ship.IsPlayer() || Input.IsActionPressed("up"))
        {
            // Move in the direction of the current rotation.
            var targetVelocity = new Vector2(-Ship.Speed, 0).Rotated(Ship.GlobalRotation);
			
            velocity.X = Mathf.MoveToward(Ship.Velocity.X, targetVelocity.X, (float) delta * Ship.Speed);
            velocity.Y = Mathf.MoveToward(Ship.Velocity.Y, targetVelocity.Y, (float) delta * Ship.Speed);
        }
        else
        {
            velocity.X = Mathf.MoveToward(Ship.Velocity.X, 0, (float) delta * Ship.Speed);
            velocity.Y = Mathf.MoveToward(Ship.Velocity.Y, 0, (float) delta * Ship.Speed);
        }

        Ship.Velocity = velocity;
        Ship.MoveAndSlide();
    }
}