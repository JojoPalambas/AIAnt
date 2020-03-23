
public class CommunicateReport
{
    public readonly AntType type;
    public readonly AntMindset mindset;
    public readonly Value hp;
    public readonly Value energy;
    public readonly Value carriedFood;
    public readonly AntWord word;

    public CommunicateReport(AntType type, AntMindset mindset, Value hp, Value energy, Value carriedFood, AntWord word)
    {
        this.type = type;
        this.mindset = mindset;
        this.hp = hp;
        this.energy = energy;
        this.carriedFood = carriedFood;
        this.word = word;
    }
}
