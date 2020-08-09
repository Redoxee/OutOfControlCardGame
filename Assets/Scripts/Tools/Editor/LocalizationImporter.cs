using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

class LocalizationImporter : EditorWindow
{
    private DefaultAsset SourceTsv = null;

    private const char fileSeparator = '\t';

    private ImportedLanguage[] importedLanguages;
    private string[] keys;
    private struct ImportedLanguage
    {
        public string languageName;
        public LocalizationCollection targetAsset;
        public string[] translations;
    }

    [MenuItem("Tools/TranslationImporter")]
    private static void OpenWindow()
    {
        LocalizationImporter window = UnityEditor.EditorWindow.GetWindow<LocalizationImporter>();
        window.titleContent.text = "Translation Importer";
    }

    private void OnGUI()
    {
        this.SourceTsv = (DefaultAsset)EditorGUILayout.ObjectField("Source to import", this.SourceTsv, typeof(DefaultAsset), false);

        using (new EditorGUI.DisabledScope(this.SourceTsv == null))
        {
            if (GUILayout.Button("Parse"))
            {
                this.ParseLocalizationAsset();
            }

            if (this.importedLanguages != null && this.importedLanguages.Length > 0)
            {
                GUILayout.Label($"Found { this.keys.Length } keys.");

                GUILayout.Label("Language");
                GUILayout.BeginHorizontal();
                {
                    for (int index = 0; index < this.importedLanguages.Length; ++index)
                    {
                        GUILayout.BeginVertical();
                        {
                            GUILayout.Label(this.importedLanguages[index].languageName);
                            this.importedLanguages[index].targetAsset = (LocalizationCollection)EditorGUILayout.ObjectField("Target collection", this.importedLanguages[index].targetAsset, typeof(LocalizationCollection), false);

                            using (new EditorGUI.DisabledScope(this.importedLanguages[index].targetAsset == null))
                            {
                                if (GUILayout.Button("Apply"))
                                {
                                    this.ApplyLocalization(ref this.importedLanguages[index]);
                                }
                            }

                            GUILayout.EndVertical();
                        }
                    }

                    GUILayout.EndHorizontal();
                }
            }
        }
    }

    private void ParseLocalizationAsset()
    {
        string projectPath = Application.dataPath;
        projectPath = projectPath.Remove(projectPath.Length - "Assets".Length);

        string sourcePath = projectPath + AssetDatabase.GetAssetPath(this.SourceTsv);
        
        Debug.Log($"parsing {sourcePath}");

        string[] allLines = System.IO.File.ReadAllLines(sourcePath);
        if (allLines.Length < 2)
        {
            Debug.LogError("Something went wrong while parsing, no content was found.");
            return;
        }

        string headerString = allLines[0];
        string[] header = headerString.Split(LocalizationImporter.fileSeparator);
        if (header.Length < 2)
        {
            Debug.LogError("Something went wrong while parsing the header.");
            return;
        }

        this.keys = new string[allLines.Length - 1];

        this.importedLanguages = new ImportedLanguage[header.Length - 1];
        for (int index = 1; index < header.Length; ++index)
        {
            this.importedLanguages[index - 1].languageName = header[index];
            ref ImportedLanguage language = ref this.importedLanguages[index - 1];
            language.targetAsset = null;
            language.translations = new string[this.keys.Length];
        }

        for (int index = 0; index < this.keys.Length; ++index)
        {
            string[] translations = allLines[index + 1].Split(LocalizationImporter.fileSeparator);
            string key = translations[0];
            this.keys[index] = key;

            for (int languageIndex = 0; languageIndex < this.importedLanguages.Length; ++languageIndex)
            {
                ref ImportedLanguage language = ref this.importedLanguages[languageIndex];
                language.translations[index] = translations[languageIndex + 1];
            }
        }
    }

    private void ApplyLocalization(ref ImportedLanguage importedLanguage)
    {
        importedLanguage.targetAsset.Language = importedLanguage.languageName;
        importedLanguage.targetAsset.name = importedLanguage.languageName;
        for (int index = 0; index < this.keys.Length; ++index)
        {
            string key = this.keys[index];
            string translation = importedLanguage.translations[index];
            importedLanguage.targetAsset.Translations[key] = translation;
        }

        UnityEditor.EditorUtility.SetDirty(importedLanguage.targetAsset);
        AssetDatabase.SaveAssets();
    }
}
