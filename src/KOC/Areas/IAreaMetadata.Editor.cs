#if UNITY_EDITOR
using Appalachia.UI.Controls.Sets.DesignTemplate;

namespace Appalachia.Prototype.KOC.Areas
{
    public partial interface IAreaMetadata
    {
        public DesignTemplateComponentSetData Templates { get; }
    }
}

#endif