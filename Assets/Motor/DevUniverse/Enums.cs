﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexDirection
{
    CENTER,
    UPLEFT,
    UPRIGHT,
    LEFT,
    RIGHT,
    DOWNLEFT,
    DOWNRIGHT
}

public enum AntType
{
    NONE,
    WORKER,
    QUEEN
}

public enum ActionType
{
    NONE,
    MOVE,
    ATTACK,
    EAT,
    STOCK,
    GIVE,
    ANALYSE,
    COMMUNICATE,
    EGG
}

enum TurnError
{
    NONE,
    COLLISION_WALL,
    COLLISION_ANT,
    COLLISION_FOOD,
    COLLISION_WATER,
    NO_TARGET,
    ILLEGAL
}

enum PheromoneType
{
    NONE,
    PHER0,
    PHER1,
    PHER2,
    PHER3
}

enum PheromoneIntensity
{
    INT0,
    INT1,
    INT2,
    INT3
}

enum AntMindset
{
    AMS0,
    AMS1,
    AMS2,
    AMS3,
    AMS4,
    AMS5,
    AMS6,
    AMS7
}

enum AntWord
{
    AW0,
    AW1,
    AW2,
    AW3,
    AW4,
    AW5,
    AW6,
    AW7,
}

public enum TerrainType
{
    NONE,
    GROUND,
    WATER
}