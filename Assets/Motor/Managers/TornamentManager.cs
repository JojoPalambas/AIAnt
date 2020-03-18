using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornamentManager : MonoBehaviour
{
    [Header("General")]
    public GameManager gameManagerPrefab;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = Instantiate(gameManagerPrefab, transform);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
