using System.Collections;
using UnityEngine;
using TMPro;

public class Rule : BorderComponent
{
    [System.NonSerialized]
    public RuleDefinition Data = null;

    [UnityEngine.SerializeField]
    private Transform IllustrationRoot = null;

    private Transform[] IllustrationAnchors = new Transform[0];

    [SerializeField]
    private float IconSeparation = 1;

    public void SetRule(RuleDefinition rule)
    {
        this.Data = rule;

        GameObject[] illustrationPrefabs = rule.GetIllustrationPrefabs();
        for (int index = 0; index < this.IllustrationAnchors.Length; ++index)
        {
            UnityEngine.GameObject.Destroy(this.IllustrationAnchors[index].gameObject);
        }

        System.Array.Resize(ref this.IllustrationAnchors, illustrationPrefabs.Length);

        float halfWidth = this.IconSeparation * (illustrationPrefabs.Length - 1) / 2;

        for (int index = 0; index < illustrationPrefabs.Length; ++index)
        {
            UnityEngine.GameObject ruleIllustration = UnityEngine.GameObject.Instantiate(illustrationPrefabs[index], this.IllustrationRoot);

            Vector3 position = Vector3.zero;
            position.y = -halfWidth + (index * this.IconSeparation);
            ruleIllustration.transform.localPosition = position;
        }
    }
}
