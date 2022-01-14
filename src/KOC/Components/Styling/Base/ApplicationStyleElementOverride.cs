using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Components.Styling.Base
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
        public TDefault Defaults
        {
            get
            {
                if (!DependenciesAreReady || (ApplicationStyleElementDefaultLookup == null))
                {
                    throw new NotSupportedException(
                        ZString.Format(
                            "Can not lookup defaults for style before dependencies are ready.  Add a dependency on {0}!",
                            nameof(ApplicationStyleElementDefaultLookup)
                        )
                    );
                }

                return ApplicationStyleElementDefaultLookup.Get<TDefault, TOverride, TInterface>();
            }
        }

        [ButtonGroup(GROUP_BUTTONS)]
        public abstract void SyncWithDefault();

        protected abstract void RegisterOverrideSubscriptions();

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            Defaults.RegisterOverride(this as TOverride);

            RegisterOverrideSubscriptions();

            MarkAsModified();
        }
    }
}
