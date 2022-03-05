using Appalachia.UI.Controls.Sets2.DesignTemplate;

#if UNITY_EDITOR
namespace Appalachia.Prototype.KOC.Areas
{
    public partial interface IAreaMetadata
    {
        public DesignTemplateComponentSetData Templates { get; }
    }
}

#endif