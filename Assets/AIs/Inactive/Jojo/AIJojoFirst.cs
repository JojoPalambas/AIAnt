using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJojoFirst : AntAI
{
    // AMS0 - AMS1 - AMS2: The Queen tries to find the center of the three free tiles to lay her eggs in
    // AMS0: The Queen is rotating CCW in order to find a out-of-bounds tile ; the first out-of-bounds tile makes the Queen become AMS1
    // AMS1: An out-of-bound tile has been found, now the Queen is rotating CW to find the first free tile
    // AMS1: When the Queen has found the first free time, it places four PHER3 showing the next tile CW, which is the center of the three free tiles to lay eggs in ; the Queen goes AMS2
    // AMS2: The Queen spawns eggs randomly in the center free tile
    // AMS2: If the Queen has a low energy, it eats if it has carried food, or else tries to lay eggs
    public override Decision OnQueenTurn(TurnInformation info)
    {
        AntMindset mindset = info.mindset;
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();
        List<PheromoneDigest> pheromones = info.pheromones;

        if (info.pastTurn == null)
        {
            mindset = AntMindset.AMS0;
            choice = ChoiceDescriptor.ChooseAnalyse((HexDirection) Random.Range(1, 7));
            Debug.Log(choice.direction);
        }
        else
        {
            switch (mindset)
            {
                case AntMindset.AMS0:

                    switch (info.pastTurn.error)
                    {
                        case TurnError.NONE: // The out-of-bounds tile has not been found

                            choice = ChoiceDescriptor.ChooseAnalyse(DirectionManip.RotateDirectionCCW(info.pastTurn.pastDecision.choice.direction));

                            break;

                        case TurnError.COLLISION_VOID: // The out-of-bounds tile has been found

                            mindset = AntMindset.AMS1;
                            choice = ChoiceDescriptor.ChooseAnalyse(DirectionManip.RotateDirectionCW(info.pastTurn.pastDecision.choice.direction));

                            break;

                        default: // Should not happen

                            choice = ChoiceDescriptor.ChooseNone();

                            break;
                    }

                    break;

                case AntMindset.AMS1:

                    switch (info.pastTurn.error)
                    {
                        case TurnError.COLLISION_VOID: // The free tile has not been found

                            choice = ChoiceDescriptor.ChooseAnalyse(DirectionManip.RotateDirectionCW(info.pastTurn.pastDecision.choice.direction));

                            break;

                        case TurnError.NONE: // The out-of-bounds tile has been found

                            mindset = AntMindset.AMS2;
                            HexDirection freeCenter = DirectionManip.RotateDirectionCW(info.pastTurn.pastDecision.choice.direction);

                            pheromones = new List<PheromoneDigest>();
                            for (int i = 0; i < 4; i++)
                                pheromones.Add(new PheromoneDigest(PheromoneType.PHER3, freeCenter));

                            choice = ChoiceDescriptor.ChooseEgg(freeCenter);

                            break;

                        default: // Should not happen

                            choice = ChoiceDescriptor.ChooseNone();

                            break;
                    }

                    break;

                case AntMindset.AMS2:

                    if (info.energy >= Value.MEDIUM)
                    {
                        HexDirection freeCenter = FindFreeCenter(info.pheromones);
                        choice = ChoiceDescriptor.ChooseEgg(freeCenter);
                    }
                    else if (info.carriedFood > Value.NONE)
                    {
                        choice = ChoiceDescriptor.ChooseEat(HexDirection.CENTER, 100);
                    }
                    else
                    {
                        HexDirection freeCenter = FindFreeCenter(info.pheromones);
                        choice = ChoiceDescriptor.ChooseEgg(freeCenter);
                    }

                    break;
            }
        }

        return new Decision(mindset, choice, pheromones);
    }

    private HexDirection RandomRotate(HexDirection direction) // FIXME Allow to forbid one of the three options (not to lay eggs twice in the same place)
    {
        int rand = Random.Range(0, 3);

        if (rand == 0)
            return DirectionManip.RotateDirectionCCW(direction);
        if (rand == 1)
            return direction;

        return DirectionManip.RotateDirectionCW(direction);
    }

    private HexDirection FindFreeCenter(List<PheromoneDigest> pheromones)
    {
        if (pheromones == null || pheromones.Count < 4)
            return HexDirection.CENTER;

        if (pheromones[0] == null || pheromones[0].direction == HexDirection.CENTER)
            return HexDirection.CENTER;
        
        HexDirection freeCenterDirection = pheromones[0].direction;
        for (int i = 0; i < 4; i++)
        {
            if (pheromones[i].type != PheromoneType.PHER3 || pheromones[i].direction != freeCenterDirection)
            {
                freeCenterDirection = HexDirection.CENTER;
                break;
            }
        }

        return freeCenterDirection;
    }
    
    // Priority #0: If the ant is attacked and is not capital, it fights back (an ant is capital if it has more than or equal to MEDIUM food, or knows where the enemy Queen is (AMS7 or recent analyse))
    // Priority #1: If the ant detects that it is on the spawn tile, it moves away to the right or to the left of the Queen, and leaves three PHER3 in the same direction as the ones under the Queen
    // AMS0 (exploration): If there is 0 pheromone under the ant, it moves straight and turns randomly when there is an obstacle (without going back), and leaves a PHER0
    // AMS0 (exploration): If there is 1 to 3 pheromones under the ant, it follows them without leaving anything, and has a chance to leave the path, leaving a trace
    // AMS0 (exploration): If there is 4 pheromones under the ant, it follows them without leaving anything
    // AMS0 (exploration): If there is 2 to 4 pheromones, if the ant tried to leave the path but found an obstacle, in has to remove its mark
    // AMS0 (exploration): If the ant bumps into an enemy, it analyses it
    public override Decision OnWorkerTurn(TurnInformation info)
    {
        AntMindset mindset = info.mindset;
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();
        List<PheromoneDigest> pheromones = info.pheromones;

        // Analyses the situation
        bool isImportant = IsImportant(info);
        KeyValuePair<HexDirection, List<PheromoneDigest>> queenPheromones = FindAdjacentQueen(info.adjacentPheromoneGroups);
        HexDirection attackOrigin = HasBeenAttacked(info.eventInputs);

        // Priority #0: Fights back if attacked and not important
        if (attackOrigin != HexDirection.CENTER == !isImportant)
        {
            choice = ChoiceDescriptor.ChooseAttack(attackOrigin);
        }
        // Priority #1: Checks if the Worker is in the spawn tile
        if (queenPheromones.Key != HexDirection.CENTER && queenPheromones.Value != null && queenPheromones.Value[0].direction == DirectionManip.InvertDirection(queenPheromones.Key))
        {
            mindset = AntMindset.AMS0;

            int rand = Random.Range(0, 2);
            if (rand == 0)
                choice = ChoiceDescriptor.ChooseMove(DirectionManip.RotateDirectionCCW(queenPheromones.Key));
            else
                choice = ChoiceDescriptor.ChooseMove(DirectionManip.RotateDirectionCW(queenPheromones.Key));

            pheromones = new List<PheromoneDigest>();
        }
        else
        {
            switch (mindset)
            {
                case AntMindset.AMS0:
                    break;
            }
        }

        return new Decision(mindset, choice, pheromones);
    }

    private bool IsImportant(TurnInformation info)
    {
        if (info.mindset == AntMindset.AMS7)
            return true;
        if (info.carriedFood >= Value.MEDIUM)
            return true;
        if (info.analyseReport != null && info.analyseReport.antType == AntType.QUEEN && info.analyseReport.isAllied == false)
            return true;

        return false;
    }

    private KeyValuePair<HexDirection, List<PheromoneDigest>> FindAdjacentQueen(Dictionary<HexDirection, List<PheromoneDigest>> pheromoneGroups)
    {
        foreach (KeyValuePair<HexDirection, List<PheromoneDigest>> pheromoneGroup in pheromoneGroups)
        {
            if (pheromoneGroup.Value == null || pheromoneGroup.Value.Count < 4 || pheromoneGroup.Value[0] == null)
                continue;

            HexDirection currentDirection = pheromoneGroup.Value[0].direction;
            bool found = true;
            foreach (PheromoneDigest pheromone in pheromoneGroup.Value)
            {
                if (pheromone.type != PheromoneType.PHER3 || pheromone.direction != currentDirection)
                    found = false;
            }
            if (found)
                return pheromoneGroup;
        }
        return new KeyValuePair<HexDirection, List<PheromoneDigest>>(HexDirection.CENTER, null);
    }

    private HexDirection HasBeenAttacked(List<EventInput> eventInputs)
    {
        foreach (EventInput eventInput in eventInputs)
        {
            if (eventInput.type == EventInputType.ATTACK)
                return eventInput.direction;
        }
        return HexDirection.CENTER;
    }
}
