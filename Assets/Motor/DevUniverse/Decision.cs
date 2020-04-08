using System.Collections.Generic;

public class Decision
{
    public readonly AntMindset newMindset;
    public readonly ChoiceDescriptor choice;
    public readonly List<PheromoneDigest> pheromones;

    public Decision(AntMindset newMindset, ChoiceDescriptor choice, List<PheromoneDigest> pheromones)
    {
        this.newMindset = newMindset;
        this.choice = choice;
        this.pheromones = pheromones;
    }

    public Decision DeepCopyWithoutIsAllied()
    {
        List<PheromoneDigest> pheromonesCopy = new List<PheromoneDigest>();
        foreach (PheromoneDigest pheromone in this.pheromones)
        {
            if (pheromone != null)
                pheromonesCopy.Add(new PheromoneDigest(pheromone.type, pheromone.direction));
            else
                pheromonesCopy.Add(null);
        }

        return new Decision(newMindset, choice != null ? choice.DeepCopy() : null, pheromonesCopy);
    }
}
