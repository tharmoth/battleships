using battleships.ShipParts;
using Godot;

namespace battleships.UI;

public partial class HealthBar : ProgressBar
{
    public HealthBar()
    {
        ShowPercentage = false;
        MaxValue = Hull.MaxHealth;
        Value = Hull.MaxHealth;
        Position = new Vector2(-64, 0);
        CustomMinimumSize = new Vector2(120, 30);
        Modulate = new Color(1, 0, 0);
        ZIndex = 2;
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        var parent = GetParent<Hull>();
        Visible = parent.Health < Hull.MaxHealth;
        Value = parent.Health;
    }
}