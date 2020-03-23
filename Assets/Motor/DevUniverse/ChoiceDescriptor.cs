using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceDescriptor
{
    public readonly ActionType type;
    public readonly HexDirection direction;
    public readonly int quantity;
    public readonly AntWord word;

    public ChoiceDescriptor(ActionType type, HexDirection direction, int quantity, AntWord word)
    {
        this.type = type;
        this.direction = direction;
        this.quantity = quantity;
        this.word = word;
    }

    public static ChoiceDescriptor ChooseNone()
    {
        return new ChoiceDescriptor(ActionType.NONE, HexDirection.CENTER, 0, AntWord.NONE);
    }

    public static ChoiceDescriptor ChooseMove(HexDirection direction)
    {
        return new ChoiceDescriptor(ActionType.MOVE, direction, 0, AntWord.NONE);
    }

    public static ChoiceDescriptor ChooseAttack(HexDirection direction)
    {
        return new ChoiceDescriptor(ActionType.ATTACK, direction, 0, AntWord.NONE);
    }

    public static ChoiceDescriptor ChooseEat(HexDirection direction, int quantity)
    {
        return new ChoiceDescriptor(ActionType.EAT, direction, quantity, AntWord.NONE);
    }

    public static ChoiceDescriptor ChooseStock(HexDirection direction, int quantity)
    {
        return new ChoiceDescriptor(ActionType.STOCK, direction, quantity, AntWord.NONE);
    }

    public static ChoiceDescriptor ChooseGive(HexDirection direction, int quantity)
    {
        return new ChoiceDescriptor(ActionType.GIVE, direction, quantity, AntWord.NONE);
    }

    public static ChoiceDescriptor ChooseAnalyse(HexDirection direction)
    {
        return new ChoiceDescriptor(ActionType.ANALYSE, direction, 0, AntWord.NONE);
    }

    public static ChoiceDescriptor ChooseCommunicate(HexDirection direction, AntWord word)
    {
        return new ChoiceDescriptor(ActionType.COMMUNICATE, direction, 0, word);
    }

    public static ChoiceDescriptor ChooseEgg(HexDirection direction)
    {
        return new ChoiceDescriptor(ActionType.EGG, direction, 0, AntWord.NONE);
    }
}
