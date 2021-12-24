using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.UI
{
    public interface IModifiableState
    {
        void SetState(ModuleState newState, bool silentUpdate);
    }
}
