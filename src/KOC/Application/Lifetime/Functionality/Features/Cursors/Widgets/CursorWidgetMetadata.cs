using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets.Complex;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets.Simple;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Widgets
{
    public class CursorWidgetMetadata : LifetimeWidgetMetadata<CursorWidget, CursorWidgetMetadata,
        CursorFeature, CursorFeatureMetadata>
    {
        [SerializeField] public SimpleCursorComponentSetData simpleCursorSet;
        [SerializeField] public ComplexCursorComponentSetData complexCursorSet;

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(CursorWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                simpleCursorSet.Changed.Event += OnChanged;
                complexCursorSet.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(CursorWidget widget)
        {
            using (_PRF_UpdateFunctionality.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                for (var setIndex = 0; setIndex < widget.ComplexCursorSets.Count; setIndex++)
                {
                    var componentSet = widget.ComplexCursorSets[setIndex];

                    RefreshAndApply(ref componentSet, ref complexCursorSet, widget);
                }

                for (var setIndex = 0; setIndex < widget.SimpleCursorSets.Count; setIndex++)
                {
                    var componentSet = widget.SimpleCursorSets[setIndex];

                    RefreshAndApply(ref componentSet, ref simpleCursorSet, widget);
                }
            }
        }
    }
}
