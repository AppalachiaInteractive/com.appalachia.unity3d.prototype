using System;
using Appalachia.Core.Attributes.Editing;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class DevTooltipComponentSetData : BaseDevTooltipComponentSetData<DevTooltipComponentSet,
        DevTooltipComponentSetData, IDevTooltipComponentSetData>
    {
        /*public void Transfer(Sets.DevTooltipComponentSetData other)
        {
            if (other == null) return;
            
            RectTransform = other.RectTransform;
            CanvasGroup = other.CanvasGroup;
            Background = other.Background;
            TriangleParent = other.TriangleParent;
            TriangleBackground = other.TriangleBackground;
            TriangleForeground = other.TriangleForeground;
            TooltipText = other.TooltipText;
            other.dataTransferred = true; 
            other.MarkAsModified();
        }*/
    }
}
