using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderComponent : MonoBehaviour
{
    [SerializeField]
    private Shapes.Rectangle Back = null;

    [SerializeField]
    private Shapes.Rectangle Border = null;

    [SerializeField]
    private Color baseBorderColor = Color.grey;
    [SerializeField]
    private Color hoverBorderColor = Color.grey;

    public delegate void Interaction(BorderComponent component, bool on);
    public event Interaction OnHover;

    private void OnMouseEnter()
    {
        this.Border.Color = this.hoverBorderColor;
        if (OnHover != null)
        {
            this.OnHover(this, true);
        }
    }

    private void OnMouseExit()
    {
        this.Border.Color = this.baseBorderColor;
        if (OnHover != null)
        {
            this.OnHover(this, false);
        }
    }

    public void SetBorderColor(Color color)
    {
        this.Border.Color = color;
    }

    public void ResetBorderColor()
    {
        this.Border.Color = this.baseBorderColor;
    }
}
