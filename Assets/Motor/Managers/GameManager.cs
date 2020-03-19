using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public Tile groundTilePrefab;
    public Tile waterTilePrefab;

    [Header("Terrain")]
    public int terrainWidth;
    public int terrainHeight;
    private Tile[][] terrain;

    // Start is called before the first frame update
    void Start()
    {
        terrain = new Tile[terrainHeight][];
        for (int i = 0; i < terrainHeight; i++)
        {
            terrain[i] = new Tile[terrainWidth];
            for (int j = 0; j < terrainWidth; j++)
            {
                if (groundTilePrefab != null)
                {
                    Vector2 currentHexPosition = CoordConverter.HexToPos(new Vector2Int(j, terrainHeight - i - 1));
                    terrain[i][j] = Instantiate(groundTilePrefab, new Vector3(currentHexPosition.x, groundTilePrefab.transform.position.y, currentHexPosition.y), groundTilePrefab.transform.rotation);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
