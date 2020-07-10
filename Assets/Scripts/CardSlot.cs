using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    [SerializeField]
    private Shapes.Rectangle Back = null;

    [SerializeField]
    private Shapes.Rectangle Border = null;

    [SerializeField]
    private Color baseBorderColor = Color.grey;
    [SerializeField]
    private Color hoverBorderColor = Color.grey;

    private void OnMouseEnter()
    {
        this.Border.Color = this.hoverBorderColor;
    }

    private void OnMouseExit()
    {
        this.Border.Color = this.baseBorderColor;
    }
}
