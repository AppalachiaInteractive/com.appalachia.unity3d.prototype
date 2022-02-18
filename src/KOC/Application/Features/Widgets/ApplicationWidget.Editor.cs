#if UNITY_EDITOR
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Widgets
{
    [ExecuteAlways]
    public abstract partial class ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
                                                    TFunctionalitySet, TIService, TIWidget, TManager>

    {
        private bool _jumpToServicesEnabled => (Feature == null ? 0 : Feature.ServiceCount) > 0;

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
        [EnableIf(nameof(_jumpToServicesEnabled))]
        [LabelText("Jump To Services")]
        private void JumpToServicesButton()
        {
            using (_PRF_JumpToServicesButton.Auto())
            {
                if (Feature.ServiceCount > 1)
                {
                    Selection.activeGameObject = Feature.ServiceParentObject;
                }
                else
                {
                    Selection.activeGameObject = Feature.Services.First().GameObject;
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_JumpToFeatureButton =
            new ProfilerMarker(_PRF_PFX + nameof(JumpToFeatureButton));

        private static readonly ProfilerMarker _PRF_JumpToServicesButton =
            new ProfilerMarker(_PRF_PFX + nameof(JumpToServicesButton));

        #endregion
    }
}

#endif
