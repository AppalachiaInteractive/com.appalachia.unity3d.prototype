#if UNITY_EDITOR
using Appalachia.UI.Controls.Sets2.DesignTemplate;

namespace Appalachia.Prototype.KOC.Areas
{
    public partial interface IAreaMetadata
    {
        public DesignTemplateComponentSetData Templates { get; }
    }
}

#endif