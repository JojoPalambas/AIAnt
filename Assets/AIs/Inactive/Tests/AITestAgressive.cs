using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITestAgressive : AntAI
{
    public override Decision OnQueenTurn(TurnInformation info)
    {
        // Setting the defaults
        AntMindset mindset = AntMindset.AMS0;
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();
        List<PheromoneDigest> pheromones = null;

        // Laying an egg in a random direction
        choice = ChoiceDescriptor.ChooseEgg((HexDirection) Random.Range(1, 8));

        return new Decision(mindset, choice, pheromones);
    }

    public override Decision OnWorkerTurn(TurnInformation info)
    {
        // Setting the defaults
        AntMindset mindset = AntMindset.AMS0;
        ChoiceDescriptor choice = ChoiceDescriptor.ChooseNone();
        List<PheromoneDigest> pheromones = null;


        HexDirection attackDirection = HexDirection.CENTER;
        if (info.pastTurn != null)
            attackDirection = GetAttackDirection(info.eventInputs);

        if (info.pastTurn == null)
            choice = ChoiceDescriptor.ChooseMove((HexDirection) Random.Range(1, 8));

        else if (attackDirection != HexDirection.CENTER)
            choice = ChoiceDescriptor.ChooseAttack(attackDirection);

        else
        {
            switch (info.pastTurn.pastDecision.choice.type)
            {
                case ActionType.MOVE:

                    switch (info.pastTurn.error)
                    {
                        case TurnError.NONE:
                            int rand = Random.Range(0, 10);
                            if (rand < 8)
                                choice = ChoiceDescriptor.ChooseMove(info.pastTurn.pastDecision.choice.direction);
                            else
                                choice = ChoiceDescriptor.ChooseMove((HexDirection) Random.Range(1, 8));
                            break;

                        case TurnError.COLLISION_ANT:
                            choice = ChoiceDescriptor.ChooseAttack(info.pastTurn.pastDecision.choice.direction);
                            break;

                        default:
                            choice = ChoiceDescriptor.ChooseMove((HexDirection) Random.Range(1, 8));
                            break;
                    }

                    break;

                case ActionType.ATTACK:

                    switch (info.pastTurn.error)
                    {
                        case TurnError.NONE:
                            choice = ChoiceDescriptor.ChooseAttack(info.pastTurn.pastDecision.choice.direction);
                            break;

                        default:
                            choice = ChoiceDescriptor.ChooseMove((HexDirection) Random.Range(1, 8));
                            break;
                    }

                    break;

                default:
                    choice = ChoiceDescriptor.ChooseMove((HexDirection) Random.Range(1, 8));
                    break;
            }
        }

        if (choice.type == ActionType.MOVE)
        {
            pheromones = new List<PheromoneDigest>();
            pheromones.Add(new PheromoneDigest(PheromoneType.PHER0, choice.direction));
        }

        return new Decision(mindset, choice, pheromones);
    }

    private HexDirection GetAttackDirection(List<EventInput> eventInputs)
    {
        HexDirection ret = HexDirection.CENTER;

        foreach (EventInput eventInput in eventInputs)
        {
            if (eventInput.type == EventInputType.ATTACK)
                return eventInput.direction;
        }

        return ret;
    }
}
