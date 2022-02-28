using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Settings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.UI
{
    public interface IModifiableState
    {
        void SetState(ModuleState newState, bool silentUpdate);
    }
}
