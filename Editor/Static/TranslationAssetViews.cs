using System;
using System.Collections.Generic;
using UnityEditor;
using UnityTranslator;

/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// A class that rpovides functionalities for translation asset views
    /// </summary>
    public static class TranslationAssetViews
    {
        /// <summary>
        /// Search in directories
        /// </summary>
        private static readonly string[] searchInDirectoryPaths = new string[] { "Assets" };

        /// <summary>
        /// Is translation asset view key a duplicate
        /// </summary>
        /// <typeparam name="TTranslationObject">Translation object type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <typeparam name="TTranslationData">Translation data type</typeparam>
        /// <typeparam name="TTranslatedData">TRanslated data type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="translationAssetViews">Translation asset views</param>
        /// <param name="isCheckingForSelectedTranslationAssetViewsOnly">Is checking for selected translation asset views only</param>
        /// <returns>"true" if transaltion asset view key is a duplicate, otherwise "false"</returns>
        public static bool IsTranslationAssetViewKeyADuplicate<TTranslationObject, TValue, TTranslationData, TTranslatedData>(string key, IEnumerable<ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData>> translationAssetViews, bool isCheckingForSelectedTranslationAssetViewsOnly) where TTranslationObject : UnityEngine.Object, ITranslationObject<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationData : ITranslationData<TValue, TTranslatedData> where TTranslatedData : ITranslatedData<TValue>
        {
            bool ret = false;
            bool is_found = false;
            foreach (ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData> translation_asset_view in translationAssetViews)
            {
                if ((!isCheckingForSelectedTranslationAssetViewsOnly || translation_asset_view.IsSelected) && (translation_asset_view.Key == key))
                {
                    if (is_found)
                    {
                        ret = true;
                        break;
                    }
                    else
                    {
                        is_found = true;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Are translation asset view keys unique
        /// </summary>
        /// <typeparam name="TTranslationObject">Translation object type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <typeparam name="TTranslationData">Translation data type</typeparam>
        /// <typeparam name="TTranslatedData">Translated data type</typeparam>
        /// <param name="translationAssetViews">Translation asset views</param>
        /// <param name="isCheckingForSelectedTranslationAssetViewsOnly">Is checking for selected translation asset views only</param>
        /// <returns>"true" when translation asset keys are unique, otherwise "false"</returns>
        public static bool AreTranslationAssetViewKeysUnique<TTranslationObject, TValue, TTranslationData, TTranslatedData>(IEnumerable<ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData>> translationAssetViews, bool isCheckingForSelectedTranslationAssetViewsOnly) where TTranslationObject : UnityEngine.Object, ITranslationObject<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationData : ITranslationData<TValue, TTranslatedData> where TTranslatedData : ITranslatedData<TValue>
        {
            bool ret = true;
            foreach (ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData> translation_asset_view in translationAssetViews)
            {
                if (IsTranslationAssetViewKeyADuplicate(translation_asset_view.Key, translationAssetViews, isCheckingForSelectedTranslationAssetViewsOnly))
                {
                    ret = false;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Gets translation asset views
        /// </summary>
        /// <typeparam name="TTranslationObject">Translation object type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <typeparam name="TTranslationData">Translation data type</typeparam>
        /// <typeparam name="TTranslatedData">Translated data type</typeparam>
        /// <param name="translationAssetViews">Translation asset views</param>
        /// <exception cref="ArgumentNullException">When "translationAssetViews" is "null"</exception>
        public static void GetTranslationAssetViews<TTranslationObject, TValue, TTranslationData, TTranslatedData>(List<ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData>> translationAssetViews) where TTranslationObject : UnityEngine.Object, ITranslationObject<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationData : ITranslationData<TValue, TTranslatedData> where TTranslatedData : ITranslatedData<TValue>
        {
            if (translationAssetViews == null)
            {
                throw new ArgumentNullException(nameof(translationAssetViews));
            }
            translationAssetViews.Clear();
            foreach (string asset_guid in AssetDatabase.FindAssets($"t:{ typeof(TTranslationObject).Name }", searchInDirectoryPaths))
            {
                string asset_path = AssetDatabase.GUIDToAssetPath(asset_guid);
                TTranslationObject translation = AssetDatabase.LoadAssetAtPath<TTranslationObject>(asset_path);
                if (translation)
                {
                    ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData> translation_asset_view = new TranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData>(asset_path, translation);
                    uint count = 0U;
                    string key = translation_asset_view.Key;
                    while (IsTranslationAssetViewKeyADuplicate(translation_asset_view.Key, translationAssetViews, false))
                    {
                        translation_asset_view.Key = $"{ key }_{ count }";
                        ++count;
                    }
                    translationAssetViews.Add(translation_asset_view);
                }
            }
            translationAssetViews.Sort((left, right) => left.AssetPath.CompareTo(right.AssetPath));
        }
    }
}
