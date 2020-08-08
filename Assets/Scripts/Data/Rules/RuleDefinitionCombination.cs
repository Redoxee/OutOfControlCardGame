
using UnityEngine;

public class RuleDefinitionCombination : RuleDefinition
{
    public RuleDefinition[] SubRules = new RuleDefinition[0];

    private System.Collections.Generic.List<Sprite> workingSpriteList = new System.Collections.Generic.List<Sprite>();
    private System.Collections.Generic.List<GameObject> workingGameObjectList = new System.Collections.Generic.List<GameObject>();

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

    public override GameObject[] GetIllustrationPrefabs()
    {
        if (this.IllustrationPrefabs == null)
        {
            this.workingGameObjectList.Clear();
            foreach (RuleDefinition rule in this.SubRules)
            {
                GameObject[] ruleIllustrations = rule.GetIllustrationPrefabs();
                foreach (GameObject illustration in ruleIllustrations)
                {
                    this.workingGameObjectList.Add(illustration);
                }
            }

            this.IllustrationPrefabs = this.workingGameObjectList.ToArray();
        }

        return this.IllustrationPrefabs;
    }
}
