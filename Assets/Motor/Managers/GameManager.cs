using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject groundTilePrefab;
    public GameObject waterTilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = -9; i <= 9; i++)
        {
            for (int j = -9; j <= 9; j++)
            {
                Vector2 currentHexPosition = CoordConverter.HexToPos(new Vector2Int(i, j));
                Instantiate(groundTilePrefab, new Vector3(currentHexPosition.x, groundTilePrefab.transform.position.y, currentHexPosition.y), groundTilePrefab.transform.rotation);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
