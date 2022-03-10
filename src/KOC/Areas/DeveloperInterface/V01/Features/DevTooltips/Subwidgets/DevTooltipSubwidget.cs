using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Contracts;
using Appalachia.UI.Controls.Components.Buttons;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Events;
using Appalachia.Utility.Extensions;
using Drawing;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets
{
    public sealed class DevTooltipSubwidget :
        DeveloperInterfaceManager_V01.InstancedSubwidget<DevTooltipSubwidget, DevTooltipSubwidgetMetadata,
            IDevTooltipSubwidget, IDevTooltipSubwidgetMetadata, DevTooltipsWidget, DevTooltipsWidgetMetadata,
            DevTooltipsFeature, DevTooltipsFeatureMetadata>,
        IDevTooltipSubwidget,
        IRectVisualizer,
        IActivable
    {
        #region Fields and Autoproperties

        [SerializeField]
        [ReadOnly]
        public DevTooltipComponentSet componentSet;

        [SerializeField]
        [TextArea]
        [ReadOnly]
        private string _currentTooltip;

        [SerializeField]
        [ReadOnly]
        private AppaButton _currentTarget;

        [SerializeField]
        [ReadOnly]
        private DevTooltipStyleOverride _currentStyle;

        public Vector2 calculatedTooltipReferencePoint;
        public Vector2 calculatedTooltipOffset;
        public Vector2 calculatedTooltipFinalPosition;
        public Vector2 calculatedTargetCenter;

        #endregion

        public AppaButton CurrentTarget => _currentTarget;

        public IDevTooltipStyle CurrentStyle => _currentStyle;

        public string CurrentTooltip => _currentTooltip;

        public override void OnClicked()
        {
            using (_PRF_OnClicked.Auto())
            {
            }
        }

        public void SetCurrentStyle(DevTooltipStyleOverride newStyle)
        {
            using (_PRF_SetCurrentStyle.Auto())
            {
                if (_currentStyle == newStyle)
                {
                    return;
                }

                _currentStyle = newStyle;
                OnChanged();
            }
        }

        public void SetCurrentTarget(AppaButton newTarget)
        {
            using (_PRF_UpdateCurrentTarget.Auto())
            {
                if (_currentTarget == newTarget)
                {
                    return;
                }

                _currentTarget = newTarget;
                OnChanged();
            }
        }

        public void SetCurrentTooltip(string newTooltip)
        {
            using (_PRF_UpdateCurrentTooltip.Auto())
            {
                if (_currentTooltip == newTooltip)
                {
                    return;
                }

                _currentTooltip = newTooltip;
                OnChanged();
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(metadata),
                    metadata == null,
                    () =>
                    {
                        if (metadata == null)
                        {
                            var metadataName = gameObject.GetFullNameForFile();
                            metadata = AppalachiaObject.LoadOrCreateNew<DevTooltipSubwidgetMetadata>(
                                metadataName,
                                ownerType: AppalachiaRepository.PrimaryOwnerType
                            );
                        }
                    }
                );
            }
        }

        #region IDevTooltipSubwidget Members

        public void Deactivate()
        {
            using (_PRF_Deactivate.Auto())
            {
                if (gameObject.activeSelf)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                if (!gameObject.activeSelf)
                {
                    gameObject.SetActive(true);
                }
            }
        }

        public void Toggle()
        {
            using (_PRF_Toggle.Auto())
            {
                if (!IsActive)
                {
                    Activate();
                }
                else
                {
                    Deactivate();
                }
            }
        }

        public bool IsActive => (DevTooltipSubwidget)Widget.ActiveSubwidget == this;

        #endregion

        #region IRectVisualizer Members

        public void VisualizeRectangles(AppaEvent<CommandBuilder>.Args drawArgs)
        {
            using (_PRF_VisualizeRectangles.Auto())
            {
                var draw = drawArgs.value;
                var alpha = 1f;
                var targetColor = Color.green.ScaleA(alpha);
                var arrowColor = Color.cyan.ScaleA(alpha);

                using (draw.WithLineWidth(2f))
                {
                    draw.Line(calculatedTargetCenter, calculatedTooltipReferencePoint, targetColor);

                    draw.Line(calculatedTooltipReferencePoint, calculatedTooltipFinalPosition, arrowColor);
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Deactivate = new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        private static readonly ProfilerMarker _PRF_SetCurrentStyle =
            new ProfilerMarker(_PRF_PFX + nameof(SetCurrentStyle));

        private static readonly ProfilerMarker _PRF_UpdateCurrentTarget =
            new ProfilerMarker(_PRF_PFX + nameof(SetCurrentTarget));

        private static readonly ProfilerMarker _PRF_UpdateCurrentTooltip =
            new ProfilerMarker(_PRF_PFX + nameof(SetCurrentTooltip));

        private static readonly ProfilerMarker _PRF_VisualizeRectangles =
            new ProfilerMarker(_PRF_PFX + nameof(VisualizeRectangles));

        #endregion

        protected override bool ShowsTooltip => false;
        
        public override string GetDevTooltipText()
        {
            throw new System.NotImplementedException();
        }

        protected override AppaButton GetTooltipTarget()
        {
            throw new System.NotImplementedException();
        }
    }
}
