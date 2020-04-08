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
            choice = ChoiceDescriptor.ChooseEgg(DirectionManip.RotateDirectionCW(info.pastTurn.pastDecision.choice.direction));

        else
            choice = ChoiceDescriptor.ChooseEgg(info.pastTurn.pastDecision.choice.direction);

        // Puts the pheromone signal on the ground to show its position
        List<PheromoneDigest> pheromoneSignal = new List<PheromoneDigest>();
        for (int i = 0; i < Const.MAX_PHEROMONE_BY_CELL; i++)
            pheromoneSignal.Add(new PheromoneDigest(PheromoneType.PHER0, HexDirection.CENTER));

        Logger.Info("Put " + pheromoneSignal.Count + " pheromones on the ground");

        return new Decision(AntMindset.AMS0, choice, pheromoneSignal);
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
        AntMindset mindset = AntMindset.AMS0;

        // If this is the first turn
        if (info.pastTurn == null)
        {
            // The Worker tries to find the Queen
            HexDirection queenDirection = HexDirection.CENTER;
            foreach (KeyValuePair<HexDirection, List<PheromoneDigest>> entry in info.adjacentPheromoneGroups)
            {
                if (IsQueenSignal(entry.Value))
                {
                    queenDirection = entry.Key;
                    break;
                }
            }

            if (queenDirection == HexDirection.CENTER)
            {
                // Logger.Info("The Queen is missing!");
            }
            else
            {
                choice = ChoiceDescriptor.ChooseMove(DirectionManip.InvertDirection(queenDirection));
                mindset = AntMindset.AMS0;
            }
        }
        else
        {
            switch (info.mindset)
            {
                case AntMindset.AMS0: // FLEES THE QUEEN
                    if (info.pastTurn.error == TurnError.NONE)
                    {
                        choice = ChoiceDescriptor.ChooseMove(info.pastTurn.pastDecision.choice.direction);
                        mindset = AntMindset.AMS0;
                    }
                    else if (info.pastTurn.error == TurnError.COLLISION_FOOD)
                    {
                        choice = ChoiceDescriptor.ChooseMove(DirectionManip.InvertDirection(info.pastTurn.pastDecision.choice.direction));
                        mindset = AntMindset.AMS1;
                    }
                    else if (info.pastTurn.error == TurnError.COLLISION_WATER)
                    {
                        choice = ChoiceDescriptor.ChooseMove(DirectionManip.InvertDirection(info.pastTurn.pastDecision.choice.direction));
                        mindset = AntMindset.AMS2;
                    }
                    else
                    {
                        choice = ChoiceDescriptor.ChooseMove(DirectionManip.InvertDirection(info.pastTurn.pastDecision.choice.direction));
                        mindset = AntMindset.AMS3;
                    }
                    break;
                default:
                    choice = ChoiceDescriptor.ChooseMove(info.pastTurn.pastDecision.choice.direction);
                    break;
            }
        }

        return new Decision(mindset, choice, info.pheromones);
    }

    private bool IsQueenSignal(List<PheromoneDigest> pheromones)
    {
        bool ret = true;
        if (pheromones.Count < 4)
        {
            ret = false;
        }
        else
        {
            foreach (PheromoneDigest pheromone in pheromones)
            {
                if (pheromone.type != PheromoneType.PHER0)
                {
                    ret = false;
                    break;
                }
            }
        }

        return ret;
    }
}
