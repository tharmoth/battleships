using Godot;

namespace battleships.ShipParts;

public partial class Bridge : Hull
{
    public Bridge(Ship ship) : base(ship)
    {
        Sprite.Texture = GD.Load<Texture2D>("res://res/parts/bridge.png");
    }
}