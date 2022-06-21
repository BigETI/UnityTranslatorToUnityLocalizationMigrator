using UnityEngine;
using UnityPatternsEditor;
using UnityTranslatorEditor;

/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// An interface that represents a migrate editor window
    /// </summary>
    public interface IMigrateEditorWindow : IEditorWindow
    {
        /// <summary>
        /// Module directory path
        /// </summary>
        string ModuleDirectoryPath { get; set; }

        /// <summary>
        /// Is module directory path existing
        /// </summary>
        bool IsModuleDirectoryPathExisting { get; }

        /// <summary>
        /// Computable locale assets directory path cache
        /// </summary>
        IComputableValueCache<string> ComputableLocaleAssetsDirectoryPathCache { get; }

        /// <summary>
        /// Computable shared audio clip table data asset path cache
        /// </summary>
        IComputableValueCache<string> ComputableSharedAudioClipTableDataAssetPathCache { get; }

        /// <summary>
        /// Computable shared material table data asset path cache
        /// </summary>
        IComputableValueCache<string> ComputableSharedMaterialTableDataAssetPathCache { get; }

        /// <summary>
        /// Computable shared mesh table data asset path cache
        /// </summary>
        IComputableValueCache<string> ComputableSharedMeshTableDataAssetPathCache { get; }

        /// <summary>
        /// Computable shared sprite table data asset path cache
        /// </summary>
        IComputableValueCache<string> ComputableSharedSpriteTableDataAssetPathCache { get; }

        /// <summary>
        /// Computable shared string table data asset path cache
        /// </summary>
        IComputableValueCache<string> ComputableSharedStringTableDataAssetPathCache { get; }

        /// <summary>
        /// Computable shared texture table data asset path cache
        /// </summary>
        IComputableValueCache<string> ComputableSharedTextureTableDataAssetPathCache { get; }

        /// <summary>
        /// Computable audio clip asset table assets directory path cache
        /// </summary>
        IComputableValueCache<string> ComputableAudioClipAssetTableAssetsDirectoryPathCache { get; }

        /// <summary>
        /// Computable material asset table assets directory path cache
        /// </summary>
        IComputableValueCache<string> ComputableMaterialAssetTableAssetsDirectoryPathCache { get; }

        /// <summary>
        /// Computable mesh asset table assets directory path cache
        /// </summary>
        IComputableValueCache<string> ComputableMeshAssetTableAssetsDirectoryPathCache { get; }

        /// <summary>
        /// Computable sprite asset table assets directory path cache
        /// </summary>
        IComputableValueCache<string> ComputableSpriteAssetTableAssetsDirectoryPathCache { get; }

        /// <summary>
        /// Computable string asset table assets directory path cache
        /// </summary>
        IComputableValueCache<string> ComputableStringTableAssetsDirectoryPathCache { get; }

        /// <summary>
        /// Computable texture asset table assets directory path cache
        /// </summary>
        IComputableValueCache<string> ComputableTextureAssetTableAssetsDirectoryPathCache { get; }

        /// <summary>
        /// Computable audio clip asset table collection asset path cache
        /// </summary>
        IComputableValueCache<string> ComputableAudioClipAssetTableCollectionAssetPathCache { get; }

        /// <summary>
        /// Computable material asset table collection asset path cache
        /// </summary>
        IComputableValueCache<string> ComputableMaterialAssetTableCollectionAssetPathCache { get; }

        /// <summary>
        /// Computable mesh asset table collection asset path cache
        /// </summary>
        IComputableValueCache<string> ComputableMeshAssetTableCollectionAssetPathCache { get; }

        /// <summary>
        /// Computable sprite asset table collection asset path cache
        /// </summary>
        IComputableValueCache<string> ComputableSpriteAssetTableCollectionAssetPathCache { get; }

        /// <summary>
        /// Computable string asset table collection asset path cache
        /// </summary>
        IComputableValueCache<string> ComputableStringTableCollectionAssetPathCache { get; }

        /// <summary>
        /// Computable texture asset table collection asset path cache
        /// </summary>
        IComputableValueCache<string> ComputableTextureAssetTableCollectionAssetPathCache { get; }

        /// <summary>
        /// Locale assets directory path
        /// </summary>
        string LocaleAssetsDirectoryPath { get; set; }

        /// <summary>
        /// Shared audio clip asset table data asset path
        /// </summary>
        string SharedAudioClipAssetTableDataAssetPath { get; set; }

        /// <summary>
        /// Shared material asset table data asset path
        /// </summary>
        string SharedMaterialAssetTableDataAssetPath { get; set; }

        /// <summary>
        /// Shared mesh asset table data asset path
        /// </summary>
        string SharedMeshAssetTableDataAssetPath { get; set; }

        /// <summary>
        /// Shared sprite asset table data asset path
        /// </summary>
        string SharedSpriteAssetTableDataAssetPath { get; set; }

        /// <summary>
        /// Shared string asset table data asset path
        /// </summary>
        string SharedStringTableDataAssetPath { get; set; }

        /// <summary>
        /// Shared texture asset table data asset path
        /// </summary>
        string SharedTextureAssetTableDataAssetPath { get; set; }

        /// <summary>
        /// Audio clip asset table assets directory path
        /// </summary>
        string AudioClipAssetTableAssetsDirectoryPath { get; set; }

        /// <summary>
        /// Material asset table assets directory path
        /// </summary>
        string MaterialAssetTableAssetsDirectoryPath { get; set; }

        /// <summary>
        /// Mesh asset table assets directory path
        /// </summary>
        string MeshAssetTableAssetsDirectoryPath { get; set; }

        /// <summary>
        /// Sprite asset table assets directory path
        /// </summary>
        string SpriteAssetTableAssetsDirectoryPath { get; set; }

        /// <summary>
        /// String asset table assets directory path
        /// </summary>
        string StringTableAssetsDirectoryPath { get; set; }

        /// <summary>
        /// Texture asset table assets directory path
        /// </summary>
        string TextureAssetTableAssetsDirectoryPath { get; set; }

        /// <summary>
        /// Audio clip asset table collection asset path
        /// </summary>
        string AudioClipAssetTableCollectionAssetPath { get; set; }

        /// <summary>
        /// Material asset table collection asset path
        /// </summary>
        string MaterialAssetTableCollectionAssetPath { get; set; }

        /// <summary>
        /// Mesh asset table collection asset path
        /// </summary>
        string MeshAssetTableCollectionAssetPath { get; set; }

        /// <summary>
        /// Sprite asset table collection asset path
        /// </summary>
        string SpriteAssetTableCollectionAssetPath { get; set; }

        /// <summary>
        /// String asset table collection asset path
        /// </summary>
        string StringTableCollectionAssetPath { get; set; }

        /// <summary>
        /// Texture asset table collection asset path
        /// </summary>
        string TextureAssetTableCollectionAssetPath { get; set; }

        /// <summary>
        /// Tokenized search field
        /// </summary>
        TokenizedSearchField TokenizedSearchField { get; }

        /// <summary>
        /// Selected translation type tab
        /// </summary>
        ETranslationType SelectedTranslationTypeTab { get; set; }

        /// <summary>
        /// Scroll view value
        /// </summary>
        Vector2 ScrollViewValue { get; }

        /// <summary>
        /// Are shared table data asset paths folded out
        /// </summary>
        bool AreSharedTableDataAssetPathsFoldedOut { get; set; }

        /// <summary>
        /// Are localization table assets directory paths folded out
        /// </summary>
        bool AreLocalizationTableAssetsDirectoryPathsFoldedOut { get; set; }

        /// <summary>
        /// Are localization table collection asset paths folded out
        /// </summary>
        bool AreLocalizationTableCollectionAssetPathsFoldedOut { get; set; }
    }
}
