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

    //[Header("Animations")]

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

    public void MoveToTarget(float remainingTime)
    {
        // The remaining time might be 0 (especially if the animation time by turn is set to 0)
        if (remainingTime <= 0)
            return;

        float elapsedPercentage = Time.deltaTime / remainingTime;
        Vector3 targetPosition = CoordConverter.PlanToWorld(CoordConverter.HexToPos(gameCoordinates), transform.position.y);

        transform.position = (targetPosition * elapsedPercentage) + (transform.position * (1 - elapsedPercentage));
    }

    public void FixAnimation()
    {
        transform.position = CoordConverter.PlanToWorld(CoordConverter.HexToPos(gameCoordinates), transform.position.y);
    }
}
