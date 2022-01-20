using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.Common.Widgets;
using Appalachia.UI.Controls.Extensions;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets
{
    [CallStaticConstructorInEditor]
    public abstract class
        DeveloperInterfaceWidget<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata> : AreaWidget<TWidget,
            TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TWidget : DeveloperInterfaceWidget<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TWidgetMetadata : DeveloperInterfaceWidgetMetadata<TWidget, TWidgetMetadata, TAreaManager,
            TAreaMetadata>
        where TAreaManager : DeveloperInterfaceManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : DeveloperInterfaceMetadata<TAreaManager, TAreaMetadata>
    {
        static DeveloperInterfaceWidget()
        {
            DeveloperInterfaceManager<TAreaManager, TAreaMetadata>.InstanceAvailable += i =>
            {
                i.InitializationComplete += _ => { instance.MoveWidgetToCorrectView(); };
            };
        }

        protected override void BeforeApplyMetadataInternal()
        {
            using (_PRF_BeforeApplyMetadataInternal.Auto())
            {
                base.BeforeApplyMetadataInternal();

                MoveWidgetToCorrectView();
            }
        }

        private void MoveWidgetToCorrectView()
        {
            using (_PRF_MoveWidgetToCorrectView.Auto())
            {
                var containerView = metadata.inUnscaledView
                    ? areaManager.UnscaledCanvas.GameObject
                    : areaManager.View.GameObject;

                if (transform.IsChildOf(containerView.transform))
                {
                    return;
                }

                GameObject widgetContainerObject = null;

                containerView.GetOrAddChild(ref widgetContainerObject, APPASTR.ObjectNames.Widgets, true);

                var widgetRectTransform = widgetContainerObject.GetComponent<RectTransform>();
                widgetRectTransform.Reset(RectResetOptions.All);

                gameObject.SetParentTo(widgetContainerObject);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_MoveWidgetToCorrectView =
            new ProfilerMarker(_PRF_PFX + nameof(MoveWidgetToCorrectView));

        #endregion
    }
}
