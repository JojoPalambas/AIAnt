using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITestNone : AntAI
{
    public override Decision OnQueenTurn(TurnInformation info)
    {
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();

        return new Decision(AntMindset.AMS0, choice, info.pheromones);
    }

    public override Decision OnWorkerTurn(TurnInformation info)
    {
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();

        return new Decision(AntMindset.AMS0, choice, info.pheromones);
    }
}
