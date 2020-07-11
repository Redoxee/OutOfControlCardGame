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
    protected Color baseBorderColor = Color.grey;
    [SerializeField]
    protected Color hoverBorderColor = Color.grey;

    public int Index = -1;

    public delegate void Interaction(BorderComponent component, bool on);
    public event Interaction OnHover;
    public event Interaction OnPressed;

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

    private void OnMouseUpAsButton()
    {
        if (this.OnPressed != null)
        {
            this.OnPressed(this, true);
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

    public void FlashRed()
    {
        StartCoroutine(this.FlashRedRoutine(3));
    }

    private IEnumerator FlashRedRoutine(float duration)
    {
        float startTime = UnityEngine.Time.realtimeSinceStartup;
        float timer = UnityEngine.Time.realtimeSinceStartup - startTime;
        float blinkCount = 9;
        float blinkFactor = (blinkCount * Mathf.PI) / duration;
        while (timer < duration)
        {
            timer = UnityEngine.Time.realtimeSinceStartup - startTime;
            Color currentColor = Color.Lerp(this.baseBorderColor, Color.red, Mathf.Sin(timer * blinkFactor) * .5f + .5f);
            this.SetBorderColor(currentColor);
            yield return null;
        }

        this.ResetBorderColor();
        yield break;
    }
}
