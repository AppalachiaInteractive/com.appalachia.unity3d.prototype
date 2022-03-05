using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Settings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.UI
{
    public interface IModifiableState
    {
        void SetState(ModuleState newState, bool silentUpdate);
    }
}
