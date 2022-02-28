using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Events;
using Drawing;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Contracts
{
    public interface IRectVisualizer : IEnableNotifier
    {
        void VisualizeRectangles(AppaEvent<CommandBuilder>.Args draw);
    }
}
