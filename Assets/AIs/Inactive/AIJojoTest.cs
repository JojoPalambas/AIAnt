using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJojoTest : AntAI
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

        return new Decision(null, AntMindset.AMS0, choice);
    }

    public override Decision OnWorkerTurn(TurnInformation info)
    {
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();

        // If there is no past turn, or if somehow the past turn does not contiain a decision or a choice, the ant moves to the left
        if (info.pastTurn == null || info.pastTurn.pastDecision == null || info.pastTurn.pastDecision.choice == null)
            choice = ChoiceDescriptor.ChooseMove((HexDirection) Random.Range(1, 7));
        else
        {
            // The ant acts differently regarding its last action
            switch (info.pastTurn.pastDecision.choice.type)
            {
                // If the action was to move, the ant tries to move
                case ActionType.MOVE:
                    // If the movement succeded, the ant does it again
                    if (info.pastTurn.error == TurnError.NONE)
                        choice = ChoiceDescriptor.ChooseMove(info.pastTurn.pastDecision.choice.direction);
                    // If the movement failed because of an ant, the ant attacks it (yes, even if it is an ally)
                    else if (info.pastTurn.error == TurnError.COLLISION_ANT)
                        choice = ChoiceDescriptor.ChooseAttack(info.pastTurn.pastDecision.choice.direction);
                    // If the movement failed for any other reason, the ant turns right
                    else
                        choice = ChoiceDescriptor.ChooseMove(RotateDirection(info.pastTurn.pastDecision.choice.direction));
                    break;
                // If the action was to attack, the ant keeps attacking
                case ActionType.ATTACK:
                    // If the attack succeeded, the ant attacks
                    if (info.pastTurn.error == TurnError.NONE)
                        choice = ChoiceDescriptor.ChooseAttack(info.pastTurn.pastDecision.choice.direction);
                    // If the ant tried to attack an ally, it changes its path
                    if (info.pastTurn.error == TurnError.NOT_ENEMY)
                        choice = ChoiceDescriptor.ChooseMove(RotateDirection(info.pastTurn.pastDecision.choice.direction));
                    // If the attack failed, the ant continues moving
                    else
                        choice = ChoiceDescriptor.ChooseMove(info.pastTurn.pastDecision.choice.direction);
                    break;
                // In any other case, the and moves left
                default:
                    choice = ChoiceDescriptor.ChooseMove(HexDirection.LEFT);
                    break;
            }
        }

        return new Decision(null, AntMindset.AMS0, choice);
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
