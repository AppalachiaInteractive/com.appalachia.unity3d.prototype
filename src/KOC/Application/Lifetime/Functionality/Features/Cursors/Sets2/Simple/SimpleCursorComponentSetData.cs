using System;
using Appalachia.Core.Attributes.Editing;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets2.Simple
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class SimpleCursorComponentSetData : BaseSimpleCursorComponentSetData<SimpleCursorComponentSet,
        SimpleCursorComponentSetData, ISimpleCursorComponentSetData>
    {
        /*public void Transfer(Sets.Simple.SimpleCursorComponentSetData other)
        {
            if (other == null) return;

            RectTransform = other.RectTransform;
            ImageData = other.ImageData;
            CanvasFadeManager = other.CanvasFadeManager;
            CanvasGroup = other.CanvasGroup;
            GraphicRaycaster = other.GraphicRaycaster;
            Canvas = other.Canvas;
            other.dataTransferred = true;
            other.MarkAsModified();
        }*/
    }
}
