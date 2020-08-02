using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class RuleDefinition : ScriptableObject
{
    [SerializeField]
    private Sprite mainSprite = null;

    [SerializeField]
    private string Description = string.Empty;

    protected Sprite[] spriteArray;

    public abstract bool IsSlotAllowed(ref CardData card, CardSlot[] cardSlots, int x, int y);

    public void GetAllowedSlots(ref CardData card, CardSlot[] cardSlots, ref bool[] slots)
    {
        for (int y = 0; y < GameController.GridSize; ++y)
        {
            for (int x = 0; x < GameController.GridSize; ++x)
            {
                slots[y * GameController.GridSize + x] = this.IsSlotAllowed(ref card, cardSlots, x, y);
            }
        }
    }

    public override string ToString()
    {
        return this.Description;
    }

    public virtual Sprite[] GetRuleSprites()
    {
        return new Sprite[] { this.mainSprite };
    }
}
