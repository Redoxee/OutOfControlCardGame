﻿using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GameController
{
    public enum State
    {
        Initializing = 0,
        CardRuleChoice = 1,
        CardPlacement = 2,
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

    public void OnHandCardPressed(BorderComponent slot, bool isOn)
    {
        if (this.currentState != State.CardRuleChoice)
        {
            return;
        }

        CardSlot cardSlot = (CardSlot)slot;
        this.SelectCard(cardSlot.Index);
        this.SelectRandomRule();
        this.currentState = State.CardPlacement;
    }

    public void OnHandRulePressed(BorderComponent slot, bool isOn)
    {
        if (this.currentState != State.CardRuleChoice)
        {
            return;
        }

        RuleSlot ruleSlot = (RuleSlot)slot;
        this.SelectHandRule(ruleSlot.Index);
        this.SelectRandomCard();
        this.currentState = State.CardPlacement;
    }

    private void SelectCard(int index)
    {
        this.nextCardSlot.Card = this.handSlots[index].Card;
        if (this.nextCardSlot.Card != null)
        {
            this.nextCardSlot.Card.transform.position = this.nextCardSlot.transform.position;
        }

        this.handSlots[index].Card = null;
        this.DrawCardForSlot(index);
    }

    private void SelectHandRule(int index)
    {
        if (this.playRuleSlots[2].Rule != null)
        {
            Destroy(this.playRuleSlots[2].Rule.gameObject);
        }

        this.playRuleSlots[2].Rule = this.playRuleSlots[1].Rule;
        if (this.playRuleSlots[2].Rule != null)
        {
            this.playRuleSlots[2].Rule.transform.position = this.playRuleSlots[2].transform.position;
        }

        this.playRuleSlots[1].Rule = this.playRuleSlots[0].Rule;
        if (this.playRuleSlots[1].Rule != null)
        {
            this.playRuleSlots[1].Rule.transform.position = this.playRuleSlots[1].transform.position;
        }

        this.playRuleSlots[0].Rule = this.handRuleSlots[index].Rule;
        if (this.playRuleSlots[0].Rule != null)
        {
            this.playRuleSlots[0].Rule.transform.position = this.playRuleSlots[0].transform.position;
        }

        this.handRuleSlots[index].Rule = null;
        this.DrawRuleForSlot(index);
    }

    private void SelectRandomRule()
    {
        int ruleIndex = UnityEngine.Random.Range(0, 3);
        this.SelectHandRule(ruleIndex);
    }

    private void SelectRandomCard()
    {
        int cardIndex = UnityEngine.Random.Range(0, 3);
        this.SelectCard(cardIndex);
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
        if (this.nextCardSlot.Card == null)
        {
            return;
        }

        if (this.playSlots[index].Card != null)
        {
            Destroy(this.playSlots[index].Card.gameObject);
        }

        this.playSlots[index].Card = this.nextCardSlot.Card;
        this.playSlots[index].Card.transform.position = this.playSlots[index].transform.position;
        this.nextCardSlot.Card = null;

        this.currentState = State.CardRuleChoice;
    }
}