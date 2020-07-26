using System.Collections.Generic;

public partial class GameController
{
    private List<RuleDefinition> combinedRules = new List<RuleDefinition>();
    private void InitializeRuleData()
    {
        RuleDefinition[] definitions = MainManager.Instance.GameSettings.GameRules;
        this.availableRules.AddRange(definitions);

        this.GenerateCombinedRules();
    }

    private void GenerateCombinedRules()
    {
        int baseNumberOfRules = this.availableRules.Count;
        for (int index = 0; index < (baseNumberOfRules - 1); ++index)
        {
            for (int jndex = index + 1; jndex < baseNumberOfRules; ++jndex)
            {
                if (this.availableRules[index] is RuleDefinitionNoConstraint || 
                    this.availableRules[jndex] is RuleDefinitionNoConstraint)
                {
                    continue;
                }

                RuleDefinitionCombination combinationRule = UnityEngine.ScriptableObject.CreateInstance<RuleDefinitionCombination>();
                combinationRule.SubRules = new RuleDefinition[]
                    {
                        this.availableRules[index],
                        this.availableRules[jndex],
                    };

                this.combinedRules.Add(combinationRule);
            }
        }
    }

    private void TransferCombinedRules()
    {
        int numberOfCombinedRules = this.combinedRules.Count;
        for (int index = 0; index < numberOfCombinedRules; ++index)
        {
            this.availableRules.Add(this.combinedRules[index]);
        }

        this.combinedRules.Clear();
    }

    private void TransferOneCombinedRule()
    {
        if (this.combinedRules.Count == 0)
        {
            return;
        }

        int selectedIndex = UnityEngine.Random.Range(0, this.combinedRules.Count);
        RuleDefinition data = this.combinedRules[selectedIndex];
        this.combinedRules.RemoveAt(selectedIndex);
        this.availableRules.Add(data);
    }
}
