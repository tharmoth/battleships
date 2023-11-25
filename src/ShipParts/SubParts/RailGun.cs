using Godot;

namespace battleships.ShipParts.SubParts;

public partial class RailGun : Cannon
{
    public RailGun(Ship ship, double cooldown = 0.1) : base(ship, cooldown)
    {
        Sprite.Texture = GD.Load<Texture2D>("res://res/parts/railgun.png");
    }

    protected override void Fire()
    {
        var bullet = new Bullet(1000, Ship);
        bullet.Position = GlobalPosition;
        bullet.Rotation = GlobalRotation + (float) GD.RandRange(-Mathf.Pi / 16, Mathf.Pi / 16);
        GetTree().Root.AddChild(bullet);
    }
}