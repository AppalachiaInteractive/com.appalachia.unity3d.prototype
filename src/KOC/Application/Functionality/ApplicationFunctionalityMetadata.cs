using System;
using Appalachia.Core.ControlModel.Contracts;
using Appalachia.Core.ControlModel.Controls;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Extensions.Debugging;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Functionality
{
    public abstract partial class ApplicationFunctionalityMetadata<TFunctionality, TFunctionalityMetadata, TManager> :
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

        protected virtual void AfterUpdateFunctionality(TFunctionality functionality)
        {
            using (_PRF_AfterUpdateFunctionality.Auto())
            {
                SubscribeResponsiveComponents(functionality);

                Updated.RaiseEvent();

                functionality.ApplyingMetadata = false;
            }
        }

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

        protected void RefreshAndApply<TControl, TConfig>(
            ref TControl control,
            ref TConfig config,
            TFunctionality functionality,
            GameObject parent = null,
            string name = null)
            where TControl : AppaControl<TControl, TConfig>, new()
            where TConfig : AppaControlConfig<TControl, TConfig>, new()
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                name ??= typeof(TFunctionality).Name;

                if (parent == null)
                {
                    parent = functionality.gameObject;
                }

                AppaControlConfig<TControl, TConfig>.RefreshAndApply(ref config, ref control, parent, name, this);
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

        #region IReleasable Members

        public bool NotReadyForRelease => _notReadyForRelease;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_RefreshAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndApply));

        private static readonly ProfilerMarker _PRF_GetAssetName = new ProfilerMarker(_PRF_PFX + nameof(GetAssetName));

        protected static readonly ProfilerMarker _PRF_AfterUpdateFunctionality =
            new ProfilerMarker(_PRF_PFX + nameof(AfterUpdateFunctionality));

        private static readonly ProfilerMarker _PRF_BeforeUpdateFunctionality =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeUpdateFunctionality));

        private static readonly ProfilerMarker _PRF_ExecuteUpdateFunctionalityInternal =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteUpdateFunctionalityInternal));

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
