/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// An interface that represents a computable value cache
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public interface IComputableValueCache<T>
    {
        /// <summary>
        /// Is value cached
        /// </summary>
        bool IsValueCached { get; }

        /// <summary>
        /// Cached value
        /// </summary>
        T CachedValue { get; set; }

        /// <summary>
        /// COmputed value
        /// </summary>
        T ComputedValue { get; }

        /// <summary>
        /// Value
        /// </summary>
        T Value { get; set; }

        /// <summary>
        /// Clears cache
        /// </summary>
        void ClearCache();
    }
}
