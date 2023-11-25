using System.Collections.Generic;
using System.Linq;
using battleships.Utils;
using Godot;

namespace battleships.UI;

public partial class StartButton : Button
{
	public StartButton()
	{
		Pressed += Save;
	}

	private void Save()
	{
		var grid = GetNode<ShipGrid>("%ShipGrid");
		Globals.ShipGrid.Clear();
		
		for (var x = 0; x < grid.Columns; x++)
		{
			var row = new List<Globals.Parts>();
			for (var y = 0; y < grid.Columns; y++)
			{
				row.Add(Globals.Parts.None);
			}
			Globals.ShipGrid.Add(row);
		}
		
		
		grid
			.GetChildren()
			.OfType<ShipGridButton>()
			.ToList()
			.ForEach(button =>
			{
				Globals.ShipGrid[button.GridPosition.X][button.GridPosition.Y] = button.Part;
			});
		
		// Change the scene to the game scene world.tscn.
		GetTree().ChangeSceneToFile("res://src/World.tscn");
	}
}