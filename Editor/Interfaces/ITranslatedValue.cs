using UnityEngine;

/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// An interface that represents a translated value
    /// </summary>
    /// <typeparam name="TValue">Value type</typeparam>
    public interface ITranslatedValue<TValue>
    {
        /// <summary>
        /// Language
        /// </summary>
        SystemLanguage Language { get; }

        /// <summary>
        /// Value
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// Is value edited
        /// </summary>
        bool IsValueEdited { get; }

        /// <summary>
        /// Is selected
        /// </summary>
        bool IsSelected { get; }
    }
}
