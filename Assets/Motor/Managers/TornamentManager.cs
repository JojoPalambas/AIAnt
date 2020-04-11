using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornamentManager : MonoBehaviour
{
    [Header("General")]
    public GameManager gameManagerPrefab;
    private GameManager gameManager;

    private List<AntAI> ais;

    [Header("Graphics")]
    public List<Color> colors;

    // Start is called before the first frame update
    void Start()
    {
        ais = new List<AntAI>();

        // This is where the fighting AIs are set
        ais.Add(new AITestAgressive());
        ais.Add(new AITestAgressive());

        gameManager = Instantiate(gameManagerPrefab, transform);
        gameManager.SetAIs(ais);
        gameManager.teamColors = colors;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
