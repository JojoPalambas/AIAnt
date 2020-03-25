using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJojoNone : AntAI
{
    public override Decision OnQueenTurn(TurnInformation info)
    {
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();

        return new Decision(null, AntMindset.AMS0, choice);
    }

    public override Decision OnWorkerTurn(TurnInformation info)
    {
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();

        return new Decision(null, AntMindset.AMS0, choice);
    }
}
