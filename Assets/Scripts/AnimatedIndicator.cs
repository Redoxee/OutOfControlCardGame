using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedIndicator : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        this.gameObject.SetActive(false);
    }
}
