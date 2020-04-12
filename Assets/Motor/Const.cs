using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Const
{
    // General
    public static readonly int MAX_PLAYERS = 6;

    // Queen stats
    public static readonly int QUEEN_STARTING_HP = 100;
    public static readonly int QUEEN_STARTING_ENERGY = 100;
    public static readonly int QUEEN_STARTING_FOOD = 0;
    public static readonly int QUEEN_ATTACK_DMG = 15;

    // Worker stats
    public static readonly int WORKER_STARTING_HP = 100;
    public static readonly int WORKER_STARTING_ENERGY = 25;
    public static readonly int WORKER_STARTING_FOOD = 0;
    public static readonly int WORKER_ATTACK_DMG = 25;

    // Action costs
    public static readonly int MOVE_COST = 0;
    public static readonly int ATTACK_COST = 0;
    public static readonly int GIVE_COST = 5;
    public static readonly int EGG_COST = 50;

    // Action quantity by turn
    public static readonly int MAX_EAT_BY_TURN = 25;
    public static readonly int MAX_STOCK_BY_TURN = 50;
    public static readonly int MAX_GIFT_BY_TURN = 50;

    // Other
    public static int MAX_PHEROMONE_BY_CELL = 4;
    public static int FOOD_SIZE = 300;
}
