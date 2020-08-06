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

                string ruleName = $"GridRules_Col_{x + 1}_row{y + 1}";
                RulesGenerator.CreateAssetsFromGridRule(gridRule, ruleName);
            }
        }

        for (int i = 0; i < 3; ++i)
        {
            {
                RuleDefinitionGrid slashColRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
                slashColRule.AllowedCells[2] = true;
                slashColRule.AllowedCells[4] = true;
                slashColRule.AllowedCells[6] = true;
                slashColRule.AllowedCells[i] = true;
                slashColRule.AllowedCells[i + 3] = true;
                slashColRule.AllowedCells[i + 6] = true;

                string ruleName = $"slash_Col_{i + 1}";
                RulesGenerator.CreateAssetsFromGridRule(slashColRule, ruleName);
            }

            {
                RuleDefinitionGrid slashRowRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
                slashRowRule.AllowedCells[2] = true;
                slashRowRule.AllowedCells[4] = true;
                slashRowRule.AllowedCells[6] = true;
                slashRowRule.AllowedCells[i * 3] = true;
                slashRowRule.AllowedCells[i * 3 + 1] = true;
                slashRowRule.AllowedCells[i * 3 + 2] = true;

                string ruleName = $"slash_Row_{i + 1}";
                RulesGenerator.CreateAssetsFromGridRule(slashRowRule, ruleName);
            }

            {
                RuleDefinitionGrid antislashColRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
                antislashColRule.AllowedCells[0] = true;
                antislashColRule.AllowedCells[4] = true;
                antislashColRule.AllowedCells[8] = true;
                antislashColRule.AllowedCells[i] = true;
                antislashColRule.AllowedCells[i + 3] = true;
                antislashColRule.AllowedCells[i + 6] = true;
                
                string ruleName = $"antislash_Col_{i + 1}";
                RulesGenerator.CreateAssetsFromGridRule(antislashColRule, ruleName);
            }

            { 
                RuleDefinitionGrid antislashRowRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
                antislashRowRule.AllowedCells[0] = true;
                antislashRowRule.AllowedCells[4] = true;
                antislashRowRule.AllowedCells[8] = true;
                antislashRowRule.AllowedCells[i * 3] = true;
                antislashRowRule.AllowedCells[i * 3 + 1] = true;
                antislashRowRule.AllowedCells[i * 3 + 2] = true;

                string ruleName = $"antislash_Row_{i + 1}";
                RulesGenerator.CreateAssetsFromGridRule(antislashRowRule, ruleName);
            }
        }

        UnityEditor.AssetDatabase.SaveAssets();
    }

    private static void CreateAssetsFromGridRule(RuleDefinitionGrid gridRule, string ruleName)
    {
        GameObject prefabRef = (GameObject)UnityEditor.AssetDatabase.LoadMainAssetAtPath("Assets/Prefabs/GridRuleRepresentation.prefab");
        GameObject instanceRoot = (GameObject)GameObject.Instantiate(prefabRef);

        for (int i = 8; i >= 0; --i)
        {
            if (!gridRule.AllowedCells[i])
            {
                Transform cell = instanceRoot.transform.GetChild(i).GetChild(0);
                GameObject.DestroyImmediate(cell.gameObject);
            }
        }

        string prefabPath = $"Assets/Prefabs/RulesIllustrations/GridRules/{ruleName}.prefab";
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(instanceRoot, prefabPath);
        GameObject.DestroyImmediate(instanceRoot);

        prefabRef = (GameObject)UnityEditor.AssetDatabase.LoadMainAssetAtPath(prefabPath);
        gridRule.IllustrationPrefab = prefabRef;

        string rulePath = $"Assets/Data/Rules/ComplexeGriRules/{ruleName}.asset";
        UnityEditor.AssetDatabase.CreateAsset(gridRule, rulePath);
        UnityEditor.EditorUtility.SetDirty(gridRule);
    }
}
