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

        this.SigilLabel.text = SigilUtils.SigilToChar(this.Data.Sigil);
    }
}
