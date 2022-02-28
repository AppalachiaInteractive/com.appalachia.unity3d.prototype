using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Settings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.UI
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
