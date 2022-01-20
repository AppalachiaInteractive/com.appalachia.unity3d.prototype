using Appalachia.UI.Controls.Sets.RootCanvas;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public partial interface IDeveloperInterfaceMetadata : IAreaMetadata
    {
        public RootCanvasComponentSetStyle UnscaledCanvas { get; }
    }
}
