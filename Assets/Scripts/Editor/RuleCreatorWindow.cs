
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;


class RuleCreatorWindow : EditorWindow
{
    List<System.Type> rulesDefintionTypes = new List<System.Type>();

    [UnityEditor.MenuItem("Asset/RuleCreations")]
    public static void OpenRuleCreationWindow()
    {
        RuleCreatorWindow window = (RuleCreatorWindow)EditorWindow.GetWindow(typeof(RuleCreatorWindow));
        window.Show();
    }

    private void OnEnable()
    {
        this.titleContent.text = "Rules";

        this.buildTypeList();
    }

    private void OnGUI()
    {
        UnityEngine.GUILayout.BeginVertical();
        {
            foreach (Type type in this.rulesDefintionTypes)
            {
                if (UnityEngine.GUILayout.Button($"Create {type.Name}"))
                {
                    this.createScriptable(type);
                }
            }

            UnityEngine.GUILayout.EndVertical();
        }
    }

    private void buildTypeList()
    {
        Type baseType = typeof(RuleDefinition);

        foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in ass.GetTypes())
            {
                if (!type.IsSubclassOf(baseType))
                {
                    continue;
                }

                if (type.IsAbstract)
                {
                    continue;
                }

                this.rulesDefintionTypes.Add(type);
            }
        }
    }

    private void createScriptable(Type type)
    {
        UnityEngine.ScriptableObject asset = UnityEngine.ScriptableObject.CreateInstance(type);
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/" + type.ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
