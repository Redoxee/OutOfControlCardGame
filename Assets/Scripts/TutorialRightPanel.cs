using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRightPanel : MonoBehaviour
{
    [SerializeField]
    private Animation fadeOutAnimation = null;

    [SerializeField]
    private BoxCollider2D collider = null;

    private bool shouldHide
    {
        get
        {
            return !this.gameObject.activeSelf || !this.fadeOutAnimation.isPlaying;
        }
    }

    public void FadeOutIfNeeded()
    {
        if (!this.shouldHide)
        {
            return;
        }

        this.fadeOutAnimation.Play();
        this.collider.enabled = false;
    }

    public void OnAnimationEnd()
    {
        this.gameObject.SetActive(false);
    }
}
