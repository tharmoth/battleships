using Godot;

namespace battleships;

public partial class Bullet : Node2D
{
    private readonly float _speed;
    private const double BulletRangeTime = 5;
    private double _time = 0;
    public readonly Ship Ship;
    
    public Bullet(float speed, Ship ship)
    {
        _speed = speed;
        Ship = ship;
        var sprite = new Sprite2D();
        sprite.Texture = GD.Load<Texture2D>("res://res/bullet.png");
        sprite.Scale = new Vector2(1/5.0f, 1/5.0f);
        AddChild(sprite);

        var hitbox = new Area2D();
        var collisionShape = new CollisionShape2D();
        var circleShape = new CircleShape2D();
        circleShape.Radius = 10;
        collisionShape.Shape = circleShape;
        hitbox.AddChild(collisionShape);
        AddChild(hitbox);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        _time += delta;
        // Move the bullet in the direction it is facing.
        Position += new Vector2(-_speed * (float) delta, 0).Rotated(Rotation);

        if (_time > BulletRangeTime)
        {
            QueueFree();
        }
    }
}