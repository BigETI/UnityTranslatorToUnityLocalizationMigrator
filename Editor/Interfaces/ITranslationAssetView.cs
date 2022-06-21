using System;
using System.Collections.Generic;
using UnityEngine;
using UnityTranslator;

/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// An interface that represents a translation assrt view
    /// </summary>
    /// <typeparam name="TTranslationObject">Translation object type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <typeparam name="TTranslationData">Translation data type</typeparam>
    /// <typeparam name="TTranslatedData">Translated data type</typeparam>
    public interface ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationObject : UnityEngine.Object, ITranslationObject<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationData : ITranslationData<TValue, TTranslatedData> where TTranslatedData : ITranslatedData<TValue>
    {
        /// <summary>
        /// Key
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// Asset path
        /// </summary>
        string AssetPath { get; }

        /// <summary>
        /// Is selected
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Is folded out
        /// </summary>
        bool IsFoldedOut { get; set; }

        /// <summary>
        /// Translation
        /// </summary>
        TTranslationObject Translation { get; }

        /// <summary>
        /// Gets the value from the specified language
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Value</returns>
        TValue GetValue(SystemLanguage language, TValue defaultValue);

        /// <summary>
        /// Gets values
        /// </summary>
        /// <param name="values">Values</param>
        /// <exception cref="ArgumentNullException">When "values" is "null"</exception>
        void GetValues(List<ITranslatedValue<TValue>> values);

        /// <summary>
        /// Edits the specified value
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="value">Value</param>
        public void EditValue(SystemLanguage language, TValue value);

        /// <summary>
        /// Resets the specified value
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>"true" if value has been successfully reset, otherwise "false"</returns>
        bool ResetValue(SystemLanguage language);

        /// <summary>
        /// Sets the selected state of a language
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="isSelected">Is selected</param>
        void SetLanguageSelectedState(SystemLanguage language, bool isSelected);

        /// <summary>
        /// Is the specified language selected
        /// </summary>
        /// <param name="language">language</param>
        /// <returns>"true" if language is selected, otherwise "false"</returns>
        bool IsLanguageSelected(SystemLanguage language);
    }
}
