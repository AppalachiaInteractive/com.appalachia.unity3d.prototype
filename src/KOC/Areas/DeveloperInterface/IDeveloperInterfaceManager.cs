using Appalachia.UI.Controls.Sets2.Canvases.UnscaledCanvas;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public interface IDeveloperInterfaceManager : IAreaManager
    {
        public UnscaledCanvasComponentSet UnscaledCanvas { get; }
    }
}
