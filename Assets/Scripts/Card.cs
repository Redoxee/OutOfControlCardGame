using System.Collections;
using Shapes;
using UnityEngine;
using TMPro;

public enum Sigil
{
    Star,
    Cross,
    Serpent,
    Heart,
}

public class Card : MonoBehaviour
{
    public int NumberValue;
    public Sigil Sigil;

    [SerializeField]
    private TextMeshPro NumberLabel = null;
    [SerializeField]
    private TextMeshPro SigilLabel = null;

    public Color BaseBorderColor = Color.black;
    public Color HoverColor = Color.yellow;

    [SerializeField]
    private Shapes.ShapeRenderer borderRectangle = null;

    private void OnMouseEnter()
    {
        this.borderRectangle.Color = this.HoverColor;
    }

    private void OnMouseExit()
    {
        this.borderRectangle.Color = this.BaseBorderColor;
    }
}
