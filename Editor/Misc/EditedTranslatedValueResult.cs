/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// A structure that describes an edited translated value result
    /// </summary>
    /// <typeparam name="TValue">Value type</typeparam>
    internal readonly struct EditedTranslatedValueResult<TValue> : IEditedTranslatedValueResult<TValue>
    {
        /// <summary>
        /// Is editing value
        /// </summary>
        public bool IsEditingValue { get; }

        /// <summary>
        /// Edited value
        /// </summary>
        public TValue EditedValue { get; }

        /// <summary>
        /// Is selected
        /// </summary>
        public bool IsSelected { get; }

        /// <summary>
        /// Constructs a new edited translated value result
        /// </summary>
        /// <param name="isEditingValue">Is editing value</param>
        /// <param name="editedValue">Edited value</param>
        /// <param name="isSelected">Is selected</param>
        public EditedTranslatedValueResult(bool isEditingValue, TValue editedValue, bool isSelected)
        {
            IsEditingValue = isEditingValue;
            EditedValue = editedValue;
            IsSelected = isSelected;
        }
    }
}
