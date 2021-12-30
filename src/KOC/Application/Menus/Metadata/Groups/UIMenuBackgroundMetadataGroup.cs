using Appalachia.Prototype.KOC.Application.Menus.Components;
using Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Menus.Metadata.Groups
{
    public class UIMenuBackgroundMetadataGroup : UIElementMetadataGroupBase<UIMenuBackgroundMetadataGroup,
        UIMenuBackgroundMetadata, UIMenuBackgroundComponentSet>
    {
        protected override void InitializeElement(UIMenuBackgroundMetadata element)
        {
            element.color = Color.white;
            element.anchorMin = Vector2.zero;
            element.anchorMax = Vector2.one;
        }
    }
}
