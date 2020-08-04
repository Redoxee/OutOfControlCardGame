
using UnityEngine;

class RuleDefinitionCombination : RuleDefinition
{
    public RuleDefinition[] SubRules = new RuleDefinition[0];

    private System.Collections.Generic.List<Sprite> workingSpriteList = new System.Collections.Generic.List<Sprite>();

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

    public override Sprite[] GetRuleSprites()
    {
        this.workingSpriteList.Clear();
        foreach (RuleDefinition rule in this.SubRules)
        {
            Sprite[] sprites = rule.GetRuleSprites();
            foreach (Sprite sprite in sprites)
            {
                this.workingSpriteList.Add(sprite);
            }
        }

        return this.workingSpriteList.ToArray();
    }
}
