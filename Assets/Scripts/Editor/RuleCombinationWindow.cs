using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

class RuleCombinationWindow : EditorWindow
{
    [SerializeField]
    private RuleDefinition[] definitionsA = new RuleDefinition[0];
    [SerializeField]
    private RuleDefinition[] definitionsB = new RuleDefinition[0];

    private RuleFolder ruleFolder = RuleFolder.Simple;

    private SerializedObject serializableObject;
    private SerializedProperty definitionAProperty;
    private SerializedProperty definitionBProperty;

    private int numberOfGeneratedRules = 0;

    private enum RuleFolder
    {
        Simple,
        Complex,
    }

    [MenuItem("Tools/Rule combination window")]
    private static void OpenWindow()
    {
        RuleCombinationWindow window = EditorWindow.GetWindow<RuleCombinationWindow>();
        window.titleContent.text = "Rule combination";
    }

    private void Awake()
    {
        this.serializableObject = new SerializedObject(this);
        this.definitionAProperty = this.serializableObject.FindProperty("definitionsA");
        this.definitionBProperty = this.serializableObject.FindProperty("definitionsB");
        this.numberOfGeneratedRules = 0;
    }

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Rules A");

                EditorGUILayout.PropertyField(this.definitionAProperty, true);

                GUILayout.EndVertical();
            }

            GUILayout.BeginVertical();
            {
                GUILayout.Label("Rules B");

                EditorGUILayout.PropertyField(this.definitionBProperty, true);

                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();
        }

        bool hasChanged = EditorGUI.EndChangeCheck();

        if(hasChanged)
        {
            this.serializableObject.ApplyModifiedProperties();
            this.numberOfGeneratedRules = 0;
            if (this.definitionsA.Length != 0 && this.definitionsB.Length != 0)
            {
                for (int i = 0; i < this.definitionsA.Length; ++i)
                {
                    for (int j = 0; j < this.definitionsB.Length; ++j)
                    {
                        if (this.definitionsA[i] != this.definitionsB[j])
                        {
                            this.numberOfGeneratedRules++;
                        }
                    }
                }
            }
        }

        GUILayout.Label($"{this.numberOfGeneratedRules} combinedRules");

        this.ruleFolder = (RuleFolder)EditorGUILayout.EnumPopup(this.ruleFolder);

        using (new EditorGUI.DisabledGroupScope(this.numberOfGeneratedRules == 0))
        {
            if (GUILayout.Button("Generate"))
            {
                for (int i = 0; i < this.definitionsA.Length; ++i)
                {
                    for (int j = 0; j < this.definitionsB.Length; ++j)
                    {
                        if (this.definitionsA[i] != this.definitionsB[j])
                        {
                            this.GenerateCombionedRules(this.definitionsA[i], this.definitionsB[j]);
                        }
                    }
                }

                AssetDatabase.SaveAssets();
            }
        }
    }

    private void GenerateCombionedRules(RuleDefinition definitionA, RuleDefinition definitionB)
    {
        RuleDefinitionCombination ruleDefintionCombination = ScriptableObject.CreateInstance<RuleDefinitionCombination>();
        ruleDefintionCombination.SubRules = new RuleDefinition[2];
        ruleDefintionCombination.SubRules[0] = definitionA;
        ruleDefintionCombination.SubRules[1] = definitionB;

        string ruleName = $"RuleCombination_{ definitionA.name }_{ definitionB.name }";
        ruleDefintionCombination.name = ruleName;

        string folder = this.ruleFolder.ToString();

        string rulePath = $"Assets/Data/Rules/Combinations/{folder}/{ruleName}.asset";
        UnityEditor.AssetDatabase.CreateAsset(ruleDefintionCombination, rulePath);
        UnityEditor.EditorUtility.SetDirty(ruleDefintionCombination);

    }
}
