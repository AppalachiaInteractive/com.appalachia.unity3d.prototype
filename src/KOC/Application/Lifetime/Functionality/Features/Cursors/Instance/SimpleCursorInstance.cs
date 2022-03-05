using System;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets2.Simple;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.State;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Instance
{
    public sealed class SimpleCursorInstance : CursorInstance<SimpleCursorInstance, SimpleCursorInstanceState,
        SimpleCursorMetadata, SimpleCursorComponentSet, SimpleCursorComponentSetData>
    {
        /// <inheritdoc />
        protected override void BeforeRendering()
        {
            using (_PRF_BeforeRendering.Auto())
            {
                if (stateData.CurrentVisibility)
                {
                    var color = stateData.Metadata.cursorColor.Overriding
                        ? stateData.Metadata.cursorColor.Value
                        : Color.white;

                    switch (stateData.CurrentState)
                    {
                        case CursorStates.Normal:
                            break;
                        case CursorStates.Hovering:
                            if (stateData.Metadata.hoveringColor.Overriding)
                            {
                                color *= stateData.Metadata.hoveringColor.Value;
                            }

                            break;
                        case CursorStates.Pressed:
                            if (stateData.Metadata.pressedColor.Overriding)
                            {
                                color *= stateData.Metadata.pressedColor.Value;
                            }

                            break;
                        case CursorStates.Disabled:
                            if (stateData.Metadata.disabledColor.Overriding)
                            {
                                color *= stateData.Metadata.disabledColor.Value;
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    stateData.RecordColor(color);

                    components.Image.CrossFadeColor(
                        color,
                        stateData.Metadata.cursorColorChangeDuration,
                        true,
                        true,
                        true
                    );
                }
            }
        }
    }
}
