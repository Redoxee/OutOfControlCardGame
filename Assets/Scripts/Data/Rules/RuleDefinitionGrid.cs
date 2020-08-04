
class RuleDefinitionGrid : RuleDefinition
{
    public bool[] AllowedCells = new bool[9];

    public override bool IsSlotAllowed(ref CardData card, CardSlot[] cardSlots, int x, int y)
    {
        int index = y * GameController.GridSize + x;
        return this.AllowedCells[index];
    }
}