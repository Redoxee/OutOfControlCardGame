using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class Rectangler : MonoBehaviour
{
    [InspectorButton("SetupRectangle", "Setup rectangle")]
    [SerializeField]
    private Rectangle back = null;
    [SerializeField]
    private Vector2 backSize = Vector2.one;
    [SerializeField]
    private Color backColor = Color.white;
    [SerializeField]
    private Rectangle border = null;
    [SerializeField]
    private float borderThickness = 1;
    [SerializeField]
    private Color borderColor = Color.white;
    [SerializeField]
    private Rectangle rim = null;
    [SerializeField]
    private float rimThickness = .1f;
    [SerializeField]
    private Color rimColor = Color.black;

    [SerializeField]
    private float cornerRadius = 0;

    private void SetupRectangle()
    {
        int childCount = this.transform.childCount;
        for (int index = 0; index < childCount; ++index)
        {
            Transform child = this.transform.GetChild(index);
            Rectangle childRectangle = child.GetComponent<Rectangle>();
            if (this.back == null && child.name == "Back" && childRectangle != null)
            {
                this.back = childRectangle;
            }

            if (this.border == null && child.name == "Border" && childRectangle != null)
            {
                this.border = childRectangle;
            }

            if (this.rim == null && child.name == "Rim" && childRectangle != null)
            {
                this.rim = childRectangle;
            }
        }

        if (this.back == null)
        {
            GameObject backObject = new GameObject("Back");
            backObject.transform.SetParent(this.transform, false);
            this.back = backObject.AddComponent(typeof(Shapes.Rectangle)) as Rectangle;
            this.back.Type = Rectangle.RectangleType.RoundedSolid;
        }

        if (this.border == null)
        {
            GameObject borderObject = new GameObject("Border");
            borderObject.transform.SetParent(this.transform, false);
            this.border = borderObject.AddComponent(typeof(Shapes.Rectangle)) as Rectangle;
            this.border.Type = Rectangle.RectangleType.RoundedHollow;
            borderObject.transform.localPosition = new Vector3(0, 0, .25f);
        }

        if (this.rim == null)
        {
            GameObject rimObject = new GameObject("Rim");
            rimObject.transform.SetParent(this.transform, false);
            this.rim = rimObject.AddComponent(typeof(Shapes.Rectangle)) as Rectangle;
            this.rim.Type = Rectangle.RectangleType.RoundedHollow;
            rimObject.transform.localPosition = new Vector3(0, 0, .5f);
        }

        this.back.CornerRadius = this.cornerRadius;
        this.border.CornerRadius = this.cornerRadius;
        this.rim.CornerRadius = this.cornerRadius;

        this.back.Color = this.backColor;
        this.border.Color = borderColor;
        this.rim.Color = this.rimColor;

        this.back.Width = this.backSize.x;
        this.back.Height = this.backSize.y;

        this.border.Width = this.back.Width + this.borderThickness;
        this.border.Height = this.back.Height + this.borderThickness;
        this.border.Thickness = this.borderThickness;

        this.rim.Width = this.border.Width + this.rimThickness;
        this.rim.Height = this.border.Height + this.rimThickness;
        this.rim.Thickness = this.rimThickness;
    }
}
