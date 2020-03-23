using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #pragma warning disable CS0649
    [SerializeField]
    private TerrainType type;

    public TerrainType Type
    {
        get { return type; }
        set { return; }
    }
}
