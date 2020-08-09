using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class RuleDefinition : AMG.Data
{
    [SerializeField]
    public GameObject IllustrationPrefab = null;

    [SerializeField]
    public string Description = string.Empty;
    
    [System.NonSerialized]
    protected GameObject[] IllustrationPrefabs = null;

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
        if (MainManager.Instance == null)
        {
            return this.Description;
        }

        LocalizationManager localizationManager = MainManager.Instance.LocalizationManager;
        return localizationManager.GetString(this.Description);
    }

    public virtual GameObject[] GetIllustrationPrefabs()
    {
        if (this.IllustrationPrefabs == null)
        {
            this.IllustrationPrefabs = new GameObject[] { this.IllustrationPrefab };
        }

        return this.IllustrationPrefabs;
    }
}
