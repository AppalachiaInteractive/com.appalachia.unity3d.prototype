using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts
{
    public interface IReadOnlySimpleCursorInstanceStateData : IReadOnlyCursorInstanceStateData
    {
        SimpleCursors Cursor { get; }
    }
}
