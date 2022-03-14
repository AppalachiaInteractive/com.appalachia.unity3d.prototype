using Appalachia.UI.Functionality.Canvas.Controls.Unscaled;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public interface IDeveloperInterfaceManager : IAreaManager
    {
        public UnscaledCanvasControl UnscaledCanvas { get; }
    }
}
