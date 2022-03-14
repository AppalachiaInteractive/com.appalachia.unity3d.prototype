using Appalachia.UI.Functionality.Canvas.Controls.Unscaled;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public partial interface IDeveloperInterfaceMetadata : IAreaMetadata
    {
        public UnscaledCanvasControlConfig UnscaledCanvas { get; }
    }
}
