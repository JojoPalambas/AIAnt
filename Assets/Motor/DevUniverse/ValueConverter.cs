using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ValueConverter
{
    public static Value Convert(int n)
    {
        if (n <= 0)
            return Value.NONE;
        if (n <= 33)
            return Value.LOW;
        if (n <= 66)
            return Value.MEDIUM;

        return Value.HIGH;
    }
    public static Value Convert(int n, int max)
    {
        if (n <= 0)
            return Value.NONE;
        if (n <= max / 3)
            return Value.LOW;
        if (n <= 2 * max / 3)
            return Value.MEDIUM;

        return Value.HIGH;
    }
}
