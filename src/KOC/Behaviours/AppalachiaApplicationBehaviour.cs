using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components;
using Appalachia.UI.Core.Styling;

namespace Appalachia.Prototype.KOC.Behaviours
{
    public abstract class AppalachiaApplicationBehaviour<T> : AppalachiaBehaviour<T>
        where T : AppalachiaApplicationBehaviour<T>
    {
        static AppalachiaApplicationBehaviour()
        {
            RegisterDependency<LifetimeComponentManager>(i => _lifetimeComponentManager = i);
            RegisterDependency<StyleElementDefaultLookup>(
                i => _styleElementDefaultLookup = i
            );
        }

        #region Static Fields and Autoproperties

        private static StyleElementDefaultLookup _styleElementDefaultLookup;

        private static LifetimeComponentManager _lifetimeComponentManager;

        #endregion

        protected static StyleElementDefaultLookup StyleElementDefaultLookup => _styleElementDefaultLookup;

        protected static LifetimeComponentManager LifetimeComponentManager => _lifetimeComponentManager;
        protected static LifetimeComponents LifetimeComponents => _lifetimeComponentManager.Components;
    }
}
