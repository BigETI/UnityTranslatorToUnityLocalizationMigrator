using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityTranslator;
using UnityTranslator.Objects;

/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// A class that describes a translation asset view
    /// </summary>
    /// <typeparam name="TTranslationObject">TRanslation object type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <typeparam name="TTranslationData">Translation data type</typeparam>
    /// <typeparam name="TTranslatedData">TRanslated data type</typeparam>
    internal class TranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData> : ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationObject : UnityEngine.Object, ITranslationObject<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationData : ITranslationData<TValue, TTranslatedData> where TTranslatedData : ITranslatedData<TValue>
    {
        /// <summary>
        /// Edited values
        /// </summary>
        private readonly Dictionary<SystemLanguage, TValue> editedValues = new();

        /// <summary>
        /// Excluded languages
        /// </summary>
        private readonly HashSet<SystemLanguage> excludedLanguages = new();

        /// <summary>
        /// Key
        /// </summary>
        private string key;

        /// <summary>
        /// Key
        /// </summary>
        public string Key
        {
            get => key;
            set => key = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Asset path
        /// </summary>
        public string AssetPath { get; }

        /// <summary>
        /// Translation
        /// </summary>
        public TTranslationObject Translation { get; }

        /// <summary>
        /// Is selected
        /// </summary>
        public bool IsSelected { get; set; } = true;

        /// <summary>
        /// Is folded out
        /// </summary>
        public bool IsFoldedOut { get; set; }

        /// <summary>
        /// Constructs a new traslation asset
        /// </summary>
        /// <param name="assetPath">Asset path</param>
        /// <param name="translation">Translation</param>
        /// <exception cref="ArgumentNullException">When "assetPath" or "translation" are "null"</exception>
        public TranslationAssetView(string assetPath, TTranslationObject translation)
        {
            if (!translation)
            {
                throw new ArgumentNullException(nameof(translation));
            }
            AssetPath = assetPath ?? throw new ArgumentNullException(nameof(assetPath));
            string suffix = translation switch
            {
                AudioClipTranslationObjectScript _ => "AudioClipTranslation",
                MaterialTranslationObjectScript _ => "MaterialTranslation",
                MeshTranslationObjectScript _ => "MeshTranslation",
                SpriteTranslationObjectScript _ => "SpriteTranslation",
                StringTranslationObjectScript _ => "StringTranslation",
                TextureTranslationObjectScript _ => "TextureTranslation",
                _ => throw new NotImplementedException($"Suffix for type \"{ typeof(TTranslationObject) }\" has not been implemented yet.")
            };
            string asset_file_name = Path.GetFileNameWithoutExtension(assetPath);
            key = asset_file_name.Replace(suffix, string.Empty, StringComparison.OrdinalIgnoreCase);
            Translation = translation;
        }

        /// <summary>
        /// Gets the value from the specified language
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Value</returns>
        public TValue GetValue(SystemLanguage language, TValue defaultValue) =>
            (editedValues.TryGetValue(language, out TValue ret) || Translation.TryGetValue(language, out ret)) ? ret : defaultValue;

        /// <summary>
        /// Gets values
        /// </summary>
        /// <param name="values">Values</param>
        /// <exception cref="ArgumentNullException">When "values" is "null"</exception>
        public void GetValues(List<ITranslatedValue<TValue>> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            values.Clear();
            foreach (TTranslatedData translated_value in Translation.Values)
            {
                bool is_value_edited = editedValues.TryGetValue(translated_value.Language, out TValue value);
                if (!is_value_edited)
                {
                    value = translated_value.Value;
                }
                values.Add(new TranslatedValue<TValue>(translated_value.Language, value, is_value_edited, !excludedLanguages.Contains(translated_value.Language)));
            }
        }

        /// <summary>
        /// Edits the specified value
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="value">Value</param>
        public void EditValue(SystemLanguage language, TValue value)
        {
            if (Translation.Translation.TryGetValue(language, out TValue current_value) && (value?.Equals(current_value) ?? (current_value == null)))
            {
                editedValues.Remove(language);
            }
            else if (!editedValues.TryAdd(language, value))
            {
                editedValues[language] = value;
            }
        }

        /// <summary>
        /// Resets the specified value
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>"true" if value has been successfully reset, otherwise "false"</returns>
        public bool ResetValue(SystemLanguage language) => editedValues.Remove(language);

        /// <summary>
        /// Sets the selected state of a language
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="isSelected">Is selected</param>
        public void SetLanguageSelectedState(SystemLanguage language, bool isSelected)
        {
            if (isSelected)
            {
                excludedLanguages.Remove(language);
            }
            else
            {
                excludedLanguages.Add(language);
            }
        }

        /// <summary>
        /// Is the specified language selected
        /// </summary>
        /// <param name="language">language</param>
        /// <returns>"true" if language is selected, otherwise "false"</returns>
        public bool IsLanguageSelected(SystemLanguage language) => !excludedLanguages.Contains(language);
    }
}
