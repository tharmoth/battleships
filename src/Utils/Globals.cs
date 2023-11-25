using System.Collections.Generic;

namespace battleships.Utils;

public static class Globals
{
    public static Parts SelectedPart = Parts.None;
    
    public enum Parts
    {
        Bridge, Hull, Engine, Railgun, Laser, None
    }

    public static readonly List<List<Parts>> ShipGrid = new();
}