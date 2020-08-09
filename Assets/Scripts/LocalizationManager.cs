using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField]
    private LocalizationCollection[] languages = null;
    private LocalizationCollection currentLanguage = null;

    private Dictionary<string, int> languageIndex = new Dictionary<string, int>();

    private string[] availableLanguages;
    public string[] AvailableLanguages
    {
        get
        {
            return this.availableLanguages;
        }
    }

    private void Start()
    {
        this.languageIndex.Clear();
        this.availableLanguages = new string[this.languages.Length];
        for (int index = 0; index < this.availableLanguages.Length; ++index)
        {
            this.availableLanguages[index] = this.languages[index].Language;
            this.languageIndex[this.availableLanguages[index]] = index;
        }

        this.currentLanguage = this.languages[0];
    }

    public bool SelectLanguage(string languageName)
    {
        Debug.Assert(this.languageIndex.ContainsKey(languageName));
        if (this.currentLanguage.Language == languageName)
        {
            return false;
        }

        this.currentLanguage = this.languages[this.languageIndex[languageName]];
        return true;
    }

    public bool HasKey(string key)
    {
        return this.currentLanguage.Translations.ContainsKey(key);
    }

    public string GetString(string key)
    {
        if (!this.currentLanguage.Translations.ContainsKey(key))
        {
            Debug.LogError($"{key} localization not found.");
            return key;
        }

        return this.currentLanguage.Translations[key];
    }
}
