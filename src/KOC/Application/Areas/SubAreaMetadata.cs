using Appalachia.Prototype.KOC.Application.Areas.Base;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    public abstract class SubAreaMetadata<TSAMD> : AreaMetadataBase<TSAMD>
        where TSAMD : SubAreaMetadata<TSAMD>
    {
    }
}