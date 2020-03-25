
public enum Value
{
    NONE,
    LOW,
    MEDIUM,
    HIGH
}

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

public enum TurnError
{
    NONE,
    COLLISION_WALL,
    COLLISION_ANT,
    COLLISION_FOOD,
    COLLISION_WATER,
    COLLISION_BOUNDS,
    COLLISION_VOID,
    NOT_ENEMY, // If an ant tries to attack its ally
    NO_TARGET, // If the target is missing (no ant when attack, no food when eat, etc.)
    NOT_QUEEN, // If a non-queen ant tries to EGG
    ILLEGAL
}

public enum PheromoneType
{
    NONE,
    PHER0,
    PHER1,
    PHER2,
    PHER3
}

public enum PheromoneIntensity
{
    INT0,
    INT1,
    INT2,
    INT3
}

public enum AntMindset
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

public enum AntWord
{
    NONE,
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