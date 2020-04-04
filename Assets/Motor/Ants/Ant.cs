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
    public Team team;
    public Vector2Int gameCoordinates;
    public AntMindset mindset;
    public int hp;
    public int energy;
    public int carriedFood;
    public bool shouldDie;

    [Header("Display")]
    public Vector2Int displayCoordinates;
    public HexDirection displayDirection;

    [Header("AI management")]
    public Decision decision;
    public PastTurnDigest pastTurn;
    public List<EventInput> eventInputs;

    [Header("Graphics")]
    public Renderer colorRenderer;
    [System.NonSerialized] public Color teamColor;

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

        eventInputs = new List<EventInput>();
    }
    protected void SuperStartLate()
    {
        colorRenderer.material.SetColor("_Color", teamColor);
        colorRenderer.material.SetFloat("_Hp", (float) hp / 100);
        colorRenderer.material.SetFloat("_Energy", (float) energy / 100);
    }

    // Update is called once per frame
    protected void SuperUpdate()
    {
    }

    public void Init(Team team, Vector2Int gameCoordinates, Color color)
    {
        this.team = team;
        this.gameCoordinates = gameCoordinates;
        this.displayCoordinates = gameCoordinates;
        this.teamColor = color;
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
        transform.LookAt(CoordConverter.PlanToWorld(CoordConverter.HexToPos(CoordConverter.MoveHex(displayCoordinates, displayDirection)), transform.position.y));
    }

    // Returns true if the ant dies when getting hurt
    public bool Hurt(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            shouldDie = true;
            return true;
        }
        colorRenderer.material.SetFloat("_Hp", (float) hp / 100);

        return false;
    }

    // Updates the energy by the given value, and returns the excess (in either the positive way or the negative one)
    public int UpdateEnergy(int variation)
    {
        energy += variation;

        int ret = 0;
        if (energy < 0)
        {
            ret = -energy;
            energy = 0;
        }
        else if (energy > 100)
        {
            ret = energy - 100;
            energy = 100;
        }

        colorRenderer.material.SetFloat("_Energy", (float) energy / 100);

        return ret;
    }

    // Updates the energy by the given value, and returns the excess (in either the positive way or the negative one)
    public int UpdateStock(int variation)
    {
        carriedFood += variation;

        int ret = 0;
        if (carriedFood < 0)
        {
            ret = -carriedFood;
            carriedFood = 0;
        }
        else if (carriedFood > 100)
        {
            ret = carriedFood - 100;
            carriedFood = 100;
        }

        colorRenderer.material.SetFloat("_Food", (float) carriedFood / 100);

        return ret;
    }

    public bool CheckEnergy(int energy)
    {
        return this.energy >= energy;
    }

    public bool CheckStock(int stock)
    {
        return carriedFood >= stock;
    }

    public abstract void Die();

    public void SuperDie()
    {
        Debug.Log("Destroy " + name);
        Destroy(gameObject);
    }
}
