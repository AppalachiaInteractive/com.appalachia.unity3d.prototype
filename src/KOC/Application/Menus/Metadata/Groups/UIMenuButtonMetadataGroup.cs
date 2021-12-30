using Appalachia.Prototype.KOC.Application.Menus.Components;
using Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Menus.Metadata.Groups
{
    public class UIMenuButtonMetadataGroup : UIElementMetadataGroupBase<UIMenuButtonMetadataGroup,
        UIMenuButtonMetadata, UIMenuButtonComponentSet>
    {
        protected override void InitializeElement(UIMenuButtonMetadata element)
        {
            using (_PRF_InitializeElement.Auto())
            {
                element.text = "Button";
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIMenuButtonMetadataGroup) + ".";

        private static readonly ProfilerMarker _PRF_InitializeElement =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeElement));

        #endregion
    }
}
