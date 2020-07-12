using System.Collections;
using Shapes;
using UnityEngine;
using TMPro;

public class Card : BorderComponent
{
    public CardData Data;

    [SerializeField]
    private TextMeshPro NumberLabel = null;

    [SerializeField]
    private Sprite[] sprites = null;

    [SerializeField]
    SpriteRenderer SigilSprite = null;

    public void SetCard(CardData cardData)
    {
        this.Data = cardData;
        this.RefreshVisuals();
    }

    void RefreshVisuals()
    {
        this.NumberLabel.text = this.Data.NumberValue.ToString();

        Sprite sprite = null;
        switch (this.Data.Sigil)
        {
            case Sigil.Star:
                sprite = this.sprites[0];
                break;
            case Sigil.Moon:
                sprite = this.sprites[1];
                break;
            case Sigil.Diamond:
                sprite = this.sprites[2];
                break;
            case Sigil.Leaf:
                sprite = this.sprites[3];
                break;
        }

        this.SigilSprite.sprite = sprite;
    }
}
