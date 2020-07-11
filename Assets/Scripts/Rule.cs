using System.Collections;
using UnityEngine;
using TMPro;

public class Rule : BorderComponent
{
    public RuleData Data = null;

    [SerializeField]
    private TextMeshPro label = null;

    public void SetRule(RuleData rule)
    {
        this.Data = rule;
        this.label.text = this.Data.ToString();
    }
}
