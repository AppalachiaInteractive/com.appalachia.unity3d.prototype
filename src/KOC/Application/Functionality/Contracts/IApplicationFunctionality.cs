using Appalachia.Core.Objects.Root.Contracts;

namespace Appalachia.Prototype.KOC.Application.Functionality.Contracts
{
    public interface IApplicationFunctionality : IBehaviour
    {
        void ApplyMetadata();
        void UnsubscribeFromAllFunctionalities();
    }
}
