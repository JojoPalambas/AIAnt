
using System.Collections.Generic;

public class TurnInformation
{
    public readonly TerrainType terrainType;

    public readonly PastTurnDigest pastTurn;
    public readonly AntMindset mindset;

    public readonly List<PheromoneDigest> pheromones;

    public readonly Value energy;
    public readonly Value hp;
    public readonly Value carriedFood;

    public readonly AnalyseReport analyseReport;
    public readonly CommunicateReport communicateReport;

    public readonly string id;

    public TurnInformation(
        TerrainType terrainType,
        PastTurnDigest pastTurn,
        AntMindset mindset,
        List<PheromoneDigest>
        pheromones,
        Value energy,
        Value hp,
        Value carriedFood,
        AnalyseReport analyseReport,
        CommunicateReport communicateReport,
        string id)
    {
        this.terrainType = terrainType;
        this.pastTurn = pastTurn;
        this.mindset = mindset;
        this.pheromones = pheromones;
        this.energy = energy;
        this.hp = hp;
        this.carriedFood = carriedFood;
        this.analyseReport = analyseReport;
        this.communicateReport = communicateReport;
        this.id = id;
    }
}

public abstract class AntAI
{
    public abstract Decision OnWorkerTurn(TurnInformation info);
    public abstract Decision OnQueenTurn(TurnInformation info);
}
