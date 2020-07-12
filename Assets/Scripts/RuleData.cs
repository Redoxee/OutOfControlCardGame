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

    public bool IsContained(RuleData other)
    {
        if (other == null)
        {
            return false;
        }

        if (this == other)
        {
            return true;
        }

        if (this is CombinedRule meCombined)
        {
            if(meCombined.rule1 == other || meCombined.rule2 == other)
            {
                return true;
            }
        }

        if (other is CombinedRule heCombined)
        {
            if (heCombined.rule1 == other || heCombined.rule2 == other)
            {
                return true;
            }
        }

        return false;
    }
}

public class NoConstraintRule : RuleData
{
    public override string ToString()
    {
        return "No Constraints";
    }

    public override bool IsSlotAllowed(ref CardData card, CardSlot[] cardSlots, int x, int y)
    {
        return true;
    }
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
        return "Same symbols can't share edges";
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
        return "Odd cards can't be stacked on odd cards";
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
        return "Even cards must be placed on even cards";
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

public class CombinedRule : RuleData
{
    public RuleData rule1 = null;
    public RuleData rule2 = null;
    public override string ToString()
    {
        return $"{this.rule1.ToString()}\n{this.rule2.ToString()}";
    }

    public override bool IsSlotAllowed(ref CardData card, CardSlot[] cardSlots, int x, int y)
    {
        return this.rule1.IsSlotAllowed(ref card, cardSlots, x, y) && 
            this.rule1.IsSlotAllowed(ref card, cardSlots, x, y);
    }
}