using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Widgets;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Complex;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Simple;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Widgets
{
    public class CursorWidgetMetadata : LifetimeWidgetMetadata<CursorWidget, CursorWidgetMetadata,
        CursorFeature, CursorFeatureMetadata>
    {
        #region Fields and Autoproperties

        public SimpleCursorComponentSetData simpleCursorSet;
        public ComplexCursorComponentSetData complexCursorSet;

        #endregion

        protected override void SubscribeResponsiveComponents(CursorWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                simpleCursorSet.Changed.Event += OnChanged;
                complexCursorSet.Changed.Event += OnChanged;
            }
        }

        protected override void UpdateFunctionality(CursorWidget widget)
        {
            using (_PRF_UpdateFunctionality.Auto())
            {
                base.UpdateFunctionality(widget);

                for (var setIndex = 0; setIndex < widget.ComplexCursorSets.Count; setIndex++)
                {
                    var componentSet = widget.ComplexCursorSets[setIndex];

                    UpdateComponentSet(ref componentSet, ref complexCursorSet, widget);
                }

                for (var setIndex = 0; setIndex < widget.SimpleCursorSets.Count; setIndex++)
                {
                    var componentSet = widget.SimpleCursorSets[setIndex];

                    UpdateComponentSet(ref componentSet, ref simpleCursorSet, widget);
                }
            }
        }
    }
}
