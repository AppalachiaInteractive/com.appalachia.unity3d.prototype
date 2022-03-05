namespace Appalachia.Prototype.KOC.Application.Features.Contracts
{
    public interface IActivable
    {
        void Deactivate();
        void Activate();
        void Toggle();
        bool IsActive { get; }
    }
}