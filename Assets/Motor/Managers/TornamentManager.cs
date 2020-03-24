using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornamentManager : MonoBehaviour
{
    [Header("General")]
    public GameManager gameManagerPrefab;
    private GameManager gameManager;

    private List<AntAI> ais;

    // Start is called before the first frame update
    void Start()
    {
        ais = new List<AntAI>();

        // This is where the fighting AIs are set
        ais.Add(new AIJojoTest());
        ais.Add(new AIJojoTest());

        gameManager = Instantiate(gameManagerPrefab, transform);
        gameManager.SetAIs(ais);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
