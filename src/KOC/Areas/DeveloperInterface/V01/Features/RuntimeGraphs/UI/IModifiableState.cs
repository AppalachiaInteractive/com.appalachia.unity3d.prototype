using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Settings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.UI
{
    public interface IModifiableState
    {
        void SetState(ModuleState newState, bool silentUpdate);
    }
}
