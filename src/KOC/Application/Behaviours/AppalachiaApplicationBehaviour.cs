using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Components;

namespace Appalachia.Prototype.KOC.Application.Behaviours
{
    public abstract class AppalachiaApplicationBehaviour<T> : AppalachiaBehaviour<T>
        where T : AppalachiaApplicationBehaviour<T>
    {
        static AppalachiaApplicationBehaviour()
        {
            RegisterDependency<LifetimeComponentManager>(i => _lifetimeComponentManager = i);
        }

        #region Static Fields and Autoproperties

        private static LifetimeComponentManager _lifetimeComponentManager;

        #endregion

        protected static LifetimeComponentManager LifetimeComponentManager => _lifetimeComponentManager;
        protected static LifetimeComponents LifetimeComponents => _lifetimeComponentManager.Components;
    }
}
