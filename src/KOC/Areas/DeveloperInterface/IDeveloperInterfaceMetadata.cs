using Appalachia.UI.Controls.Sets.Canvases.UnscaledCanvas;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public partial interface IDeveloperInterfaceMetadata : IAreaMetadata
    {
        public UnscaledCanvasComponentSetData UnscaledCanvas { get; }
    }
}
