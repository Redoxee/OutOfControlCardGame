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
        for (int y = 0; y < 3; ++y)
        {
            for (int x = 0; x < 3; ++x)
            {
                slots[y * 3 + x] = this.IsSlotAllowed(ref card, cardSlots, x, y);
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