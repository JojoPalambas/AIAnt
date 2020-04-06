using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [Header("Gameplay")]
    public Team team;
    public int startingRoundsBeforeCracking;
    public int roundsBeforeCracking;
    public Vector2Int gameCoordinates;

    [Header("Graphics")]
    public GameObject displayObject;

    // Start is called before the first frame update
    void Start()
    {
        float displaySize = .25f + (1 - (float) roundsBeforeCracking / (float) startingRoundsBeforeCracking) * .75f;
        displayObject.transform.localScale = new Vector3(displaySize, displaySize, displaySize);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Init(Team team, Vector2Int gameCoordinates, Color color)
    {
        this.team = team;
        this.gameCoordinates = gameCoordinates;

        roundsBeforeCracking = startingRoundsBeforeCracking + 1; // The +1 is there because the egg is going to age right after instantiating
    }

    public void ScaleToTarget(float elapsedTime, float totalTime)
    {
        // The remaining time might be 0 (especially if the animation time by turn is set to 0)
        if (elapsedTime >= totalTime)
            return;

        float elapsedPercentage = elapsedTime / totalTime;

        float displaySize = .25f + (1 - (float) roundsBeforeCracking / (float) startingRoundsBeforeCracking) * .75f;
        Vector3 targetScale = new Vector3(displaySize, displaySize, displaySize);

        displayObject.transform.localScale = Vector3.Slerp(displayObject.transform.localScale, targetScale, elapsedPercentage);
    }

    public void FixAnimation()
    {
        float displaySize = .25f + (1 - (float) roundsBeforeCracking / (float) startingRoundsBeforeCracking) * .75f;
        displayObject.transform.localScale = new Vector3(displaySize, displaySize, displaySize);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
