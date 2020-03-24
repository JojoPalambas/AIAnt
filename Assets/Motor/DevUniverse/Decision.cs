
public class Decision
{
    public readonly PheromonePlacement pheromone;
    public readonly AntMindset newMindset;
    public readonly ChoiceDescriptor choice;

    public Decision(PheromonePlacement pheromone, AntMindset newMindset, ChoiceDescriptor choice)
    {
        this.pheromone = pheromone;
        this.newMindset = newMindset;
        this.choice = choice;
    }
}
