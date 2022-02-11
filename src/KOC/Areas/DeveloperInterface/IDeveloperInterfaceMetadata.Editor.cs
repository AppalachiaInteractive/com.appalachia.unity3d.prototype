#if UNITY_EDITOR
using Appalachia.UI.Controls.Sets.DesignTemplate;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public partial interface IDeveloperInterfaceMetadata
    {
        public DesignTemplateComponentSetData UnscaledTemplates { get; }
    }
}

#endif