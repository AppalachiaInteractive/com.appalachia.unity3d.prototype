using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Styling.Base
{
    [CallStaticConstructorInEditor]
    [Serializable]
    public abstract class
        ApplicationStyleElementOverride<TDefault, TOverride, TInterface> : ApplicationStyleElement<TOverride,
            TInterface>
        where TDefault : ApplicationStyleElementDefault<TDefault, TOverride, TInterface>, TInterface
        where TOverride : ApplicationStyleElementOverride<TDefault, TOverride, TInterface>, TInterface
        where TInterface : IApplicationStyle
    {
        static ApplicationStyleElementOverride()
        {
            RegisterDependency<ApplicationStyleElementDefaultLookup>(
                i => _applicationStyleElementDefaultLookup = i
            );
        }

        #region Static Fields and Autoproperties

        private static ApplicationStyleElementDefaultLookup _applicationStyleElementDefaultLookup;

        #endregion

        public TDefault Defaults
        {
            get
            {
                if (!DependenciesAreReady || !FullyInitialized)
                {
                    throw new NotSupportedException(
                        ZString.Format(
                            "Can not lookup defaults for style before dependencies are ready.  Add a dependency on {0}!",
                            nameof(ApplicationStyleElementDefaultLookup)
                        )
                    );
                }

                return _applicationStyleElementDefaultLookup.Get<TDefault, TOverride, TInterface>();
            }
        }

        [ButtonGroup(GROUP_BUTTONS)]
        public abstract void SyncWithDefault();

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                Defaults.RegisterOverride(this as TOverride);

                MarkAsModified();
            }
        }

        #region Profiling

        private const string _PRF_PFX =
            nameof(ApplicationStyleElementOverride<TDefault, TOverride, TInterface>) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
