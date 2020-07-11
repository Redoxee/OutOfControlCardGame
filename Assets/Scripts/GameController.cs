using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private CardSlot[] handSlots = null;
    [SerializeField]
    private RuleSlot[] handRuleSlots = null;

    [SerializeField]
    private CardSlot[] playSlots = null;
    [SerializeField]
    private RuleSlot[] playRuleSlots = null;

    private List<CardData> allCards = new List<CardData>();
    private List<CardData> availableCards = new List<CardData>();

    [SerializeField]
    private GameObject cardPrefab = null;
    [SerializeField]
    private GameObject rulePrefab = null;

    private void InitializeCardData()
    {
        for (int index = 0; index < 10; ++index)
        {
            for (int sigilIndex = 0; sigilIndex < 4; ++sigilIndex)
            {
                Sigil sigil = (Sigil)(1 << sigilIndex);
                CardData cData = new CardData()
                {
                    NumberValue = index + 1,
                    Sigil = sigil,
                };

                this.allCards.Add(cData);
                this.availableCards.Add(cData);
            }
        }
    }

    private void Start()
    {
        this.InitializeCardData();

        GameObject cardObject = Instantiate(this.cardPrefab);
        Card card = cardObject.GetComponent<Card>();
        Debug.Assert(card != null);
        card.SetCard(this.availableCards[39]);
        cardObject.transform.position = this.handSlots[0].transform.position;
    }
}
