using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStatus
{
    ROTATING,
    ANIMATING,
    THINKING,
    ACTING
}

[System.Serializable]
public class TileContent
{
    public Tile tile;
    public Ant ant;
    // public List<Pheromone> pheromones;
    public Food food;

    public TileContent(Tile tile, Food food)
    {
        this.tile = tile;
        this.food = food;
    }
}

public class Team
{
    public int teamId;
    public AntAI ai;

    public Queen queen;
    public List<Worker> workers;
    public List<Worker> newBorns; // FIXME Replace this by eggs

    public Color color;

    public Team(int teamId, Queen queen, AntAI ai, Color color)
    {
        this.teamId = teamId;
        this.ai = ai;

        this.queen = queen;
        this.workers = new List<Worker>();
        this.newBorns = new List<Worker>();

        this.color = color;
    }

    public void Die()
    {
        queen.Die();
        foreach (Worker worker in workers)
        {
            worker.Die();
        }
        foreach (Worker newBorn in newBorns)
        {
            newBorn.Die();
        }
    }
}

public class GameManager : MonoBehaviour
{
    private GameStatus status = GameStatus.THINKING;

    [Header("Prefabs")]

    [Header("Terrain")]
    public int terrainWidth;
    public int terrainHeight;
    public Tile groundTilePrefab;
    public Tile waterTilePrefab;
    public Food foodPrefab;
    private List<Vector2Int> protectedTiles;
    private TileContent[][] terrain;
    private List<Food> foods;
    public float waterProbability;
    public float foodProbability;

    [Header("Gameplay")]
    public Queen queenPrefab;
    public Worker workerPrefab;
    public List<AntAI> aisToCompete;
    private List<Team> teams;

    [Header("Animations")]
    public float animationTime;
    public float rotationTime;
    private float currentAnimationTime;
    
    [System.NonSerialized] public List<Color> teamColors;


    /*
     * HEXAGONAL TERRAIN REPRESENTATION
     * 
     * Hex form:
     *     A   B   C 
     *   D   E   F
     * G   H   I
     * 
     * Hex coordinates:
     *       (0|0) (1|0) (2|0)
     *    (0|1) (1|1) (2|1)
     * (0|2) (1|2) (2|2)
     * 
     * Terrain table form:
     * A D G
     * B E H
     * C F I
     * 
     * Terrain table coordinates :
     * (0|0) (0|1) (0|2)
     * (1|0) (1|1) (1|2)
     * (2|0) (2|1) (2|2)
    */

