using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeHolder : MonoBehaviour
{
    public Transform[] Lives = new Transform[0];

    public void SetLifeCount(int count)
    {
        for (int index = 0; index < this.Lives.Length; ++index)
        {
            this.Lives[index].gameObject.SetActive(index < count);
        }
    }
}
