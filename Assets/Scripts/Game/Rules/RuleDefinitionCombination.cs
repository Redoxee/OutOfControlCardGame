
class RuleDefinitionCombination : RuleDefinition
{
    public RuleDefinition[] SubRules = new RuleDefinition[0];

    public override bool IsSlotAllowed(ref CardData card, CardSlot[] cardSlots, int x, int y)
    {
        for (int index = 0; index < this.SubRules.Length; ++index)
        {
            if(!this.SubRules[index].IsSlotAllowed(ref card, cardSlots, x, y))
            {
                return false;
            }
        }

        return true;
    }

    public override string ToString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        for (int index = 0; index < this.SubRules.Length; ++index)
        {
            builder.Append(this.SubRules[index].ToString());
            if (index < this.SubRules.Length - 1)
            {
                builder.Append("\n");
            }
        }

        return builder.ToString();
    }
}
