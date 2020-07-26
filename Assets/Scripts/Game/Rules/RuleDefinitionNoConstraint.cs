
class RuleDefinitionNoConstraint : RuleDefinition
{
    public override bool IsSlotAllowed(ref CardData card, CardSlot[] cardSlots, int x, int y)
    {
        return true;
    }
}
