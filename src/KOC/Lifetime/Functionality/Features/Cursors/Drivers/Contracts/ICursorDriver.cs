using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.Contracts
{
    public interface ICursorDriver
    {
        void DriveCursorInstance(
            IReadLimitedWriteCursorInstanceStateData stateData,
            float time,
            float deltaTime,
            Rect bounds);
    }
}
