using Appalachia.UI.Functionality.Animation;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.MultiPartCanvas.Contracts;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex.Contracts
{
    public interface IComplexCursorControlConfig : IMultiPartCanvasControlConfig<
        ImageComponentGroup, ImageComponentGroup.List, ImageComponentGroupConfig, ImageComponentGroupConfig.List>
    {
        AnimatorConfig AnimatorConfig { get; }
    }
}
