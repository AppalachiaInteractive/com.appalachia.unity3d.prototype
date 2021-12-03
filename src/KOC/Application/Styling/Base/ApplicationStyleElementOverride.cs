using System;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Styling.Base
{
    [Serializable]
    public abstract class
        ApplicationStyleElementOverride<TDefault, TOverride, TInterface> : ApplicationStyleElement<TInterface>
        where TDefault : ApplicationStyleElementDefault<TDefault, TOverride, TInterface>, TInterface
        where TOverride : ApplicationStyleElementOverride<TDefault, TOverride, TInterface>, TInterface
        where TInterface : IApplicationStyle
    {
        public TDefault Defaults
        {
            get
            {
                var lookup = ApplicationStyleElementDefaultLookup.instance;

                return lookup.Get<TDefault, TOverride, TInterface>();
            }
        }

        [ButtonGroup(GROUP_BUTTONS)]
        public abstract void SyncWithDefault();

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                Defaults.RegisterOverride(this as TOverride);
                
                this.MarkAsModified();
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
