using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.UI.Controls.Sets2.Buttons.Button;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Sets2
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class
        StatusBarSubwidgetComponentSetData : BaseButtonComponentSetData<StatusBarSubwidgetComponentSet,
            StatusBarSubwidgetComponentSetData>
    {
        
        /*public void Transfer(Sets.StatusBarSubwidgetComponentSetData other)
        {
            if (other == null) return;

            this.RectTransform = other.RectTransform;
            this.AppaButton = other.AppaButton;
            this.ButtonBackground = other.ButtonBackground;
            this.ButtonIcon = other.ButtonIcon;
            this.ButtonShadow = other.ButtonShadow;
            this.ButtonText = other.ButtonText;
            this.LayoutGroup = other.LayoutGroup;
            other.dataTransferred = true; 
            other.MarkAsModified();
        } */
    }
}
