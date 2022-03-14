using Appalachia.UI.Functionality.Design.Controls.Template;

#if UNITY_EDITOR
namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public partial interface IDeveloperInterfaceMetadata
    {
        public TemplateControlConfig.Optional UnscaledTemplates { get; }
    }
}

#endif