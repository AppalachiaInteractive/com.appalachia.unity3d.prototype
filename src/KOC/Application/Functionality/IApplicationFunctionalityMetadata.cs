namespace Appalachia.Prototype.KOC.Application.Functionality
{
    public interface IApplicationFunctionalityMetadata<in TFunctionality>
    {
        void UpdateFunctionality(TFunctionality functionality);
    }
}
