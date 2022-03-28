#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core
{
    public abstract partial class StatusBarSubwidget<TSubwidget, TSubwidgetMetadata>
    {
        #region Constants and Static Readonly

        private const string NAVIGATION_BUTTON_GROUP = nameof(NAVIGATION_BUTTON_GROUP);

        #endregion
    }
}

#endif
