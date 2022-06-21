using System;
using UnityEditor;

/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// A class that provides functionalities for UNity's asset database
    /// </summary>
    public static class AssetDatabaseUtilities
    {
        /// <summary>
        /// Combines paths
        /// </summary>
        /// <param name="parentPath">Parent path</param>
        /// <param name="localPath">Local path</param>
        /// <returns>Combined paths</returns>
        /// <exception cref="ArgumentNullException">When parentPath"" or "localPath" are "null"</exception>
        public static string CombinePaths(string parentPath, string localPath)
        {
            if (string.IsNullOrWhiteSpace(parentPath))
            {
                throw new ArgumentNullException(nameof(parentPath));
            }
            if (string.IsNullOrWhiteSpace(localPath))
            {
                throw new ArgumentNullException(nameof(localPath));
            }
            return $"{ (parentPath.EndsWith('/') ? parentPath : $"{ parentPath }/") }{ localPath }";
        }

        /// <summary>
        /// Creates asset
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="path">Asset path</param>
        /// <exception cref="ArgumentNullException">When "obj" or "path" are null</exception>
        public static void CreateAsset(UnityEngine.Object obj, string path)
        {
            if (!obj)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(path);
            }
            string[] asset_path_parts = path.Split('/');
            if ((asset_path_parts.Length > 0) && (asset_path_parts[0] == "Assets"))
            {
                string asset_path = "Assets";
                for (int asset_path_part_index = 1; asset_path_part_index < asset_path_parts.Length; asset_path_part_index++)
                {
                    string asset_path_part = asset_path_parts[asset_path_part_index];
                    string parent_directory_path = asset_path;
                    asset_path = $"{ asset_path }/{ asset_path_part }";
                    if (asset_path_part_index < (asset_path_parts.Length - 1))
                    {
                        if (!AssetDatabase.IsValidFolder(asset_path))
                        {
                            AssetDatabase.CreateFolder(parent_directory_path, asset_path_part);
                        }
                    }
                    else
                    {
                        AssetDatabase.DeleteAsset(asset_path);
                        AssetDatabase.CreateAsset(obj, asset_path);
                    }
                }
            }
        }
    }
}
