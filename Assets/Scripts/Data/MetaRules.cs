
public class MetaRules : AMG.Data
{
    public RuleDefinition[] RuleDeck = new RuleDefinition[0];
    public RuleWave[] RuleWaves = new RuleWave[0];

    public DeckCard[] CardDeck = new DeckCard[0];

    [System.Serializable]
    public struct DeckCard
    {
        public int Value;
        public Sigil sigil;
        public int NumberOfCopyInDeck;
    }

    [System.Serializable]
    public struct RuleWave
    {
        public RuleDefinition[] RuleDefintions;
    }
}
