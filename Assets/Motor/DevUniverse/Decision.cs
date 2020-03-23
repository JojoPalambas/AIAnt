using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decision
{
    public readonly PheromoneType pheromone;
    public readonly PheromoneIntensity pheromoneIntensity;
    public readonly AntMindset newMindset;
    public readonly ChoiceDescriptor choice;

    public Decision(PheromoneType pheromone, PheromoneIntensity pheromoneIntensity, AntMindset newMindset, ChoiceDescriptor choice)
    {
        this.pheromone = pheromone;
        this.pheromoneIntensity = pheromoneIntensity;
        this.newMindset = newMindset;
        this.choice = choice;
    }
}
