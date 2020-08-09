using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationCollection : AMG.Data, UnityEngine.ISerializationCallbackReceiver
{
    public string Language;
    public Dictionary<string, string> Translations = new Dictionary<string, string>();

    [UnityEngine.HideInInspector]
    [UnityEngine.SerializeField]
    private List<string> serializedKey = new List<string>();

    [UnityEngine.HideInInspector]
    [UnityEngine.SerializeField]
    private List<string> serializedTranslation = new List<string>();

    public void OnAfterDeserialize()
    {
        this.Translations.Clear();
        int nbTranslation = this.serializedKey.Count;
        for (int index = 0; index < nbTranslation; ++index)
        {
            this.Translations[this.serializedKey[index]] = this.serializedTranslation[index];
        }
    }

    public void OnBeforeSerialize()
    {
        this.serializedKey.Clear();
        this.serializedTranslation.Clear();

        foreach (var kvp in this.Translations)
        {
            this.serializedKey.Add(kvp.Key);
            this.serializedTranslation.Add(kvp.Value);
        }
    }
}
