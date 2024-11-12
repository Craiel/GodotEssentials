namespace Craiel.Essentials.Input;

using Godot;

public static class InputDirectionUtils
{
    private static readonly Vector2 NorthWestVector = new Vector2(-1, -1);
    private static readonly Vector2 NorthVector = new Vector2(0, -1);
    private static readonly Vector2 NorthEastVector = new Vector2(1, -1);
    private static readonly Vector2 EastVector = new Vector2(1, 0);
    private static readonly Vector2 SouthEastVector = new Vector2(1, 1);
    private static readonly Vector2 SouthVector = new Vector2(0, 1);
    private static readonly Vector2 SouthWestVector = new Vector2(-1, 1);
    private static readonly Vector2 WestVector = new Vector2(-1, 0);
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static InputDirection8 GetDirection8(Vector2 vector)
    {
        var direction = new Vector2(Mathf.Sign(vector.X), Mathf.Sign(vector.Y));
        
        if (direction == NorthWestVector) return InputDirection8.NW;
        if (direction == NorthVector) return InputDirection8.N;
        if (direction == NorthEastVector) return InputDirection8.NE;
        if (direction == EastVector) return InputDirection8.E;
        if (direction == SouthEastVector) return InputDirection8.SE;
        if (direction == SouthVector) return InputDirection8.S;
        if (direction == SouthWestVector) return InputDirection8.SW;
        if (direction == WestVector) return InputDirection8.W;

        return InputDirection8.E;
    }
}