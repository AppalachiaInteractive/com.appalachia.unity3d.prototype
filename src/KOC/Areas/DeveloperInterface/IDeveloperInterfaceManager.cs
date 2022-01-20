using Appalachia.UI.Controls.Sets.RootCanvas;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public interface IDeveloperInterfaceManager : IAreaManager
    {
        public RootCanvasComponentSet UnscaledCanvas { get; }
    }
}
