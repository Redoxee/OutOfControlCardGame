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

    [SerializeField]
    protected Color baseBackColor = Color.white;

    public int Index = -1;

    public delegate void Interaction(BorderComponent component, bool on);
    public event Interaction OnHover;
    public event Interaction OnPressed;

    public bool DeactivateHover = false;

    private void OnMouseEnter()
    {
        if (this.DeactivateHover)
        {
            return;
        }

        this.Border.Color = this.hoverBorderColor;
        if (OnHover != null)
        {
            this.OnHover(this, true);
        }
    }

    private void OnMouseExit()
    {
        if (this.DeactivateHover)
        {
            return;
        }

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
        StartCoroutine(this.FlashRoutine(3, 9, Color.red));
    }

    public void FlashGreen()
    {
        StartCoroutine(this.FlashRoutine(3, 9, Color.green));
    }

    public void FlashWhite()
    {
        StartCoroutine(this.FlashRoutine(2, 2, Color.yellow));
    }

    public void FadeBorderToColor(Color color, Color backColor, float duration)
    {
        StartCoroutine(this.FadeBorderColor(color, backColor, duration));
    }

    private IEnumerator FadeBorderColor(Color color, Color backColor, float duration)
    {
        float startTime = UnityEngine.Time.realtimeSinceStartup;
        float timer = 0;

        while (timer < duration)
        {
            timer = UnityEngine.Time.realtimeSinceStartup - startTime;
            float progression = timer / duration;
            this.Border.Color = this.baseBorderColor + (color - this.baseBorderColor) * progression;
            this.Back.Color = this.baseBackColor + (backColor - this.baseBackColor) * progression;
            yield return null;
        }

        this.Border.Color = color;
        this.baseBorderColor = color;
        this.baseBackColor = backColor;
    }


    private IEnumerator FlashRoutine(float duration,int blinkCount, Color color)
    {
        float startTime = UnityEngine.Time.realtimeSinceStartup;
        float timer = 0;
        float blinkFactor = (blinkCount * Mathf.PI * 2);
        float offset = Mathf.PI * .5f;
        while (timer < duration)
        {
            timer = UnityEngine.Time.realtimeSinceStartup - startTime;
            float progression = timer / duration;
            Color currentColor = Color.Lerp(this.baseBorderColor, color, Mathf.Sin(progression * blinkFactor - offset) * .5f + .5f);
            this.SetBorderColor(currentColor);
            yield return null;
        }

        this.ResetBorderColor();
        yield break;
    }
}
