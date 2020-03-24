using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJojoTest : AntAI
{
    public override Decision OnQueenTurn(TurnInformation info)
    {
        ChoiceDescriptor choice =  ChoiceDescriptor.ChooseMove(HexDirection.LEFT);

        return new Decision(null, AntMindset.AMS0, choice);
    }

    public override Decision OnWorkerTurn(TurnInformation info)
    {
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseMove(HexDirection.LEFT);

        return new Decision(null, AntMindset.AMS0, choice);
    }
}
