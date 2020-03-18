using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoordConverter
{
    // Converts hexagon axial coordinates to world position coordinates
    // https://www.redblobgames.com/grids/hexagons/
    // https://en.wikipedia.org/wiki/Hexagon
    public static Vector2 HexToPos(Vector2Int hex)
    {
        // Coordinates given by the transformation of the x component of hex
        Vector2 xTransformation = new Vector2((float)(hex.x) * Mathf.Sqrt(3) / 2, 0);
        // Coordinates given by the transformation of the y component of hex
        Vector2 yTransformation = new Vector2((float)(hex.y) * Mathf.Sqrt(3) / 4, (float)(hex.y) * .75f);

        return xTransformation + yTransformation;
    }
}
