#if UNITY_EDITOR
using Appalachia.UI.Controls.Sets2.DesignTemplate;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public partial interface IDeveloperInterfaceMetadata
    {
        public DesignTemplateComponentSetData.Optional UnscaledTemplates { get; }
    }
}

#endif