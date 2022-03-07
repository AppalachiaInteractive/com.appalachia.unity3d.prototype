namespace Appalachia.Prototype.KOC.Application.Features.Contracts
{
    public interface IReleasable
    {
        bool NotReadyForRelease { get; }
    }
}
