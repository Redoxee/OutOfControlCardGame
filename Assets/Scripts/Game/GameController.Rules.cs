using System.Collections.Generic;

public partial class GameController
{
    private List<List<RuleDefinition>> wavesRules = new List<List<RuleDefinition>>();

    private void InitializeRuleData()
    {
        RuleDefinition[] definitions = MainManager.Instance.GameConfiguration.RuleDeck;
        this.availableRules.AddRange(definitions);

        this.wavesRules.Clear();
        var wavesDefintion = MainManager.Instance.GameConfiguration.RuleWaves;
        for (int waveIndex = 0; waveIndex < wavesDefintion.Length; ++waveIndex)
        {
            List<RuleDefinition> wave = new List<RuleDefinition>();
            wave.AddRange(wavesDefintion[waveIndex].RuleDefintions);
            this.wavesRules.Add(wave);
        }
    }

    private void TransferWaveRule()
    {
        if (this.wavesRules.Count == 0)
        {
            return;
        }

        List<RuleDefinition> wave = this.wavesRules[0];
        this.availableRules.AddRange(wave);
        this.wavesRules.RemoveAt(0);
    }

    private void TransferOneCombinedRule()
    {
        if (this.wavesRules.Count == 0)
        {
            return;
        }

        List<RuleDefinition> wave = this.wavesRules[0];
        if (wave.Count > 0)
        {
            this.availableRules.Add(wave[0]);
            wave.RemoveAt(0);
        }

        if (wave.Count == 0)
        {
            this.wavesRules.RemoveAt(0);
        }
    }
}
