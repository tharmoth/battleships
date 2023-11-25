using System.Collections.Generic;
using System.Linq;
using Godot;

namespace battleships.ShipParts.SubParts;

public partial class LazerGun : Cannon
{
    public LazerGun(Ship ship, double cooldown = 0.1) : base(ship, cooldown)
    {
        Sprite.Texture = GD.Load<Texture2D>("res://res/parts/laser.png");
    }

    protected override void Fire()
    {
        var target = GlobalPosition + new Vector2(-1, 0).Rotated(GlobalRotation) * 2000;
        GetHullsAlongLine(GlobalPosition, target).ToList().ForEach(node =>
        {
            if (node is not Hull hull) return;
            if (hull.Ship == Ship) return;
            hull.TakeDamage(1);
        });
        
        // Draw the laser
        var line = new Line2D();
        line.AddPoint(GlobalPosition);
        line.AddPoint(target);
        line.DefaultColor = new Color(1, 0, 0);
        line.Width = 2;
        GetTree().Root.AddChild(line);
        GetTree().CreateTween().TweenProperty(line, "modulate:a", 0, 1);
        GetTree().CreateTween().TweenCallback(new Callable(line, "queue_free")).SetDelay(1);
    }
    
    private HashSet<Node2D> GetHullsAlongLine(Vector2 from, Vector2 to)
    {
        // Create a ray from the player's position to the mouse cursor position
        var spaceState = GetWorld2D().DirectSpaceState;
        var query = new PhysicsRayQueryParameters2D();
        query.From = from;
        query.To = to;
        query.CollideWithAreas = true;

        // Get all intersected nodes along the line
        HashSet<Node2D> intersectedNodes = new();
        var result = spaceState.IntersectRay(query);
        int max = 100;  // Avoid infinite loops
        while (result.Count > 0)
        {
            if (max-- <= 0) break;
            
            var node = result["collider"].As<Node>();
            if (node is not Area2D area) continue;
            if (area.GetParent() is not Hull hull) continue;
			
            intersectedNodes.Add(hull);

            // Move the ray forward to check for the next collision point
            query.From = (Vector2) result["position"] - (query.From - query.To).Normalized() * 10f;

            result = spaceState.IntersectRay(query);
        }

        // Return the array of intersected nodes
        return intersectedNodes;
    }
}