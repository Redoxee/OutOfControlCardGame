using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleSlot : BorderComponent
{
    public Rule Rule = null;

    public GameObject CheckMark = null;
    public Animation CheckMarkAnimation = null;
    public GameObject CrossMark = null;
    public Animation CrossMarkAnimation = null;

    private void OnEnable()
    {
    }

    public void PlayCheckMark()
    {
        this.CheckMark.SetActive(true);
        this.CheckMarkAnimation.Play();
    }

    public void PlayCrossMark()
    {
        this.CrossMark.SetActive(true);
        this.CrossMarkAnimation.Play();
    }
}
