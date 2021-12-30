using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Styling.Base
{
    [Serializable]
    public abstract class
        ApplicationStyleElementDefault<TDefault, TOverride, TInterface> : ApplicationStyleElement<TDefault,
            TInterface>
        where TDefault : ApplicationStyleElementDefault<TDefault, TOverride, TInterface>, TInterface
        where TOverride : ApplicationStyleElementOverride<TDefault, TOverride, TInterface>, TInterface
        where TInterface : IApplicationStyle
    {
        #region Fields and Autoproperties

        [SerializeField] private List<TOverride> _overrides;

        #endregion

        public List<TOverride> Overrides => _overrides;

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

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                for (var i = _overrides.Count - 1; i >= 0; i--)
                {
                    if (_overrides[i] == null)
                    {
                        _overrides.RemoveAt(i);
                    }
                }

                foreach (var overrideValue in _overrides)
                {
                    overrideValue.SyncWithDefault();
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
