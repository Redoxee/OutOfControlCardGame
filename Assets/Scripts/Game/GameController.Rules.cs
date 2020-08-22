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
            ShuffleList(wave);
            this.wavesRules.Add(wave);
        }
    }

    private static void ShuffleList<T>(List<T> list)
    {
        if (list == null)
        {
            return;
        }

        for (int index = 0; index < list.Count; ++index)
        {
            int other = UnityEngine.Random.Range(index, list.Count - 1);
            if (other == index)
            {
                continue;
            }

            T temp = list[index];
            list[index] = list[other];
            list[other] = temp;
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
