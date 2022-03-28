using System;
using Appalachia.Core.ControlModel.Contracts;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Events.Collections;
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

        #endregion

        /// <summary>
        ///     Given a functionality, applies this metadata to it to ensure that its
        ///     using the current metadata settings.  Like a "settings sync".
        /// </summary>
        /// <param name="functionality">The functionality to update.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="functionality" /> is null.</exception>
        public void Apply(TFunctionality functionality)
        {
            using (_PRF_Apply.Auto())
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

                        ExecuteOnApply(functionality);
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

                ExecuteOnApply(functionality);
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
        public void SubscribeToChanges(TFunctionality functionality, Func<Action> delegateCreator)
        {
            using (_PRF_SubscribeToChanges.Auto())
            {
                _delegates ??= new();
                _delegates.Subscribe(functionality, ref Changed, delegateCreator);

                if (Changed.SubscriberCount == 0)
                {
                    APPADEBUG.BREAKPOINT(() => nameof(Changed), functionality);
                }
            }
        }

        /// <summary>
        ///     Unsubscribes the provided functionality from any subsequent metadata updates.
        ///     This will only have an impact if you have previously subscribed for updates
        ///     via <see cref="SubscribeToChanges" />.
        /// </summary>
        /// <param name="functionality">The functionality to unsubscribe.</param>
        public void UnsubscribeFromChanges(TFunctionality functionality)
        {
            using (_PRF_UnsubscribeFromChanges.Auto())
            {
                _delegates ??= new();
                _delegates.Unsubscribe(functionality, ref Changed);
            }
        }

        protected virtual void AfterApplying(TFunctionality functionality)
        {
            using (_PRF_AfterApplying.Auto())
            {
                SubscribeResponsiveComponents(functionality);

                functionality.ApplyingMetadata = false;
                Changed.Unsuspend();
                UnsuspendResponsiveComponents(functionality);
            }
        }

        protected virtual void BeforeApplying(TFunctionality subwidget)
        {
            using (_PRF_BeforeApplying.Auto())
            {
                SuspendResponsiveComponents(subwidget);
                Changed.Suspend();
                subwidget.ApplyingMetadata = true;
            }
        }

        protected virtual void OnApply(TFunctionality functionality)
        {
            using (_PRF_OnApply.Auto())
            {
            }
        }

        // ReSharper disable once UnusedParameter.Global
        protected virtual void SubscribeResponsiveComponents(TFunctionality functionality)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        protected virtual void SuspendResponsiveComponents(TFunctionality functionality)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
            }
        }

        protected virtual void UnsuspendResponsiveComponents(TFunctionality functionality)
        {
            using (_PRF_UnsuspendResponsiveComponents.Auto())
            {
            }
        }

        /*
        public override void UnsuspendChanges(TFunctionality functionality)
        {
            using (_PRF_UnsuspendChanges.Auto())
            {
                base.UnsuspendChanges();
            
                UnsuspendResponsiveComponents(functionality);
            }
        }

        public override void SuspendChanges(TFunctionality functionality)
        {
            using (_PRF_SuspendChanges.Auto())
            {
                base.SuspendChanges();
            
                SuspendResponsiveComponents(functionality);
            }
        }
        */

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

        private void ExecuteOnApply(TFunctionality functionality)
        {
            using (_PRF_ExecuteOnApply.Auto())
            {
                BeforeApplying(functionality);
                OnApply(functionality);
                AfterApplying(functionality);
            }
        }

        #region IApplicationFunctionalityMetadata<TFunctionality> Members

        void IApplicationFunctionalityMetadata<TFunctionality>.Apply(TFunctionality functionality)
        {
            using (_PRF_Apply.Auto())
            {
                Apply(functionality);
            }
        }

        #endregion

        #region IReleasable Members

        public bool NotReadyForRelease => _notReadyForRelease;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetAssetName = new ProfilerMarker(_PRF_PFX + nameof(GetAssetName));

        protected static readonly ProfilerMarker _PRF_AfterApplying =
            new ProfilerMarker(_PRF_PFX + nameof(AfterApplying));

        protected static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        protected static readonly ProfilerMarker _PRF_BeforeApplying =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeApplying));

        private static readonly ProfilerMarker _PRF_ExecuteOnApply =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteOnApply));

        protected static readonly ProfilerMarker _PRF_OnApply = new ProfilerMarker(_PRF_PFX + nameof(OnApply));

        protected static readonly ProfilerMarker _PRF_SubscribeResponsiveComponents =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeResponsiveComponents));

        protected static readonly ProfilerMarker _PRF_SuspendResponsiveComponents =
            new ProfilerMarker(_PRF_PFX + nameof(SuspendResponsiveComponents));

        protected static readonly ProfilerMarker _PRF_UnsuspendResponsiveComponents =
            new ProfilerMarker(_PRF_PFX + nameof(UnsuspendResponsiveComponents));

        #endregion
    }
}