    // Start is called before the first frame update
    void Start()
    {
        // If the map too small
        if (terrainWidth < 4 || terrainHeight < 4)
            return;

        // Creates the protected tiles
        protectedTiles = new List<Vector2Int>();
        int index = 0;

        for (int i = 0; i < aisToCompete.Count; i++)
        {
            switch (i)
            {
                case 0:
                    protectedTiles.Add(new Vector2Int(0, 0));
                    protectedTiles.Add(new Vector2Int(1, 0));
                    protectedTiles.Add(new Vector2Int(0, 1));
                    protectedTiles.Add(new Vector2Int(1, 1));
                    break;
                case 1:
                    protectedTiles.Add(new Vector2Int(terrainWidth - 1, terrainHeight - 1));
                    protectedTiles.Add(new Vector2Int(terrainWidth - 2, terrainHeight - 1));
                    protectedTiles.Add(new Vector2Int(terrainWidth - 1, terrainHeight - 2));
                    protectedTiles.Add(new Vector2Int(terrainWidth - 2, terrainHeight - 2));
                    break;
                case 2:
                    protectedTiles.Add(new Vector2Int(terrainWidth - 1, 0));
                    protectedTiles.Add(new Vector2Int(terrainWidth - 2, 0));
                    protectedTiles.Add(new Vector2Int(terrainWidth - 1, 1));
                    protectedTiles.Add(new Vector2Int(terrainWidth - 2, 1));
                    break;
                case 3:
                    protectedTiles.Add(new Vector2Int(0, terrainHeight - 1));
                    protectedTiles.Add(new Vector2Int(1, terrainHeight - 1));
                    protectedTiles.Add(new Vector2Int(0, terrainHeight - 2));
                    protectedTiles.Add(new Vector2Int(1, terrainHeight - 2));
                    break;
                default:
                    break;
            }
        }

        // Fills the terrain with tiles
        terrain = new TileContent[terrainWidth][];
        foods = new List<Food>();
        for (int i = 0; i < terrainWidth; i++)
        {
            terrain[i] = new TileContent[terrainHeight];
            for (int j = 0; j < terrainHeight; j++)
            {
                if (groundTilePrefab != null)
                {
                    Vector2 currentTilePosition = CoordConverter.HexToPos(new Vector2Int(i, j));

                    float rand = Random.Range(0f, 1f);

                    // Water is placed if the random number picked it AND the tile is not protected
                    Tile newTile = null;
                    Food newFood = null;
                    if (!protectedTiles.Contains(new Vector2Int(i, j)) && rand < waterProbability)
                    {
                        newTile = Instantiate(waterTilePrefab, CoordConverter.PlanToWorld(currentTilePosition, waterTilePrefab.transform.position.y), waterTilePrefab.transform.rotation);
                    }
                    else
                    {
                        newTile = Instantiate(groundTilePrefab, CoordConverter.PlanToWorld(currentTilePosition, groundTilePrefab.transform.position.y), groundTilePrefab.transform.rotation);

                        rand = Random.Range(0f, 1f);
                        // Food is placed if the random number picked it AND the tile is not protected (AND the tile is not water)
                        if (!protectedTiles.Contains(new Vector2Int(i, j)) && rand < foodProbability)
                        {
                            newFood = Instantiate(foodPrefab, CoordConverter.PlanToWorld(currentTilePosition, foodPrefab.transform.position.y), foodPrefab.transform.rotation);
                            foods.Add(newFood);
                        }
                    }

                    terrain[i][j] = new TileContent(newTile, newFood);
                }
            }
        }

        // Instantiate the teams (number inferior or equal to 4)
        teams = new List<Team>();
        index = 0;
        foreach (AntAI ai in aisToCompete)
        {
            Vector2Int queenPosition = new Vector2Int();
            switch (index)
            {
                case 0:
                    queenPosition = new Vector2Int(0, 0);
                    break;
                case 1:
                    queenPosition = new Vector2Int(terrainWidth - 1, terrainHeight - 1);
                    break;
                case 2:
                    queenPosition = new Vector2Int(terrainWidth - 1, 0);
                    break;
                case 3:
                    queenPosition = new Vector2Int(0, terrainHeight - 1);
                    break;
                default:
                    queenPosition = new Vector2Int(0, 0);
                    break;
            }

            Vector3 queenWorldPosition = CoordConverter.PlanToWorld(CoordConverter.HexToPos(queenPosition), queenPrefab.transform.position.y);
            Queen newQueen = Instantiate(queenPrefab, queenWorldPosition, queenPrefab.transform.rotation);

            terrain[queenPosition.x][queenPosition.y].ant = newQueen;

            Color teamColor = teamColors.Count > index ? teamColors[index] : new Color(255, 255, 255);
            Team newTeam = new Team(index, newQueen, ai, teamColor);
            teams.Add(newTeam);

            newQueen.Init(newTeam, queenPosition, teamColor);

            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<Team> winningTeams = null;
        switch (status)
        {
            case GameStatus.ROTATING:

                currentAnimationTime -= Time.deltaTime;
                Rotate();

                if (currentAnimationTime <= 0)
                {
                    currentAnimationTime = animationTime;
                    status = GameStatus.ANIMATING;
                    break;
                }

                break;

            case GameStatus.ANIMATING:

                currentAnimationTime -= Time.deltaTime;
                Animate();

                if (currentAnimationTime <= 0)
                {
                    status = GameStatus.THINKING;
                    break;
                }

                break;

            case GameStatus.THINKING:

                Logger.Info("========== NEW TURN");
                Logger.Info("===== THINK");

                FixAllAnimations();
                winningTeams = CheckForWin();

                if (winningTeams != null && winningTeams.Count > 0)
                    break;

                Think();
                
                status = GameStatus.ACTING;
                break;

            case GameStatus.ACTING:

                Logger.Info("===== ACT");

                Act();

                currentAnimationTime = rotationTime;
                status = GameStatus.ROTATING;
                break;

            default:

                Debug.LogWarning("Illegal game status: " + status.ToString());
                break;
        }

        if (winningTeams != null && winningTeams.Count > 0)
        {
            // Announce victory / tie
        }
    }

    public void SetAIs(List<AntAI> ais)
    {
        aisToCompete = new List<AntAI>();
        foreach (AntAI ai in ais)
        {
            aisToCompete.Add(ai);
        }
    }

    private void Rotate()
    {
        foreach (Team team in teams)
        {
            team.queen.RotateToTarget(rotationTime - currentAnimationTime, rotationTime);

            foreach (Worker worker in team.workers)
            {
                worker.RotateToTarget(rotationTime - currentAnimationTime, rotationTime);
            }
        }
    }

    private void Animate()
    {
        foreach (Team team in teams)
        {
            team.queen.MoveToTarget(animationTime - currentAnimationTime, animationTime);

            foreach (Worker worker in team.workers)
            {
                worker.MoveToTarget(animationTime - currentAnimationTime, animationTime);
            }
        }

        foreach (Food food in foods)
        {
            if (food == null)
                continue;

            food.ScaleToTarget(animationTime - currentAnimationTime, animationTime);
        }
    }

    private void FixAllAnimations()
    {
        foreach (Team team in teams)
        {
            team.queen.FixAnimation();
            
            foreach (Worker worker in team.workers)
            {
                worker.FixAnimation();
            }
        }

        foreach (Food food in foods)
        {
            if (food == null)
                continue;

            food.FixAnimation();
        }
        foods.RemoveAll(food => food == null);
    }

    private List<Team> CheckForWin()
    {
        return null;
    }

    private void Think() // FIXME Factorize the two parts of this method
    {
        foreach (Team team in teams)
        {
            team.queen.decision = team.ai.OnQueenTurn(new TurnInformation(
                terrain[team.queen.gameCoordinates.x][team.queen.gameCoordinates.y].tile.Type,
                team.queen.pastTurn != null ? team.queen.pastTurn.DeepCopy() : null,
                team.queen.mindset,
                null,
                ValueConverter.Convert(team.queen.energy),
                ValueConverter.Convert(team.queen.hp),
                ValueConverter.Convert(team.queen.carriedFood),
                team.queen.analyseReport,
                team.queen.communicateReport,
                team.queen.eventInputs,
                team.queen.GetInstanceID()
            ));
            team.queen.displayDirection = team.queen.decision.choice.direction;
            team.queen.ClearInputs(); // The inputs are flushed here so they can be filled up by the resolution of the actions

            foreach (Worker worker in team.workers)
            {
                worker.decision = team.ai.OnWorkerTurn(new TurnInformation(
                   terrain[worker.gameCoordinates.x][worker.gameCoordinates.y].tile.Type,
                   worker.pastTurn != null ? worker.pastTurn.DeepCopy() : null,
                   worker.mindset,
                   null,
                   ValueConverter.Convert(worker.energy),
                   ValueConverter.Convert(worker.hp),
                   ValueConverter.Convert(worker.carriedFood),
                   worker.analyseReport,
                   worker.communicateReport,
                   worker.eventInputs,
                   worker.GetInstanceID()
                ));
                worker.displayDirection = worker.decision.choice.direction;
                worker.ClearInputs(); // The inputs are flushed here so they can be filled up by the resolution of the actions
            }
        }
    }

    private void Act()
    {
        // Makes all the teams play
        foreach (Team team in teams)
        {
            // Makes the queen and all the workers resolve their actions
            ResolveDecision(team.queen);
            foreach (Worker worker in team.workers)
            {
                ResolveDecision(worker);
            }

            // Makes all the newborns adult ants
            foreach (Worker newBorn in team.newBorns)
            {
                team.workers.Add(newBorn);
            }
            team.newBorns = new List<Worker>();
        }
        List<Team> finishedTeams = new List<Team>();
        foreach (Team team in teams)
        {
            // Makes all the ands that shoudl die die
            if (team.queen.shouldDie)
                finishedTeams.Add(team);

            List<Worker> toDie = new List<Worker>();
            foreach (Worker worker in team.workers)
            {
                if (worker.shouldDie)
                    toDie.Add(worker);
            }
            foreach (Worker worker in toDie)
            {
                team.workers.Remove(worker);
                Destroy(worker.gameObject);
            }
        }
        foreach (Team team in finishedTeams)
        {
            team.Die();
            teams.Remove(team);
        }
    }

    private void ResolveDecision(Ant ant)
    {
        TurnError error = TreatDecision(ant);

        ant.pastTurn = new PastTurnDigest(ant.decision, error);
    }

    private TurnError TreatDecision(Ant ant)
    {
        Decision decision = ant.decision;

        if (decision.choice == null)
            return TurnError.ILLEGAL;

        switch (decision.choice.type)
        {
            case ActionType.NONE:
                return TurnError.NONE;

            case ActionType.MOVE:
                return ActMove(ant, decision.choice.direction);

            case ActionType.ATTACK:
                return ActAttack(ant, decision.choice.direction);

            case ActionType.EAT:
                return ActEat(ant, decision.choice.direction, decision.choice.quantity);

            case ActionType.STOCK:
                return ActStock(ant, decision.choice.direction, decision.choice.quantity);

            case ActionType.GIVE:
                return ActGive(ant, decision.choice.direction, decision.choice.quantity);

            case ActionType.ANALYSE:
                return ActAnalyse(ant, decision.choice.direction);

            case ActionType.COMMUNICATE:
                return ActCommunicate(ant, decision.choice.direction, decision.choice.word);

            case ActionType.EGG:
                return ActEgg(ant, decision.choice.direction);

            default:
                Debug.LogWarning("Unknwo ActionType: " + decision.choice.type);
                return TurnError.ILLEGAL;

        }
    }

    private TurnError ActMove(Ant ant, HexDirection direction)
    {
        if (direction == HexDirection.CENTER)
            return TurnError.ILLEGAL;

        Vector2Int newCoord = CoordConverter.MoveHex(ant.gameCoordinates, direction);

        TurnError tileError = CheckWalkability(newCoord);
        if (tileError != TurnError.NONE)
        {
            if (tileError == TurnError.COLLISION_ANT)
            {
                terrain[newCoord.x][newCoord.y].ant.eventInputs.Add(new EventInputBump(CoordConverter.InvertDirection(direction)));
                Logger.Info(ant.GetInstanceID().ToString() + " bumps (mvt) into " + terrain[newCoord.x][newCoord.y].ant.GetInstanceID().ToString() + " at position " + direction);
            }
            return tileError;
        }

        if (ant.CheckEnergy(Const.MOVE_COST))
            ant.UpdateEnergy(-Const.MOVE_COST);
        else
            return TurnError.NO_ENERGY;

        terrain[ant.gameCoordinates.x][ant.gameCoordinates.y].ant = null;
        terrain[newCoord.x][newCoord.y].ant = ant;
        ant.gameCoordinates = newCoord;

        return TurnError.NONE;
    }

    private TurnError ActAttack(Ant ant, HexDirection direction)
    {
        if (direction == HexDirection.CENTER)
            return TurnError.ILLEGAL;

        Vector2Int target = CoordConverter.MoveHex(ant.gameCoordinates, direction);

        TurnError tileError = CheckAttackability(target, ant);
        if (tileError != TurnError.NONE)
        {
            if (tileError == TurnError.NOT_ENEMY)
            {
                terrain[target.x][target.y].ant.eventInputs.Add(new EventInputBump(CoordConverter.InvertDirection(direction)));
                Logger.Info(ant.GetInstanceID().ToString() + " bumps (atk) into " + terrain[target.x][target.y].ant.GetInstanceID().ToString() + " at position " + direction);
            }
            return tileError;
        }

        if (ant.CheckEnergy(Const.ATTACK_COST))
            ant.UpdateEnergy(-Const.ATTACK_COST);
        else
            return TurnError.NO_ENERGY;

        Ant victim = terrain[target.x][target.y].ant;
        if (ant.Type == AntType.QUEEN)
            victim.Hurt(Const.QUEEN_ATTACK_DMG);
        else
            victim.Hurt(Const.WORKER_ATTACK_DMG);

        victim.eventInputs.Add(new EventInputAttack(CoordConverter.InvertDirection(direction)));
        Logger.Info(ant.GetInstanceID().ToString() + " attacks " + victim.GetInstanceID().ToString() + " at position " + direction);

        return TurnError.NONE;
    }

    private TurnError ActEat(Ant ant, HexDirection direction, int quantity)
    {
        if (direction == HexDirection.CENTER) // FIXME This should be legal
            return TurnError.ILLEGAL;

        Vector2Int target = CoordConverter.MoveHex(ant.gameCoordinates, direction);

        TurnError tileError = CheckEdibility(target);
        if (tileError != TurnError.NONE)
        {
            if (tileError == TurnError.COLLISION_ANT)
            {
                terrain[target.x][target.y].ant.eventInputs.Add(new EventInputBump(CoordConverter.InvertDirection(direction)));
                Logger.Info(ant.GetInstanceID().ToString() + " bumps (eat) into " + terrain[target.x][target.y].ant.GetInstanceID().ToString() + " at position " + direction);
            }
            return tileError;
        }

        Food victim = terrain[target.x][target.y].food;
        int quantityToEat = Mathf.Min(quantity, Const.MAX_FOOD_BY_TURN);
        quantityToEat = victim.GetFood(quantityToEat);

        // The ant can eat more than it can store, so that it can remove food from the terrain if needed
        ant.UpdateEnergy(quantityToEat);

        return TurnError.NONE;
    }

    private TurnError ActStock(Ant ant, HexDirection direction, int quantity)
    {
        if (direction == HexDirection.CENTER)
            return TurnError.ILLEGAL;

        Vector2Int target = CoordConverter.MoveHex(ant.gameCoordinates, direction);

        TurnError tileError = CheckEdibility(target);
        if (tileError != TurnError.NONE)
        {
            if (tileError == TurnError.COLLISION_ANT)
            {
                terrain[target.x][target.y].ant.eventInputs.Add(new EventInputBump(CoordConverter.InvertDirection(direction)));
                Logger.Info(ant.GetInstanceID().ToString() + " bumps (stk) into " + terrain[target.x][target.y].ant.GetInstanceID().ToString() + " at position " + direction);
            }
            return tileError;
        }

        Food victim = terrain[target.x][target.y].food;
        int quantityToStock = Mathf.Min(quantity, Const.MAX_STOCK_BY_TURN);
        quantityToStock = victim.GetFood(quantityToStock);

        // The ant can eat more than it can store, so that it can remove food from the terrain if needed
        ant.UpdateStock(quantityToStock);

        return TurnError.NONE;
    }

    private TurnError ActGive(Ant ant, HexDirection direction, int quantity)
    {
        if (direction == HexDirection.CENTER)
            return TurnError.ILLEGAL;

        Vector2Int target = CoordConverter.MoveHex(ant.gameCoordinates, direction);

        TurnError tileError = CheckGivability(target, ant);
        if (tileError != TurnError.NONE)
        {
            if (tileError == TurnError.NOT_ALLY)
            {
                terrain[target.x][target.y].ant.eventInputs.Add(new EventInputBump(CoordConverter.InvertDirection(direction)));
                Logger.Info(ant.GetInstanceID().ToString() + " bumps (giv) into " + terrain[target.x][target.y].ant.GetInstanceID().ToString() + " at position " + direction);
            }
            return tileError;
        }

        if (ant.CheckEnergy(Const.GIVE_COST))
            ant.UpdateEnergy(-Const.GIVE_COST);
        else
            return TurnError.NO_ENERGY;

        Ant beneficiary = terrain[target.x][target.y].ant;

        // Calculates how much to give
        int quantityToGive = Mathf.Min(new int[] { quantity, Const.MAX_GIFT_BY_TURN, ant.carriedFood });
        quantityToGive -= ant.UpdateStock(-quantityToGive);

        // Give the energy to the beneficiary, then gives back the excess to the giver
        int quantityToGiveBack = quantityToGive - beneficiary.UpdateEnergy(quantityToGive);
        ant.UpdateStock(quantityToGiveBack);

        return TurnError.NONE;
    }

    private TurnError ActCommunicate(Ant ant, HexDirection direction, AntWord word)
    {
        if (direction == HexDirection.CENTER)
            return TurnError.ILLEGAL;

        Vector2Int target = CoordConverter.MoveHex(ant.gameCoordinates, direction);

        TurnError tileError = CheckCommunicability(target, ant);
        if (tileError != TurnError.NONE)
        {
            if (tileError == TurnError.NOT_ALLY)
            {
                terrain[target.x][target.y].ant.eventInputs.Add(new EventInputBump(CoordConverter.InvertDirection(direction)));
                Logger.Info(ant.GetInstanceID().ToString() + " bumps (com) into " + terrain[target.x][target.y].ant.GetInstanceID().ToString() + " at position " + direction);
            }
            return tileError;
        }

        if (ant.CheckEnergy(Const.GIVE_COST))
            ant.UpdateEnergy(-Const.GIVE_COST);
        else
            return TurnError.NO_ENERGY;

        Ant receptor = terrain[target.x][target.y].ant;

        Logger.Info(ant.GetInstanceID().ToString() + " communicates with " + receptor.GetInstanceID().ToString() + " at position " + direction);

        // Gives the info to the emitter
        ant.communicateReport = new CommunicateReport(
            receptor.Type,
            receptor.mindset,
            ValueConverter.Convert(receptor.hp),
            ValueConverter.Convert(receptor.energy),
            ValueConverter.Convert(receptor.carriedFood),
            AntWord.NONE);

        // Gives the cmmunication to the receptor
        receptor.eventInputs.Add(new EventInputComunicate(CoordConverter.InvertDirection(direction), new CommunicateReport(
            ant.Type,
            ant.mindset,
            ValueConverter.Convert(ant.hp),
            ValueConverter.Convert(ant.energy),
            ValueConverter.Convert(ant.carriedFood),
            word)));

        return TurnError.NONE;
    }

    private TurnError ActAnalyse(Ant ant, HexDirection direction)
    {
        if (direction == HexDirection.CENTER)
            return TurnError.ILLEGAL;

        Vector2Int target = CoordConverter.MoveHex(ant.gameCoordinates, direction);

        TurnError tileError = CheckAnalyzability(target);
        if (tileError != TurnError.NONE)
            return tileError;

        // Set all the fields of the response to 0
        TerrainType terrainType = TerrainType.NONE;
        AntType antType = AntType.NONE;
        bool isAllied = false;
        Value foodValue = Value.NONE;

        if (terrain[target.x][target.y] == null || terrain[target.x][target.y].tile == null) { } // Leave everything like that
        else
        {
            terrainType = terrain[target.x][target.y].tile.Type;

            if (terrain[target.x][target.y].ant == null) { } // Leave everything like that
            else
            {
                antType = terrain[target.x][target.y].ant.Type;
                isAllied = terrain[target.x][target.y].ant.team.teamId == ant.team.teamId;
            }

            if (terrain[target.x][target.y].food == null) { } // Leave everything like that
            else
            {
                foodValue = ValueConverter.Convert(terrain[target.x][target.y].food.value);
            }
        }

        ant.analyseReport = new AnalyseReport(terrainType, antType, isAllied, foodValue, null);
        Logger.Info(ant.GetInstanceID().ToString() + " analyzes: " + ant.analyseReport.ToString());

        return TurnError.NONE;
    }

    private TurnError ActEgg(Ant ant, HexDirection direction)
    {
        if (direction == HexDirection.CENTER)
            return TurnError.ILLEGAL;

        if (ant.Type != AntType.QUEEN)
            return TurnError.NOT_QUEEN;

        Vector2Int eggCoord = CoordConverter.MoveHex(ant.gameCoordinates, direction);

        TurnError tileError = CheckWalkability(eggCoord);
        if (tileError != TurnError.NONE)
        {
            if (tileError == TurnError.COLLISION_ANT)
            {
                terrain[eggCoord.x][eggCoord.y].ant.eventInputs.Add(new EventInputBump(CoordConverter.InvertDirection(direction)));
                Logger.Info(ant.GetInstanceID().ToString() + " bumps (egg) into " + terrain[eggCoord.x][eggCoord.y].ant.GetInstanceID().ToString() + " at position " + direction);
            }
            return tileError;
        }

        if (ant.CheckEnergy(Const.EGG_COST))
            ant.UpdateEnergy(-Const.EGG_COST);
        else
            return TurnError.NO_ENERGY;

        Vector3 newAntWorldPosition = CoordConverter.PlanToWorld(CoordConverter.HexToPos(eggCoord), workerPrefab.transform.position.y);
        Worker newWorker = Instantiate(workerPrefab, newAntWorldPosition, workerPrefab.transform.rotation);
        newWorker.Init(ant.team, eggCoord, ant.team.color);

        ant.team.newBorns.Add(newWorker);
        terrain[eggCoord.x][eggCoord.y].ant = newWorker;

        return TurnError.NONE;
    }

    private bool CheckCoordinatesValidity(Vector2Int coord)
    {
        return coord.x >= 0 && coord.y >= 0 && coord.x < terrainWidth && coord.y < terrainHeight;
    }

    // Checks that a tile can be walked in
    private TurnError CheckWalkability(Vector2Int coord)
    {
        if (!CheckCoordinatesValidity(coord))
            return TurnError.COLLISION_BOUNDS;

        TileContent tileContent = terrain[coord.x][coord.y];
        if (tileContent == null)
        {
            Debug.Log("Tile content does not exist at coordinates " + coord.ToString());
            return TurnError.COLLISION_VOID;
        }
        if (tileContent.ant != null)
            return TurnError.COLLISION_ANT;
        if (tileContent.food != null)
            return TurnError.COLLISION_FOOD;
        if (tileContent.tile == null)
            return TurnError.COLLISION_VOID;
        if (tileContent.tile.Type != TerrainType.GROUND)
        {
            return TurnError.COLLISION_WATER;
        }

        return TurnError.NONE;
    }

    // Checks that a tile can be walked in
    private TurnError CheckAttackability(Vector2Int coord, Ant attacker)
    {
        if (!CheckCoordinatesValidity(coord))
            return TurnError.COLLISION_BOUNDS;

        TileContent tileContent = terrain[coord.x][coord.y];
        if (tileContent == null)
        {
            Debug.Log("Tile content does not exist at coordinates " + coord.ToString());
            return TurnError.COLLISION_VOID;
        }

        if (tileContent.ant == null)
            return TurnError.NO_TARGET;
        if (tileContent.ant.team.teamId == attacker.team.teamId)
            return TurnError.NOT_ENEMY;

        return TurnError.NONE;
    }

    // Checks that a tile can be walked in
    private TurnError CheckGivability(Vector2Int coord, Ant giver)
    {
        if (!CheckCoordinatesValidity(coord))
            return TurnError.COLLISION_BOUNDS;

        TileContent tileContent = terrain[coord.x][coord.y];
        if (tileContent == null)
        {
            Debug.Log("Tile content does not exist at coordinates " + coord.ToString());
            return TurnError.COLLISION_VOID;
        }

        if (tileContent.ant == null)
            return TurnError.NO_TARGET;
        if (tileContent.ant.team.teamId != giver.team.teamId)
            return TurnError.NOT_ALLY;

        return TurnError.NONE;
    }

    // Checks that a tile can be walked in
    private TurnError CheckAnalyzability(Vector2Int coord)
    {
        if (!CheckCoordinatesValidity(coord))
            return TurnError.COLLISION_BOUNDS;

        return TurnError.NONE;
    }

    // Checks that a tile can be walked in
    private TurnError CheckCommunicability(Vector2Int coord, Ant emitter)
    {
        if (!CheckCoordinatesValidity(coord))
            return TurnError.COLLISION_BOUNDS;

        TileContent tileContent = terrain[coord.x][coord.y];
        if (tileContent == null)
        {
            Debug.Log("Tile content does not exist at coordinates " + coord.ToString());
            return TurnError.COLLISION_VOID;
        }

        if (tileContent.ant == null)
            return TurnError.NO_TARGET;
        if (tileContent.ant.team.teamId != emitter.team.teamId)
            return TurnError.NOT_ALLY;

        return TurnError.NONE;
    }

    // Checks that a tile can be eaten from
    private TurnError CheckEdibility(Vector2Int coord)
    {
        if (!CheckCoordinatesValidity(coord))
            return TurnError.COLLISION_BOUNDS;

        TileContent tileContent = terrain[coord.x][coord.y];
        if (tileContent == null)
        {
            Debug.Log("Tile content does not exist at coordinates " + coord.ToString());
            return TurnError.COLLISION_VOID;
        }

        if (tileContent.food == null)
            return TurnError.NO_TARGET;
        // Should never be triggred, but gives another security
        if (tileContent.food.value <= 0)
        {
            tileContent.food.Die();
            return TurnError.NO_TARGET;
        }

        return TurnError.NONE;
    }
}
