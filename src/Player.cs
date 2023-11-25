using System;
using System.Collections.Generic;
using System.Linq;
using battleships.ShipParts;
using battleships.UI;
using battleships.Utils;
using Godot;
using ShipEngine = battleships.ShipParts.ShipEngine;
using Turret = battleships.ShipParts.Turret;

namespace battleships;

public partial class Player : Ship
{
	public Player()
	{
		if (Globals.ShipGrid.Count > 0)
		{
			Globals.ShipGrid.ForEach(row => HullGrid.Add(GetHullsFromParts(row)));
		}
		else
		{
			// Load the default ship
			HullGrid.Add(new List<Hull> {new Bridge(this)});
			HullGrid.Add(new List<Hull> {new Turret(this, "railgun")});
			HullGrid.Add(new List<Hull> {new Turret(this, "laser", 2)});
			HullGrid.Add(new List<Hull> {new ShipEngine(this)});
		}
	}

	public override void _Ready()
	{
		base._Ready();
		CenterBridge();
	}

	// The bridge can occur anywhere in the ship grid. This method finds the bridge and adjust the offsets of each of
	// The hulls so that the bridge is in the center of the ship.
	private void CenterBridge()
	{
		var bridge = HullGrid.SelectMany(row => row).OfType<Bridge>().First();
		Vector2 offset = bridge.Position;
		GetChildren().OfType<Hull>().ToList().ForEach(hull => hull.Position -= offset);
	}

	private List<Hull> GetHullsFromParts(List<Globals.Parts> partsList)
	{
		var hullRow = new List<Hull>();
		// Convert each part in the row to the corret type of hull.
		foreach (var part in partsList)
		{
			GD.Print("Loading Part: " + part);
			switch (part)
			{
				case Globals.Parts.Bridge:
					hullRow.Add(new Bridge(this));
					break;
				case Globals.Parts.Hull:
					hullRow.Add(new Hull(this));
					break;
				case Globals.Parts.Engine:
					hullRow.Add(new ShipEngine(this));
					break;
				case Globals.Parts.Railgun:
					hullRow.Add(new Turret(this, "railgun"));
					break;
				case Globals.Parts.Laser:
					hullRow.Add(new Turret(this, "laser", 2));
					break;
				default:
					hullRow.Add(null);
					break;
			}
		}

		return hullRow;
	}

	public override Vector2 GetTargetPosition()
	{
		return GetGlobalMousePosition();
	}

	public override bool ShouldFire()
	{
		return Input.IsActionPressed("fire");
	}

	public override bool IsPlayer()
	{
		return true;
	}
}