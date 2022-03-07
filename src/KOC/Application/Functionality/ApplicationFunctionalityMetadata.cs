using System;
using System.Diagnostics.CodeAnalysis;
using Appalachia.Core.Objects.Components.Contracts;
using Appalachia.Core.Objects.Components.Sets;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Preferences;
using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Extensions.Debugging;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Functionality
{
    public abstract class ApplicationFunctionalityMetadata<TFunctionality, TFunctionalityMetadata, TManager> :
        SingletonAppalachiaObject<TFunctionalityMetadata>,
        IApplicationFunctionalityMetadata<TFunctionality>,
        IInspectorVisibility,
        IFieldLockable,
        IReleasable
        where TFunctionality : ApplicationFunctionality<TFunctionality, TFunctionalityMetadata, TManager>
        where TFunctionalityMetadata :
        ApplicationFunctionalityMetadata<TFunctionality, TFunctionalityMetadata, TManager>
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>, IApplicationFunctionalityManager

    {
        #region Preferences

        [NonSerialized] private PREF<Color> _advancedToggleColorOff;
        [NonSerialized] private PREF<Color> _advancedToggleColorOn;

        [NonSerialized] private PREF<Color> _hideToggleColorOff;
        [NonSerialized] private PREF<Color> _hideToggleColorOn;
        [NonSerialized] private PREF<Color> _lockToggleColorOff;
        [NonSerialized] private PREF<Color> _lockToggleColorOn;
        [NonSerialized] private PREF<Color> _showToggleColorOff;
        [NonSerialized] private PREF<Color> _showToggleColorOn;
        [NonSerialized] private PREF<Color> _suspendToggleColorOff;

        [NonSerialized] private PREF<Color> _suspendToggleColorOn;

        private PREF<Color> AdvancedToggleColorOff
        {
            get
            {
                if (_advancedToggleColorOff == null)
                {
                    _advancedToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(AdvancedToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _advancedToggleColorOff;
            }
        }

        private PREF<Color> AdvancedToggleColorOn
        {
            get
            {
                if (_advancedToggleColorOn == null)
                {
                    _advancedToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(AdvancedToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _advancedToggleColorOn;
            }
        }

        private PREF<Color> HideToggleColorOff
        {
            get
            {
                if (_hideToggleColorOff == null)
                {
                    _hideToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(HideToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _hideToggleColorOff;
            }
        }

        private PREF<Color> HideToggleColorOn
        {
            get
            {
                if (_hideToggleColorOn == null)
                {
                    _hideToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(HideToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _hideToggleColorOn;
            }
        }

        private PREF<Color> LockToggleColorOff
        {
            get
            {
                if (_lockToggleColorOff == null)
                {
                    _lockToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(LockToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _lockToggleColorOff;
            }
        }

        private PREF<Color> LockToggleColorOn
        {
            get
            {
                if (_lockToggleColorOn == null)
                {
                    _lockToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(LockToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _lockToggleColorOn;
            }
        }

        private PREF<Color> ShowToggleColorOff
        {
            get
            {
                if (_showToggleColorOff == null)
                {
                    _showToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(ShowToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _showToggleColorOff;
            }
        }

        private PREF<Color> ShowToggleColorOn
        {
            get
            {
                if (_showToggleColorOn == null)
                {
                    _showToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(ShowToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _showToggleColorOn;
            }
        }

        private PREF<Color> SuspendToggleColorOff
        {
            get
            {
                if (_suspendToggleColorOff == null)
                {
                    _suspendToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(SuspendToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _suspendToggleColorOff;
            }
        }

        private PREF<Color> SuspendToggleColorOn
        {
            get
            {
                if (_suspendToggleColorOn == null)
                {
                    _suspendToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(SuspendToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _suspendToggleColorOn;
            }
        }

        #endregion

        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        private bool _showAdvancedOptions;

        private ReusableDelegateCollection<TFunctionality> _delegates;

        [NonSerialized] private bool _showAllFields;

        [SerializeField, HideInInspector]
        private bool _hideAllFields;

        [SerializeField, HideInInspector]
        private bool _disableAllFields;

        [SerializeField, HideInInspector]
        private bool _suspendFieldApplication;

        private Func<bool> _shouldEnable;

        [SerializeField, HideInInspector]
        private bool _notReadyForRelease;

        /// <summary>
        ///     Offers notifications whenever this metadata is applied to a functionality.
        ///     Use this to drive any further behaviour needed to keep the functionality in sync.
        /// </summary>
        public AppaEvent.Data Updated;

        #endregion

        [SuppressMessage("ReSharper", "NotAccessedVariable")]
        public override void InitializePreferences()
        {
            using (_PRF_InitializePreferences.Auto())
            {
                base.InitializePreferences();

                var tempPreference = AdvancedToggleColorOff;
                tempPreference = AdvancedToggleColorOff;
                tempPreference = HideToggleColorOff;

                tempPreference = HideToggleColorOn;

                tempPreference = LockToggleColorOff;

                tempPreference = LockToggleColorOn;

                tempPreference = ShowToggleColorOff;

                tempPreference = ShowToggleColorOn;

                tempPreference = SuspendToggleColorOff;

                tempPreference = SuspendToggleColorOn;
            }
        }

        /// <summary>
        ///     A simple convenience method to call
        ///     <see cref="ComponentSetData{TComponentSet, TComponentSetData}" />
        ///     .
        ///     <see
        ///         cref="ComponentSetData{TComponentSet,TComponentSetData}.RefreshAndApply(ref TComponentSetData,ref TComponentSet,UnityEngine.GameObject,string)" />
        ///     ,
        ///     which will ensure the provided component set is synced with its configuration.
        /// </summary>
        /// <param name="data">The component set data.</param>
        /// <param name="set">The component set.</param>
        /// <param name="parent">The parent of the component set.  Only used if we need to create the component set.</param>
        /// <param name="setName">The name of the component set.</param>
        /// <typeparam name="TComponentSet">The component set.</typeparam>
        /// <typeparam name="TComponentSetData">The component set data.</typeparam>
        public void RefreshAndApply<TComponentSet, TComponentSetData>(
            ref TComponentSetData data,
            ref TComponentSet set,
            GameObject parent,
            string setName)
            where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
            where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>, new()
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                ComponentSetData<TComponentSet, TComponentSetData>.RefreshAndApply(ref data, ref set, parent, setName);
            }
        }

        /// <summary>
        ///     Subscribes the provided functionality to metadata updates, using the provided delegate
        ///     creator to produce an event handler.
        /// </summary>
        /// <param name="functionality">The functionality to subscribe.</param>
        /// <param name="delegateCreator">
        ///     A function that produces an event handler.  Essentially,
        ///     this is the "what you want to happen" when the event is raised.
        /// </param>
        public void SubscribeForUpdates(TFunctionality functionality, Func<Action> delegateCreator)
        {
            using (_PRF_SubscribeForUpdates.Auto())
            {
                _delegates ??= new();
                _delegates.Subscribe(functionality, ref Updated, delegateCreator);

                if (Updated.SubscriberCount == 0)
                {
                    APPADEBUG.BREAKPOINT(() => nameof(Updated), functionality);
                }
            }
        }

        /// <summary>
        ///     Unsubscribes the provided functionality from any subsequent metadata updates.
        ///     This will only have an impact if you have previously subscribed for updates
        ///     via <see cref="SubscribeForUpdates" />.
        /// </summary>
        /// <param name="functionality">The functionality to unsubscribe.</param>
        public void UnsubscribeFromUpdates(TFunctionality functionality)
        {
            using (_PRF_UnsubscribeFromUpdates.Auto())
            {
                _delegates ??= new();
                _delegates.Unsubscribe(functionality, ref Updated);
            }
        }

        /// <summary>
        ///     Given a functionality, applies this metadata to it to ensure that its
        ///     using the current metadata settings.  Like a "settings sync".
        /// </summary>
        /// <param name="functionality">The functionality to update.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="functionality" /> is null.</exception>
        public void UpdateFunctionality(TFunctionality functionality)
        {
            using (_PRF_UpdateFunctionality.Auto())
            {
                if (functionality == null)
                {
                    throw new ArgumentNullException(
                        nameof(functionality),
                        "Applying metadata before the functionality has been assigned!"
                    );
                }

                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (functionality == null)
                        {
                            return;
                        }

                        ExecuteUpdateFunctionalityInternal(functionality);
                    }

                    return SubscribableDelegate;
                }

                _delegates ??= new();

                _delegates.Subscribe(functionality, ref functionality.Changed, CreateSubscribableDelegate);
                _delegates.Subscribe(functionality, ref Changed,               CreateSubscribableDelegate);

                if (functionality.Changed.SubscriberCount == 0)
                {
                    APPADEBUG.BREAKPOINT(() => nameof(functionality.Changed), functionality);
                }

                if (Changed.SubscriberCount == 0)
                {
                    APPADEBUG.BREAKPOINT(() => nameof(Changed), functionality);
                }

                ExecuteUpdateFunctionalityInternal(functionality);
            }
        }

        // ReSharper disable once UnusedParameter.Global
        protected abstract void SubscribeResponsiveComponents(TFunctionality functionality);

        protected abstract void UpdateFunctionalityInternal(TFunctionality functionality);

        /// <summary>
        ///     Returns an asset name which which concatenates the current functionality <see cref="Type" />.<see cref="Type.Name" />
        ///     with the <see cref="Type" />.<see cref="Type.Name" /> of the provided <see cref="T" />.
        /// </summary>
        /// <typeparam name="T">The type whose name should be the second half of the output name.</typeparam>
        /// <returns>The formatted name.</returns>
        /// <example>
        ///     If the <see cref="TFunctionality" /> is "MySpecialWidget", and
        ///     <see cref="T" /> is "MySpecialComponent", the resulting output will be:
        ///     "MySpecialWidgetMySpecialComponent"
        /// </example>
        protected static string GetAssetName<T>()
        {
            using (_PRF_GetAssetName.Auto())
            {
                return typeof(TFunctionality).Name + typeof(T).Name;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
#if UNITY_EDITOR
                ValidateAddressableInformation();
#endif
            }
        }

        private void AfterUpdateFunctionality(TFunctionality functionality)
        {
            using (_PRF_AfterUpdateFunctionality.Auto())
            {
                SubscribeResponsiveComponents(functionality);

                Updated.RaiseEvent();

                functionality.ApplyingMetadata = false;
            }
        }

        private void BeforeUpdateFunctionality(TFunctionality functionality)
        {
            using (_PRF_BeforeUpdateFunctionality.Auto())
            {
                functionality.ApplyingMetadata = true;
            }
        }

        private void ExecuteUpdateFunctionalityInternal(TFunctionality functionality)
        {
            using (_PRF_ExecuteUpdateFunctionalityInternal.Auto())
            {
                BeforeUpdateFunctionality(functionality);
                UpdateFunctionalityInternal(functionality);
                AfterUpdateFunctionality(functionality);
            }
        }

        #region IApplicationFunctionalityMetadata<TFunctionality> Members

        void IApplicationFunctionalityMetadata<TFunctionality>.UpdateFunctionality(TFunctionality functionality)
        {
            using (_PRF_UpdateFunctionality.Auto())
            {
                UpdateFunctionality(functionality);
            }
        }

        #endregion

        #region IFieldLockable Members

        public Color LockToggleColor => DisableAllFields ? LockToggleColorOn.v : LockToggleColorOff.v;

        public Color SuspendToggleColor => SuspendFieldApplication ? SuspendToggleColorOn.v : SuspendToggleColorOff.v;

        public bool DisableAllFields
        {
            get => _disableAllFields;
            set => _disableAllFields = value;
        }

        public bool SuspendFieldApplication
        {
            get => _suspendFieldApplication;
            set => _suspendFieldApplication = value;
        }

        #endregion

        #region IInspectorVisibility Members

        public bool ShowAdvancedOptions
        {
            get => _showAdvancedOptions || HideAllFields || ShowAllFields || SuspendFieldApplication || DisableAllFields;
            set => _showAdvancedOptions = value;
        }

        public Color AdvancedToggleColor => ShowAdvancedOptions ? AdvancedToggleColorOn.v : AdvancedToggleColorOff.v;

        public Color ShowToggleColor => ShowAllFields ? ShowToggleColorOn.v : ShowToggleColorOff.v;

        public Color HideToggleColor => HideAllFields ? HideToggleColorOn.v : HideToggleColorOff.v;

        public bool HideAllFields
        {
            get => _hideAllFields && !_showAllFields;
            set => _hideAllFields = value;
        }

        public bool ShowAllFields
        {
            get => _showAllFields;
            set => _showAllFields = value;
        }

        #endregion

        #region IReleasable Members

        public bool NotReadyForRelease => _notReadyForRelease;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetAssetName = new ProfilerMarker(_PRF_PFX + nameof(GetAssetName));

        private static readonly ProfilerMarker _PRF_AfterUpdateFunctionality =
            new ProfilerMarker(_PRF_PFX + nameof(AfterUpdateFunctionality));

        private static readonly ProfilerMarker _PRF_BeforeUpdateFunctionality =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeUpdateFunctionality));

        private static readonly ProfilerMarker _PRF_ExecuteUpdateFunctionalityInternal =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteUpdateFunctionalityInternal));

        private static readonly ProfilerMarker _PRF_RefreshAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndApply));

        private static readonly ProfilerMarker _PRF_SubscribeForUpdates =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeForUpdates));

        protected static readonly ProfilerMarker _PRF_SubscribeResponsiveComponents =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeResponsiveComponents));

        private static readonly ProfilerMarker _PRF_UnsubscribeFromUpdates =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromUpdates));

        protected static readonly ProfilerMarker _PRF_UpdateFunctionality =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateFunctionality));

        protected static readonly ProfilerMarker _PRF_UpdateFunctionalityInternal =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateFunctionalityInternal));

        #endregion
    }
}
