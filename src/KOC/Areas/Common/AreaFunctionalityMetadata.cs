using Appalachia.Prototype.KOC.Application;

namespace Appalachia.Prototype.KOC.Areas.Common
{
    public abstract class AreaFunctionalityMetadata<TFunctionality, TFunctionalityMetadata, TAreaManager,
                                                    TAreaMetadata> : ApplicationFunctionalityMetadata<
        TFunctionality, TFunctionalityMetadata>
        where TFunctionality : AreaFunctionality<TFunctionality, TFunctionalityMetadata, TAreaManager,
            TAreaMetadata>
        where TFunctionalityMetadata : AreaFunctionalityMetadata<TFunctionality, TFunctionalityMetadata,
            TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
