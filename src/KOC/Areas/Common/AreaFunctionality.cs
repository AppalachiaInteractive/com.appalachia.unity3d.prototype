using Appalachia.Core.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application;

namespace Appalachia.Prototype.KOC.Areas.Common
{
    [CallStaticConstructorInEditor]
    [SmartLabelChildren]
    public abstract class AreaFunctionality<TFunctionality, TFunctionalityMetadata, TAreaManager,
                                            TAreaMetadata> : ApplicationFunctionality<TFunctionality,
        TFunctionalityMetadata>
        where TFunctionality : AreaFunctionality<TFunctionality, TFunctionalityMetadata, TAreaManager,
            TAreaMetadata>
        where TFunctionalityMetadata : AreaFunctionalityMetadata<TFunctionality, TFunctionalityMetadata,
            TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
