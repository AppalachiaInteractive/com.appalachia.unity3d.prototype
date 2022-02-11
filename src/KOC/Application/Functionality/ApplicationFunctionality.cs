using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Preferences;
using Appalachia.Prototype.KOC.Application.Features.Availability;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Reflection.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Functionality
{
    [CallStaticConstructorInEditor]
    [SmartLabelChildren]
    [ExecuteAlways]
    public abstract class
        ApplicationFunctionality<TFunctionality, TFunctionalityMetadata, TManager> :
            SingletonAppalachiaBehaviour<TFunctionality>,
            IApplicationFunctionality
        where TFunctionality : ApplicationFunctionality<TFunctionality, TFunctionalityMetadata, TManager>
        where TFunctionalityMetadata :
        ApplicationFunctionalityMetadata<TFunctionality, TFunctionalityMetadata, TManager>
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>
    {
        #region Constants and Static Readonly

        protected const string WAS_NOT_READY_IN_TIME = "was not ready in time!";

        #endregion

        static ApplicationFunctionality()
        {
            RegisterDependency<TFunctionalityMetadata>(md => { metadata = md; });

            RegisterInstanceCallbacks
               .For<ApplicationFunctionality<TFunctionality, TFunctionalityMetadata, TManager>>()
               .When.Behaviour<TManager>()
               .IsAvailableThen(manager => { _manager = manager; });
        }

        #region Preferences

        private PREF<Color> _disableColor = PREFS.REG(
            PKG.Prefs.Group,
            "Disabled Color",
            Colors.CadmiumOrange
        );

        private PREF<Color> _enableColor = PREFS.REG(PKG.Prefs.Group, "Enabled Color", Colors.PaleGreen4);

        #endregion

        #region Static Fields and Autoproperties

        [PropertyOrder(-10)]
        [ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        protected static TFunctionalityMetadata metadata;

        private static IFeatureAvailabilitySet _when;

        private static TManager _manager;

        #endregion

        #region Fields and Autoproperties

        private bool _applyingMetadata;

        #endregion

        public new static IFeatureAvailabilitySet When
        {
            get
            {
                if (_when == null)
                {
                    _when = new FeatureAvailabilitySet(typeof(TFunctionality));
                }

                return _when;
            }
        }

        protected static TManager Manager => _manager;

        public TFunctionalityMetadata Metadata => metadata;

        public bool ApplyingMetadata
        {
            get => _applyingMetadata;
            internal set => _applyingMetadata = value;
        }

        protected Color disableColor => _disableColor;

        protected Color enableColor => _enableColor;

        /*
        [Button]
        [LabelText("Refresh Subscriptions")]
        protected internal virtual void SubscribeToOtherFunctionalities()
        {
            using (_PRF_SubscribeToAllFunctionalties.Auto())
            {
            }
        }*/

        protected internal void OnDependencyChanged()
        {
            using (_PRF_OnApplyMetadata.Auto())
            {
                if (!FullyInitialized)
                {
                    return;
                }

                UpdateFunctionality();
            }
        }

        protected virtual async AppaTask DelayEnabling()
        {
            await AppaTask.CompletedTask;
        }

        protected virtual void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
            }
        }

        protected new static void RegisterDependency<TDependency>(
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>,
            IRepositoryDependencyTracker<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                if (typeof(TDependency).ImplementsOrInheritsFrom(typeof(IApplicationFunctionality)))
                {
                    throw new NotSupportedException(
                        ZString.Format(
                            "{0} may not depend on {1}.  Dependencies should be defined at the feature level using the 'Feature Set'.",
                            typeof(TFunctionality).FormatForLogging(),
                            typeof(TDependency).FormatForLogging()
                        )
                    );
                }

                _dependencyTracker.RegisterDependency(handler);
            }
        }

        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();

            using (_PRF_WhenDisabled.Auto())
            {
                metadata.Changed.Event -= OnDependencyChanged;
                UnsubscribeFromAllFunctionalities();
            }
        }

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();
            await DelayEnabling();
            await AppaTask.WaitUntil(() => metadata != null);

            using (_PRF_WhenEnabled.Auto())
            {
                ApplicationFunctionalityMetadata<TFunctionality, TFunctionalityMetadata, TManager>
                   .UpdateFunctionality(metadata, this as TFunctionality);
            }
        }

        protected void NullCheck(object obj, string msg, string primaryName, params string[] names)
        {
            using (_PRF_NullCheck.Auto())
            {
                if (obj != null)
                {
                    return;
                }

                var namePart = primaryName;

                for (var i = 0; i < names.Length; i++)
                {
                    namePart += ZString.Format(".{0}", names[i]);
                }

                var message = ZString.Format("The {0} {1}", namePart, msg);

                Context.Log.Error(message);
                throw new NullReferenceException(message);
            }
        }

        protected void NullCheck(
            UnityEngine.Object obj,
            string msg,
            string primaryName,
            params string[] names)
        {
            using (_PRF_NullCheck.Auto())
            {
                if (obj != null)
                {
                    return;
                }

                var namePart = primaryName;

                for (var i = 0; i < names.Length; i++)
                {
                    namePart += ZString.Format(".{0}", names[i]);
                }

                var message = ZString.Format("The {0} {1}", namePart, msg);

                Context.Log.Error(message);
                throw new NullReferenceException(message);
            }
        }

        protected void UpdateFunctionality()
        {
            using (_PRF_ApplyMetadata.Auto())
            {
                ApplicationFunctionalityMetadata<TFunctionality, TFunctionalityMetadata, TManager>
                   .UpdateFunctionality(metadata, this as TFunctionality);
            }
        }

        #region IApplicationFunctionality Members

        /*void IApplicationFunctionality.SubscribeToOtherFunctionalities()
        {
            SubscribeToOtherFunctionalities();
        }*/

        void IApplicationFunctionality.UnsubscribeFromAllFunctionalities()
        {
            UnsubscribeFromAllFunctionalities();
        }

        void IApplicationFunctionality.UpdateFunctionality()
        {
            UpdateFunctionality();
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_ApplyMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateFunctionality));

        private static readonly ProfilerMarker _PRF_NullCheck =
            new ProfilerMarker(_PRF_PFX + nameof(NullCheck));

        protected static readonly ProfilerMarker _PRF_OnApplyMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(OnDependencyChanged));

        /*protected static readonly ProfilerMarker _PRF_SubscribeToAllFunctionalties =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeToOtherFunctionalities));*/

        protected static readonly ProfilerMarker _PRF_UnsubscribeFromAllFunctionalities =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromAllFunctionalities));

        #endregion
    }
}
