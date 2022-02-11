namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts
{
    public partial interface IReadOnlyCursorInstanceStateData
    {
        bool Animate { get; }
        bool AnimateMovement { get; }
        bool AnimateState { get; }
        float AnimationMovementDuration { get; }
        float AnimationRadius { get; }
        float AnimationStateChangeDuration { get; }
    }
}
