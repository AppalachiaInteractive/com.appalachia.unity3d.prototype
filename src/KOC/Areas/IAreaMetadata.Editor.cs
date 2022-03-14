using Appalachia.UI.Functionality.Design.Controls.Template;

#if UNITY_EDITOR
namespace Appalachia.Prototype.KOC.Areas
{
    public partial interface IAreaMetadata
    {
        public TemplateControlConfig Templates { get; }
    }
}

#endif