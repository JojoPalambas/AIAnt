using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStatus
{
    ANIMATION,
    THINKING
}

[System.Serializable]
public class TileContent
{
    public Tile tile;
    public Ant ant;
    // List<Pheromone> pheromones
    // Food food

    public TileContent(Tile tile, Ant ant)
    {
        this.tile = tile;
        this.ant = ant;
    }
}

public class Team
{
    public int teamId;
    public Queen queen;
    public List<Worker> workers;

    public Team(int teamId, Queen queen)
    {
        this.teamId = teamId;
        this.queen = queen;
        this.workers = new List<Worker>();
    }
}

public class GameManager : MonoBehaviour
{
    private GameStatus status = GameStatus.THINKING;

    [Header("Prefabs")]
    public Tile groundTilePrefab;
    public Tile waterTilePrefab;

    [Header("Terrain")]
    public int terrainWidth;
    public int terrainHeight;
    private TileContent[][] terrain;

    [Header("Gameplay")]
    public Queen queenPrefab;
    public Worker workerPrefab;
    public int numberOfTeams;
    private List<Team> teams;

    [Header("Animations")]
    public float turnAnimationTime;
    private float currentTurnAnimationTime;


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
        if (terrainWidth < 2 || terrainHeight < 2)
            return;

        // Fills the terrain with tiles
        terrain = new TileContent[terrainWidth][];
        for (int i = 0; i < terrainWidth; i++)
        {
            terrain[i] = new TileContent[terrainHeight];
            for (int j = 0; j < terrainHeight; j++)
            {
                if (groundTilePrefab != null)
                {
                    Vector2 currentHexPosition = CoordConverter.HexToPos(new Vector2Int(i, terrainHeight - j - 1));
                    Tile newTile = Instantiate(groundTilePrefab, new Vector3(currentHexPosition.x, groundTilePrefab.transform.position.y, currentHexPosition.y), groundTilePrefab.transform.rotation);
                    terrain[i][j] = new TileContent(newTile, null);
                }
            }
        }

        // Instantiate the teams (number inferior or equal to 4)
        teams = new List<Team>();
        for (int i = 0; i < numberOfTeams && i < 4; i++)
        {
            Vector2Int queenPosition = new Vector2Int();
            switch (i)
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
            newQueen.gameCoordinates = queenPosition;

            teams.Add(new Team(i, newQueen));

        }
    }

    // Update is called once per frame
    void Update()
    {
        List<Team> winningTeams = null;
        switch (status)
        {
            case GameStatus.THINKING:

                FixAllAnimations();
                winningTeams = CheckForWin();

                if (winningTeams != null && winningTeams.Count > 0)
                    break;

                Think();

                currentTurnAnimationTime = turnAnimationTime;
                status = GameStatus.ANIMATION;
                break;

            case GameStatus.ANIMATION:

                currentTurnAnimationTime -= Time.deltaTime;
                Animate();

                if (currentTurnAnimationTime <= 0)
                {
                    status = GameStatus.THINKING;
                    break;
                }

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
    }

    private List<Team> CheckForWin()
    {
        return null;
    }

    private void Think()
    {
        foreach (Team team in teams)
        {
            HexDirection randDirection = (HexDirection) Random.Range(1, 7);

            Vector2Int newCoord = CoordConverter.MoveHex(team.queen.gameCoordinates, randDirection);

            if (!CheckWalkability(newCoord))
                continue;

            terrain[team.queen.gameCoordinates.x][team.queen.gameCoordinates.y].ant = null;
            terrain[newCoord.x][newCoord.y].ant = team.queen;
            team.queen.gameCoordinates = newCoord;
        }
    }

    private bool CheckCoordinatesValidity(Vector2Int coord)
    {
        return coord.x >= 0 && coord.y >= 0 && coord.x < terrainWidth && coord.y < terrainHeight;
    }

    // Checks that a tile can be walked in
    private bool CheckWalkability(Vector2Int coord)
    {
        if (!CheckCoordinatesValidity(coord))
            return false;

        TileContent tileContent = terrain[coord.x][coord.y];
        if (tileContent == null)
        {
            Debug.Log("Tile content does not exist at coordinates " + coord.ToString());
            return false;
        }
        if (tileContent.ant != null)
            return false;
        if (tileContent.tile == null || tileContent.tile.Type != TerrainType.GROUND)
            return false;

        return true;
    }

    private void Animate()
    {
        foreach (Team team in teams)
        {
            team.queen.MoveToTarget(currentTurnAnimationTime);

            foreach (Worker worker in team.workers)
            {
                worker.MoveToTarget(currentTurnAnimationTime);
            }
        }
    }
}
