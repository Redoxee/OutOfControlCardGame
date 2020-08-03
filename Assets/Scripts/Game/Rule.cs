using System.Collections;
using UnityEngine;
using TMPro;

public class Rule : BorderComponent
{
    [System.NonSerialized]
    public RuleDefinition Data = null;

    [SerializeField]
    private SpriteRenderer[] IconRenderer = null;
    [SerializeField]
    private float IconSeparation = 1;

    public void SetRule(RuleDefinition rule)
    {
        this.Data = rule;

        Sprite[] sprites = rule.GetRuleSprites();

        float halfWidth = this.IconSeparation * (sprites.Length - 1) / 2;

        for (int index = 0; index < this.IconRenderer.Length; ++index)
        {
            if (index >= sprites.Length)
            {
                this.IconRenderer[index].gameObject.SetActive(false);
                continue;
            }

            this.IconRenderer[index].gameObject.SetActive(true);
            this.IconRenderer[index].sprite = sprites[index];
            Vector3 position = this.IconRenderer[index].transform.localPosition;
            position.y = -halfWidth + (index * this.IconSeparation);
            this.IconRenderer[index].transform.localPosition = position;
        }
    }
}
