using Appalachia.Prototype.KOC.Application.Areas.Base;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    public abstract class AreaMetadata<TAMD> : AreaMetadataBase<TAMD>
        where TAMD : AreaMetadata<TAMD>
    {
    }
}
