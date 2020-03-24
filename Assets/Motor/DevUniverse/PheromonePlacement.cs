using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromonePlacement
{
    public readonly PheromoneType type;
    public readonly PheromoneIntensity intensity;
    public readonly HexDirection input;
    public readonly HexDirection output;
    public readonly bool placeIfError; // True if the pheromone should be placed even if the action fails

    public PheromonePlacement(PheromoneType type, PheromoneIntensity intensity, HexDirection input, HexDirection output, bool placeIfError)
    {
        this.type = type;
        this.intensity = intensity;
        this.input = input;
        this.output = output;
        this.placeIfError = placeIfError;
    }
}
