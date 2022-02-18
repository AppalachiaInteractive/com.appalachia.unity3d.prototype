#if UNITY_EDITOR
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Services
{
    [ExecuteAlways]
    public abstract partial class ApplicationService<TService, TServiceMetadata, TFeature, TFeatureMetadata,
                                                     TFunctionalitySet, TIService, TIWidget, TManager>
    {
        private bool _jumpToWidgetsEnabled => (Feature == null ? 0 : Feature.WidgetCount) > 0;

        [ButtonGroup(FEATURE_BUTTON_GROUP)]
        [GUIColor(nameof(NavigationColor))]
        [LabelText("Jump To Feature")]
        private void JumpToFeatureButton()
        {
            using (_PRF_JumpToFeatureButton.Auto())
            {
                Selection.activeGameObject = Feature.GameObject;
            }
        }

        [ButtonGroup(FEATURE_BUTTON_GROUP)]
        [GUIColor(nameof(NavigationColor))]
        [EnableIf(nameof(_jumpToWidgetsEnabled))]
        [LabelText("Jump To Widgets")]
        private void JumpToWidgetsButton()
        {
            using (_PRF_JumpToWidgetsButton.Auto())
            {
                if (Feature.WidgetCount > 1)
                {
                    Selection.activeGameObject = Feature.WidgetParentObject;
                }
                else
                {
                    Selection.activeGameObject = Feature.Widgets.First().GameObject;
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_JumpToFeatureButton =
            new ProfilerMarker(_PRF_PFX + nameof(JumpToFeatureButton));

        private static readonly ProfilerMarker _PRF_JumpToWidgetsButton =
            new ProfilerMarker(_PRF_PFX + nameof(JumpToWidgetsButton));

        #endregion
    }
}

#endif
