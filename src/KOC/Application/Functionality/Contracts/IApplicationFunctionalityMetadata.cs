namespace Appalachia.Prototype.KOC.Application.Functionality.Contracts
{
    public interface IApplicationFunctionalityMetadata<in TFunctionality>
    {
        void UpdateFunctionality(TFunctionality functionality);
    }
}
