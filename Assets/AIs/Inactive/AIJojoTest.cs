using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJojoTest : AntAI
{
    public override Decision OnQueenTurn(TurnInformation info)
    {
        Debug.Log("Queen turn");

        ChoiceDescriptor choice =  ChoiceDescriptor.ChooseEgg(HexDirection.RIGHT);

        return new Decision(null, AntMindset.AMS0, choice);
    }

    public override Decision OnWorkerTurn(TurnInformation info)
    {
        Debug.Log("Worker turn");

        ChoiceDescriptor choice = ChoiceDescriptor.ChooseMove(HexDirection.DOWNLEFT);

        return new Decision(null, AntMindset.AMS0, choice);
    }
}
