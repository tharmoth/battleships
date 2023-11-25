using System.Collections.Generic;
using System.Linq;
using battleships.ShipParts;
using battleships.Utils;
using Godot;
using Godot.Collections;

namespace battleships;

public abstract partial  class Ship : CharacterBody2D, IRotateable
{
    public const int CellSize = 128;
    public float Speed => 1800 * (Engines.Count / (float)Hulls.Count);
    private float RotationSpeed => Mathf.Pi * (Engines.Count / (float)Hulls.Count);
    protected List<ShipEngine> Engines => GetChildren().OfType<ShipEngine>().ToList();
    protected List<Hull> Hulls => GetChildren().OfType<Hull>().Where(hull => !hull.IsDestroyed).ToList();
    protected readonly List<List<Hull>> HullGrid = new();
    
    public abstract bool ShouldFire();
    public abstract Vector2 GetTargetPosition();
    public abstract bool IsPlayer();

    protected Ship()
    {
        Scale = Vector2.One * 0.5f;
    }
    
    public override void _Ready()
    {
        AddParts();
    }
    
    private void AddParts()
    {
        for (var rowIndex = 0; rowIndex < HullGrid.Count; rowIndex++)
        {
            for (var colIndex = 0; colIndex < HullGrid[rowIndex].Count; colIndex++)
            {
                var hull = HullGrid[rowIndex][colIndex];
                if (hull == null) continue;
                hull.Position = new Vector2(CellSize * rowIndex, CellSize * colIndex);
                AddChild(hull);
            }
        }
    }
    
    public void HullDestroyed(Hull hull)
    {
        RemoveHull(hull);
        List<Hull> intactHulls = new();

        var bridge = GetChildren().OfType<Bridge>().First();
        
        // Find all of the parts of the ship still connected to the bridge.
        FloodFill(bridge.GridPosition.X, bridge.GridPosition.Y, HullGrid, intactHulls);

        // Remove the parts of the ship that are no longer connected to the bridge.
        Utils.Utils.GetSetDifference(Hulls, intactHulls)
            .FindAll(IsInstanceValid)
            .ForEach(RemoveHull);

        // If there are no more hulls left, destroy the ship.
        if (Hulls.Count > 0) return;
        GetTree().CreateTween().TweenCallback(new Callable(this, "queue_free")).SetDelay(2);
    }

    private void RemoveHull(Hull hull)
    {
        HullGrid[hull.GridPosition.X][hull.GridPosition.Y] = null;
        hull.Destroy();
    }
    
    public void FloodFill(int x, int y, List<List<Hull>> grid, List<Hull> hulls)
    {
        if (x < 0 || x >= grid.Count || y < 0 || y >= grid[0].Count)
        {
            return;
        }
        
        var hull = grid[x][y];
        if (hull == null || hulls.Contains(hull))
        {
            return;
        }
        
        hulls.Add(hull);
        FloodFill(x - 1, y, grid, hulls);
        FloodFill(x + 1, y, grid, hulls);
        FloodFill(x, y - 1, grid, hulls);
        FloodFill(x, y + 1, grid, hulls);
    }
    
    public Vector2 GetGlobalPosition()
    {
        return GlobalPosition;
    }

    public float GetGlobalRotation()
    {
        return GlobalRotation;
    }

    public float GetRotationSpeed()
    {
        return RotationSpeed;
    }

    public void SetGlobalRotation(float rotation)
    {
        GlobalRotation = rotation;
    }

}