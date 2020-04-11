using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Ant
{
    public override AntType Type
    {
        get { return AntType.QUEEN; }
    }

    // Start is called before the first frame update
    void Start()
    {
        SuperStart();
    }

    public override void Init(Team team, Vector2Int gameCoordinates, Color color)
    {
        base.Init(team, gameCoordinates, color);

        hp = Const.QUEEN_STARTING_HP;
        energy = Const.QUEEN_STARTING_ENERGY;
        carriedFood = Const.QUEEN_STARTING_FOOD;
    }

    // Update is called once per frame
    void Update()
    {
        SuperUpdate();
    }

    public override void Die()
    {
        SuperDie();
    }
}
