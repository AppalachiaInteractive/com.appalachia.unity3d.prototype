using Appalachia.UI.Controls.Sets2.Canvases.UnscaledCanvas;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public partial interface IDeveloperInterfaceMetadata : IAreaMetadata
    {
        public UnscaledCanvasComponentSetData UnscaledCanvas { get; }
    }
}
