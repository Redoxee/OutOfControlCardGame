
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AMG
{
    class AssetCreatorWindow : EditorWindow
    {
        List<System.Type> rulesDefintionTypes = new List<System.Type>();

        [UnityEditor.MenuItem("Asset/DataCreation")]
        public static void OpenRuleCreationWindow()
        {
            AssetCreatorWindow window = (AssetCreatorWindow)EditorWindow.GetWindow(typeof(AssetCreatorWindow));
            window.Show();
        }

        private void OnEnable()
        {
            this.titleContent.text = "Data";

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
            Type baseType = typeof(Data);

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
}