using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.UI
{
    public interface IMovable
    {
        /// <summary>
        ///     Sets the position of the module.
        /// </summary>
        /// <param name="newModulePosition">
        ///     The new position of the module.
        /// </param>
        void SetPosition(ModulePosition newModulePosition);
    }
}
