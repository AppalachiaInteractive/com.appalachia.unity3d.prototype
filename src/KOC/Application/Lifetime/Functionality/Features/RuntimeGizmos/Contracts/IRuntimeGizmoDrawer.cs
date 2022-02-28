using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Events;
using Drawing;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Contracts
{
    public interface IRuntimeGizmoDrawer : IEnableNotifier
    {
        void DrawGizmos(AppaEvent<CommandBuilder>.Args drawArgs);
    }
}
