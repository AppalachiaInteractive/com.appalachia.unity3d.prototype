using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Appalachia.UI.Functionality.Tooltips.Components;
using Appalachia.UI.Functionality.Tooltips.Controls.Default.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.Tooltips.Widgets
{
    [CallStaticConstructorInEditor]
    public abstract class AreaTooltipsWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
                                             TAreaMetadata> : AreaWidget<TWidget, TWidgetMetadata, TFeature,
        TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TWidget : AreaTooltipsWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>
        where TWidgetMetadata : AreaTooltipsWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TFeature : AreaTooltipsFeature<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>
        where TFeatureMetadata : AreaTooltipsFeatureMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        static AreaTooltipsWidget()
        {
        }

        #region Fields and Autoproperties

        private Dictionary<AppaTooltipTarget, ITooltipControl> _controlsByTarget;

        private List<ITooltipControl> _managedTooltips;

        #endregion

        public IReadOnlyList<ITooltipControl> ManagedControls => _managedTooltips;

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
                DiscoverManagerTooltips();
            }
        }

        private void DiscoverManagerTooltips()
        {
            using (_PRF_DiscoverManagerTooltips.Auto())
            {
                Manager.ForEachControl(
                    control =>
                    {
                        if (control is (ITooltipControl tc))
                        {
                            RegisterTooltip(tc);
                        }
                    }
                );
            }
        }

        
        private void RegisterTooltip(ITooltipControl tooltip)
        {
            using (_PRF_RegisterTooltip.Auto())
            {
                _controlsByTarget ??= new();
                _managedTooltips ??= new();

                if (!_controlsByTarget.TryAdd(tooltip.Target, tooltip))
                {
                    var existing = _controlsByTarget[tooltip.Target];

                    if (existing != tooltip)
                    {
                        _managedTooltips.Remove(existing);

                        existing.DestroySafely(true);

                        _controlsByTarget[tooltip.Target] = tooltip;
                    }
                }

                if (!_managedTooltips.Contains(tooltip))
                {
                    _managedTooltips.Add(tooltip);
                }

                var tooltipObject = tooltip.GameObject;

                tooltipObject.SetParentTo(instance.canvas.ChildContainer);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_DiscoverManagerTooltips =
            new ProfilerMarker(_PRF_PFX + nameof(DiscoverManagerTooltips));

        private static readonly ProfilerMarker _PRF_RegisterTooltip =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterTooltip));

        #endregion
    }
}
