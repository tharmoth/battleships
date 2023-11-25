using battleships.Utils;
using Godot;

namespace battleships.UI;

public partial class ShipGridButton : Button
{
    public readonly Vector2I GridPosition;
    public Globals.Parts Part = Globals.Parts.None;
    public ShipGridButton(Vector2I gridPosition)
    {
        GridPosition = gridPosition;
        CustomMinimumSize = new Vector2(136, 136);
        Pressed += AddPart;
    }
    
    public void SetPart(Globals.Parts part)
    {
        Part = part;
        Icon = part != Globals.Parts.None ? GD.Load<Texture2D>($"res://res/Parts/{part}.png") : null;
        Globals.ShipGrid[GridPosition.X][GridPosition.Y] = part;
    }
    
    private void AddPart()
    {
        if (Globals.SelectedPart == Globals.Parts.None) return;
        SetPart(Globals.SelectedPart);
    }

    public override void _GuiInput(InputEvent @event)
    {
        base._GuiInput(@event);
        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Right)
        {
            SetPart(Globals.Parts.None);
        }
    }
}