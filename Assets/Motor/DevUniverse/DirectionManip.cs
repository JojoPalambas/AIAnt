using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DirectionManip
{
    // Rotates the given direction clockwise by 1 step
    public static HexDirection RotateDirectionCW(HexDirection direction)
    {
        switch (direction)
        {
            case HexDirection.LEFT:
                return HexDirection.UPLEFT;
            case HexDirection.UPLEFT:
                return HexDirection.UPRIGHT;
            case HexDirection.UPRIGHT:
                return HexDirection.RIGHT;
            case HexDirection.RIGHT:
                return HexDirection.DOWNRIGHT;
            case HexDirection.DOWNRIGHT:
                return HexDirection.DOWNLEFT;
            case HexDirection.DOWNLEFT:
                return HexDirection.LEFT;
            default:
                return HexDirection.CENTER;
        }
    }
    // Rotates the given direction clockwise by 1 step
    public static HexDirection RotateDirectionCCW(HexDirection direction)
    {
        switch (direction)
        {
            case HexDirection.LEFT:
                return HexDirection.DOWNLEFT;
            case HexDirection.UPLEFT:
                return HexDirection.LEFT;
            case HexDirection.UPRIGHT:
                return HexDirection.UPLEFT;
            case HexDirection.RIGHT:
                return HexDirection.UPRIGHT;
            case HexDirection.DOWNRIGHT:
                return HexDirection.RIGHT;
            case HexDirection.DOWNLEFT:
                return HexDirection.DOWNRIGHT;
            default:
                return HexDirection.CENTER;
        }
    }
    // Gives back the opposite of the given direction
    public static HexDirection InvertDirection(HexDirection direction)
    {
        switch (direction)
        {
            case HexDirection.DOWNLEFT:
                return HexDirection.UPRIGHT;
            case HexDirection.LEFT:
                return HexDirection.RIGHT;
            case HexDirection.UPLEFT:
                return HexDirection.DOWNRIGHT;
            case HexDirection.UPRIGHT:
                return HexDirection.DOWNLEFT;
            case HexDirection.RIGHT:
                return HexDirection.LEFT;
            case HexDirection.DOWNRIGHT:
                return HexDirection.UPLEFT;
            default:
                return HexDirection.CENTER;
        }
    }
}
