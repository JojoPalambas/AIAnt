using System.Collections.Generic;

public abstract class AntAI
{
    public abstract Decision OnWorkerTurn(TurnInformation info);
    public abstract Decision OnQueenTurn(TurnInformation info);
}
