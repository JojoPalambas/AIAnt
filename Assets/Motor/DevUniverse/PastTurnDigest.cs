
public class PastTurnDigest
{
    public readonly Decision pastDecision;
    TurnError error;

    public PastTurnDigest(Decision pastDecision, TurnError error)
    {
        this.pastDecision = pastDecision;
        this.error = error;
    }
}
