using System;
using System.Collections.Generic;
using battleships.ShipParts;
using Godot;
using Godot.Collections;

namespace battleships.Utils;

public static class Utils
{
    public static List<T> GetSetDifference<T>(List<T> a, List<T> b)
    {
        List<T>  result = new();
        foreach (var item in a)
        {
            if (!b.Contains(item))
            {
                result.Add(item);
            }
        }

        return result;
    }

    public static Vector2 GetRandomVector(int maxRadius = 1)
    {
        return new Vector2(GD.RandRange(-maxRadius, maxRadius), GD.RandRange(-maxRadius, maxRadius));
    }
    
    public static List<List<Globals.Parts>> LoadShip(string path = "user://savegame.save")
    {
        List<List<Globals.Parts>> grid = new();
        using var saveGame = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        while (saveGame.GetPosition() < saveGame.GetLength())
        {
            var jsonString = saveGame.GetLine();
            var json = new Json();
            var parseResult = json.Parse(jsonString);
			
            if (parseResult != Error.Ok)
            {
                GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
                continue;
            }

            // Get the data from the JSON object
            var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);
			
            List<Globals.Parts> loadRow = new();

            foreach (var cell in nodeData)
            {
                Enum.TryParse((string) cell.Value, out Globals.Parts part);
                loadRow.Add(part);
            }
            grid.Add(loadRow);
        }
        saveGame.Close();
        return grid;
    }
}