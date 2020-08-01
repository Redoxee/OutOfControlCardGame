using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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

    [SerializeField]
    private BorderComponent playMatBorder = null;

    private Card nextPlayedCard = null;
    private Rule nextRule = null;

    [SerializeField]
    private AnimationCurve randomPointerCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
    [SerializeField]
    private float randomPointerAnimationDuration = 2;
    [SerializeField]
    private GameObject cardRandomPointer = null;
    [SerializeField]
    private GameObject ruleRandomPointer = null;

    [SerializeField]
    private AnimationCurve cardSlideCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

    [SerializeField]
    private PlayLeftPanel leftPanel = null;

    [SerializeField]
    private List<CardData> availableCards = new List<CardData>();

    [SerializeField]
    private List<RuleDefinition> availableRules = new List<RuleDefinition>();

    [SerializeField]
    private int startingLives = 4;
    private int lifeCount = 4;
    private int score = 0;
    [SerializeField]
    private TextMeshPro scoreLabel = null;
    [SerializeField]
    private TextMeshPro lifeLabel = null;

    [SerializeField]
    private Transform IntermediateCardAnchor = null;

    [SerializeField]
    private Transform rulePreDestination = null;
    [SerializeField]
    private AnimationCurve ruleSlideCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

    [SerializeField]
    private float playCardAnimDuration = 1;
    [SerializeField]
    private AnimationCurve playCardCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
    [SerializeField]
    private AnimationCurve playCardScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

    [SerializeField]
    private float playRuleAnimationDuration = 1f;
    [SerializeField]
    private AnimationCurve playRuleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
    [SerializeField]
    private AnimationCurve playRuleScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

    [SerializeField]
    private Transform HandTransform = null;
    [SerializeField]
    private AnimationCurve HandAnimationCurve = new AnimationCurve(new Keyframe[]{new Keyframe(0, 0), new Keyframe(1, 1) });
    [SerializeField]
    private Transform VisibleHandPosition = null;
    [SerializeField]
    private Transform HiddenHandPosition = null;

    [SerializeField]
    private Transform RuleHandTransform = null;
    [SerializeField]
    private Transform VisibleRuleHandPosition = null;
    [SerializeField]
    private Transform HiddenRuleHandPosition = null;

    [SerializeField]
    private Transform RuleExitAnchor = null;

    private Rule ruleToPhaseOut = null;

    private IEnumerator Start()
    {
        this.Bind();

        if (MainManager.Instance == null)
        {
            MainManager.LoadMainSceneIfNecessary();

            int failSafe = 200;
            while (failSafe > 0 && MainManager.Instance == null)
            {
                failSafe--;
                yield return null;
            }

            if (failSafe <= 0)
            {
                yield break;
            }
        }


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

        for (int index = 0; index < this.playRuleSlots.Length; ++index)
        {
            this.playRuleSlots[index].DeactivateHover = false;
            this.playRuleSlots[index].OnHover += OnPlayRuleHover;
        }

        this.RefreshGameLabels();
    }

    private void OnDisable()
    {
        for (int index = 0; index < this.playRuleSlots.Length; ++index)
        {
            this.playRuleSlots[index].OnHover -= OnPlayRuleHover;
        }
    }

    private void OnPlayRuleHover(BorderComponent component, bool hovered)
    {
        RuleSlot slot = component.gameObject.GetComponent<RuleSlot>();
        if (currentState != State.CardPlacement || 
            slot.Rule == null ||
            !hovered)
        {
            for (int index = 0; index < this.playSlots.Length; ++index)
            {
                this.playSlots[index].ResetBorderColor();
            }

            return;
        }

        for (int x = 0; x < GameController.GridSize; ++x)
        {
            for (int y = 0; y < GameController.GridSize; ++y)
            {
                int slotIndex = y * GameController.GridSize + x;
                if (slot.Rule.Data.IsSlotAllowed(ref this.nextPlayedCard.Data, this.playSlots, x, y))
                {
                    this.playSlots[slotIndex].SetBorderColor(Color.green);
                }
                else
                {
                    this.playSlots[slotIndex].SetBorderColor(Color.red);
                }
            }
        }
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

    private RuleDefinition DrawRule()
    {
        Debug.Assert(this.availableRules.Count > 0);

        int ruleIndex = -1;
        RuleDefinition rule = null;

        ruleIndex = Random.Range(0, this.availableRules.Count);
        rule = this.availableRules[ruleIndex];

        this.availableRules.RemoveAt(ruleIndex);
        return rule;
    }

    private void FreeRuleData(RuleDefinition data)
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
        cardObject.transform.SetParent(this.handSlots[slotIndex].transform, true);
        this.handSlots[slotIndex].Card = card;
    }

    private void DeleteCard(Card card)
    {
        this.FreeCardData(card.Data);
        Destroy(card.gameObject);
    }

    private void DrawRuleForSlot(int slotIndex)
    {
        RuleDefinition rData = this.DrawRule();
        GameObject ruleObject = Instantiate(this.rulePrefab, this.handRuleSlots[slotIndex].transform);
        Rule rule = ruleObject.GetComponent<Rule>();
        Debug.Assert(rule != null);
        rule.SetRule(rData);
        ruleObject.transform.position = this.handRuleSlots[slotIndex].transform.position;
        this.handRuleSlots[slotIndex].Rule = rule;
    }

    private void DeleteRule(Rule rule)
    {
        this.FreeRuleData(rule.Data);
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
        this.lifeLabel.text = $"Lives : {this.lifeCount}";
    }
}
