using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #pragma warning disable CS0649
    [SerializeField]
    private TerrainType type;

    public Renderer colorRenderer;

    public TerrainType Type
    {
        get { return type; }
        set { return; }
    }

    private void Start()
    {
        colorRenderer.material.SetFloat("_Offset", Random.Range(1f, 100f));
    }
}
