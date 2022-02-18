#if UNITY_EDITOR
using System.Linq;
using Appalachia.CI.Constants;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features
{
    [ExecuteAlways]
    public abstract partial class ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
                                                     TIWidget, TManager>
    {
        private bool _jumpToServicesEnabled => ServiceCount > 0;
        private bool _jumpToWidgetsEnabled => WidgetCount > 0;

        [ButtonGroup(APPASTR.Enable_Feature)]
        [GUIColor(nameof(DisableColor))]
        [PropertyOrder(-3)]
        [LabelText(APPASTR.Disable_Feature)]
        private void DisableFeatureButton()
        {
            using (_PRF_DisableFeatureButton.Auto())
            {
                DisableFeature().Forget();
            }
        }

        [ButtonGroup(APPASTR.Enable_Feature)]
        [GUIColor(nameof(EnableColor))]
        [PropertyOrder(-4)]
        [LabelText(APPASTR.Enable_Feature)]
        private void EnableFeatureButton()
        {
            using (_PRF_EnableFeatureButton.Auto())
            {
                EnableFeature().Forget();
            }
        }

        [ButtonGroup(FEATURE_BUTTON_GROUP)]
        [GUIColor(nameof(NavigationColor))]
        [EnableIf(nameof(_jumpToServicesEnabled))]
        [LabelText(APPASTR.Jump_To_Services)]
        private void JumpToServicesButton()
        {
            using (_PRF_JumpToServicesButton.Auto())
            {
                if (ServiceCount > 1)
                {
                    Selection.activeGameObject = ServiceParentObject;
                }
                else
                {
                    Selection.activeGameObject = _functionalitySet.Services.First().GameObject;
                }
            }
        }

        [ButtonGroup(FEATURE_BUTTON_GROUP)]
        [GUIColor(nameof(NavigationColor))]
        [EnableIf(nameof(_jumpToWidgetsEnabled))]
        [LabelText(APPASTR.Jump_To_Widgets)]
        private void JumpToWidgetsButton()
        {
            using (_PRF_JumpToWidgetsButton.Auto())
            {
                if (WidgetCount > 1)
                {
                    Selection.activeGameObject = WidgetParentObject;
                }
                else
                {
                    Selection.activeGameObject = _functionalitySet.Widgets.First().GameObject;
                }
            }
        }

        [ButtonGroup(APPASTR.Hide)]
        [GUIColor(nameof(EnableColor))]
        [PropertyOrder(-2)]
        [LabelText(APPASTR.Show)]
        private void ShowFeatureButton()
        {
            using (_PRF_ShowFeatureButton.Auto())
            {
                ShowFeature().Forget();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_DisableFeatureButton =
            new ProfilerMarker(_PRF_PFX + nameof(DisableFeatureButton));

        private static readonly ProfilerMarker _PRF_EnableFeatureButton =
            new ProfilerMarker(_PRF_PFX + nameof(EnableFeatureButton));

        private static readonly ProfilerMarker _PRF_JumpToServicesButton =
            new ProfilerMarker(_PRF_PFX + nameof(JumpToServicesButton));

        private static readonly ProfilerMarker _PRF_JumpToWidgetsButton =
            new ProfilerMarker(_PRF_PFX + nameof(JumpToWidgetsButton));

        private static readonly ProfilerMarker _PRF_ShowFeatureButton =
            new ProfilerMarker(_PRF_PFX + nameof(ShowFeatureButton));

        #endregion
    }
}

#endif
