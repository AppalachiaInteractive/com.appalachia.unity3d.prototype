using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets2.Complex;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets2.Simple;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Widgets
{
    public class CursorWidgetMetadata : LifetimeWidgetMetadata<CursorWidget, CursorWidgetMetadata,
        CursorFeature, CursorFeatureMetadata>
    {
        #region Fields and Autoproperties

        public SimpleCursorComponentSetData simpleCursorSet;
        public ComplexCursorComponentSetData complexCursorSet;

        public SimpleCursorComponentSetData simpleCursorSet2;
        public ComplexCursorComponentSetData complexCursorSet2;
        
        #endregion

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

                    RefreshAndUpdate(ref componentSet, ref complexCursorSet, widget);
                }

                for (var setIndex = 0; setIndex < widget.SimpleCursorSets.Count; setIndex++)
                {
                    var componentSet = widget.SimpleCursorSets[setIndex];

                    RefreshAndUpdate(ref componentSet, ref simpleCursorSet, widget);
                }

                complexCursorSet = complexCursorSet2;
                simpleCursorSet = simpleCursorSet2;
            }
        }
    }
}
