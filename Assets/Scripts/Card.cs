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
    private TextMeshPro SigilLabel = null;

    public void SetCard(CardData cardData)
    {
        this.Data = cardData;
        this.RefreshVisuals();
    }

    void RefreshVisuals()
    {
        this.NumberLabel.text = this.Data.NumberValue.ToString();

        string sigilDisplay = string.Empty;
        switch (this.Data.Sigil)
        {
            case Sigil.Star:
                sigilDisplay = "*";
                break;
            case Sigil.Cross:
                sigilDisplay = "+";
                break;
            case Sigil.Serpent:
                sigilDisplay = "~";
                break;
            case Sigil.Heart:
                sigilDisplay = "<3";
                break;
            default:
                throw new System.NotImplementedException();
        }

        this.SigilLabel.text = sigilDisplay;
    }
}
