using battleships.UI;
using Godot;

namespace battleships.ShipParts;

public partial class Hull : Node2D
{
    public readonly Ship Ship;
    protected readonly Sprite2D Sprite;

    public Vector2I GridPosition => new(Mathf.FloorToInt(Position.X / Ship.CellSize), Mathf.FloorToInt(Position.Y / Ship.CellSize));
    
    public const int MaxHealth = 10;
    public int Health = MaxHealth;
    private Tween _motionTween;
    private Tween _colorTween;
    private Area2D _hitBox = new Area2D();
    public bool IsDestroyed;
    
    public Hull(Ship ship)
    {
        Ship = ship;
        
        Sprite = new Sprite2D();
        Sprite.Texture = GD.Load<Texture2D>("res://res/parts/hull.png");
        Sprite.Material = (ShaderMaterial)GD.Load<ShaderMaterial>("res://src/Shaders/flash_material.tres").Duplicate();
        AddChild(Sprite);

        _hitBox.AreaEntered += HitboxOnAreaEntered;
        _hitBox.AddChild(GetShape(128 + 32));
        AddChild(_hitBox);

        AddChild(new HealthBar());
    }
    
    private static CollisionShape2D GetShape(float size = 64)
    {
        var collisionShape = new CollisionShape2D();
        var rectangleShape = new RectangleShape2D();
        rectangleShape.Size = Vector2.One * size;
        collisionShape.Shape = rectangleShape;
        return collisionShape;
    }
    
    private void HitboxOnAreaEntered(Area2D area)
    {
        if (area.GetParent() is Bullet bullet && bullet.Ship != Ship && Health > 0)
        {
            bullet.QueueFree();
            TakeDamage();
        }

    }

    public void TakeDamage(int amount = 1)
    {
        Health -= amount;
        Sprite.Rotation = 0;
            
        _motionTween?.Kill();
        _motionTween = GetTree().CreateTween();
        _motionTween.TweenProperty(Sprite, "rotation", Mathf.Pi / 16, 0.05f);
        _motionTween.TweenProperty(Sprite, "rotation", 0, 0.05f);

        _colorTween?.Kill();
        _colorTween = GetTree().CreateTween();
        _colorTween.TweenCallback(new Callable(this, "FlashOn"));
        _colorTween.TweenCallback(new Callable(this, "FlashOff")).SetDelay(.05f);
        
        if (Health > 0) return;
        Ship.HullDestroyed(this);
    }
    
    public void Destroy()
    {
        if (IsDestroyed) return;
        IsDestroyed = true;
        
        _motionTween?.Kill();
        _colorTween?.Kill();
        Sprite.Material = null;
        
        var time = GD.RandRange(0.5f, 1.5f);
        GetTree().CreateTween().TweenProperty(Sprite, "rotation", GD.RandRange(Mathf.Pi, Mathf.Pi * 2), time);
        GetTree().CreateTween().TweenProperty(Sprite, "scale", Vector2.Zero, time);
        GetTree().CreateTween().TweenCallback(new Callable(this, "queue_free")).SetDelay(time);
        GetTree().CreateTween().TweenProperty(Sprite, "position", Utils.Utils.GetRandomVector(Ship.CellSize), time);
        GetTree().CreateTween().TweenProperty(Sprite, "modulate:a", 0, time);
        // GetTree().CreateTween().TweenMethod(new Callable(this, "SetAlpha"), Sprite.Modulate.A, 0, time);
    }
    
    private void SetAlpha(float alpha)
    {
        Sprite.Modulate = new Color(1, 1, 1, alpha);
    }

    private void FlashOn()
    {
        Flash(true);
    }
    
    private void FlashOff()
    {
        Flash(false);
    }
    
    private void Flash(bool active)
    {
        if (Sprite.Material is ShaderMaterial mat)
        {
            mat.SetShaderParameter("active", active);
        }
    }
}