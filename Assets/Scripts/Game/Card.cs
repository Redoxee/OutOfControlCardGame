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
    private Color[] colors = null;

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
        Color color = this.baseBackColor;
        switch (this.Data.Sigil)
        {
            case Sigil.Diamond:
                sprite = this.sprites[0];
                color = this.colors[0];
                break;
            case Sigil.Heart:
                sprite = this.sprites[1];
                color = this.colors[1];
                break;
            case Sigil.Clover:
                sprite = this.sprites[2];
                color = this.colors[2];
                break;
            case Sigil.Spade:
                sprite = this.sprites[3];
                color = this.colors[3];
                break;
        }

        this.SigilSprite.sprite = sprite;
        this.baseBackColor = color;
        this.Back.Color = color;
    }
}
