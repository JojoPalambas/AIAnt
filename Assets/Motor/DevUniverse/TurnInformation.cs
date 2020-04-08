using System.Collections.Generic;

public class TurnInformation
{
    public readonly TerrainType terrainType;

    public readonly PastTurnDigest pastTurn;
    public readonly AntMindset mindset;

    public readonly List<PheromoneDigest> pheromones;
    public readonly Dictionary<HexDirection, List<PheromoneDigest>> adjacentPheromoneGroups;

    public readonly Value energy;
    public readonly Value hp;
    public readonly Value carriedFood;

    public readonly AnalyseReport analyseReport;
    public readonly CommunicateReport communicateReport;

    public readonly List<EventInput> eventInputs;

    public readonly int id;

    public TurnInformation(
        TerrainType terrainType,
        PastTurnDigest pastTurn,
        AntMindset mindset,
        List<PheromoneDigest> pheromones,
        Dictionary<HexDirection, List<PheromoneDigest>> adjacentPheromoneGroups,
        Value energy,
        Value hp,
        Value carriedFood,
        AnalyseReport analyseReport,
        CommunicateReport communicateReport,
        List<EventInput> eventInputs,
        int id)
    {
        this.terrainType = terrainType;
        this.pastTurn = pastTurn;
        this.mindset = mindset;
        this.pheromones = pheromones;
        this.adjacentPheromoneGroups = adjacentPheromoneGroups;
        this.energy = energy;
        this.hp = hp;
        this.carriedFood = carriedFood;
        this.analyseReport = analyseReport;
        this.communicateReport = communicateReport;
        this.eventInputs = eventInputs;
        this.id = id;
    }
}
