using Appalachia.Core.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Delegates;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application
{
    [CallStaticConstructorInEditor]
    [SmartLabelChildren]
    public abstract class
        ApplicationFunctionality<TFunctionality, TFunctionalityMetadata> :
            SingletonAppalachiaBehaviour<TFunctionality>,
            IApplicationFunctionality
        where TFunctionality : ApplicationFunctionality<TFunctionality, TFunctionalityMetadata>
        where TFunctionalityMetadata :
        ApplicationFunctionalityMetadata<TFunctionality, TFunctionalityMetadata>
    {
        static ApplicationFunctionality()
        {
            RegisterDependency<TFunctionalityMetadata>(i => metadata = i);
            ApplicationManager.InstanceAvailable += i =>
            {
                applicationManager = i;
                instance.WhenApplicationManagerAvailable(i);
            };
        }

        #region Static Fields and Autoproperties

        protected static ApplicationManager applicationManager;

        [ShowInInspector, HideLabel, InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        protected static TFunctionalityMetadata metadata;

        #endregion

        protected abstract bool ParentObjectIsUI { get; }

        protected abstract string ParentObjectName { get; }

        protected virtual bool NestUnderApplicationManager => true;

        protected abstract void ApplyMetadataInternal();

        [ButtonGroup("Subscriptions")]
        [LabelText("Subscribe")]
        protected abstract void SubscribeToAllFunctionalties();

        [ButtonGroup("Subscriptions")]
        [LabelText("Unsubscribe")]
        protected abstract void UnsubscribeFromAllFunctionalities();

        protected virtual void BeforeApplyMetadataInternal()
        {
            using (_PRF_BeforeApplyMetadataInternal.Auto())
            {
            }
        }

        protected virtual void WhenApplicationManagerAvailable(ApplicationManager manager)
        {
            using (_PRF_WhenApplicationManagerAvailable.Auto())
            {
                if (NestUnderApplicationManager)
                {
                    GameObject containerObject = null;

                    manager.gameObject.GetOrAddChild(ref containerObject, ParentObjectName, ParentObjectIsUI);

                    gameObject.SetParentTo(containerObject);
                }
            }
        }

        protected override void AfterInitialization()
        {
            using (_PRF_AfterInitialization.Auto())
            {
                base.AfterInitialization();

                ApplyMetadata();
            }
        }

        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();

            using (_PRF_WhenDisabled.Auto())
            {
                metadata.SettingsChanged -= OnApplyMetadata;
                UnsubscribeFromAllFunctionalities();
            }
        }

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                ApplyMetadata();
            }
        }

        private void OnApplyMetadata(ValueArgs<TFunctionalityMetadata> args)
        {
            using (_PRF_OnApplyMetadata.Auto())
            {
                ApplyMetadata();
            }
        }

        [ButtonGroup("Subscriptions")]
        [LabelText("Refresh")]
        private void UpdateFunctionalitySubscriptions()
        {
            using (_PRF_UpdateFunctionalitySubscriptions.Auto())
            {
                UnsubscribeFromAllFunctionalities();
                SubscribeToAllFunctionalties();
            }
        }

        #region IApplicationFunctionality Members

        [ButtonGroup("Metadata")]
        public void ApplyMetadata()
        {
            using (_PRF_ApplyMetadata.Auto())
            {
                metadata.SettingsChanged -= OnApplyMetadata;
                metadata.SettingsChanged += OnApplyMetadata;

                BeforeApplyMetadataInternal();

                metadata.Apply(this as TFunctionality);

                ApplyMetadataInternal();
                UpdateFunctionalitySubscriptions();
            }
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_AfterInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialization));

        protected static readonly ProfilerMarker _PRF_ApplyMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyMetadata));

        protected static readonly ProfilerMarker _PRF_ApplyMetadataInternal =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyMetadataInternal));

        protected static readonly ProfilerMarker _PRF_BeforeApplyMetadataInternal =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeApplyMetadataInternal));

        protected static readonly ProfilerMarker _PRF_OnApplyMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplyMetadata));

        protected static readonly ProfilerMarker _PRF_SubscribeToAllFunctionalties =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeToAllFunctionalties));

        protected static readonly ProfilerMarker _PRF_UnsubscribeFromAllFunctionalities =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromAllFunctionalities));

        protected static readonly ProfilerMarker _PRF_UpdateFunctionalitySubscriptions =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateFunctionalitySubscriptions));

        private static readonly ProfilerMarker _PRF_WhenApplicationManagerAvailable =
            new ProfilerMarker(_PRF_PFX + nameof(WhenApplicationManagerAvailable));

        #endregion
    }
}
