using System.Collections.Generic;
using battleships.Utils;
using Godot;

namespace battleships.UI;

public partial class SaveButton : Button
{
	public SaveButton()
	{
		Pressed += Save;
	}
	
	public void Save()
	{
		using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);
		Globals.ShipGrid.ForEach(row => saveGame.StoreLine(Json.Stringify(SaveRow(row))));
		saveGame.Close();
	}
	
	public Godot.Collections.Dictionary<string, Variant> SaveRow(List<Globals.Parts> row)
	{
		Godot.Collections.Dictionary<string, Variant> saveData = new();
		for (int i = 0; i < row.Count; i++)
		{
			saveData.Add(i.ToString(), row[i].ToString());
		}
		return saveData;
	}
}