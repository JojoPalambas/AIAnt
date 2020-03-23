using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoordConverter
{
    // Converts hexagon axial coordinates to world position coordinates
    // https://www.redblobgames.com/grids/hexagons/
    // https://en.wikipedia.org/wiki/Hexagon
    /*
     * The hex coordinates work like this:
     *       (0|0) (1|0) (2|0) (3|0)
     *    (0|1) (1|1) (2|1) (3|1)
     * (0|2) (1|2) (2|2) (3|2)
    */
    public static Vector2 HexToPos(Vector2Int hex)
    {
        // Coordinates given by the transformation of the x component of hex
        Vector2 xTransformation = new Vector2((float)(hex.x) * Mathf.Sqrt(3) / 2, 0);
        // Coordinates given by the transformation of the y component of hex
        Vector2 yTransformation = new Vector2(-(float)(hex.y) * Mathf.Sqrt(3) / 4, (float)(hex.y) * .75f);

        return xTransformation + yTransformation;
    }

    public static Vector2Int MoveHex(Vector2Int hex, HexDirection direction)
    {
        switch (direction)
        {
            case HexDirection.CENTER:
                return hex;
            case HexDirection.UPLEFT:
                return hex + new Vector2Int(-1, -1);
            case HexDirection.UPRIGHT:
                return hex + new Vector2Int(0, -1);
            case HexDirection.LEFT:
                return hex + new Vector2Int(-1, 0);
            case HexDirection.RIGHT:
                return hex + new Vector2Int(1, 0);
            case HexDirection.DOWNLEFT:
                return hex + new Vector2Int(0, 1);
            case HexDirection.DOWNRIGHT:
                return hex + new Vector2Int(1, 1);
            default:
                return hex;
        }
    }

    public static Vector3 PlanToWorld(Vector2 coord, float height)
    {
        return new Vector3(coord.x, height, coord.y);
    }
}
