
using System.Collections.Generic;

public class PheromoneDigest
{
    public readonly PheromoneType type;
    public readonly PheromoneIntensity intensity;
    public readonly List<HexDirection> inputs;
    public readonly List<HexDirection> outputs;
    public readonly bool isAllied;

    public PheromoneDigest(PheromoneType type, PheromoneIntensity intensity, List<HexDirection> inputs, List<HexDirection> outputs, bool isAllied)
    {
        this.type = type;
        this.intensity = intensity;
        this.inputs = inputs;
        this.outputs = outputs;
        this.isAllied = isAllied;
    }
}
