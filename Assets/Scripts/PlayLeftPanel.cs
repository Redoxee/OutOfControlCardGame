using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLeftPanel : MonoBehaviour
{

    const string inAnimation = "PlayPanelInAnimation";
    const string outAnimation = "PlayPanelOutAnimation";
    [SerializeField]
    private Animation fadeAnimation = null;

    public void FadeIn()
    {
        if (this.fadeAnimation.isPlaying)
        {
            this.fadeAnimation.Stop();
        }

        this.fadeAnimation.Play(PlayLeftPanel.inAnimation);
    }

    public void FadeOut()
    {
        if (this.fadeAnimation.isPlaying)
        {
            this.fadeAnimation.Stop();
        }

        this.fadeAnimation.Play(PlayLeftPanel.outAnimation);
    }
}
