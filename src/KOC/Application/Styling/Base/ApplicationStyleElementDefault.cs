using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Styling.Base
{
    [Serializable]
    public abstract class
        ApplicationStyleElementDefault<TDefault, TOverride, TInterface> : ApplicationStyleElement<TInterface>
        where TDefault : ApplicationStyleElementDefault<TDefault, TOverride, TInterface>, TInterface
        where TOverride : ApplicationStyleElementOverride<TDefault, TOverride, TInterface>, TInterface
        where TInterface : IApplicationStyle
    {
        #region Fields and Autoproperties

        [SerializeField] private List<TOverride> _overrides;

        #endregion

        public List<TOverride> Overrides => _overrides;

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                foreach (var overrideValue in _overrides)
                {
                    overrideValue.SyncWithDefault();
                }
            }
        }

        public void RegisterOverride(TOverride overrideStyle)
        {
            using (_PRF_RegisterOverride.Auto())
            {
                _overrides ??= new List<TOverride>();

                if (!_overrides.Contains(overrideStyle))
                {
                    _overrides.Add(overrideStyle);
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX =
            nameof(ApplicationStyleElementDefault<TDefault, TOverride, TInterface>) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_RegisterOverride =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverride));

        #endregion
    }
}
