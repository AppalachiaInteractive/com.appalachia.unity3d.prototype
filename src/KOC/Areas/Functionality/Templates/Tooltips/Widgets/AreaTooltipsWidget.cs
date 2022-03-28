using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Appalachia.UI.Functionality.Tooltips.Controls.Default;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.Tooltips.Widgets
{
    [CallStaticConstructorInEditor]
    public abstract class TooltipsWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
                                         TAreaMetadata> : AreaWidget<TWidget, TWidgetMetadata, TFeature,
        TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TWidget : TooltipsWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>
        where TWidgetMetadata : AreaTooltipsWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TFeature : TooltipsFeature<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>
        where TFeatureMetadata : TooltipsFeatureMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField] private List<TooltipControl> _managedTooltips;

        #endregion

        public IReadOnlyList<TooltipControl> ManagedControls => _managedTooltips;

        public void RegisterTooltip(TooltipControl tooltip)
        {
            using (_PRF_RegisterTooltip.Auto())
            {
                if (!_managedTooltips.Contains(tooltip))
                {
                    _managedTooltips.Add(tooltip);
                }

                var tooltipObject = tooltip.gameObject;
                
                tooltipObject.SetParentTo(canvas.ChildContainer);
            }
        }

        /// <inheritdoc />
        protected override void EnsureWidgetIsCorrectSize()
        {
            using (_PRF_EnsureWidgetIsCorrectSize.Auto())
            {
                base.EnsureWidgetIsCorrectSize();
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                base.UnsubscribeFromAllFunctionalities();
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterTooltip =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterTooltip));

        #endregion
    }
}
