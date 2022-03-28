using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Simple;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Widgets
{
    public class CursorWidgetMetadata : LifetimeWidgetMetadata<CursorWidget, CursorWidgetMetadata, CursorFeature,
        CursorFeatureMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField] public SimpleCursorControlConfig.List simpleCursors;

        [SerializeField] public ComplexCursorControlConfig.List complexCursors;

        #endregion

        /// <inheritdoc />
        protected override void UnsuspendResponsiveComponents(CursorWidget target)
        {
            using (_PRF_UnsuspendResponsiveComponents.Auto())
            {
                base.UnsuspendResponsiveComponents(target);

                foreach (var simpleCursor in simpleCursors)
                {
                    simpleCursor.UnsuspendChanges();
                }

                foreach (var complexCursor in complexCursors)
                {
                    complexCursor.UnsuspendChanges();
                }
            }
        }

        /// <inheritdoc />
        protected override void SuspendResponsiveComponents(CursorWidget target)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
                base.SuspendResponsiveComponents(target);

                foreach (var simpleCursor in simpleCursors)
                {
                    simpleCursor.SuspendChanges();
                }

                foreach (var complexCursor in complexCursors)
                {
                    complexCursor.SuspendChanges();
                }
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(CursorWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                foreach (var simpleCursor in simpleCursors)
                {
                    simpleCursor.SubscribeToChanges(OnChanged);
                }

                foreach (var complexCursor in complexCursors)
                {
                    complexCursor.SubscribeToChanges(OnChanged);
                }
            }
        }

        /// <inheritdoc />
        protected override void OnApply(CursorWidget widget)
        {
            using (_PRF_Apply.Auto())
            {
                base.OnApply(widget);

                for (var index = 0; index < widget.ComplexCursors.Count; index++)
                {
                    var control = widget.ComplexCursors[index];
                    var config = complexCursors[index];

                    ComplexCursorControlConfig.Refresh(ref config, widget.Metadata);
                    complexCursors[index] = config;
                    
                    config.Apply(control);
                }

                for (var index = 0; index < widget.SimpleCursors.Count; index++)
                {
                    var control = widget.SimpleCursors[index];
                    var config = simpleCursors[index];

                    SimpleCursorControlConfig.Refresh(ref config, widget.Metadata);
                    simpleCursors[index] = config;
                    
                    config.Apply(control);
                }
            }
        }
    }
}
