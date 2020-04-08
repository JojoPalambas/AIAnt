using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITest1 : AntAI
{
    public override Decision OnQueenTurn(TurnInformation info)
    {
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();

        // If there is no past turn, or if somehow the past turn does not contiain a decision or a choice, the ant moves to the left
        if (info.pastTurn == null || info.pastTurn.pastDecision == null || info.pastTurn.pastDecision.choice == null)
            choice = ChoiceDescriptor.ChooseEgg((HexDirection) Random.Range(1, 7));
        else if (info.pastTurn.error != TurnError.NONE)
            choice = ChoiceDescriptor.ChooseEgg(RotateDirection(info.pastTurn.pastDecision.choice.direction));
        else
            choice = ChoiceDescriptor.ChooseEgg(info.pastTurn.pastDecision.choice.direction);

        return new Decision(AntMindset.AMS0, choice, info.pheromones);
    }

    public override Decision OnWorkerTurn(TurnInformation info)
    {
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();

        // If there is no past turn, or if somehow the past turn does not contiain a decision or a choice, the ant moves to the left
        if (info.pastTurn == null || info.pastTurn.pastDecision == null || info.pastTurn.pastDecision.choice == null)
            choice = ChoiceDescriptor.ChooseMove((HexDirection) Random.Range(1, 7));
        else
        {
            switch (info.pastTurn.pastDecision.choice.type)
            {
                case ActionType.MOVE:
                    if (info.pastTurn.error == TurnError.NONE)
                        choice = ChoiceDescriptor.ChooseMove(info.pastTurn.pastDecision.choice.direction);
                    else
                        choice = ChoiceDescriptor.ChooseAnalyse(info.pastTurn.pastDecision.choice.direction);
                    break;

                case ActionType.ANALYSE:
                    if (info.pastTurn.error == TurnError.NONE)
                        choice = ChoiceDescriptor.ChooseMove(RotateDirection(info.pastTurn.pastDecision.choice.direction));
                    else
                        choice = ChoiceDescriptor.ChooseMove(RotateDirection(info.pastTurn.pastDecision.choice.direction));
                    break;
                    
                default:
                    choice = ChoiceDescriptor.ChooseMove(RotateDirection(info.pastTurn.pastDecision.choice.direction));
                    break;
            }
        }

        return new Decision(AntMindset.AMS0, choice, info.pheromones);
    }

    // Rotates the given direction clockwise by 1 step
    private HexDirection RotateDirection(HexDirection direction)
    {
        switch (direction)
        {
            case HexDirection.LEFT:
                return HexDirection.UPLEFT;
            case HexDirection.UPLEFT:
                return HexDirection.UPRIGHT;
            case HexDirection.UPRIGHT:
                return HexDirection.RIGHT;
            case HexDirection.RIGHT:
                return HexDirection.DOWNRIGHT;
            case HexDirection.DOWNRIGHT:
                return HexDirection.DOWNLEFT;
            case HexDirection.DOWNLEFT:
                return HexDirection.LEFT;
            default:
                return HexDirection.LEFT;
        }
    }
}
