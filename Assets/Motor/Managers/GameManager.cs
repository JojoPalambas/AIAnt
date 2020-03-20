using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        // If the map too small
        if (terrainWidth < 2 || terrainHeight < 2)
            return;

            terrain = new TileContent[terrainHeight][];
        for (int i = 0; i < terrainHeight; i++)
        {
            terrain[i] = new TileContent[terrainWidth];
            for (int j = 0; j < terrainWidth; j++)
            {
                if (groundTilePrefab != null)
                {
                    Vector2 currentHexPosition = CoordConverter.HexToPos(new Vector2Int(j, terrainHeight - i - 1));
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
                    queenPosition = new Vector2Int(terrainHeight - 1, terrainWidth - 1);
                    break;
                case 2:
                    queenPosition = new Vector2Int(terrainHeight - 1, 0);
                    break;
                case 3:
                    queenPosition = new Vector2Int(0, terrainWidth - 1);
                    break;
                default:
                    queenPosition = new Vector2Int(0, 0);
                    break;
            }

            Vector3 queenWorldPosition = CoordConverter.PlanToWorld(CoordConverter.HexToPos(queenPosition), queenPrefab.transform.position.y);
            Queen newQueen = Instantiate(queenPrefab, queenWorldPosition, queenPrefab.transform.rotation);

            teams.Add(new Team(i, newQueen));

        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
