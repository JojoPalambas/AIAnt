using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITestPheromones : AntAI
{
    // The Queen leaves four PHER0 pheromones on its tile to show where she is
    public override Decision OnQueenTurn(TurnInformation info)
    {
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();
        
        // If it is the first turn
        if (info.pastTurn == null || info.pastTurn.pastDecision == null || info.pastTurn.pastDecision.choice == null)
            choice = ChoiceDescriptor.ChooseEgg((HexDirection) Random.Range(1, 7));

        // If there was an error
        else if (info.pastTurn.error != TurnError.NONE)
            choice = ChoiceDescriptor.ChooseEgg(RotateDirection(info.pastTurn.pastDecision.choice.direction));

        else
            choice = ChoiceDescriptor.ChooseEgg(info.pastTurn.pastDecision.choice.direction);

        return new Decision(AntMindset.AMS0, choice, info.pheromones);
    }

    // The Worker first goes away from the queen (mindset AMS0), leaving the PHER0 (for exploration) pheromone behind
    // The Queen can be found because it leaves four PHER0 pheromones on her tile
    // When an obstacle is hit, its mindset changes and the ant comes back to the queen, changing the pheromone to a pheromone describing the obstacle:
    // - AMS1 and PHER1 mean FOOD
    // - AMS2 and PHER2 mean WATER
    // - AMS3 and PHER3 mean OTHER
    public override Decision OnWorkerTurn(TurnInformation info)
    {
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();

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
