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

public abstract class Ant : MonoBehaviour
{
    [Header("Gameplay")]
    public Vector2Int gameCoordinates;
    public AntMindset mindset;
    public int hp;
    public int energy;
    public int carriedFood;

    [Header("Display")]
    public Vector2Int displayCoordinates;
    public HexDirection displayDirection;

    [Header("AI management")]
    public Decision decision;
    public PastTurnDigest pastTurn;

    public abstract AntType Type
    {
        get;
    }

    // Start is called before the first frame update
    protected void SuperStart()
    {
        pastTurn = null;

        hp = 100;
        energy = 100;
        carriedFood = 0;
    }

    // Update is called once per frame
    protected void SuperUpdate()
    {
    }

    public void Init(Vector2Int gameCoordinates)
    {
        this.gameCoordinates = gameCoordinates;
        this.displayCoordinates = gameCoordinates;
    }

    public void RotateToTarget(float elapsedTime, float totalTime)
    {
        // The remaining time might be 0 (especially if the animation time by turn is set to 0)
        if (elapsedTime >= totalTime)
            return;

        if (displayDirection == HexDirection.CENTER)
            return;

        float elapsedPercentage = elapsedTime / totalTime;

        // FIXME Quite inelegant way to rotate slowly to face the next tile
        Quaternion formerRotation = transform.rotation;
        transform.LookAt(CoordConverter.PlanToWorld(CoordConverter.HexToPos(CoordConverter.MoveHex(displayCoordinates, displayDirection)), transform.position.y));
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
        displayCoordinates = gameCoordinates;
    }
}
