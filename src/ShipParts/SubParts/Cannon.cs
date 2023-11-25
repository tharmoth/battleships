using battleships.Utils;
using Godot;

namespace battleships.ShipParts.SubParts;

public abstract partial class Cannon : Node2D, IRotateable
{
    private const float RotationSpeed = Mathf.Pi / 2;
    private double _bulletCooldown;
    private readonly double _cooldown;
    public readonly Ship Ship;
    protected readonly Sprite2D Sprite = new();

    public Cannon(Ship ship, double cooldown = 0.1)
    {
        Ship = ship;
        _cooldown = cooldown;
        AddChild(Sprite);
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (GetParent<Hull>().IsDestroyed)
        {
            Visible = false;
            return;
        }
        _bulletCooldown += delta;
		
        if (Ship.ShouldFire() && _bulletCooldown > _cooldown)
        {
            _bulletCooldown = 0;
            Fire();
        }

        IRotateable.RotateShip(delta, this);
    }

    protected abstract void Fire();

    public Vector2 GetTargetPosition()
    {
        return Ship.GetTargetPosition();
    }

    public Vector2 GetGlobalPosition()
    {
        return GlobalPosition;
    }

    public float GetGlobalRotation()
    {
        return GlobalRotation;
    }

    public float GetRotationSpeed()
    {
        return RotationSpeed;
    }

    public void SetGlobalRotation(float rotation)
    {
        GlobalRotation = rotation;
    }
}