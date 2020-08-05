using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesGenerator : MonoBehaviour
{
    [UnityEditor.MenuItem("Asset/CreateComplexeRules")]
    public static void GenerateComplexeGridRules()
    {
        for (int x = 0; x < 3; ++x)
        {
            for (int y = 0; y < 3; ++y)
            {
                RuleDefinitionGrid gridRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        gridRule.AllowedCells[i + j * 3] = (i == x || j == y);
                    }
                }

                string rulePath = $"Assets/Data/Rules/ComplexeGriRules/GridRules_Col_{x + 1}_row{y + 1}.asset";

                UnityEditor.AssetDatabase.CreateAsset(gridRule, rulePath);
                UnityEditor.EditorUtility.SetDirty(gridRule);
            }
        }

        UnityEditor.AssetDatabase.SaveAssets();
    }
}
