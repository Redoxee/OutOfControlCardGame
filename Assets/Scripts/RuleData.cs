using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class RuleData
{
    public void GetAllowedSlots(ref CardData card, CardSlot[] cardSlots, ref bool[] slots)
    {
        for (int y = 0; y < GameController.GridSize; ++y)
        {
            for (int x = 0; x < GameController.GridSize; ++x)
            {
                slots[y * GameController.GridSize + x] = this.IsSlotAllowed(ref card, cardSlots, x, y);
            }
        }
    }

    public abstract bool IsSlotAllowed(ref CardData card, CardSlot[] cardSlots, int x, int y);
}

public class StaticArrayRule : RuleData
{
    public string Text = string.Empty;

    public Sigil TargetedSigil;
    public int[] Numbers;

    public bool[][] AllowedCells;

    public override string ToString()
    {
        return this.Text;
    }

    public override bool IsSlotAllowed(ref CardData card, CardSlot[] cardSlots, int x, int y)
    {
        bool isOnArray = this.AllowedCells[y][x];
        bool isCardMatching = true;
        if(((int)this.TargetedSigil > 0) 
            && ((this.TargetedSigil & card.Sigil) == 0))
        {
            isCardMatching = false;
        }

        if (this.Numbers != null && System.Array.IndexOf(this.Numbers, card.NumberValue) < 0)
        {
            isCardMatching &= false;
        }

        return isOnArray;
    }
}

public class DifferentSigilAdjascentRule : RuleData
{
    public override string ToString()
    {
        return "Same symblos can't share edges";
    }

    public override bool IsSlotAllowed(ref CardData card, CardSlot[] cardSlots, int x, int y)
    {
        if (x > 0)
        {
            CardSlot otherSlot = cardSlots[y * GameController.GridSize + x - 1];
            if (otherSlot.Card != null && otherSlot.Card.Data.Sigil == card.Sigil)
            {
                return false;
            }
        }

        if (x < GameController.GridSize - 1)
        {
            CardSlot otherSlot = cardSlots[y * GameController.GridSize + x + 1];
            if (otherSlot.Card != null && otherSlot.Card.Data.Sigil == card.Sigil)
            {
                return false;
            }
        }

        if (y > 0)
        {
            CardSlot otherSlot = cardSlots[(y - 1) * GameController.GridSize + x];
            if (otherSlot.Card != null && otherSlot.Card.Data.Sigil == card.Sigil)
            {
                return false;
            }
        }

        if (y < GameController.GridSize - 1)
        {
            CardSlot otherSlot = cardSlots[(y + 1) * GameController.GridSize + x];
            if (otherSlot.Card != null && otherSlot.Card.Data.Sigil == card.Sigil)
            {
                return false;
            }
        }

        return true;
    }
}

public class OddCardsOnOddCardsRule : RuleData
{
    public override string ToString()
    {
        return "Odd cards can't be stacked";
    }

    public override bool IsSlotAllowed(ref CardData card, CardSlot[] cardSlots, int x, int y)
    {
        CardSlot prevSlot = cardSlots[y * GameController.GridSize + x];
        if (prevSlot.Card == null)
        {
            return true;
        }

        if (card.NumberValue % 2 == 0)
        {
            return true;
        }

        if (prevSlot.Card.Data.NumberValue % 2 != 0)
        {
            return false;
        }

        return true;
    }
}

public class EvenCardsMustBeStackedRule : RuleData
{
    public override string ToString()
    {
        return "Even cards must placed on even cards";
    }

    public override bool IsSlotAllowed(ref CardData card, CardSlot[] cardSlots, int x, int y)
    {
        if (card.NumberValue % 2 != 0)
        {
            return true;
        }

        CardSlot prevSlot = cardSlots[y * GameController.GridSize + x];
        if (prevSlot.Card == null)
        {
            return false;
        }

        if (prevSlot.Card.Data.NumberValue % 2 != 0)
        {
            return false;
        }

        return true;
    }
}