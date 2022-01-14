using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components;
using Appalachia.Prototype.KOC.Components.Styling;

namespace Appalachia.Prototype.KOC.Behaviours
{
    public abstract class AppalachiaApplicationBehaviour<T> : AppalachiaBehaviour<T>
        where T : AppalachiaApplicationBehaviour<T>
    {
        static AppalachiaApplicationBehaviour()
        {
            RegisterDependency<LifetimeComponentManager>(i => _lifetimeComponentManager = i);
            RegisterDependency<ApplicationStyleElementDefaultLookup>(
                i => _applicationStyleElementDefaultLookup = i
            );
        }

        #region Static Fields and Autoproperties

        private static ApplicationStyleElementDefaultLookup _applicationStyleElementDefaultLookup;

        private static LifetimeComponentManager _lifetimeComponentManager;

        #endregion

        protected static ApplicationStyleElementDefaultLookup ApplicationStyleElementDefaultLookup =>
            _applicationStyleElementDefaultLookup;

        protected static LifetimeComponentManager LifetimeComponentManager => _lifetimeComponentManager;
        protected static LifetimeComponents LifetimeComponents => _lifetimeComponentManager.Components;
    }
}
