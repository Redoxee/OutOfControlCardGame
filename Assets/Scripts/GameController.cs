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

    private List<CardData> availableCards = new List<CardData>();

    private List<RuleData> availableRules = new List<RuleData>();

    private void Start()
    {
        this.InitializeCardData();
        this.InitializeRuleData();

        this.DrawCardForSlot(0);
        this.DrawCardForSlot(1);
        this.DrawCardForSlot(2);

        this.InitializeRuleSlots();

        this.DrawRuleForSlot(0);
    }

    [SerializeField]
    private GameObject cardPrefab = null;
    [SerializeField]
    private GameObject rulePrefab = null;

    private bool[] workingAllowedArray = new bool[9];

    private void InitializeCardData()
    {
        this.availableCards.Clear();
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

                this.availableCards.Add(cData);
            }
        }
    }

    private void InitializeRuleData()
    {
        this.availableRules.Clear();

        availableRules.Add(
                new DiagonalRule {
                    TargetedSigil = Sigil.Star,
                }
            );
    }

    private void InitializeRuleSlots()
    {
        for (int index = 0; index < 3; ++index)
        {
            this.handRuleSlots[index].OnHover += OnHandRuleHovered;
        }
    }

    private CardData DrawCard()
    {
        if (this.availableCards.Count == 0)
        {
            this.InitializeCardData();
        }

        int cardIndex = Random.Range(0, this.availableCards.Count);
        CardData result = this.availableCards[cardIndex];
        this.availableCards.RemoveAt(cardIndex);

        return result;
    }

    private void DrawCardForSlot(int slotIndex)
    {
        CardData cData = this.DrawCard();

        GameObject cardObject = Instantiate(this.cardPrefab);
        Card card = cardObject.GetComponent<Card>();
        Debug.Assert(card != null);
        card.SetCard(cData);
        cardObject.transform.position = this.handSlots[slotIndex].transform.position;

        this.handSlots[slotIndex].Card = card;
    }

    private RuleData DrawRule()
    {
        if (this.availableRules.Count == 0)
        {
            this.InitializeRuleData();
        }

        int ruleIndex = Random.Range(0, this.availableRules.Count);
        RuleData rule = this.availableRules[ruleIndex];
        this.availableRules.RemoveAt(ruleIndex);
        return rule;
    }

    private void DrawRuleForSlot(int slotIndex)
    {
        RuleData rData = this.DrawRule();
        GameObject ruleObject = Instantiate(this.rulePrefab);
        Rule rule = ruleObject.GetComponent<Rule>();
        Debug.Assert(rule != null);
        rule.Data = rData;
        ruleObject.transform.position = this.handRuleSlots[slotIndex].transform.position;
        this.handRuleSlots[slotIndex].Rule = rule;
    }

    public void OnHandRuleHovered(BorderComponent slot, bool on)
    {
        RuleSlot ruleSlot = (RuleSlot)slot;
        if (on)
        {
            if (ruleSlot.Rule != null && ruleSlot.Rule.Data != null)
            {
                ruleSlot.Rule.Data.GetAllowedSlots(ref this.handSlots[0].Card.Data, this.playSlots, ref this.workingAllowedArray);
                for (int slotIndex = 0; slotIndex < 9; ++slotIndex)
                {
                    if (!this.workingAllowedArray[slotIndex])
                    {
                        this.playSlots[slotIndex].SetBorderColor(Color.red);
                    }
                    else
                    {
                        this.playSlots[slotIndex].SetBorderColor(Color.green);
                    }
                }
            }
        }
        else
        {
            this.ResetPlayMatHoverBorders();
        }
    }

    private void ResetPlayMatHoverBorders()
    {
        for (int index = 0; index < this.playSlots.Length; ++index)
        {
            this.playSlots[index].ResetBorderColor();
        }
    }
}
