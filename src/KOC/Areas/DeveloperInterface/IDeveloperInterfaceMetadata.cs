using Appalachia.UI.Controls.Sets.UnscaledCanvas;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public partial interface IDeveloperInterfaceMetadata : IAreaMetadata
    {
        public UnscaledCanvasComponentSetData UnscaledCanvas { get; }
    }
}
