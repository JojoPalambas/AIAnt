
using System.Collections.Generic;

public class AnalyseReport
{
    public readonly TerrainType terrainType;
    public readonly AntType antType;
    public readonly bool isAllied;
    public readonly Value foodValue;
    public readonly List<PheromoneDigest> pheromones;

    public AnalyseReport(TerrainType terrainType, AntType antType, bool isAllied, Value foodValue, List<PheromoneDigest> pheromones)
    {
        this.terrainType = terrainType;
        this.antType = antType;
        this.isAllied = isAllied;
        this.foodValue = foodValue;
        this.pheromones = pheromones;
    }

    public override string ToString()
    {
        return "{ terrain: " + terrainType + ", antType: " + antType + ", isAllied: " + isAllied + ", foodValue: " + foodValue + " }";
    }
}
