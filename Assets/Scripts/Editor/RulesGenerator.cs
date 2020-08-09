using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesGenerator : MonoBehaviour
{
    private static System.Text.StringBuilder workingStringBuilder = new System.Text.StringBuilder();

    [UnityEditor.MenuItem("Tools/CreateGridRules")]
    public static void GenerateComplexeGridRules()
    {
        RulesGenerator.workingStringBuilder.Clear();

        for (int i = 0; i < 3; ++i)
        {

            RuleDefinitionGrid rowRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
            RuleDefinitionGrid colRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
            for (int j = 0; j < 9; ++j)
            {
                rowRule.AllowedCells[j] = (j / 3 != i);
                colRule.AllowedCells[j] = (j % 3 != i);
            }

            string rowRuleName = $"GridRules_Row_{i + 1}";
            RulesGenerator.CreateAssetsFromGridRule("GridRules/Simple",rowRule, rowRuleName);

            string colRuleName = $"GridRules_Col_{i + 1}";
            RulesGenerator.CreateAssetsFromGridRule("GridRules/Simple", colRule, colRuleName);
        }

        RuleDefinitionGrid antiSlash = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
        antiSlash.AllowedCells = new bool[] 
        {
            false, true, true,
            true, false, true,
            true, true, false,
        };

        RuleDefinitionGrid slashRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
        slashRule.AllowedCells = new bool[]
        {
            true, true, false,
            true, false, true,
            false, true, true,
        };

        RulesGenerator.CreateAssetsFromGridRule("GridRules/Simple", antiSlash, "GridRules_AntiSlash");
        RulesGenerator.CreateAssetsFromGridRule("GridRules/Simple", slashRule, "GridRules_Slash");

        for (int x = 0; x < 3; ++x)
        {
            for (int y = 0; y < 3; ++y)
            {
                RuleDefinitionGrid gridRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        gridRule.AllowedCells[i + j * 3] = (i != x || j != y);
                    }
                }

                string ruleName = $"GridRules_Col_{x + 1}_row{y + 1}";
                RulesGenerator.CreateAssetsFromGridRule("GridRules/Complex", gridRule, ruleName);
            }
        }

        for (int i = 0; i < 3; ++i)
        {
            {
                RuleDefinitionGrid slashColRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
                slashColRule.AllowedCells = new bool[]
                {
                    true, true, false,
                    true, false, true,
                    false, true, true,
                };
                slashColRule.AllowedCells[i] = false;
                slashColRule.AllowedCells[i + 3] = false;
                slashColRule.AllowedCells[i + 6] = false;

                string ruleName = $"slash_Col_{i + 1}";
                RulesGenerator.CreateAssetsFromGridRule("GridRules/Complex", slashColRule, ruleName);
            }

            {
                RuleDefinitionGrid slashRowRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
                slashRowRule.AllowedCells = new bool[]
                {
                    true, true, false,
                    true, false, true,
                    false, true, true,
                };

                slashRowRule.AllowedCells[i * 3] = false;
                slashRowRule.AllowedCells[i * 3 + 1] = false;
                slashRowRule.AllowedCells[i * 3 + 2] = false;

                string ruleName = $"slash_Row_{i + 1}";
                RulesGenerator.CreateAssetsFromGridRule("GridRules/Complex", slashRowRule, ruleName);
            }

            {
                RuleDefinitionGrid antislashColRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
                antislashColRule.AllowedCells = new bool[]
                {
                    false, true, true,
                    true, false, true,
                    true, true, false,
                };

                antislashColRule.AllowedCells[i] = false;
                antislashColRule.AllowedCells[i + 3] = false;
                antislashColRule.AllowedCells[i + 6] = false;
                
                string ruleName = $"antislash_Col_{i + 1}";
                RulesGenerator.CreateAssetsFromGridRule("GridRules/Complex", antislashColRule, ruleName);
            }

            { 
                RuleDefinitionGrid antislashRowRule = ScriptableObject.CreateInstance<RuleDefinitionGrid>();
                antislashRowRule.AllowedCells = new bool[]
                {
                    false, true, true,
                    true, false, true,
                    true, true, false,
                };

                antislashRowRule.AllowedCells[i * 3] = false;
                antislashRowRule.AllowedCells[i * 3 + 1] = false;
                antislashRowRule.AllowedCells[i * 3 + 2] = false;

                string ruleName = $"antislash_Row_{i + 1}";
                RulesGenerator.CreateAssetsFromGridRule("GridRules/Complex", antislashRowRule, ruleName);
            }
        }

        UnityEditor.AssetDatabase.SaveAssets();

        TextEditor te = new TextEditor();
        te.text = RulesGenerator.workingStringBuilder.ToString();
        te.SelectAll();
        te.Copy();
        Debug.Log($"Copied {RulesGenerator.workingStringBuilder.ToString()}");
    }

    private static void CreateAssetsFromGridRule(string ruleFolder, RuleDefinitionGrid gridRule, string ruleName)
    {
        GameObject prefabRef = (GameObject)UnityEditor.AssetDatabase.LoadMainAssetAtPath("Assets/Prefabs/GridRuleRepresentation.prefab");
        GameObject instanceRoot = (GameObject)GameObject.Instantiate(prefabRef);

        for (int i = 8; i >= 0; --i)
        {
            if (gridRule.AllowedCells[i])
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
        gridRule.Description = $"%{ruleName}";

        string rulePath = $"Assets/Data/Rules/{ruleFolder}/{ruleName}.asset";
        UnityEditor.AssetDatabase.CreateAsset(gridRule, rulePath);
        UnityEditor.EditorUtility.SetDirty(gridRule);

        RulesGenerator.workingStringBuilder.Append($"%{ruleName}\n");
    }
}
