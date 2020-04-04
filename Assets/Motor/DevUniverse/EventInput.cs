using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventInputType
{
    BUMP, // If the ant has been bumped into (failed movement, failed attack, failed egg, etc. (is there anything else?))
    ATTACK, // If the ant has been attacked (only successful attacks, or it will be a "bump")
    ANALYSE, // If the tile of the ant has been analysed
    COMMUNICATE // If an ant has communicated with this ant
}

public abstract class EventInput
{
    public readonly EventInputType type;
    public readonly HexDirection direction;

    protected EventInput(EventInputType type, HexDirection direction)
    {
        this.type = type;
        this.direction = direction;
    }
}

public class EventInputBump : EventInput
{
    public EventInputBump(HexDirection direction) : base(EventInputType.BUMP, direction) { }
}

public class EventInputAttack : EventInput
{
    public EventInputAttack(HexDirection direction) : base(EventInputType.BUMP, direction) { }
}

public class EventInputAnalyse : EventInput
{
    public EventInputAnalyse(HexDirection direction) : base(EventInputType.BUMP, direction) { }
}

public class EventInputComunicate : EventInput
{
    public readonly CommunicateReport payload;
    public EventInputComunicate(HexDirection direction, CommunicateReport payload) : base(EventInputType.BUMP, direction)
    {
        this.payload = payload;
    }
}