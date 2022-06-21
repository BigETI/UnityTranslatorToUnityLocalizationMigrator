using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// An interface that represents a system language locale identifier
    /// </summary>
    public interface ISystemLanguageLocaleIdentifier
    {
        /// <summary>
        /// Language
        /// </summary>
        SystemLanguage Language { get; }

        /// <summary>
        /// Locale identifier
        /// </summary>
        LocaleIdentifier LocaleIdentifier { get; }

        /// <summary>
        /// Locale name
        /// </summary>
        string LocaleName { get; set; }

        /// <summary>
        /// Audio clip asset table name
        /// </summary>
        string AudioClipAssetTableName { get; set; }
        
        /// <summary>
        /// Material asset table name
        /// </summary>
        string MaterialAssetTableName { get; set; }
        
        /// <summary>
        /// Mesh asset table name
        /// </summary>
        string MeshAssetTableName { get; set; }
        
        /// <summary>
        /// Sprite asset table name
        /// </summary>
        string SpriteAssetTableName { get; set; }
        
        /// <summary>
        /// String table name
        /// </summary>
        string StringTableName { get; set; }
        
        /// <summary>
        /// Texture asset table name
        /// </summary>
        string TextureAssetTableName { get; set; }
    }
}
