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

    [Header("Display")]
    public HexDirection displayDirection;

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

    public void RotateToTarget(float elapsedTime, float totalTime)
    {
        // The remaining time might be 0 (especially if the animation time by turn is set to 0)
        if (elapsedTime >= totalTime)
            return;

        float elapsedPercentage = elapsedTime / totalTime;

        // FIXME Quite inelegant way to rotate slowly to face the next tile
        Quaternion formerRotation = transform.rotation;
        transform.LookAt(CoordConverter.PlanToWorld(CoordConverter.HexToPos(CoordConverter.MoveHex(gameCoordinates, displayDirection)), transform.position.y));
        transform.rotation = Quaternion.Slerp(formerRotation, transform.rotation, elapsedPercentage);
    }

    public void MoveToTarget(float elapsedTime, float totalTime)
    {
        // The remaining time might be 0 (especially if the animation time by turn is set to 0)
        if (elapsedTime >= totalTime)
            return;

        float elapsedPercentage = elapsedTime / totalTime;
        
        Vector3 targetPosition = CoordConverter.PlanToWorld(CoordConverter.HexToPos(gameCoordinates), transform.position.y);
        transform.position = Vector3.Slerp(transform.position, targetPosition, elapsedPercentage);
    }

    public void FixAnimation()
    {
        transform.position = CoordConverter.PlanToWorld(CoordConverter.HexToPos(gameCoordinates), transform.position.y);
    }
}
