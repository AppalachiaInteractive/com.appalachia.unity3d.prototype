using System;
using Appalachia.Core.Attributes.Editing;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets2.Complex
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
   
    public sealed class ComplexCursorComponentSetData : BaseComplexCursorComponentSetData<ComplexCursorComponentSet,
        ComplexCursorComponentSetData, IComplexCursorComponentSetData>
    {
        /*public void Transfer(Sets.Complex.ComplexCursorComponentSetData other)
        {
            if (other == null) return;

            RectTransform = other.RectTransform;
            AnimatorData = other.AnimatorData;
            ComponentDataList = other.ComponentDataList;
            CanvasFadeManager = other.CanvasFadeManager;
            CanvasGroup = other.CanvasGroup;
            GraphicRaycaster = other.GraphicRaycaster;
            Canvas = other.Canvas;
            other.dataTransferred = true; 
            other.MarkAsModified();
        }*/
    }
}
