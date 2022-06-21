using System;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// A class tat describes a system language locale identifier
    /// </summary>
    internal class SystemLanguageLocaleIdentifier : ISystemLanguageLocaleIdentifier
    {
        /// <summary>
        /// Locale name
        /// </summary>
        private string localeName;

        /// <summary>
        /// Audio clip asset table name
        /// </summary>
        private string audioClipAssetTableName;

        /// <summary>
        /// Material asset table name
        /// </summary>
        private string materialAssetTableName;

        /// <summary>
        /// Mesh asset table name
        /// </summary>
        private string meshAssetTableName;

        /// <summary>
        /// Sprite asset table name
        /// </summary>
        private string spriteAssetTableName;

        /// <summary>
        /// String table name
        /// </summary>
        private string stringTableName;

        /// <summary>
        /// Texture asset table name
        /// </summary>
        private string textureAssetTableName;

        /// <summary>
        /// Language
        /// </summary>
        public SystemLanguage Language { get; }

        /// <summary>
        /// Locale identifier
        /// </summary>
        public LocaleIdentifier LocaleIdentifier { get; }

        /// <summary>
        /// Locale name
        /// </summary>
        public string LocaleName
        {
            get => localeName;
            set => localeName = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Audio clip asset table name
        /// </summary>
        public string AudioClipAssetTableName
        {
            get => audioClipAssetTableName;
            set => audioClipAssetTableName = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Material asset table name
        /// </summary>
        public string MaterialAssetTableName
        {
            get => materialAssetTableName;
            set => materialAssetTableName = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Mesh asset table name
        /// </summary>
        public string MeshAssetTableName
        {
            get => meshAssetTableName;
            set => meshAssetTableName = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Sprite asset table name
        /// </summary>
        public string SpriteAssetTableName
        {
            get => spriteAssetTableName;
            set => spriteAssetTableName = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// String table name
        /// </summary>
        public string StringTableName
        {
            get => stringTableName;
            set => stringTableName = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Texture asset table name
        /// </summary>
        public string TextureAssetTableName
        {
            get => textureAssetTableName;
            set => textureAssetTableName = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Contructs a new system languge locale identifier
        /// </summary>
        /// <param name="language">Language</param>
        public SystemLanguageLocaleIdentifier(SystemLanguage language)
        {
            Language = language;
            LocaleIdentifier = new LocaleIdentifier(language);
            localeName = $"{ language }Locale";
            audioClipAssetTableName = $"{ language }AudioClipAssetTable";
            materialAssetTableName = $"{ language }MaterialAssetTable";
            meshAssetTableName = $"{ language }MeshAssetTable";
            spriteAssetTableName = $"{ language }SpriteAssetTable";
            stringTableName = $"{ language }StringTable";
            textureAssetTableName = $"{ language }TextureAssetTable";
        }
    }
}
