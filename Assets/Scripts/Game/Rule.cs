using System.Collections;
using UnityEngine;
using TMPro;

public class Rule : BorderComponent
{
    public RuleDefinition Data = null;

    [SerializeField]
    private TextMeshPro label = null;

    public void SetRule(RuleDefinition rule)
    {
        this.Data = rule;
        this.label.text = this.Data.ToString();
    }
}
