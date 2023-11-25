using System.Collections.Generic;
using battleships.Utils;
using Godot;

namespace battleships.UI;

public partial class ShipGrid : GridContainer
{
	public readonly List<List<ShipGridButton>> Grid = new();
	
	public override void _Ready()
	{
		for (var x = 0; x < Columns; x++)
		{
			Grid.Add(new List<ShipGridButton>());
			Globals.ShipGrid.Add(new List<Globals.Parts>());
			for (var y = 0; y < Columns; y++)
			{
				var button = new ShipGridButton(new Vector2I(x, y));
				Grid[x].Add(button);
				Globals.ShipGrid[x].Add(Globals.Parts.None);
				AddChild(button);
			}
		}
	}
}