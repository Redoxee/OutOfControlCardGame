using System;
using System.Collections;
using UnityEngine;

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
        this.currentState = State.CardPlacement;

        this.tutorialRightPanel.FadeOutIfNeeded();
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
        if (this.playRuleSlots[this.playRuleSlots.Length - 1].Rule != null)
        {
            this.DeleteRule(this.playRuleSlots[this.playRuleSlots.Length - 1].Rule);
            this.playRuleSlots[this.playRuleSlots.Length - 1].Rule = null;
        }

        for (int ruleIndex = this.playRuleSlots.Length - 1; ruleIndex > 0; --ruleIndex)
        {
            this.playRuleSlots[ruleIndex].Rule = this.playRuleSlots[ruleIndex - 1].Rule;

            if (this.playRuleSlots[ruleIndex].Rule != null)
            {
                this.playRuleSlots[ruleIndex].Rule.transform.position = this.playRuleSlots[ruleIndex].transform.position;
            }
        }

        this.playRuleSlots[0].Rule = this.handRuleSlots[index].Rule;
        if (this.playRuleSlots[0].Rule != null)
        {
            this.playRuleSlots[0].Rule.transform.position = this.playRuleSlots[0].transform.position;
        }

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

        this.tutorialRightPanel.FadeOutIfNeeded();

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

        this.currentState = State.CardPlacement;

        this.tutorialRightPanel.FadeOutIfNeeded();
        this.SelectCard(selectedindex);

        yield return this.PrepareCardPlacementRoutine(selectedindex, ruleIndex);
    }

    IEnumerator PrepareCardPlacementRoutine(int cardIndex, int ruleIndex)
    {
        this.leftPanel.FadeIn();
        Vector3 nextCardPosition = this.nextPlayedCard.transform.position;
        Vector3 cardEndPosition = this.leftPanel.CardAnchor.transform.position;
        nextCardPosition.z = cardEndPosition.z;
        this.nextPlayedCard.transform.position = nextCardPosition;
        float translationDuration = 1;
        float start = Time.timeSinceLevelLoad;
        float timer = 0;
        while (timer < translationDuration)
        {
            timer = Time.timeSinceLevelLoad - start;
            float progression = this.cardSlideCurve.Evaluate(timer / translationDuration);
            this.nextPlayedCard.transform.position = nextCardPosition + (cardEndPosition - nextCardPosition) * progression;
            yield return null;
        }

        this.nextPlayedCard.transform.position = cardEndPosition;

        this.DrawCardForSlot(cardIndex);
        this.DrawRuleForSlot(ruleIndex);
        this.currentState = State.CardPlacement;
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

    private void PlayCard(int index)
    {
        if (this.nextPlayedCard == null)
        {
            return;
        }

        int numberOfFailures = 0;

        for (int ruleIndex = 0; ruleIndex < this.playRuleSlots.Length; ++ruleIndex)
        {
            RuleData ruleData = this.playRuleSlots[ruleIndex].Rule?.Data;
            if (ruleData == null)
            {
                continue;
            }

            int x = index % 3;
            int y = index / 3;
            bool isAllowed = ruleData.IsSlotAllowed(ref this.nextPlayedCard.Data, this.playSlots, x, y);

            if (isAllowed)
            {
                this.score += 1;
                this.playRuleSlots[ruleIndex].FlashGreen();
                this.playRuleSlots[ruleIndex].PlayCheckMark();
            }
            else
            {
                numberOfFailures++;
                this.playRuleSlots[ruleIndex].FlashRed();
                this.playRuleSlots[ruleIndex].PlayCrossMark();
            }
        }

        if (numberOfFailures > 0)
        {
            this.lifeCount--;
            if (this.lifeCount < 1)
            {
                this.currentState = State.Lost;
                StartCoroutine(this.EndGameRoutine());
            }
        }

        this.RefreshGameLabels();

        if (this.playSlots[index].Card != null)
        {
            this.DeleteCard(this.playSlots[index].Card);
        }

        this.playSlots[index].Card = this.nextPlayedCard;
        this.playSlots[index].Card.transform.position = this.playSlots[index].transform.position;
        this.nextPlayedCard = null;

        this.currentState = State.CardRuleChoice;
        this.leftPanel.FadeOut();
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
