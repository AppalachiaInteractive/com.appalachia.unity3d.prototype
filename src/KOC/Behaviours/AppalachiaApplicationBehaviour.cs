using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Lifetime;
using Appalachia.UI.Styling;

namespace Appalachia.Prototype.KOC.Behaviours
{
    public abstract class AppalachiaApplicationBehaviour<T> : AppalachiaBehaviour<T>
        where T : AppalachiaApplicationBehaviour<T>
    {
        static AppalachiaApplicationBehaviour()
        {
            RegisterDependency<LifetimeComponentManager>(i => _lifetimeComponentManager = i);
            RegisterDependency<StyleElementDefaultLookup>(i => _styleLookup = i);
        }

        #region Static Fields and Autoproperties

        private static LifetimeComponentManager _lifetimeComponentManager;

        private static StyleElementDefaultLookup _styleLookup;

        #endregion

        protected static LifetimeComponentManager LifetimeComponentManager => _lifetimeComponentManager;

        protected static StyleElementDefaultLookup StyleLookup => _styleLookup;
    }
}
