using Appalachia.UI.Controls.Sets.UnscaledCanvas;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public interface IDeveloperInterfaceManager : IAreaManager
    {
        public UnscaledCanvasComponentSet UnscaledCanvas { get; }
    }
}
