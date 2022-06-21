using System;

/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// A class that describes a computable value cache
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    internal class ComputableValueCache<T> : IComputableValueCache<T>
    {
        /// <summary>
        /// Cached value
        /// </summary>
        private T cachedValue;

        /// <summary>
        /// Gets invoked when value needs to be computed
        /// </summary>
        private Func<T> onComputeValue;

        /// <summary>
        /// Is value cached
        /// </summary>
        public bool IsValueCached { get; private set; }

        /// <summary>
        /// Cached value
        /// </summary>
        public T CachedValue
        {
            get => cachedValue;
            set
            {
                cachedValue = value;
                IsValueCached = true;
            }
        }

        /// <summary>
        /// COmputed value
        /// </summary>
        public T ComputedValue => onComputeValue();

        /// <summary>
        /// Value
        /// </summary>
        public T Value
        {
            get
            {
                T ret;
                if (IsValueCached)
                {
                    ret = CachedValue;
                }
                else
                {
                    ret = ComputedValue;
                    CachedValue = ret;
                }
                return ret;
            }
            set => CachedValue = value;
        }

        /// <summary>
        /// Constructs a new computable value cache
        /// </summary>
        /// <param name="onComputeValue">Gets invoked when value needs to be computed</param>
        /// <exception cref="ArgumentNullException">When "onComputeValue" is "null"</exception>
        public ComputableValueCache(Func<T> onComputeValue) =>
            this.onComputeValue = onComputeValue ?? throw new ArgumentNullException(nameof(onComputeValue));

        /// <summary>
        /// Clears cache
        /// </summary>
        public void ClearCache()
        {
            IsValueCached = false;
            cachedValue = default;
        }
    }
}
