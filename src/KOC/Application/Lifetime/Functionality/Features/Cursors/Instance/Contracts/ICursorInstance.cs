using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Drivers.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.State.Contracts;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Instance.Contracts
{
    public interface ICursorInstance
    {
        ICursorDriver Driver { get; }
        IReadLimitedWriteCursorInstanceStateData StateData { get; }
        void ApplyDrivenData(float deltaTime);
        void SetDriver(ICursorDriver driver);
        void UpdateForRendering();
    }
}
