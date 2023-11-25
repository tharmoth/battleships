using Godot;

namespace battleships.Utils;

public interface IRotateable
{
    public Vector2 GetTargetPosition();
    public Vector2 GetGlobalPosition();
    public float GetGlobalRotation();
    public float GetRotationSpeed();
    public void SetGlobalRotation(float rotation);
    
    public static void RotateShip(double delta, IRotateable rotateable)
    {
        // Calculate the angle towards the cursor.
        var targetRotation = rotateable.GetTargetPosition().AngleToPoint(rotateable.GetGlobalPosition());

        // Calculate the shortest angle difference between the current rotation and the target rotation.
        var rotationDifference = AngleDifference(rotateable.GetGlobalRotation(), targetRotation);

        // Use MoveToward with the corrected target rotation.
        rotateable.SetGlobalRotation(Mathf.MoveToward(rotateable.GetGlobalRotation(), rotateable.GetGlobalRotation() + rotationDifference, (float) delta * rotateable.GetRotationSpeed()));
    }
	
    private static float AngleDifference(float a, float b)
    {
        var diff = b - a;
        diff = (diff + Mathf.Pi) % (2 * Mathf.Pi) - Mathf.Pi;

        if (diff < -Mathf.Pi)
        {
            diff += 2 * Mathf.Pi;
        }

        return diff;
    }
}