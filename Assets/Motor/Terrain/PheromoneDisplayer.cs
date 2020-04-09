using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneDisplayer : MonoBehaviour
{
    public PheromoneType type;

    public Renderer colorRenderer;
    public HexDirection displayDirection;

    private void Start()
    {
        colorRenderer.material.SetFloat("_Offset", Random.Range(-100f, 100f));
    }

    public void SetColor(Color color)
    {
        colorRenderer.material.SetColor("_Color", color);
    }
}
