namespace Appalachia.Prototype.KOC.Application
{
    public interface IApplicationFunctionalityMetadata<in TFunctionality>
    {
        void Apply(TFunctionality functionality);
    }
}
