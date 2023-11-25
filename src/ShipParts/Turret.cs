using battleships.ShipParts.SubParts;
using battleships.Utils;
using Godot;

namespace battleships.ShipParts;

public partial class Turret : Hull
{
    public Turret(Ship ship, string type, double cooldown = .1) : base(ship)
    {
        switch (type)
        {
            case "railgun":
                AddChild(new RailGun(ship, cooldown));
                break;
            case "laser":
                AddChild(new LazerGun(ship, cooldown));
                break;
        }
    }
}