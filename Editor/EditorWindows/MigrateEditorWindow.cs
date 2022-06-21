using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityPatternsEditor.EditorWindows;
using UnityTranslator;
using UnityTranslator.Data;
using UnityTranslator.Objects;
using UnityTranslatorEditor;

/// <summary>
/// Unity translator to Unity localization migrator editor editor windows namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor.EditorWindows
{
    /// <summary>
    /// A class that describes a migrate editor window
    /// </summary>
    public class MigrateEditorWindow : AEditorWindow, IMigrateEditorWindow
    {
        /// <summary>
        /// COmputable module directory path cache
        /// </summary>
        private readonly IComputableValueCache<string> computableModuleDirectoryPathCache =
            new ComputableValueCache<string>
            (
                () =>
                {
                    string[] asset_path_parts = AssetDatabase.GetAssetPath(Selection.activeInstanceID).Split('/');
                    return ((asset_path_parts.Length > 1) && (asset_path_parts[0] == "Assets")) ? $"Assets/{ asset_path_parts[1] }" : "Assets";
                }
            );

        /// <summary>
        /// System language locale identifiers
        /// </summary>
        private readonly List<ISystemLanguageLocaleIdentifier> systemLanguageLocaleIdentifiers = new();

        /// <summary>
        /// Locales
        /// </summary>
        private readonly List<Locale> locales = new();

        /// <summary>
        /// Audio clip asset tables
        /// </summary>
        private readonly List<AssetTable> audioClipAssetTables = new();

        /// <summary>
        /// Material asset tables
        /// </summary>
        private readonly List<AssetTable> materialAssetTables = new();

        /// <summary>
        /// Mesh asset tables
        /// </summary>
        private readonly List<AssetTable> meshAssetTables = new();

        /// <summary>
        /// Sprite asset tables
        /// </summary>
        private readonly List<AssetTable> spriteAssetTables = new();

        /// <summary>
        /// String tables
        /// </summary>
        private readonly List<StringTable> stringTables = new();

        /// <summary>
        /// Texture asset tables
        /// </summary>
        private readonly List<AssetTable> textureAssetTables = new();

        /// <summary>
        /// Computable locale assets directory path cache
        /// </summary>
        private IComputableValueCache<string> computableLocaleAssetsDirectoryPathCache;

        /// <summary>
        /// Computable shared audio clip table data asset path cache
        /// </summary>
        private IComputableValueCache<string> computableSharedAudioClipTableDataAssetPathCache;

        /// <summary>
        /// Computable shared material table data asset path cache
        /// </summary>
        private IComputableValueCache<string> computableSharedMaterialTableDataAssetPathCache;

        /// <summary>
        /// Computable shared mesh table data asset path cache
        /// </summary>
        private IComputableValueCache<string> computableSharedMeshTableDataAssetPathCache;

        /// <summary>
        /// Computable shared sprite table data asset path cache
        /// </summary>
        private IComputableValueCache<string> computableSharedSpriteTableDataAssetPathCache;

        /// <summary>
        /// Computable shared string table data asset path cache
        /// </summary>
        private IComputableValueCache<string> computableSharedStringTableDataAssetPathCache;

        /// <summary>
        /// Computable shared texture table data asset path cache
        /// </summary>
        private IComputableValueCache<string> computableSharedTextureTableDataAssetPathCache;

        /// <summary>
        /// Computable audio clip assets directory path cache
        /// </summary>
        private IComputableValueCache<string> computableAudioClipAssetTableAssetsDirectoryPathCache;

        /// <summary>
        /// Computable material assets directory path cache
        /// </summary>
        private IComputableValueCache<string> computableMaterialAssetTableAssetsDirectoryPathCache;

        /// <summary>
        /// Computable mesh assets directory path cache
        /// </summary>
        private IComputableValueCache<string> computableMeshAssetTableAssetsDirectoryPathCache;

        /// <summary>
        /// Computable sprite assets directory path cache
        /// </summary>
        private IComputableValueCache<string> computableSpriteAssetTableAssetsDirectoryPathCache;

        /// <summary>
        /// Computable string assets directory path cache
        /// </summary>
        private IComputableValueCache<string> computableStringTableAssetsDirectoryPathCache;

        /// <summary>
        /// Computable texture assets directory path cache
        /// </summary>
        private IComputableValueCache<string> computableTextureAssetTableAssetsDirectoryPathCache;

        /// <summary>
        /// Computable audio clip asset table collection asset path cache
        /// </summary>
        private IComputableValueCache<string> computableAudioClipAssetTableCollectionAssetPathCache;

        /// <summary>
        /// Computable material asset table collection asset path cache
        /// </summary>
        private IComputableValueCache<string> computableMaterialAssetTableCollectionAssetPathCache;

        /// <summary>
        /// Computable mesh asset table collection asset path cache
        /// </summary>
        private IComputableValueCache<string> computableMeshAssetTableCollectionAssetPathCache;

        /// <summary>
        /// Computable sprite asset table collection asset path cache
        /// </summary>
        private IComputableValueCache<string> computableSpriteAssetTableCollectionAssetPathCache;

        /// <summary>
        /// Computable string asset table collection asset path cache
        /// </summary>
        private IComputableValueCache<string> computableStringTableCollectionAssetPathCache;

        /// <summary>
        /// Computable texture asset table collection asset path cache
        /// </summary>
        private IComputableValueCache<string> computableTextureAssetTableCollectionAssetPathCache;

        /// <summary>
        /// Audio clip translation asset views
        /// </summary>
        private List<ITranslationAssetView<AudioClipTranslationObjectScript, AudioClip, AudioClipTranslationData, TranslatedAudioClipData>> audioClipTranslationAssetViews;

        /// <summary>
        /// Material translation asset views
        /// </summary>
        private List<ITranslationAssetView<MaterialTranslationObjectScript, Material, MaterialTranslationData, TranslatedMaterialData>> materialTranslationAssetViews;

        /// <summary>
        /// Mesh translation asset views
        /// </summary>
        private List<ITranslationAssetView<MeshTranslationObjectScript, Mesh, MeshTranslationData, TranslatedMeshData>> meshTranslationAssetViews;

        /// <summary>
        /// Sprite translation asset views
        /// </summary>
        private List<ITranslationAssetView<SpriteTranslationObjectScript, Sprite, SpriteTranslationData, TranslatedSpriteData>> spriteTranslationAssetViews;

        /// <summary>
        /// String translation asset views
        /// </summary>
        private List<ITranslationAssetView<StringTranslationObjectScript, string, StringTranslationData, TranslatedStringData>> stringTranslationAssetViews;

        /// <summary>
        /// Texture translation asset views
        /// </summary>
        private List<ITranslationAssetView<TextureTranslationObjectScript, Texture, TextureTranslationData, TranslatedTextureData>> textureTranslationAssetViews;

        /// <summary>
        /// Tabs
        /// </summary>
        private IReadOnlyList<ITab> tabs;

        /// <summary>
        /// Audio clip asset table collection
        /// </summary>
        private AssetTableCollection audioClipAssetTableCollection;

        /// <summary>
        /// Material asset table collection
        /// </summary>
        private AssetTableCollection materialAssetTableCollection;

        /// <summary>
        /// Mesh asset table collection
        /// </summary>
        private AssetTableCollection meshAssetTableCollection;

        /// <summary>
        /// Sprite asset table collection
        /// </summary>
        private AssetTableCollection spriteAssetTableCollection;

        /// <summary>
        /// String asset table collection
        /// </summary>
        private StringTableCollection stringTableCollection;

        /// <summary>
        /// Texture asset table collection
        /// </summary>
        private AssetTableCollection textureAssetTableCollection;

        /// <summary>
        /// Module directory path
        /// </summary>
        public string ModuleDirectoryPath
        {
            get => computableModuleDirectoryPathCache.Value.TrimEnd('/');
            set
            {
                if (computableModuleDirectoryPathCache.Value != value)
                {
                    computableModuleDirectoryPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
                    ComputableLocaleAssetsDirectoryPathCache.ClearCache();
                    ComputableAudioClipAssetTableAssetsDirectoryPathCache.ClearCache();
                    ComputableMaterialAssetTableAssetsDirectoryPathCache.ClearCache();
                    ComputableMeshAssetTableAssetsDirectoryPathCache.ClearCache();
                    ComputableSpriteAssetTableAssetsDirectoryPathCache.ClearCache();
                    ComputableStringTableAssetsDirectoryPathCache.ClearCache();
                    ComputableTextureAssetTableAssetsDirectoryPathCache.ClearCache();
                    ComputableAudioClipAssetTableCollectionAssetPathCache.ClearCache();
                    ComputableMaterialAssetTableCollectionAssetPathCache.ClearCache();
                    ComputableMeshAssetTableCollectionAssetPathCache.ClearCache();
                    ComputableSpriteAssetTableCollectionAssetPathCache.ClearCache();
                    ComputableStringTableCollectionAssetPathCache.ClearCache();
                    ComputableTextureAssetTableCollectionAssetPathCache.ClearCache();
                }
            }
        }

        /// <summary>
        /// Is module directory path existing
        /// </summary>
        public bool IsModuleDirectoryPathExisting => AssetDatabase.IsValidFolder(ModuleDirectoryPath);

        /// <summary>
        /// Computable locale assets directory path cache
        /// </summary>
        public IComputableValueCache<string> ComputableLocaleAssetsDirectoryPathCache =>
            computableLocaleAssetsDirectoryPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/Locales");

        /// <summary>
        /// Computable shared audio clip table data asset path cache
        /// </summary>
        public IComputableValueCache<string> ComputableSharedAudioClipTableDataAssetPathCache =>
            computableSharedAudioClipTableDataAssetPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/SharedTableData/AudioClips/AudioClipSharedTableData.asset");

        /// <summary>
        /// Computable shared material table data asset path cache
        /// </summary>
        public IComputableValueCache<string> ComputableSharedMaterialTableDataAssetPathCache =>
            computableSharedMaterialTableDataAssetPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/SharedTableData/Materials/MaterialSharedTableData.asset");

        /// <summary>
        /// Computable shared mesh table data asset path cache
        /// </summary>
        public IComputableValueCache<string> ComputableSharedMeshTableDataAssetPathCache =>
            computableSharedMeshTableDataAssetPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/SharedTableData/Meshes/MeshSharedTableData.asset");

        /// <summary>
        /// Computable shared sprite table data asset path cache
        /// </summary>
        public IComputableValueCache<string> ComputableSharedSpriteTableDataAssetPathCache =>
            computableSharedSpriteTableDataAssetPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/SharedTableData/Sprites/SpriteSharedTableData.asset");

        /// <summary>
        /// Computable shared string table data asset path cache
        /// </summary>
        public IComputableValueCache<string> ComputableSharedStringTableDataAssetPathCache =>
            computableSharedStringTableDataAssetPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/SharedTableData/Strings/StringSharedTableData.asset");

        /// <summary>
        /// Computable shared texture table data asset path cache
        /// </summary>
        public IComputableValueCache<string> ComputableSharedTextureTableDataAssetPathCache =>
            computableSharedTextureTableDataAssetPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/SharedTableData/Textures/TextureSharedTableData.asset");

        /// <summary>
        /// Computable audio clip asset table assets directory path cache
        /// </summary>
        public IComputableValueCache<string> ComputableAudioClipAssetTableAssetsDirectoryPathCache =>
            computableAudioClipAssetTableAssetsDirectoryPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/AssetTables/AudioClips");

        /// <summary>
        /// Computable material asset table assets directory path cache
        /// </summary>
        public IComputableValueCache<string> ComputableMaterialAssetTableAssetsDirectoryPathCache =>
            computableMaterialAssetTableAssetsDirectoryPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/AssetTables/Materials");

        /// <summary>
        /// Computable mesh asset table assets directory path cache
        /// </summary>
        public IComputableValueCache<string> ComputableMeshAssetTableAssetsDirectoryPathCache =>
            computableMeshAssetTableAssetsDirectoryPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/AssetTables/Meshes");

        /// <summary>
        /// Computable sprite asset table assets directory path cache
        /// </summary>
        public IComputableValueCache<string> ComputableSpriteAssetTableAssetsDirectoryPathCache =>
            computableSpriteAssetTableAssetsDirectoryPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/AssetTables/Sprites");

        /// <summary>
        /// Computable string asset table assets directory path cache
        /// </summary>
        public IComputableValueCache<string> ComputableStringTableAssetsDirectoryPathCache =>
            computableStringTableAssetsDirectoryPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/StringTables");

        /// <summary>
        /// Computable texture asset table assets directory path cache
        /// </summary>
        public IComputableValueCache<string> ComputableTextureAssetTableAssetsDirectoryPathCache =>
            computableTextureAssetTableAssetsDirectoryPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/AssetTables/Textures");

        /// <summary>
        /// Computable audio clip asset table collection asset path cache
        /// </summary>
        public IComputableValueCache<string> ComputableAudioClipAssetTableCollectionAssetPathCache =>
            computableAudioClipAssetTableCollectionAssetPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/AssetTableCollections/AudioClips/AudioClipAssetTableCollection.asset");

        /// <summary>
        /// Computable material asset table collection asset path cache
        /// </summary>
        public IComputableValueCache<string> ComputableMaterialAssetTableCollectionAssetPathCache =>
            computableMaterialAssetTableCollectionAssetPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/AssetTableCollections/Materials/MaterialAssetTableCollection.asset");

        /// <summary>
        /// Computable mesh asset table collection asset path cache
        /// </summary>
        public IComputableValueCache<string> ComputableMeshAssetTableCollectionAssetPathCache =>
            computableMeshAssetTableCollectionAssetPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/AssetTableCollections/Meshes/MeshAssetTableCollection.asset");

        /// <summary>
        /// Computable sprite asset table collection asset path cache
        /// </summary>
        public IComputableValueCache<string> ComputableSpriteAssetTableCollectionAssetPathCache =>
            computableSpriteAssetTableCollectionAssetPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/AssetTableCollections/Sprites/SpriteAssetTableCollection.asset");

        /// <summary>
        /// Computable string asset table collection asset path cache
        /// </summary>
        public IComputableValueCache<string> ComputableStringTableCollectionAssetPathCache =>
            computableStringTableCollectionAssetPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/StringTableCollections/StringTableCollection.asset");

        /// <summary>
        /// Computable texture asset table collection asset path cache
        /// </summary>
        public IComputableValueCache<string> ComputableTextureAssetTableCollectionAssetPathCache =>
            computableTextureAssetTableCollectionAssetPathCache ??= new ComputableValueCache<string>(() => $"{ ModuleDirectoryPath }/AssetTableCollections/Textures/TextureAssetTableCollection.asset");

        /// <summary>
        /// Locale assets directory path
        /// </summary>
        public string LocaleAssetsDirectoryPath
        {
            get => ComputableLocaleAssetsDirectoryPathCache.Value;
            set => ComputableLocaleAssetsDirectoryPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Shared audio clip asset table data asset path
        /// </summary>
        public string SharedAudioClipAssetTableDataAssetPath
        {
            get => ComputableSharedAudioClipTableDataAssetPathCache.Value;
            set => ComputableSharedAudioClipTableDataAssetPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Shared material asset table data asset path
        /// </summary>
        public string SharedMaterialAssetTableDataAssetPath
        {
            get => ComputableSharedMaterialTableDataAssetPathCache.Value;
            set => ComputableSharedMaterialTableDataAssetPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Shared mesh asset table data asset path
        /// </summary>
        public string SharedMeshAssetTableDataAssetPath
        {
            get => ComputableSharedMeshTableDataAssetPathCache.Value;
            set => ComputableSharedMeshTableDataAssetPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Shared sprite asset table data asset path
        /// </summary>
        public string SharedSpriteAssetTableDataAssetPath
        {
            get => ComputableSharedSpriteTableDataAssetPathCache.Value;
            set => ComputableSharedSpriteTableDataAssetPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Shared string asset table data asset path
        /// </summary>
        public string SharedStringTableDataAssetPath
        {
            get => ComputableSharedStringTableDataAssetPathCache.Value;
            set => ComputableSharedStringTableDataAssetPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Shared texture asset table data asset path
        /// </summary>
        public string SharedTextureAssetTableDataAssetPath
        {
            get => ComputableSharedTextureTableDataAssetPathCache.Value;
            set => ComputableSharedTextureTableDataAssetPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Audio clip asset table assets directory path
        /// </summary>
        public string AudioClipAssetTableAssetsDirectoryPath
        {
            get => ComputableAudioClipAssetTableAssetsDirectoryPathCache.Value;
            set => ComputableAudioClipAssetTableAssetsDirectoryPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Material asset table assets directory path
        /// </summary>
        public string MaterialAssetTableAssetsDirectoryPath
        {
            get => ComputableMaterialAssetTableAssetsDirectoryPathCache.Value;
            set => ComputableMaterialAssetTableAssetsDirectoryPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Mesh asset table assets directory path
        /// </summary>
        public string MeshAssetTableAssetsDirectoryPath
        {
            get => ComputableMeshAssetTableAssetsDirectoryPathCache.Value;
            set => ComputableMeshAssetTableAssetsDirectoryPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Sprite asset table assets directory path
        /// </summary>
        public string SpriteAssetTableAssetsDirectoryPath
        {
            get => ComputableSpriteAssetTableAssetsDirectoryPathCache.Value;
            set => ComputableSpriteAssetTableAssetsDirectoryPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// String asset table assets directory path
        /// </summary>
        public string StringTableAssetsDirectoryPath
        {
            get => ComputableStringTableAssetsDirectoryPathCache.Value;
            set => ComputableStringTableAssetsDirectoryPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Texture asset table assets directory path
        /// </summary>
        public string TextureAssetTableAssetsDirectoryPath
        {
            get => ComputableTextureAssetTableAssetsDirectoryPathCache.Value;
            set => ComputableTextureAssetTableAssetsDirectoryPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Audio clip asset table collection asset path
        /// </summary>
        public string AudioClipAssetTableCollectionAssetPath
        {
            get => ComputableAudioClipAssetTableCollectionAssetPathCache.Value;
            set => ComputableAudioClipAssetTableCollectionAssetPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Material asset table collection asset path
        /// </summary>
        public string MaterialAssetTableCollectionAssetPath
        {
            get => ComputableMaterialAssetTableCollectionAssetPathCache.Value;
            set => ComputableMaterialAssetTableCollectionAssetPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Mesh asset table collection asset path
        /// </summary>
        public string MeshAssetTableCollectionAssetPath
        {
            get => ComputableMeshAssetTableCollectionAssetPathCache.Value;
            set => ComputableMeshAssetTableCollectionAssetPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Sprite asset table collection asset path
        /// </summary>
        public string SpriteAssetTableCollectionAssetPath
        {
            get => ComputableSpriteAssetTableCollectionAssetPathCache.Value;
            set => ComputableSpriteAssetTableCollectionAssetPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// String asset table collection asset path
        /// </summary>
        public string StringTableCollectionAssetPath
        {
            get => ComputableStringTableCollectionAssetPathCache.Value;
            set => ComputableStringTableCollectionAssetPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Texture asset table collection asset path
        /// </summary>
        public string TextureAssetTableCollectionAssetPath
        {
            get => ComputableTextureAssetTableCollectionAssetPathCache.Value;
            set => ComputableTextureAssetTableCollectionAssetPathCache.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Tokenized search field
        /// </summary>
        public TokenizedSearchField TokenizedSearchField { get; private set; }

        /// <summary>
        /// Selected translation type tab
        /// </summary>
        public ETranslationType SelectedTranslationTypeTab { get; set; } = ETranslationType.String;

        /// <summary>
        /// Scroll view value
        /// </summary>
        public Vector2 ScrollViewValue { get; private set; }

        /// <summary>
        /// Are shared table data asset paths folded out
        /// </summary>
        public bool AreSharedTableDataAssetPathsFoldedOut { get; set; }

        /// <summary>
        /// Are localization table assets directory paths folded out
        /// </summary>
        public bool AreLocalizationTableAssetsDirectoryPathsFoldedOut { get; set; }

        /// <summary>
        /// Are localization table collection asset paths folded out
        /// </summary>
        public bool AreLocalizationTableCollectionAssetPathsFoldedOut { get; set; }

        /// <summary>
        /// Sets translation asset view selected state
        /// </summary>
        /// <typeparam name="TTranslationObject">Translation object type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <typeparam name="TTranslationData">TRanslation data type</typeparam>
        /// <typeparam name="TTranslatedData">Translated data type</typeparam>
        /// <param name="areSelected">Are selected</param>
        /// <param name="translationAssetViews">Translation asset views</param>
        private static void SetTranslationAssetViewsSelectedState<TTranslationObject, TValue, TTranslationData, TTranslatedData>(bool areSelected, IEnumerable<ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData>> translationAssetViews) where TTranslationObject : UnityEngine.Object, ITranslationObject<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationData : ITranslationData<TValue, TTranslatedData> where TTranslatedData : ITranslatedData<TValue>
        {
            List<ITranslatedValue<TValue>> translated_values = new();
            foreach (ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData> translation_asset_view in translationAssetViews)
            {
                translation_asset_view.IsSelected = areSelected;
                if (areSelected)
                {
                    translation_asset_view.GetValues(translated_values);
                    foreach (ITranslatedValue<TValue> translated_value in translated_values)
                    {
                        translation_asset_view.SetLanguageSelectedState(translated_value.Language, true);
                    }
                }
            }
            translated_values.Clear();
        }

        /// <summary>
        /// Draws computed value cache
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="computableValueCache">Computable value cache</param>
        private static void DrawComputableValueCache(string title, IComputableValueCache<string> computableValueCache)
        {
            string computed_value = computableValueCache.ComputedValue;
            string value = computableValueCache.Value;
            Color original_color = GUI.color;
            Color original_background_color = GUI.backgroundColor;
            bool is_original = computed_value == value;
            GUI.color = is_original ? original_color : Color.yellow;
            EditorGUILayout.BeginHorizontal();
            computableValueCache.Value = EditorGUILayout.TextField(title, value);
            GUI.color = original_color;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(is_original ? 0.0f : 20.0f)))
            {
                computableValueCache.ClearCache();
                GUI.FocusControl(null);
            }
            GUI.backgroundColor = original_background_color;
            GUI.color = is_original ? original_color : Color.yellow;
            EditorGUILayout.EndHorizontal();
            GUI.color = original_color;
        }

        /// <summary>
        /// Are selected translation asset views contained
        /// </summary>
        /// <typeparam name="TTranslationObject">Translation object type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <typeparam name="TTranslationData">Translation data type</typeparam>
        /// <typeparam name="TTranslatedData">Translated data type</typeparam>
        /// <param name="translationAssetViews">Ttanslation asset views</param>
        /// <returns>"true" if selected translation asset views are contained, otherwise "false"</returns>
        private static bool AreSelectedTranslationAssetViewsContained<TTranslationObject, TValue, TTranslationData, TTranslatedData>(IEnumerable<ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData>> translationAssetViews) where TTranslationObject : UnityEngine.Object, ITranslationObject<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationData : ITranslationData<TValue, TTranslatedData> where TTranslatedData : ITranslatedData<TValue>
        {
            bool ret = false;
            List<ITranslatedValue<TValue>> translated_values = new();
            foreach (ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData> translation_asset_view in translationAssetViews)
            {
                if (translation_asset_view.IsSelected)
                {
                    translation_asset_view.GetValues(translated_values);
                    foreach (ITranslatedValue<TValue> translated_value in translated_values)
                    {
                        if (translated_value.IsSelected)
                        {
                            ret = true;
                            break;
                        }
                    }
                    if (ret)
                    {
                        break;
                    }
                }
            }
            translated_values.Clear();
            return ret;
        }

        /// <summary>
        /// Shows migrate edtor window
        /// </summary>
        [MenuItem("Tools/Migration/Unity translator -> Unity's localization system")]
        public static void ShowMigrateEdtorWindow() =>
            GetWindow<MigrateEditorWindow>(true, "Migrate from Unity translator to Unity's localization system", true);

        /// <summary>
        /// Ensures localization table assets
        /// </summary>
        /// <typeparam name="TLocalizationTableCollection">Localization table collection type</typeparam>
        /// <typeparam name="TLocalizationTable">Localization table type</typeparam>
        /// <typeparam name="TSharedTableData">Shared table data type</typeparam>
        /// <param name="sharedTableDataAssetPath">Shared table data asset path</param>
        /// <param name="localizationTableAssetsDirectoryPath">Localization table assets directory path</param>
        /// <param name="localizationTableCollectionAssetPath">Localization table collection asset path</param>
        /// <param name="translationType">Translation type</param>
        /// <param name="localizationTables">Localization tables</param>
        /// <param name="localizationTableCollection">Localization table collection</param>
        /// <exception cref="NotImplementedException">When a value for "translationType" has not been implemented yet</exception>
        private void EnsureLocalizationTableAssets<TLocalizationTableCollection, TLocalizationTable, TSharedTableData>(string sharedTableDataAssetPath, string localizationTableAssetsDirectoryPath, string localizationTableCollectionAssetPath, ETranslationType translationType, List<TLocalizationTable> localizationTables, ref TLocalizationTableCollection localizationTableCollection) where TLocalizationTableCollection : LocalizationTableCollection where TLocalizationTable : LocalizationTable where TSharedTableData : SharedTableData
        {
            localizationTables.Clear();
            string shared_table_data_asset_path = sharedTableDataAssetPath.EndsWith(".asset", StringComparison.OrdinalIgnoreCase) ? sharedTableDataAssetPath : $"{ sharedTableDataAssetPath }.asset";
            TSharedTableData shared_table_data = AssetDatabase.LoadAssetAtPath<TSharedTableData>(shared_table_data_asset_path);
            if (!shared_table_data)
            {
                shared_table_data = CreateInstance<TSharedTableData>();
                shared_table_data.name = Path.GetFileNameWithoutExtension(shared_table_data_asset_path);
                AssetDatabaseUtilities.CreateAsset(shared_table_data, shared_table_data_asset_path);
            }
            AssetDatabase.ForceReserializeAssets(new[] { shared_table_data_asset_path });
            AssetDatabase.SaveAssets();
            for (int locale_index = 0; locale_index < systemLanguageLocaleIdentifiers.Count; locale_index++)
            {
                ISystemLanguageLocaleIdentifier system_language_locale_identifier = systemLanguageLocaleIdentifiers[locale_index];
                string localization_table_name = translationType switch
                {
                    ETranslationType.AudioClip => system_language_locale_identifier.AudioClipAssetTableName,
                    ETranslationType.Material => system_language_locale_identifier.MaterialAssetTableName,
                    ETranslationType.Mesh => system_language_locale_identifier.MeshAssetTableName,
                    ETranslationType.Sprite => system_language_locale_identifier.SpriteAssetTableName,
                    ETranslationType.String => system_language_locale_identifier.StringTableName,
                    ETranslationType.Texture => system_language_locale_identifier.TextureAssetTableName,
                    _ => throw new NotImplementedException($"Localization table name for \"{ translationType }\" has not been implemented yet."),
                };
                string localization_table_asset_path = AssetDatabaseUtilities.CombinePaths(localizationTableAssetsDirectoryPath, localization_table_name.EndsWith(".asset") ? localization_table_name : $"{ localization_table_name }.asset");
                TLocalizationTable localization_table = AssetDatabase.LoadAssetAtPath<TLocalizationTable>(localization_table_asset_path);
                if (!localization_table)
                {
                    localization_table = CreateInstance<TLocalizationTable>();
                    localization_table.name = Path.GetFileNameWithoutExtension(localization_table_asset_path);
                    localization_table.LocaleIdentifier = system_language_locale_identifier.LocaleIdentifier;
                    AssetDatabaseUtilities.CreateAsset(localization_table, localization_table_asset_path);
                }
                if (!shared_table_data)
                {
                    Debug.LogError("Shared table data is null.");
                }
                localization_table.SharedData = shared_table_data;
                // Use, but do nothing so it can be serialized
                localization_table.SharedData.GetId(string.Empty);
                if (!shared_table_data)
                {
                    Debug.LogError("Shared table data is null.");
                }
                if (!localization_table.SharedData)
                {
                    Debug.LogError($"Localization table \"{ localization_table.name }\" shared data is null.");
                }
                localizationTables.Add(localization_table);
            }
            SaveAssets();
            string localization_table_collection_asset_path = localizationTableCollectionAssetPath.EndsWith(".asset", StringComparison.OrdinalIgnoreCase) ? localizationTableCollectionAssetPath : $"{ localizationTableCollectionAssetPath }.asset";
            localizationTableCollection = AssetDatabase.LoadAssetAtPath<TLocalizationTableCollection>(localization_table_collection_asset_path);
            if (!localizationTableCollection)
            {
                localizationTableCollection = CreateInstance<TLocalizationTableCollection>();
                string localization_table_collection_name = Path.GetFileNameWithoutExtension(localization_table_collection_asset_path);
                localizationTableCollection.name = localization_table_collection_name;
                // TODO: Remove set by reflection when API changes
                //localizationTableCollection.SharedData = shared_table_data;
                typeof(TLocalizationTableCollection).GetProperty("SharedData").SetValue(localizationTableCollection, shared_table_data, null);
                AssetDatabaseUtilities.CreateAsset(localizationTableCollection, localization_table_collection_asset_path);
            }
            foreach (TLocalizationTable localization_table in localizationTables)
            {
                if (!localizationTableCollection.ContainsTable(localization_table))
                {
                    if (!localization_table.SharedData)
                    {
                        Debug.LogError($"Localization table \"{ localization_table.name }\" shared data is null.");
                    }
                    if (!localizationTableCollection.SharedData)
                    {
                        Debug.LogError($"Localization table collection \"{ localizationTableCollection.name }\" shared data is null.");
                    }
                    localizationTableCollection.AddTable(localization_table);
                }
            }
            SaveAssets();
        }

        /// <summary>
        /// Ensures localization assets
        /// </summary>
        /// <param name="translationType">Translation type</param>
        private void EnsureLocalizationAssets(ETranslationType? translationType = null)
        {
            locales.Clear();
            audioClipAssetTables.Clear();
            materialAssetTables.Clear();
            meshAssetTables.Clear();
            spriteAssetTables.Clear();
            stringTables.Clear();
            textureAssetTables.Clear();
            audioClipAssetTableCollection = null;
            materialAssetTableCollection = null;
            meshAssetTableCollection = null;
            spriteAssetTableCollection = null;
            stringTableCollection = null;
            textureAssetTableCollection = null;
            string locale_assets_directory_path = LocaleAssetsDirectoryPath;
            for (int locale_index = 0; locale_index < systemLanguageLocaleIdentifiers.Count; locale_index++)
            {
                ISystemLanguageLocaleIdentifier system_language_locale_identifier = systemLanguageLocaleIdentifiers[locale_index];
                string locale_asset_path = AssetDatabaseUtilities.CombinePaths(locale_assets_directory_path, system_language_locale_identifier.LocaleName.EndsWith(".asset", StringComparison.OrdinalIgnoreCase) ? system_language_locale_identifier.LocaleName : $"{ system_language_locale_identifier.LocaleName }.asset");
                Locale locale = AssetDatabase.LoadAssetAtPath<Locale>(locale_asset_path);
                if (locale)
                {
                    locale.Identifier = system_language_locale_identifier.LocaleIdentifier;
                    locale.SortOrder = (ushort)locale_index;
                }
                else
                {
                    locale = CreateInstance<Locale>();
                    locale.name = Path.GetFileNameWithoutExtension(locale_asset_path);
                    locale.Identifier = system_language_locale_identifier.LocaleIdentifier;
                    locale.SortOrder = (ushort)locale_index;
                    AssetDatabaseUtilities.CreateAsset(locale, locale_asset_path);
                }
                locales.Add(locale);
            }
            if (((translationType == null) || (translationType == ETranslationType.AudioClip)) && AreSelectedTranslationAssetViewsContained(audioClipTranslationAssetViews))
            {
                EnsureLocalizationTableAssets<AssetTableCollection, AssetTable, SharedTableData>(SharedAudioClipAssetTableDataAssetPath, AudioClipAssetTableAssetsDirectoryPath, AudioClipAssetTableCollectionAssetPath, ETranslationType.AudioClip, audioClipAssetTables, ref audioClipAssetTableCollection);
            }
            if (((translationType == null) || (translationType == ETranslationType.Material)) && AreSelectedTranslationAssetViewsContained(materialTranslationAssetViews))
            {
                EnsureLocalizationTableAssets<AssetTableCollection, AssetTable, SharedTableData>(SharedMaterialAssetTableDataAssetPath, MaterialAssetTableAssetsDirectoryPath, MaterialAssetTableCollectionAssetPath, ETranslationType.Material, materialAssetTables, ref materialAssetTableCollection);
            }
            if (((translationType == null) || (translationType == ETranslationType.Mesh)) && AreSelectedTranslationAssetViewsContained(meshTranslationAssetViews))
            {
                EnsureLocalizationTableAssets<AssetTableCollection, AssetTable, SharedTableData>(SharedMeshAssetTableDataAssetPath, MeshAssetTableAssetsDirectoryPath, MeshAssetTableCollectionAssetPath, ETranslationType.Mesh, meshAssetTables, ref meshAssetTableCollection);
            }
            if (((translationType == null) || (translationType == ETranslationType.Sprite)) && AreSelectedTranslationAssetViewsContained(spriteTranslationAssetViews))
            {
                EnsureLocalizationTableAssets<AssetTableCollection, AssetTable, SharedTableData>(SharedSpriteAssetTableDataAssetPath, SpriteAssetTableAssetsDirectoryPath, SpriteAssetTableCollectionAssetPath, ETranslationType.Sprite, spriteAssetTables, ref spriteAssetTableCollection);
            }
            if (((translationType == null) || (translationType == ETranslationType.String)) && AreSelectedTranslationAssetViewsContained(stringTranslationAssetViews))
            {
                EnsureLocalizationTableAssets<StringTableCollection, StringTable, SharedTableData>(SharedStringTableDataAssetPath, StringTableAssetsDirectoryPath, StringTableCollectionAssetPath, ETranslationType.String, stringTables, ref stringTableCollection);
            }
            if (((translationType == null) || (translationType == ETranslationType.Texture)) && AreSelectedTranslationAssetViewsContained(textureTranslationAssetViews))
            {
                EnsureLocalizationTableAssets<AssetTableCollection, AssetTable, SharedTableData>(SharedTextureAssetTableDataAssetPath, TextureAssetTableAssetsDirectoryPath, TextureAssetTableCollectionAssetPath, ETranslationType.Texture, textureAssetTables, ref textureAssetTableCollection);
            }
        }

        /// <summary>
        /// Migrates translation asset views
        /// </summary>
        /// <typeparam name="TTranslationObject">Translation object type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <typeparam name="TTranslationData">Translation data type</typeparam>
        /// <typeparam name="TTranslatedData">TRanslated data type</typeparam>
        /// <typeparam name="TLocalizationTable">Localization table type</typeparam>
        /// <param name="translationAssetViews">Translation asset views</param>
        /// <param name="localizationTables">Localization tables</param>
        private static void MigrateTranslationAssetViews<TTranslationObject, TValue, TTranslationData, TTranslatedData, TLocalizationTable>(IEnumerable<ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData>> translationAssetViews, IEnumerable<TLocalizationTable> localizationTables) where TTranslationObject : UnityEngine.Object, ITranslationObject<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationData : ITranslationData<TValue, TTranslatedData> where TTranslatedData : ITranslatedData<TValue> where TLocalizationTable : LocalizationTable
        {
            List<ITranslatedValue<TValue>> translated_values = new();
            foreach (ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData> translation_asset_view in translationAssetViews)
            {
                translation_asset_view.GetValues(translated_values);
                foreach (ITranslatedValue<TValue> translated_value in translated_values)
                {
                    LocaleIdentifier current_locale_identifier = new(translated_value.Language);
                    switch (localizationTables)
                    {
                        case IEnumerable<StringTable> string_tables:
                            foreach (StringTable string_table in string_tables)
                            {
                                if (string_table.LocaleIdentifier == current_locale_identifier)
                                {
                                    string_table.AddEntry(translation_asset_view.Key, translated_value.Value.ToString());
                                    break;
                                }
                            }
                            break;
                        case IEnumerable<AssetTable> asset_tables:
                            foreach (AssetTable asset_table in asset_tables)
                            {
                                if (asset_table.LocaleIdentifier == current_locale_identifier)
                                {
                                    if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(translated_value.Value as UnityEngine.Object, out string guid, out long _))
                                    {
                                        asset_table.AddEntry(translation_asset_view.Key, guid);
                                    }
                                    else
                                    {
                                        Debug.LogError($"Failed to get GUID and local file identifier for \"{ translated_value.Value }\".");
                                    }
                                    break;
                                }
                            }
                            break;
                    }
                }
            }
            translated_values.Clear();
        }

        /// <summary>
        /// Saves assets
        /// </summary>
        private void SaveAssets()
        {
            List<string> asset_paths = new();
            foreach (Locale locale in locales)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(locale));
            }
            foreach (AssetTable audio_clip_asset_table in audioClipAssetTables)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(audio_clip_asset_table));
            }
            foreach (AssetTable material_asset_table in materialAssetTables)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(material_asset_table));
            }
            foreach (AssetTable mesh_asset_table in meshAssetTables)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(mesh_asset_table));
            }
            foreach (AssetTable sprite_asset_table in spriteAssetTables)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(sprite_asset_table));
            }
            foreach (StringTable string_table in stringTables)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(string_table));
            }
            foreach (AssetTable texture_asset_table in textureAssetTables)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(texture_asset_table));
            }
            if (audioClipAssetTableCollection)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(audioClipAssetTableCollection));
            }
            if (materialAssetTableCollection)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(materialAssetTableCollection));
            }
            if (meshAssetTableCollection)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(meshAssetTableCollection));
            }
            if (spriteAssetTableCollection)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(spriteAssetTableCollection));
            }
            if (stringTableCollection)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(stringTableCollection));
            }
            if (textureAssetTableCollection)
            {
                asset_paths.Add(AssetDatabase.GetAssetPath(textureAssetTableCollection));
            }
            AssetDatabase.ForceReserializeAssets(asset_paths);
            asset_paths.Clear();
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Is translation key a duplicate
        /// </summary>
        /// <typeparam name="TTranslationObject">Translation object type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <typeparam name="TTranslationData">Translation data type</typeparam>
        /// <typeparam name="TTranslatedData">Translated data type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="translationAssetViews">Translation asset views</param>
        /// <returns></returns>
        private bool IsTranslationKeyADuplicate<TTranslationObject, TValue, TTranslationData, TTranslatedData>(string key, IEnumerable<ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData>> translationAssetViews) where TTranslationObject : UnityEngine.Object, ITranslationObject<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationData : ITranslationData<TValue, TTranslatedData> where TTranslatedData : ITranslatedData<TValue>
        {
            bool ret = false;
            bool is_found = false;
            foreach (ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData> translation_asset_view in translationAssetViews)
            {
                if (translation_asset_view.Key == key)
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
        /// Draws translation asset views
        /// </summary>
        /// <typeparam name="TTranslationObject">TRanslation object type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <typeparam name="TTranslationData">Translation data type</typeparam>
        /// <typeparam name="TTranslatedData">TRanslated data type</typeparam>
        /// <param name="translationAssetViews">Translation asset views</param>
        /// <param name="onDrawTranslatedAsset">Gets invoked when translated asset needs to be drawn</param>
        private void DrawTranslationAssetViews<TTranslationObject, TValue, TTranslationData, TTranslatedData>(IEnumerable<ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData>> translationAssetViews, Func<ITranslatedValue<TValue>, IEditedTranslatedValueResult<TValue>> onDrawTranslatedAsset) where TTranslationObject : UnityEngine.Object, ITranslationObject<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationData : ITranslationData<TValue, TTranslatedData> where TTranslatedData : ITranslatedData<TValue>
        {
            List<ITranslatedValue<TValue>> translated_values = new();
            foreach (ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData> translation_asset_view in translationAssetViews)
            {
                if (TokenizedSearchField.IsContainedInSearch(translation_asset_view.AssetPath))
                {
                    Color original_background_color = GUI.backgroundColor;
                    bool is_enabled = GUI.enabled;
                    GUI.backgroundColor = translation_asset_view.IsSelected ? original_background_color : Color.red;
                    EditorGUILayout.BeginHorizontal();
                    translation_asset_view.IsSelected = EditorGUILayout.Toggle(translation_asset_view.IsSelected, GUILayout.Width(20.0f));
                    GUI.enabled = translation_asset_view.IsSelected && is_enabled;
                    translation_asset_view.IsFoldedOut = EditorGUILayout.BeginFoldoutHeaderGroup(translation_asset_view.IsSelected && translation_asset_view.IsFoldedOut, Path.GetFileNameWithoutExtension(translation_asset_view.AssetPath));
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    EditorGUILayout.EndHorizontal();
                    if (translation_asset_view.IsSelected && translation_asset_view.IsFoldedOut)
                    {
                        translation_asset_view.IsFoldedOut = true;
                        GUI.enabled = false;
                        EditorGUILayout.ObjectField(translation_asset_view.Translation, typeof(TTranslationObject), false);
                        GUI.enabled = translation_asset_view.IsSelected && is_enabled;
                        translation_asset_view.Key = EditorGUILayout.TextField(translation_asset_view.Key);
                        if (IsTranslationKeyADuplicate(translation_asset_view.Key, translationAssetViews))
                        {
                            Color original_color = GUI.color;
                            GUI.color = Color.yellow;
                            EditorGUILayout.LabelField("Key is a duplicate!");
                            GUI.color = original_color;
                        }
                        translation_asset_view.GetValues(translated_values);
                        foreach (ITranslatedValue<TValue> translated_value in translated_values)
                        {
                            IEditedTranslatedValueResult<TValue> edited_translated_value_result = onDrawTranslatedAsset(translated_value);
                            if (edited_translated_value_result.IsEditingValue)
                            {
                                translation_asset_view.EditValue(translated_value.Language, edited_translated_value_result.EditedValue);
                            }
                            else
                            {
                                translation_asset_view.ResetValue(translated_value.Language);
                            }
                            translation_asset_view.SetLanguageSelectedState(translated_value.Language, edited_translated_value_result.IsSelected);
                        }
                    }
                    else
                    {
                        translation_asset_view.IsFoldedOut = false;
                    }
                    GUI.enabled = is_enabled;
                    GUI.backgroundColor = original_background_color;
                }
            }
            translated_values.Clear();
        }

        /// <summary>
        /// Is system language locale identifier contained
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>"true" if system language locale identifier is contained, otherwise "false"</returns>
        private bool IsSystemLanguageLocaleIdentifierContained(SystemLanguage language)
        {
            bool ret = false;
            foreach (ISystemLanguageLocaleIdentifier system_language_locale_identifier in systemLanguageLocaleIdentifiers)
            {
                if (system_language_locale_identifier.Language == language)
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Initializes translation asset views
        /// </summary>
        /// <typeparam name="TTranslationObject">Translation object type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <typeparam name="TTranslationData">Translation data type</typeparam>
        /// <typeparam name="TTranslatedData">Translated data type</typeparam>
        /// <param name="isForcingRefresh">Is forcing refresh</param>
        /// <param name="translationAssetViews">Translation asset views</param>
        private void InitializeTranslationAssetViews<TTranslationObject, TValue, TTranslationData, TTranslatedData>(bool isForcingRefresh, ref List<ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData>> translationAssetViews) where TTranslationObject : UnityEngine.Object, ITranslationObject<TTranslationObject, TValue, TTranslationData, TTranslatedData> where TTranslationData : ITranslationData<TValue, TTranslatedData> where TTranslatedData : ITranslatedData<TValue>
        {
            if (isForcingRefresh || (translationAssetViews == null))
            {
                translationAssetViews ??= new();
                TranslationAssetViews.GetTranslationAssetViews(translationAssetViews);
                List<ITranslatedValue<TValue>> translated_values = new();
                foreach (ITranslationAssetView<TTranslationObject, TValue, TTranslationData, TTranslatedData> translation_asset_view in translationAssetViews)
                {
                    translation_asset_view.GetValues(translated_values);
                    for (int translated_value_index = 0; translated_value_index < translated_values.Count; translated_value_index++)
                    {
                        ITranslatedValue<TValue> translated_value = translated_values[translated_value_index];
                        if (!IsSystemLanguageLocaleIdentifierContained(translated_value.Language))
                        {
                            systemLanguageLocaleIdentifiers.Insert(Mathf.Min(translated_value_index, systemLanguageLocaleIdentifiers.Count), new SystemLanguageLocaleIdentifier(translated_value.Language));
                        }
                    }
                }
                translated_values.Clear();
            }
        }

        /// <summary>
        /// Initializes translation asset views
        /// </summary>
        /// <param name="isForcingRefresh">Is forcing refresh</param>
        private void InitializeTranslationAssetViews(bool isForcingRefresh)
        {
            InitializeTranslationAssetViews(isForcingRefresh, ref audioClipTranslationAssetViews);
            InitializeTranslationAssetViews(isForcingRefresh, ref materialTranslationAssetViews);
            InitializeTranslationAssetViews(isForcingRefresh, ref meshTranslationAssetViews);
            InitializeTranslationAssetViews(isForcingRefresh, ref spriteTranslationAssetViews);
            InitializeTranslationAssetViews(isForcingRefresh, ref stringTranslationAssetViews);
            InitializeTranslationAssetViews(isForcingRefresh, ref textureTranslationAssetViews);
        }

        /// <summary>
        /// Sets translation asset views selected state
        /// </summary>
        /// <param name="areSelected">Are selected</param>
        private void SetTranslationAssetViewsSelectedState(bool areSelected)
        {
            SetTranslationAssetViewsSelectedState(areSelected, audioClipTranslationAssetViews);
            SetTranslationAssetViewsSelectedState(areSelected, materialTranslationAssetViews);
            SetTranslationAssetViewsSelectedState(areSelected, meshTranslationAssetViews);
            SetTranslationAssetViewsSelectedState(areSelected, spriteTranslationAssetViews);
            SetTranslationAssetViewsSelectedState(areSelected, stringTranslationAssetViews);
            SetTranslationAssetViewsSelectedState(areSelected, textureTranslationAssetViews);
        }

        /// <summary>
        /// Migrates translation asset views
        /// </summary>
        private void MigrateTranslationAssetViews()
        {
            MigrateTranslationAssetViews(audioClipTranslationAssetViews, audioClipAssetTables);
            MigrateTranslationAssetViews(materialTranslationAssetViews, materialAssetTables);
            MigrateTranslationAssetViews(meshTranslationAssetViews, meshAssetTables);
            MigrateTranslationAssetViews(spriteTranslationAssetViews, spriteAssetTables);
            MigrateTranslationAssetViews(stringTranslationAssetViews, stringTables);
            MigrateTranslationAssetViews(textureTranslationAssetViews, textureAssetTables);
        }

        /// <summary>
        /// Gets invoked when a Unity object needs to be drawn
        /// </summary>
        /// <typeparam name="TTranslationObject">Translation object type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="translatedValue">Translated value</param>
        /// <returns>Edited translated value result</returns>
        private IEditedTranslatedValueResult<TValue> DrawUnityObjectEvent<TTranslationObject, TValue>(ITranslatedValue<TValue> translatedValue) where TTranslationObject : UnityEngine.Object where TValue : UnityEngine.Object
        {
            Color original_background_color = GUI.backgroundColor;
            bool is_enabled = GUI.enabled;
            Color background_color = IsSystemLanguageLocaleIdentifierContained(translatedValue.Language) ? (translatedValue.IsSelected ? (translatedValue.IsValueEdited ? Color.yellow : original_background_color) : Color.red) : Color.grey;
            GUI.backgroundColor = background_color;
            EditorGUILayout.BeginHorizontal();
            bool is_selected = EditorGUILayout.Toggle(translatedValue.Language.ToString(), translatedValue.IsSelected);
            bool is_language_missing = !IsSystemLanguageLocaleIdentifierContained(translatedValue.Language);
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Create missing locale", GUILayout.Width(is_language_missing ? 200.0f : 0.0f)) && is_language_missing)
            {
                systemLanguageLocaleIdentifiers.Add(new SystemLanguageLocaleIdentifier(translatedValue.Language));
            }
            GUI.backgroundColor = original_background_color;
            EditorGUILayout.EndHorizontal();
            GUI.enabled = is_enabled && is_selected;
            EditorGUILayout.BeginHorizontal();
            TValue edited_value = (TValue)EditorGUILayout.ObjectField(translatedValue.Value, typeof(TValue), false);
            GUI.backgroundColor = translatedValue.IsValueEdited ? Color.red : original_background_color;
            bool is_editing_value = !GUILayout.Button("X", GUILayout.Width(translatedValue.IsValueEdited ? 20.0f : 0.0f));
            if (!is_editing_value)
            {
                GUI.FocusControl(null);
            }
            GUI.backgroundColor = background_color;
            EditorGUILayout.EndHorizontal();
            GUI.enabled = is_enabled;
            GUI.backgroundColor = original_background_color;
            return new EditedTranslatedValueResult<TValue>(is_selected && is_editing_value, edited_value, is_selected);
        }

        /// <summary>
        /// Gets invoked when script has been enabled
        /// </summary>
        protected virtual void OnEnable()
        {
            tabs ??= new ITab[]
            {
                new Tab("Audio clips", () => DrawTranslationAssetViews(audioClipTranslationAssetViews, DrawUnityObjectEvent<AudioClipTranslationObjectScript, AudioClip>)),
                new Tab("Materials", () => DrawTranslationAssetViews(materialTranslationAssetViews, DrawUnityObjectEvent<MaterialTranslationObjectScript, Material>)),
                new Tab("Meshes", () => DrawTranslationAssetViews(meshTranslationAssetViews, DrawUnityObjectEvent<MeshTranslationObjectScript, Mesh>)),
                new Tab("Sprites", () => DrawTranslationAssetViews(spriteTranslationAssetViews, DrawUnityObjectEvent<SpriteTranslationObjectScript, Sprite>)),
                new Tab
                (
                    "Strings",
                    () =>
                        DrawTranslationAssetViews
                        (
                            stringTranslationAssetViews,
                            (translatedValue) =>
                            {
                                Color original_background_color = GUI.backgroundColor;
                                bool is_enabled = GUI.enabled;
                                Color background_color = translatedValue.IsSelected ? (translatedValue.IsValueEdited ? Color.yellow : original_background_color) : Color.red;
                                GUI.backgroundColor = background_color;
                                EditorGUILayout.BeginHorizontal();
                                bool is_selected = EditorGUILayout.Toggle(translatedValue.Language.ToString(), translatedValue.IsSelected);
                                bool is_language_missing = !IsSystemLanguageLocaleIdentifierContained(translatedValue.Language);
                                GUI.backgroundColor = Color.green;
                                if (GUILayout.Button("Create missing locale", GUILayout.Width(is_language_missing ? 200.0f : 0.0f)) && is_language_missing)
                                {
                                    systemLanguageLocaleIdentifiers.Add(new SystemLanguageLocaleIdentifier(translatedValue.Language));
                                }
                                GUI.backgroundColor = original_background_color;
                                EditorGUILayout.EndHorizontal();
                                GUI.enabled = is_enabled && is_selected;
                                EditorGUILayout.BeginHorizontal();
                                string edited_value = EditorGUILayout.TextArea(translatedValue.Value, GUILayout.Height(80.0f));
                                GUI.backgroundColor = translatedValue.IsValueEdited ? Color.red : original_background_color;
                                bool is_editing_value = !GUILayout.Button("X", GUILayout.Width(translatedValue.IsValueEdited ? 20.0f : 0.0f));
                                if (!is_editing_value)
                                {
                                    GUI.FocusControl(null);
                                }
                                GUI.backgroundColor = background_color;
                                EditorGUILayout.EndHorizontal();
                                GUI.enabled = is_enabled;
                                GUI.backgroundColor = original_background_color;
                                return new EditedTranslatedValueResult<string>(is_selected && is_editing_value, edited_value, is_selected);
                            }
                        )
                ),
                new Tab("Textures", () => DrawTranslationAssetViews(textureTranslationAssetViews, DrawUnityObjectEvent<TextureTranslationObjectScript, Texture>)),
            };
            TokenizedSearchField ??= new TokenizedSearchField();
        }

        /// <summary>
        /// Gets invoked when GUI needs to be drawn
        /// </summary>
        /// <exception cref="NotImplementedException">When a value for "SelectedTranslationTypeTab" has not been implemented yet</exception>
        protected virtual void OnGUI()
        {
            EditorGUILayout.LabelField("Directory paths");
            ModuleDirectoryPath = EditorGUILayout.TextField("Module path", ModuleDirectoryPath);
            if (IsModuleDirectoryPathExisting)
            {
                EditorGUILayout.LabelField("Module directory path exists.");
            }
            else
            {
                Color original_content_color = GUI.contentColor;
                GUI.contentColor = Color.yellow;
                EditorGUILayout.LabelField("Module directory path does not exist yet.");
                GUI.contentColor = original_content_color;
            }
            EditorGUILayout.Space(20.0f);
            DrawComputableValueCache("Locales", ComputableLocaleAssetsDirectoryPathCache);
            AreSharedTableDataAssetPathsFoldedOut = EditorGUILayout.Foldout(AreSharedTableDataAssetPathsFoldedOut, "Shared table data");
            if (AreSharedTableDataAssetPathsFoldedOut)
            {
                DrawComputableValueCache("Audio clips", ComputableSharedAudioClipTableDataAssetPathCache);
                DrawComputableValueCache("Materials", ComputableSharedMaterialTableDataAssetPathCache);
                DrawComputableValueCache("Meshes", ComputableSharedMeshTableDataAssetPathCache);
                DrawComputableValueCache("Sprites", ComputableSharedSpriteTableDataAssetPathCache);
                DrawComputableValueCache("Strings", ComputableSharedStringTableDataAssetPathCache);
                DrawComputableValueCache("Texture", ComputableSharedTextureTableDataAssetPathCache);
            }
            AreLocalizationTableAssetsDirectoryPathsFoldedOut = EditorGUILayout.Foldout(AreLocalizationTableAssetsDirectoryPathsFoldedOut, "Localization table directories");
            if (AreLocalizationTableAssetsDirectoryPathsFoldedOut)
            {
                DrawComputableValueCache("Audio clips", ComputableAudioClipAssetTableAssetsDirectoryPathCache);
                DrawComputableValueCache("Materials", ComputableMaterialAssetTableAssetsDirectoryPathCache);
                DrawComputableValueCache("Meshes", ComputableMeshAssetTableAssetsDirectoryPathCache);
                DrawComputableValueCache("Sprites", ComputableSpriteAssetTableAssetsDirectoryPathCache);
                DrawComputableValueCache("Strings", ComputableStringTableAssetsDirectoryPathCache);
                DrawComputableValueCache("Texture", ComputableTextureAssetTableAssetsDirectoryPathCache);
            }
            AreLocalizationTableCollectionAssetPathsFoldedOut = EditorGUILayout.Foldout(AreLocalizationTableCollectionAssetPathsFoldedOut, "Localization table collections");
            if (AreLocalizationTableCollectionAssetPathsFoldedOut)
            {
                DrawComputableValueCache("Audio clips", ComputableAudioClipAssetTableCollectionAssetPathCache);
                DrawComputableValueCache("Materials", ComputableMaterialAssetTableCollectionAssetPathCache);
                DrawComputableValueCache("Meshes", ComputableMeshAssetTableCollectionAssetPathCache);
                DrawComputableValueCache("Sprites", ComputableSpriteAssetTableCollectionAssetPathCache);
                DrawComputableValueCache("Strings", ComputableStringTableCollectionAssetPathCache);
                DrawComputableValueCache("Texture", ComputableTextureAssetTableCollectionAssetPathCache);
            }
            InitializeTranslationAssetViews(false);
            EditorGUILayout.BeginHorizontal();
            for (int tab_index = 0; tab_index < tabs.Count; tab_index++)
            {
                Color original_background_color = GUI.backgroundColor;
                GUI.backgroundColor = ((int)SelectedTranslationTypeTab == tab_index) ? Color.green : original_background_color;
                if (GUILayout.Button(tabs[tab_index].Name))
                {
                    SelectedTranslationTypeTab = (ETranslationType)tab_index;
                }
                GUI.backgroundColor = original_background_color;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh assets"))
            {
                InitializeTranslationAssetViews(true);
            }
            if (GUILayout.Button("Select everything"))
            {
                SetTranslationAssetViewsSelectedState(true);
            }
            if (GUILayout.Button("Deselect everything"))
            {
                SetTranslationAssetViewsSelectedState(false);
            }
            ITab selected_tab = tabs[(int)SelectedTranslationTypeTab];
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button($"Select all \"{ selected_tab.Name }\"s"))
            {
                switch (SelectedTranslationTypeTab)
                {
                    case ETranslationType.AudioClip:
                        SetTranslationAssetViewsSelectedState(true, audioClipTranslationAssetViews);
                        break;
                    case ETranslationType.Material:
                        SetTranslationAssetViewsSelectedState(true, materialTranslationAssetViews);
                        break;
                    case ETranslationType.Mesh:
                        SetTranslationAssetViewsSelectedState(true, meshTranslationAssetViews);
                        break;
                    case ETranslationType.Sprite:
                        SetTranslationAssetViewsSelectedState(true, spriteTranslationAssetViews);
                        break;
                    case ETranslationType.String:
                        SetTranslationAssetViewsSelectedState(true, stringTranslationAssetViews);
                        break;
                    case ETranslationType.Texture:
                        SetTranslationAssetViewsSelectedState(true, textureTranslationAssetViews);
                        break;
                }
            }
            if (GUILayout.Button($"Deselect all \"{ selected_tab.Name }\"s"))
            {
                switch (SelectedTranslationTypeTab)
                {
                    case ETranslationType.AudioClip:
                        SetTranslationAssetViewsSelectedState(false, audioClipTranslationAssetViews);
                        break;
                    case ETranslationType.Material:
                        SetTranslationAssetViewsSelectedState(false, materialTranslationAssetViews);
                        break;
                    case ETranslationType.Mesh:
                        SetTranslationAssetViewsSelectedState(false, meshTranslationAssetViews);
                        break;
                    case ETranslationType.Sprite:
                        SetTranslationAssetViewsSelectedState(false, spriteTranslationAssetViews);
                        break;
                    case ETranslationType.String:
                        SetTranslationAssetViewsSelectedState(false, stringTranslationAssetViews);
                        break;
                    case ETranslationType.Texture:
                        SetTranslationAssetViewsSelectedState(false, textureTranslationAssetViews);
                        break;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            bool is_migration_of_all_allowed =
                TranslationAssetViews.AreTranslationAssetViewKeysUnique(audioClipTranslationAssetViews, true) &&
                TranslationAssetViews.AreTranslationAssetViewKeysUnique(materialTranslationAssetViews, true) &&
                TranslationAssetViews.AreTranslationAssetViewKeysUnique(meshTranslationAssetViews, true) &&
                TranslationAssetViews.AreTranslationAssetViewKeysUnique(spriteTranslationAssetViews, true) &&
                TranslationAssetViews.AreTranslationAssetViewKeysUnique(stringTranslationAssetViews, true) &&
                TranslationAssetViews.AreTranslationAssetViewKeysUnique(textureTranslationAssetViews, true);
            bool is_enabled = GUI.enabled;
            GUI.enabled = is_enabled && is_migration_of_all_allowed;
            if (GUILayout.Button("Migrate all translations") && is_migration_of_all_allowed)
            {
                EnsureLocalizationAssets();
                MigrateTranslationAssetViews();
                SaveAssets();
            }
            bool is_migration_allowed = SelectedTranslationTypeTab switch
            {
                ETranslationType.AudioClip => TranslationAssetViews.AreTranslationAssetViewKeysUnique(audioClipTranslationAssetViews, true),
                ETranslationType.Material => TranslationAssetViews.AreTranslationAssetViewKeysUnique(materialTranslationAssetViews, true),
                ETranslationType.Mesh => TranslationAssetViews.AreTranslationAssetViewKeysUnique(meshTranslationAssetViews, true),
                ETranslationType.Sprite => TranslationAssetViews.AreTranslationAssetViewKeysUnique(spriteTranslationAssetViews, true),
                ETranslationType.String => TranslationAssetViews.AreTranslationAssetViewKeysUnique(stringTranslationAssetViews, true),
                ETranslationType.Texture => TranslationAssetViews.AreTranslationAssetViewKeysUnique(textureTranslationAssetViews, true),
                _ => throw new NotImplementedException($"Is migration allowed check has not been implemented for selected translation type tab \"{ SelectedTranslationTypeTab }\" yet.")
            };
            GUI.enabled = is_enabled && is_migration_allowed;
            if (GUILayout.Button($"Migrate \"{ selected_tab.Name }\"s only") && is_migration_allowed)
            {
                EnsureLocalizationAssets(SelectedTranslationTypeTab);
                switch (SelectedTranslationTypeTab)
                {
                    case ETranslationType.AudioClip:
                        MigrateTranslationAssetViews(audioClipTranslationAssetViews, audioClipAssetTables);
                        break;
                    case ETranslationType.Material:
                        MigrateTranslationAssetViews(materialTranslationAssetViews, materialAssetTables);
                        break;
                    case ETranslationType.Mesh:
                        MigrateTranslationAssetViews(meshTranslationAssetViews, meshAssetTables);
                        break;
                    case ETranslationType.Sprite:
                        MigrateTranslationAssetViews(spriteTranslationAssetViews, spriteAssetTables);
                        break;
                    case ETranslationType.String:
                        MigrateTranslationAssetViews(stringTranslationAssetViews, stringTables);
                        break;
                    case ETranslationType.Texture:
                        MigrateTranslationAssetViews(textureTranslationAssetViews, textureAssetTables);
                        break;
                    default:
                        throw new NotImplementedException($"Migration for selected translation type tab \"{ SelectedTranslationTypeTab }\" has not been implemented yet.");
                }
                SaveAssets();
            }
            GUI.enabled = is_enabled;
            EditorGUILayout.EndHorizontal();
            TokenizedSearchField.Draw();
            ScrollViewValue = EditorGUILayout.BeginScrollView(ScrollViewValue);
            for (int index = 0; index < systemLanguageLocaleIdentifiers.Count; index++)
            {
                ISystemLanguageLocaleIdentifier system_language_locale_identifiers = systemLanguageLocaleIdentifiers[index];
                EditorGUILayout.BeginHorizontal();
                GUI.enabled = is_enabled && (index > 0);
                if (GUILayout.Button("^", GUILayout.Width(20.0f)) && (index > 0))
                {
                    ISystemLanguageLocaleIdentifier other_system_language_locale_identifiers = systemLanguageLocaleIdentifiers[index - 1];
                    systemLanguageLocaleIdentifiers[index] = other_system_language_locale_identifiers;
                    systemLanguageLocaleIdentifiers[index - 1] = system_language_locale_identifiers;
                    system_language_locale_identifiers = other_system_language_locale_identifiers;
                }
                GUI.enabled = is_enabled && (index < (systemLanguageLocaleIdentifiers.Count - 1));
                if (GUILayout.Button("v", GUILayout.Width(20.0f)) && (index < (systemLanguageLocaleIdentifiers.Count - 1)))
                {
                    ISystemLanguageLocaleIdentifier other_system_language_locale_identifiers = systemLanguageLocaleIdentifiers[index + 1];
                    systemLanguageLocaleIdentifiers[index] = other_system_language_locale_identifiers;
                    systemLanguageLocaleIdentifiers[index + 1] = system_language_locale_identifiers;
                    system_language_locale_identifiers = other_system_language_locale_identifiers;
                }
                GUI.enabled = is_enabled;
                system_language_locale_identifiers.LocaleName = EditorGUILayout.TextField($"{ system_language_locale_identifiers.LocaleIdentifier.CultureInfo.DisplayName } ({ system_language_locale_identifiers.LocaleIdentifier.CultureInfo.Name })", system_language_locale_identifiers.LocaleName);
                Color original_background_color = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(20.0f)))
                {
                    systemLanguageLocaleIdentifiers.RemoveAt(index);
                    --index;
                }
                GUI.backgroundColor = original_background_color;
                EditorGUILayout.EndHorizontal();
            }
            selected_tab.Draw();
            EditorGUILayout.EndScrollView();
        }
    }
}
