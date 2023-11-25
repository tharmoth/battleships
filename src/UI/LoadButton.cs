using Godot;

namespace battleships.UI;

public partial class LoadButton : Button
{
	public LoadButton()
	{
		Pressed += Load;
	}

	private void Load()
	{
		var partsGrid = Utils.Utils.LoadShip();
		var grid = GetNode<ShipGrid>("%ShipGrid");
		
		for (var x = 0; x < grid.Columns; x++)
		{
			for (var y = 0; y < grid.Columns; y++)
			{
				grid.Grid[x][y].SetPart(partsGrid[x][y]);
			}
		}
	}
}