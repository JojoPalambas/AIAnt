using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornamentManager : MonoBehaviour
{
    [Header("General")]
    public int gamesToPlay;
    public GameManager gameManagerPrefab;
    private GameManager gameManager;

    private List<AntAI> ais;
    private Dictionary<string, float> leaderBoard;

    [Header("Graphics")]
    public List<Color> colors;

    // Start is called before the first frame update
    void Start()
    {
        ais = new List<AntAI>();
        leaderBoard = new Dictionary<string, float>();

        // This is where the fighting AIs are set
        ais.Add(new AITestPheromones());
        ais.Add(new AITestAgressive());

        Boot();
    }

    private void Boot()
    {
        Logger.Info("");
        Logger.Info("========== NEW GAME");
        Logger.Info("");

        gamesToPlay--;
        gameManager = Instantiate(gameManagerPrefab, transform);
        gameManager.tornamentManager = this;
        gameManager.SetAIs(ais);
        gameManager.teamColors = colors;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager == null)
        {
            if (gamesToPlay > 0)
            {
                Boot();
            }
            else
            {
                Die();
            }
        }
    }

    public void InitLeaderboard(List<Team> teams)
    {
        foreach (Team team in teams)
        {
            if (!leaderBoard.ContainsKey(team.aiId))
                leaderBoard.Add(team.aiId, 0);
        }
    }

    public void RegisterWinners(List<Team> teams)
    {
        string winnersString = "";

        if (teams.Count <= 0)
            Debug.Log("Perfect Tie, the several queens have died at the same time! No point is attributed.");
        foreach (Team team in teams)
        {
            winnersString += team.aiId + " ";
            if (leaderBoard.ContainsKey(team.aiId))
                leaderBoard[team.aiId] += 1f / teams.Count;
            else
                leaderBoard.Add(team.aiId, 1f / teams.Count);
        }

        Debug.Log("The game has ended! Winners: " + winnersString);
    }

    private void Die()
    {
        Debug.Log("The tornament has ended!");
        Debug.Log("Leaderboard:");
        foreach (KeyValuePair<string, float> score in leaderBoard)
        {
            Debug.Log(score.Key + " | " + score.Value);
        }

        Destroy(gameObject);
    }
}
