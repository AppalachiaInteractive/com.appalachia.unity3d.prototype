using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.UI.Functionality.Canvas.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Images.Groups.Default;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Simple.Contracts
{
    public interface ISimpleCursorControlConfig : ICanvasControlConfig
    {
        ImageComponentGroupConfig Image { get; }
        SimpleCursorMetadata Metadata { get; }
    }
}
