using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITestPheromones : AntAI
{
    // The Queen leaves four PHER0 pheromones on its tile to show where she is
    public override Decision OnQueenTurn(TurnInformation info)
    {
        Logger.Info(info.carriedFood);

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

        return new Decision(AntMindset.AMS0, choice, pheromoneSignal);
    }

    // The Worker first goes away from the queen (mindset AMS0), leaving the PHER0 (for exploration) pheromone behind
    // The Queen can be found because it leaves four PHER0 pheromones on her tile
    // When an obstacle is hit, its mindset changes and the ant comes back to the queen, changing the pheromone to a pheromone describing the obstacle:
    // - AMS1 and PHER1 mean FOOD
    public override Decision OnWorkerTurn(TurnInformation info)
    {
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();
        AntMindset mindset = AntMindset.AMS0;
        List<PheromoneDigest> pheromones = info.pheromones;

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
                pheromones = MarkExploration(DirectionManip.InvertDirection(queenDirection));
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
                        pheromones = MarkExploration(info.pastTurn.pastDecision.choice.direction);
                    }
                    else if (info.pastTurn.error == TurnError.COLLISION_FOOD)
                    {
                        //choice = ChoiceDescriptor.ChooseMove(DirectionManip.InvertDirection(info.pastTurn.pastDecision.choice.direction));
                        choice = ChoiceDescriptor.ChooseStock(info.pastTurn.pastDecision.choice.direction, 100);
                        mindset = AntMindset.AMS1;
                        pheromones = info.pheromones; // Because theoretically the tile has been marked during the previous turn
                    }
                    else
                    {
                        choice = ChoiceDescriptor.ChooseMove(DirectionManip.RotateDirectionCW(info.pastTurn.pastDecision.choice.direction));
                        mindset = AntMindset.AMS0;
                        pheromones = MarkExploration(DirectionManip.RotateDirectionCW(info.pastTurn.pastDecision.choice.direction));
                    }
                    break;

                case AntMindset.AMS1: // COMES BACK WITH FOOD
                    Debug.Log(info.pastTurn.error);
                    if (info.pastTurn.pastDecision.choice.type == ActionType.STOCK) // Stock => go back
                    {
                        choice = ChoiceDescriptor.ChooseMove(GoBackExploration(info.adjacentPheromoneGroups));
                        mindset = AntMindset.AMS1;
                        pheromones = MarkFood(info.pastTurn.pastDecision.choice.direction);
                    }
                    else if (info.pastTurn.error == TurnError.NONE) // No error => go back
                    {
                        HexDirection direction = GoBackExploration(info.adjacentPheromoneGroups);
                        choice = ChoiceDescriptor.ChooseMove(direction != HexDirection.CENTER ? direction : info.pastTurn.pastDecision.choice.direction);
                        mindset = AntMindset.AMS1;
                        pheromones = MarkFood(info.pastTurn.pastDecision.choice.direction);
                    }
                    else if (info.pastTurn.error == TurnError.COLLISION_ANT) // No error => go back
                    {
                        Debug.Log(info.id + " GIVES!");
                        choice = ChoiceDescriptor.ChooseGive(info.pastTurn.pastDecision.choice.direction, 100);
                        mindset = AntMindset.AMS1;
                        pheromones = MarkFood(info.pastTurn.pastDecision.choice.direction);
                    }
                    else // Error => still try to go back
                    {
                        choice = ChoiceDescriptor.ChooseMove(GoBackExploration(info.adjacentPheromoneGroups));
                        mindset = AntMindset.AMS1;
                        pheromones = MarkFood(info.pastTurn.pastDecision.choice.direction);
                    }
                    break;

                default:
                    choice = ChoiceDescriptor.ChooseNone();
                    mindset = AntMindset.AMS0;
                    pheromones = info.pheromones;
                    break;
            }
        }

        return new Decision(mindset, choice, pheromones);
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

    private List<PheromoneDigest> MarkExploration(HexDirection direction)
    {
        List<PheromoneDigest> pheromones = new List<PheromoneDigest>();
        pheromones.Add(new PheromoneDigest(PheromoneType.PHER0, direction));
        return pheromones;
    }

    private List<PheromoneDigest> MarkFood(HexDirection direction)
    {
        List<PheromoneDigest> pheromones = new List<PheromoneDigest>();
        pheromones.Add(new PheromoneDigest(PheromoneType.PHER1, direction));
        return pheromones;
    }

    private HexDirection GoBackExploration(Dictionary<HexDirection, List<PheromoneDigest>> pheromoneGroups)
    {
        foreach (KeyValuePair<HexDirection, List<PheromoneDigest>> entry in pheromoneGroups)
        {
            // If there is exactly one pheromone, that it is an exploration one pointing towards the current tile, go there
            if (entry.Value.Count == 1 && entry.Value[0].type == PheromoneType.PHER0 && entry.Value[0].direction == DirectionManip.InvertDirection(entry.Key))
                return entry.Key;
        }
        return HexDirection.CENTER;
    }
}
