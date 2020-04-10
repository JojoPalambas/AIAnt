using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Const
{
    // Ant stats

    // Queen stats
    public static int QUEEN_STARTING_HP = 100;
    public static int QUEEN_STARTING_ENERGY = 100;
    public static int QUEEN_STARTING_FOOD = 0;
    public static int QUEEN_ATTACK_DMG = 15;

    // Worker stats
    public static int WORKER_STARTING_HP = 100;
    public static int WORKER_STARTING_ENERGY = 25;
    public static int WORKER_STARTING_FOOD = 0;
    public static int WORKER_ATTACK_DMG = 25;

    // Action costs
    public static int MOVE_COST = 0;
    public static int ATTACK_COST = 5;
    public static int GIVE_COST = 5;
    public static int EGG_COST = 25;

    // Action quantity by turn
    public static int MAX_EAT_BY_TURN = 25;
    public static int MAX_STOCK_BY_TURN = 50;
    public static int MAX_GIFT_BY_TURN = 50;

    // Other
    public static int MAX_PHEROMONE_BY_CELL = 4;
}
