using UnityEngine;

public class RuleDefinitionSymbolStacking : RuleDefinition
{
    [SerializeField]
    private Sigil topSigil = (Sigil)0;
    [SerializeField]
    private Sigil bottomSigils = (Sigil)0;

    public override bool IsSlotAllowed(ref CardData topCard, CardSlot[] cardSlots, int x, int y)
    {
        Card bottomCard = cardSlots[x % GameController.GridSize + y * GameController.GridSize].Card;
        if (bottomCard == null)
        {
            return true;
        }

        if ((bottomCard.Data.Sigil & this.bottomSigils) == 0)
        {
            return true;
        }

        if ((topCard.Sigil & this.topSigil) != 0)
        {
            return true;
        }

        return false;
    }
}
