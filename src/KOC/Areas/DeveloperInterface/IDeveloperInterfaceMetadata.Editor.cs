using Appalachia.UI.Controls.Sets2.DesignTemplate;

#if UNITY_EDITOR
namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public partial interface IDeveloperInterfaceMetadata
    {
        public DesignTemplateComponentSetData.Optional UnscaledTemplates { get; }
    }
}

#endif