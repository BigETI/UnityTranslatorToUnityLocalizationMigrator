/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// An interface that represents an edited translated value result
    /// </summary>
    /// <typeparam name="TValue">Value type</typeparam>
    public interface IEditedTranslatedValueResult<TValue>
    {
        /// <summary>
        /// Is editing value
        /// </summary>
        bool IsEditingValue { get; }

        /// <summary>
        /// Edited value
        /// </summary>
        TValue EditedValue { get; }

        /// <summary>
        /// Is selected
        /// </summary>
        bool IsSelected { get; }
    }
}
