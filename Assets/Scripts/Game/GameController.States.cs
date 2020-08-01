using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public partial class GameController
{
    public enum State
    {
        Initializing = 0,
        CardRuleChoice = 1,
        CardPlacement = 2,
        Lost = 3,
        TransitionToPlacement = 4,
    }

    public enum RulePassingState
    {
        None,
        Success,
        Failed,
    }

    private void Bind()
    {
        for (int index = 0; index < 3; ++index)
        {
            this.handSlots[index].Index = index;
            this.handSlots[index].OnPressed += this.OnHandCardPressed;
        }

        for (int index = 0; index < 3; ++index)
        {
            this.handRuleSlots[index].Index = index;
            this.handRuleSlots[index].OnHover += this.OnHandRuleHovered;
            this.handRuleSlots[index].OnPressed += this.OnHandRulePressed;
        }

        for (int index = 0; index < 9; ++index)
        {
            this.playSlots[index].Index = index;
            this.playSlots[index].OnPressed += this.OnPlayMatCardPressed;
        }

        for (int index = 0; index < 3; ++index)
        {
            this.playRuleSlots[index].Index = index;
        }
    }

    private State currentState = State.Initializing;

    public void OnHandRuleHovered(BorderComponent slot, bool on)
    {
    }

    public void OnHandCardPressed(BorderComponent slot, bool isOn)
    {
        if (this.currentState != State.CardRuleChoice)
        {
            return;
        }

        CardSlot cardSlot = (CardSlot)slot;
        this.SelectCard(cardSlot.Index);
        this.SelectRandomRule(cardSlot.Index);
    }

    public void OnHandRulePressed(BorderComponent slot, bool isOn)
    {
        if (this.currentState != State.CardRuleChoice)
        {
            return;
        }

        RuleSlot ruleSlot = (RuleSlot)slot;
        this.SelectHandRule(ruleSlot.Index);
        this.SelectRandomCard(ruleSlot.Index);
    }
    
    private void SelectCard(int index)
    {
        this.nextPlayedCard = this.handSlots[index].Card;

        this.handSlots[index].Card = null;
    }

    private void SelectHandRule(int index)
    {
        this.nextRule = this.handRuleSlots[index].Rule;
        this.handRuleSlots[index].Rule = null;
    }

    private void SelectRandomRule(int cardIndex)
    {
        this.currentState = State.TransitionToPlacement;
        StartCoroutine(this.RandomRuleCoroutine(cardIndex));
    }

    public IEnumerator RandomRuleCoroutine(int selectedCard)
    {
        int ruleIndex = UnityEngine.Random.Range(0, this.handRuleSlots.Length);
        float start = Time.timeSinceLevelLoad;
        float timer = Time.timeSinceLevelLoad - start;
        this.ruleRandomPointer.gameObject.SetActive(true);

        while (timer < this.randomPointerAnimationDuration)
        {
            timer = Time.timeSinceLevelLoad - start;
            float progression = this.randomPointerCurve.Evaluate(timer / this.randomPointerAnimationDuration);
            int fakeIndex = (int)(progression * this.handSlots.Length * 8 + ruleIndex) % this.handRuleSlots.Length;
            this.ruleRandomPointer.transform.position = this.handRuleSlots[fakeIndex].transform.position;
            yield return null;
        }

        this.handRuleSlots[ruleIndex].FlashGreen();
        yield return new WaitForSeconds(2);
        this.ruleRandomPointer.gameObject.SetActive(false);

        this.SelectHandRule(ruleIndex);
        this.StartCoroutine(this.PrepareCardPlacementRoutine(selectedCard, ruleIndex));
    }

    private void SelectRandomCard(int ruleIndex)
    {
        this.currentState = State.TransitionToPlacement;
        this.StartCoroutine(this.RandomCardRoutine(ruleIndex));
    }

    public IEnumerator RandomCardRoutine(int ruleIndex)
    {
        int selectedindex = UnityEngine.Random.Range(0, this.handSlots.Length);
        float start = Time.timeSinceLevelLoad;
        float timer = Time.timeSinceLevelLoad - start;
        this.cardRandomPointer.gameObject.SetActive(true);

        while (timer < this.randomPointerAnimationDuration)
        {
            timer = Time.timeSinceLevelLoad - start;
            float progression = this.randomPointerCurve.Evaluate(timer / this.randomPointerAnimationDuration);
            int fakeIndex = (int)(progression * this.handSlots.Length * 8 + selectedindex) % this.handSlots.Length;
            this.cardRandomPointer.transform.position = this.handSlots[fakeIndex].transform.position;
            yield return null;
        }

        this.handSlots[selectedindex].FlashGreen();
        yield return new WaitForSeconds(2);
        this.cardRandomPointer.gameObject.SetActive(false);
        
        this.SelectCard(selectedindex);

        yield return this.PrepareCardPlacementRoutine(selectedindex, ruleIndex);
    }

    IEnumerator PrepareCardPlacementRoutine(int cardIndex, int ruleIndex)
    {
        this.leftPanel.FadeOut();

        Vector3 nextCardPosition = this.nextPlayedCard.transform.position;
        Vector3 cardEndPosition = this.IntermediateCardAnchor.position;

        nextCardPosition.z = cardEndPosition.z;
        this.nextPlayedCard.transform.SetParent(this.transform, true);
        this.nextPlayedCard.transform.position = nextCardPosition;

        this.nextRule.transform.SetParent(this.transform, true);
        Vector3 ruleStartingPosition = this.nextRule.transform.position;
        Vector3 rulePreDestination = this.rulePreDestination.position;
        ruleStartingPosition.z = rulePreDestination.z;
        this.nextRule.transform.position = ruleStartingPosition;

        Vector3 handStartPosition = this.VisibleHandPosition.position;
        Vector3 handEndPosition = this.HiddenHandPosition.position;

        float translationDuration = .5f;

        this.nextRule.transform.DOMove(this.playRuleSlots[0].transform.position, this.playRuleAnimationDuration).SetEase(this.playRuleCurve);
        this.nextRule.transform.DOScale(new Vector3(1.4f, 1.4f, 0), this.playRuleAnimationDuration).SetEase(this.playRuleScaleCurve);

        this.hideRulesTween = this.RuleHandTransform.DOMove(this.HiddenRuleHandPosition.position, translationDuration);
        this.hideRulesTween.SetEase(Ease.InCubic);

        float startDate = Time.timeSinceLevelLoad;
        float timer = 0;
        while (timer < translationDuration)
        {
            timer = Time.timeSinceLevelLoad - startDate;
            float cardProgression = this.cardSlideCurve.Evaluate(timer / translationDuration);
            this.nextPlayedCard.transform.position = nextCardPosition + (cardEndPosition - nextCardPosition) * cardProgression;

            float handProgression = this.HandAnimationCurve.Evaluate(timer / translationDuration);
            this.HandTransform.transform.position = handStartPosition + (handEndPosition - handStartPosition) * handProgression;
            yield return null;
        }

        this.nextPlayedCard.transform.position = cardEndPosition;
        this.HandTransform.transform.position = handEndPosition;

        timer = 0;
        startDate = Time.timeSinceLevelLoad;

        FlashPlayMat();

        this.playRuleSlots[0].Rule = this.nextRule;

        while (timer < translationDuration)
        {
            timer = Time.timeSinceLevelLoad - startDate;
            yield return null;
        }


        this.DrawCardForSlot(cardIndex);
        this.DrawRuleForSlot(ruleIndex);
        this.currentState = State.CardPlacement;
    }

    private void FlashPlayMat()
    {
        this.playMatBorder.FlashWhite();
        for (int index = 0; index < this.playSlots.Length; ++index)
        {
            this.playSlots[index].FlashWhite();
        }
    }

    public void OnPlayMatCardPressed(BorderComponent slot, bool isOn)
    {
        if (this.currentState != State.CardPlacement)
        {
            return;
        }

        CardSlot cardSlot = (CardSlot)slot;
        PlayCard(cardSlot.Index);
    }

    private void PlayCard(int slotIndex)
    {
        if (this.nextPlayedCard == null)
        {
            return;
        }

        int numberOfFailures = 0;
        RulePassingState[] rulePassingStates = new RulePassingState[this.playRuleSlots.Length];

        for (int ruleIndex = 0; ruleIndex < this.playRuleSlots.Length; ++ruleIndex)
        {
            RuleDefinition ruleDefinition = this.playRuleSlots[ruleIndex].Rule?.Data;
            if (ruleDefinition == null)
            {
                rulePassingStates[ruleIndex] = RulePassingState.None;
                continue;
            }

            int x = slotIndex % 3;
            int y = slotIndex / 3;
            bool isAllowed = ruleDefinition.IsSlotAllowed(ref this.nextPlayedCard.Data, this.playSlots, x, y);
            rulePassingStates[ruleIndex] = isAllowed ? RulePassingState.Success : RulePassingState.Failed;
            if (!isAllowed)
            {
                numberOfFailures++;
            }
            else
            {
                this.score++;
                this.TransferOneCombinedRule();
            }
        }

        if (numberOfFailures > 0)
        {
            this.lifeCount--;
        }

        if (this.playRuleSlots[this.playRuleSlots.Length - 1].Rule != null)
        {
            this.ruleToPhaseOut = this.playRuleSlots[this.playRuleSlots.Length - 1].Rule;
        }

        for (int index = this.playRuleSlots.Length - 1; index > 0; --index)
        {
            if (this.playRuleSlots[index - 1].Rule == null)
            {
                continue;
            }

            this.playRuleSlots[index].Rule = this.playRuleSlots[index - 1].Rule;
            this.playRuleSlots[index].Rule.transform.SetParent(this.playRuleSlots[index].transform);
        }

        this.RefreshGameLabels();
        this.currentState = State.TransitionToPlacement;
        StartCoroutine(this.PlayCardRoutine(slotIndex, rulePassingStates));
    }

    private IEnumerator PlayCardRoutine(int cardSlotindex, RulePassingState[] rulePassingStates)
    {
        float startDate = Time.timeSinceLevelLoad;
        float timer = 0;
        Vector3 startPosition = this.IntermediateCardAnchor.transform.position;
        Vector3 endPosition = this.playSlots[cardSlotindex].transform.position;
        endPosition.z -= 3;
        startPosition.z = endPosition.z;

        while (timer < this.playCardAnimDuration)
        {
            timer = Time.timeSinceLevelLoad - startDate;
            float progression = timer / this.playCardAnimDuration;
            this.nextPlayedCard.transform.position = startPosition + (endPosition - startPosition) * this.playCardCurve.Evaluate(progression);
            this.nextPlayedCard.transform.localScale = Vector3.one * this.playCardScaleCurve.Evaluate(progression);

            yield return null;
        }

        for (int ruleIndex = 0; ruleIndex < this.playRuleSlots.Length; ++ruleIndex)
        {
            RulePassingState passingState = rulePassingStates[ruleIndex];
            if (passingState == RulePassingState.None)
            {
                continue;
            }

            if (passingState == RulePassingState.Success)
            {
                this.playRuleSlots[ruleIndex].FlashGreen();
                this.playRuleSlots[ruleIndex].PlayCheckMark();
            }
            else
            {
                this.playRuleSlots[ruleIndex].FlashRed();
                this.playRuleSlots[ruleIndex].PlayCrossMark();
            }

            yield return new WaitForSeconds(.5f);
        }

        timer = 0;
        float translationDuration = .5f;

        this.showRulesTween =  this.RuleHandTransform.DOMove(this.VisibleRuleHandPosition.position, translationDuration);
        this.showRulesTween.SetEase(Ease.OutCubic);

        startDate = Time.timeSinceLevelLoad;
        Vector3 handStartPosition = this.HiddenHandPosition.position;
        Vector3 handEndPosition = this.VisibleHandPosition.position;

        this.leftPanel.FadeIn();

        while (timer < translationDuration)
        {
            timer = Time.timeSinceLevelLoad - startDate;

            float handProgression = this.HandAnimationCurve.Evaluate(timer / translationDuration);
            this.HandTransform.transform.position = handStartPosition + (handEndPosition - handStartPosition) * handProgression;

            yield return null;
        }

        this.HandTransform.transform.position = handEndPosition;
        this.RefreshGameLabels();

        if (this.playSlots[cardSlotindex].Card != null)
        {
            this.DeleteCard(this.playSlots[cardSlotindex].Card);
        }

        Color noBorder = Color.black;
        noBorder.a = 0f;
        Color backColor = Color.white;
        backColor.a = .5f;

        this.nextPlayedCard.FadeBorderToColor(noBorder, backColor, .5f);

        this.playSlots[cardSlotindex].Card = this.nextPlayedCard;
        this.playSlots[cardSlotindex].Card.transform.position = this.playSlots[cardSlotindex].transform.position;
        this.nextPlayedCard = null;
        
        if (this.lifeCount < 1)
        {
            this.currentState = State.Lost;
            StartCoroutine(this.EndGameRoutine());
            yield break;
        }

        float ruleAnimationDuration = .5f;
        Rule ruleToRemove = this.ruleToPhaseOut;
        if ( ruleToRemove != null)
        {
            Tween ruleExitTween = ruleToRemove.transform.DOMove(this.RuleExitAnchor.position, ruleAnimationDuration);
            ruleExitTween.SetEase(Ease.InCubic);
            ruleExitTween.onComplete += () => 
            {
                this.DeleteRule(ruleToRemove);
            };
        }

        for (int index = 0; index < this.playRuleSlots.Length; ++index)
        {
            if (this.playRuleSlots[index].Rule == null)
            {
                continue;
            }

            this.playRuleSlots[index].Rule.transform.DOLocalMove(Vector3.zero, ruleAnimationDuration);
        }

        this.currentState = State.CardRuleChoice;
    }

    private IEnumerator EndGameRoutine()
    {
        yield return new UnityEngine.WaitForSeconds(3);
        MainManager mainManager = MainManager.Instance;
        if (mainManager == null)
        {
            yield break;
        }

        mainManager.NotifyEndGame(this.score);
    }
}
