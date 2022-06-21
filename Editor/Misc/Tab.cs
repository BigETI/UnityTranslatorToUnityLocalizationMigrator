using System;

/// <summary>
/// Unity translator to Unity localization migrator editor namespace
/// </summary>
namespace UnityTranslatorToUnityLocalizationMigratorEditor
{
    /// <summary>
    /// A structure that describes a tab
    /// </summary>
    internal readonly struct Tab : ITab
    {
        /// <summary>
        /// Gets invoked when tab needs to be drawn
        /// </summary>
        private readonly Action onDrawTab;

        /// <summary>
        /// Tab name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// COnstructs a new tab
        /// </summary>
        /// <param name="name">Tab name</param>
        /// <param name="onDrawTab">Gets invoked when tab needs to be drawn</param>
        /// <exception cref="ArgumentNullException">When "name" or "onDrawTab" are "null"</exception>
        public Tab(string name, Action onDrawTab)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.onDrawTab = onDrawTab ?? throw new ArgumentNullException(nameof(onDrawTab));
        }

        /// <summary>
        /// Draws this tab
        /// </summary>
        public void Draw() => onDrawTab();
    }
}
