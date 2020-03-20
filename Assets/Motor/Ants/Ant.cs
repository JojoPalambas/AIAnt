using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VariableDescriptor
{
    NONE,
    LOW,
    MEDIUM,
    HIGH
}

public enum AntType
{
    NONE,
    WORKER,
    QUEEN
}

public abstract class Ant : MonoBehaviour
{
    [Header("Gameplay")]
    public Vector2Int gameCoordinates;
    private int hp;
    private int energy;
    private int carriedFood;

    public abstract AntType Type
    {
        get;
    }

    // Start is called before the first frame update
    protected void SuperStart()
    {
    }

    // Update is called once per frame
    protected void SuperUpdate()
    {
    }
}
