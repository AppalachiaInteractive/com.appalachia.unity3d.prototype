using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Simple;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Widgets
{
    public class CursorWidgetMetadata : LifetimeWidgetMetadata<CursorWidget, CursorWidgetMetadata,
        CursorFeature, CursorFeatureMetadata>
    {
        [FormerlySerializedAs("simpleCursorSet")] [SerializeField] public SimpleCursorControlConfig simpleCursorControl;
        [FormerlySerializedAs("complexCursorSet")] [SerializeField] public ComplexCursorControlConfig complexCursorControl;

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(CursorWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                simpleCursorControl.Changed.Event += OnChanged;
                complexCursorControl.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(CursorWidget widget)
        {
            using (_PRF_UpdateFunctionality.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                for (var setIndex = 0; setIndex < widget.ComplexCursorControls.Count; setIndex++)
                {
                    var control = widget.ComplexCursorControls[setIndex];

                    RefreshAndApply(ref control, ref complexCursorControl, widget);
                }

                for (var setIndex = 0; setIndex < widget.SimpleCursorControls.Count; setIndex++)
                {
                    var control = widget.SimpleCursorControls[setIndex];

                    RefreshAndApply(ref control, ref simpleCursorControl, widget);
                }
            }
        }
    }
}
