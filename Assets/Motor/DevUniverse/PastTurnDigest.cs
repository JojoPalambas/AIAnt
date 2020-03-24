
public class PastTurnDigest
{
    public readonly Decision pastDecision;
    public readonly TurnError error;

    public PastTurnDigest(Decision pastDecision, TurnError error)
    {
        this.pastDecision = pastDecision;
        this.error = error;
    }

    public PastTurnDigest DeepCopy()
    {
        return new PastTurnDigest(pastDecision != null ? pastDecision.DeepCopy() : null, error);
    }
}
