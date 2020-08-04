
class RuleDefinitionSigilAdjacenci : RuleDefinition
{
    public Sigil TargetSigil = Sigil.Diamond;

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
