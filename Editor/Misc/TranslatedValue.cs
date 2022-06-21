using UnityEngine;

/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// A structure that describes a translated value
    /// </summary>
    /// <typeparam name="TValue">Value type</typeparam>
    internal readonly struct TranslatedValue<TValue> : ITranslatedValue<TValue>
    {
        /// <summary>
        /// Language
        /// </summary>
        public SystemLanguage Language { get; }

        /// <summary>
        /// Value
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Is value edited
        /// </summary>
        public bool IsValueEdited { get; }

        /// <summary>
        /// Is selected
        /// </summary>
        public bool IsSelected { get; }

        /// <summary>
        /// Constructs a new translated value
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="value">Value</param>
        /// <param name="isValueEdited">Is value edited</param>
        /// <param name="isSelected">Is value edited</param>
        public TranslatedValue(SystemLanguage language, TValue value, bool isValueEdited, bool isSelected)
        {
            Language = language;
            Value = value;
            IsValueEdited = isValueEdited;
            IsSelected = isSelected;
        }
    }
}
