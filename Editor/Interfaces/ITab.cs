/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// An interface that represents a tab
    /// </summary>
    public interface ITab
    {
        /// <summary>
        /// Tab name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Draws this tab
        /// </summary>
        void Draw();
    }
}
