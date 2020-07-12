using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLeftPanel : MonoBehaviour
{

    const string inAnimation = "PlayPanelInAnimation";
    const string outAnimation = "PlayPanelOutAnimation";
    [SerializeField]
    private Animation fadeAnimation = null;
    [SerializeField]
    private BoxCollider2D collider = null;

    public Transform CardAnchor = null;

    public void FadeIn()
    {
        if (this.fadeAnimation.isPlaying)
        {
            this.fadeAnimation.Stop();
        }

        this.collider.enabled = true;
        this.fadeAnimation.Play(PlayLeftPanel.inAnimation);
    }

    public void FadeOut()
    {
        if (this.fadeAnimation.isPlaying)
        {
            this.fadeAnimation.Stop();
        }

        this.collider.enabled = false;
        this.fadeAnimation.Play(PlayLeftPanel.outAnimation);
    }
}
