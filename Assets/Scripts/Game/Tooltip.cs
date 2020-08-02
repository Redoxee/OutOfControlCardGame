using UnityEngine;
using Shapes;
using DG.Tweening;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public enum AnchorPosition
    {
        Right = 1 << 0,
        Top = 1 << 1,
        Left = 1 << 2,
        Bottom = 1 << 3,
        TopRight = 1 << 0 | 1 << 1,
        TopLeft = 1 << 1 | 1 << 2,
        BottomLeft = 1 << 3 | 1<< 2,
        BottomRight = 1 << 3 | 1 << 0,
    }

    public enum State
    {
        Hidden,
        TransitionToVisible,
        Visible,
        TransitionToHidden,
    }

    [SerializeField]
    private float transitionDuration = 1f;

    [SerializeField]
    private float labelMargin = .5f;

    [SerializeField]
    private float targetMargin = .5f;

    [SerializeField]
    private TextMeshPro label = null;

    [SerializeField]
    Rectangle back = null;

    private Color labelColorVisible = Color.white;
    private Color labelColorTransparent = Color.white;

    private Color backColorVisible = Color.black;
    private Color backColorHidden = Color.black;

    private Tween currentTween = null;
    private float showDate = -1;
    private float duration = 0;
    private Vector3 hidePosition;
    private State currentState;

    private void Start()
    {
        this.labelColorVisible = this.label.color;
        this.labelColorTransparent = this.labelColorVisible;
        this.labelColorTransparent.a = 0;

        this.backColorVisible = this.back.Color;
        this.backColorHidden = this.backColorVisible;
        this.backColorHidden.a = 0;

        this.label.color = this.labelColorTransparent;
        this.back.Color = this.backColorHidden;
        this.gameObject.SetActive(false);
        this.currentState = State.Hidden;
    }

    public void ShowTooltip(string message, Rectangle target, AnchorPosition anchor, float duration)
    {
        if (this.currentTween != null)
        {
            this.currentTween.Kill();
        }

        this.gameObject.SetActive(true);

        this.duration = duration;
        this.label.text = message;
        this.label.ComputeMarginSize();

        float width = this.label.GetPreferredValues().x + 2 * this.labelMargin;
        this.back.Width = width;
        float height = this.back.Height;

        float targetWidth = target.Width;
        float targetHeight = target.Height;

        DOTween.To((float t) =>
        {
            this.backColorHidden.a = t;
            this.back.Color = this.backColorHidden;
        },
        0.0f,
        1.0f,
        this.transitionDuration);

        this.label.DOColor(this.labelColorVisible, this.transitionDuration);

        Vector2 deltaDirection = this.AnchorToVector(anchor);
        Vector3 position = target.transform.position;
        position.x += deltaDirection.x * targetWidth * .5f + width * .5f * deltaDirection.x;
        position.y += deltaDirection.y * targetHeight * .5f + height * .5f * deltaDirection.y;


        this.hidePosition = position - new Vector3(deltaDirection.x, deltaDirection.y, 0) * this.targetMargin;
        this.transform.position = this.hidePosition;
        this.currentTween = this.transform.DOMove(position, this.transitionDuration).SetEase(Ease.OutCubic).OnKill(() =>
        {
            this.currentTween = null;
            this.currentState = State.Visible;
        });

        this.showDate = UnityEngine.Time.timeSinceLevelLoad;
        this.currentState = State.TransitionToVisible;
    }

    public void Hide()
    {
        if (this.currentState == State.Hidden || this.currentState == State.TransitionToHidden)
        {
            return;
        }

        if (this.currentTween != null)
        {
            this.currentTween.Kill();
        }

        this.showDate = -1;

        DOTween.To((float t) =>
        {
            this.backColorHidden.a = t;
            this.back.Color = this.backColorHidden;
        },
        1.0f,
        0.0f,
        this.transitionDuration);
        this.label.DOColor(this.labelColorTransparent, this.transitionDuration);
        this.currentTween = this.transform.DOMove(this.hidePosition, this.transitionDuration).OnKill(()=>
        {
            this.currentTween = null;
            this.gameObject.SetActive(false);
            this.currentState = State.Hidden;
        });

        this.currentState = State.TransitionToHidden;
    }

    private void Update()
    {
        if (this.showDate > 0)
        {
            if (UnityEngine.Time.timeSinceLevelLoad - this.showDate >= this.duration)
            {
                this.Hide();
            }
        }
    }

    private Vector2 AnchorToVector(AnchorPosition anchorPosition)
    {
        Vector2 result = new Vector2(0, 0);
        result.x += (anchorPosition & AnchorPosition.Right) > 0 ? 1 : 0;
        result.x += (anchorPosition & AnchorPosition.Left) > 0 ? -1 : 0;
        result.y += (anchorPosition & AnchorPosition.Top) > 0 ? 1 : 0;
        result.y += (anchorPosition & AnchorPosition.Bottom) > 0 ? -1 : 0;

        return result;
    }
}
