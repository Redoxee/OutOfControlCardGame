﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public partial class GameController : MonoBehaviour
{
    public const int GridSize = 3;

    [SerializeField]
    private CardSlot[] handSlots = null;
    [SerializeField]
    private RuleSlot[] handRuleSlots = null;

    [SerializeField]
    private CardSlot[] playSlots = null;
    [SerializeField]
    private RuleSlot[] playRuleSlots = null;

    private Card nextPlayedCard = null;
    private Rule nextRule = null;

    [SerializeField]
    private AnimationCurve randomPointerCurve;
    [SerializeField]
    private float randomPointerAnimationDuration = 2;
    [SerializeField]
    private GameObject cardRandomPointer = null;
    [SerializeField]
    private GameObject ruleRandomPointer = null;

    [SerializeField]
    private AnimationCurve cardSlideCurve;

    [SerializeField]
    private PlayLeftPanel leftPanel = null;

    [SerializeField]
    private TutorialRightPanel tutorialRightPanel = null;

    private List<CardData> availableCards = new List<CardData>();

    private List<RuleData> availableRules = new List<RuleData>();

    [SerializeField]
    private int startingLives = 4;
    private int lifeCount = 4;
    private int score = 0;
    [SerializeField]
    private TextMeshPro scoreLabel = null;
    [SerializeField]
    private TextMeshPro lifeLabel = null;

    [SerializeField]
    private Transform rulePreDestination = null;
    [SerializeField]
    AnimationCurve ruleSlideCurve;

    private void Start()
    {
        this.Bind();

        this.InitializeCardData();
        this.InitializeRuleData();

        this.DrawCardForSlot(0);
        this.DrawCardForSlot(1);
        this.DrawCardForSlot(2);

        this.DrawRuleForSlot(0);
        this.DrawRuleForSlot(1);
        this.DrawRuleForSlot(2);

        this.currentState = State.CardRuleChoice;

        this.score = 0;
        this.lifeCount = this.startingLives;
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

    private CardData DrawCard()
    {
        Debug.Assert(this.availableCards.Count > 0);

        int cardIndex = Random.Range(0, this.availableCards.Count);
        CardData result = this.availableCards[cardIndex];
        this.availableCards.RemoveAt(cardIndex);

        return result;
    }

    private void FreeCardData(CardData data)
    {
        this.availableCards.Add(data);
    }

    private RuleData DrawRule()
    {
        Debug.Assert(this.availableRules.Count > 0);

        int ruleIndex = Random.Range(0, this.availableRules.Count);
        RuleData rule = this.availableRules[ruleIndex];
        this.availableRules.RemoveAt(ruleIndex);
        return rule;
    }

    private void FreeRuleData(RuleData data)
    {
        this.availableRules.Add(data);
    }

    private void DrawCardForSlot(int slotIndex)
    {
        CardData cData = this.DrawCard();

        GameObject cardObject = Instantiate(this.cardPrefab, this.transform);
        Card card = cardObject.GetComponent<Card>();
        Debug.Assert(card != null);
        card.SetCard(cData);
        cardObject.transform.position = this.handSlots[slotIndex].transform.position;

        this.handSlots[slotIndex].Card = card;
    }

    private void DeleteCard(Card card)
    {
        this.FreeCardData(card.Data);
        Destroy(card.gameObject);
    }

    private void DrawRuleForSlot(int slotIndex)
    {
        RuleData rData = this.DrawRule();
        GameObject ruleObject = Instantiate(this.rulePrefab, this.transform);
        Rule rule = ruleObject.GetComponent<Rule>();
        Debug.Assert(rule != null);
        rule.SetRule(rData);
        ruleObject.transform.position = this.handRuleSlots[slotIndex].transform.position;
        this.handRuleSlots[slotIndex].Rule = rule;
    }

    private void DeleteRule(Rule rule)
    {
        if (rule.Data != null)
        {
            this.FreeRuleData(rule.Data);
        }

        Destroy(rule.gameObject);
    }

    private void ResetPlayMatHoverBorders()
    {
        for (int index = 0; index < this.playSlots.Length; ++index)
        {
            this.playSlots[index].ResetBorderColor();
        }
    }

    private void RefreshGameLabels()
    {
        this.scoreLabel.text = $"Score : {this.score}";
        this.lifeLabel.text = $"Lifes : {this.lifeCount}";
    }
}
