using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSetup : MonoBehaviour
{

    [SerializeField]
    [Min(0)]
    private int NumberOfItems = 9;

    [SerializeField]
    [Min(1)]
    private int ColumnNumber = 3;

    [SerializeField]
    private Vector2 Spacing = Vector2.one;

    [InspectorButton("RefreshGrid","Refresh")]
    [SerializeField]
    private GameObject ItemPrefab = null;

    private GameObject[] Items = new GameObject[0];


    private void RefreshGrid()
    {
        if (this.ItemPrefab == null)
        {
            return;
        }

        if (this.Items.Length != this.NumberOfItems)
        {
            if (this.Items.Length > this.NumberOfItems)
            {
                for (int index = this.NumberOfItems; index < this.Items.Length; ++index)
                {
                    GameObject.DestroyImmediate(this.Items[index]);
                }

                System.Array.Resize(ref this.Items, this.NumberOfItems);
            }
            else
            {
                int oldLength = this.Items.Length;
                System.Array.Resize(ref this.Items, this.NumberOfItems);
                for (int index = oldLength; index < this.NumberOfItems; ++index)
                {
                    this.Items[index] = GameObject.Instantiate(this.ItemPrefab, this.transform);
                }
            }
        }

        for (int index = 0; index < this.Items.Length; ++index)
        {
            Vector3 position = new Vector3(this.Spacing.x * (index % this.ColumnNumber), this.Spacing.y * (index / this.ColumnNumber), 0);
            this.Items[index].transform.localPosition = position;
        }
    }
}
