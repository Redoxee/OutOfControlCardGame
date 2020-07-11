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

public class Card : BorderComponent
{
    public int NumberValue;
    public Sigil Sigil;

    [SerializeField]
    private TextMeshPro NumberLabel = null;
    [SerializeField]
    private TextMeshPro SigilLabel = null;
}
