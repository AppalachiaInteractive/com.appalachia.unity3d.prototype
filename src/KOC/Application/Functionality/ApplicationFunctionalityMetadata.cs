using Appalachia.Core.Events;
using Appalachia.Core.Events.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Objects.Sets;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Functionality
{
    public abstract class ApplicationFunctionalityMetadata<TFunctionality, TFunctionalityMetadata, TManager> :
        SingletonAppalachiaObject<TFunctionalityMetadata>,
        IApplicationFunctionalityMetadata<TFunctionality>
        where TFunctionality : ApplicationFunctionality<TFunctionality, TFunctionalityMetadata, TManager>
        where TFunctionalityMetadata :
        ApplicationFunctionalityMetadata<TFunctionality, TFunctionalityMetadata, TManager>
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>
    {
        #region Fields and Autoproperties

        public AppaEvent.Data Updated;

        #endregion

        public static void UpdateFunctionality(TFunctionalityMetadata data, TFunctionality target)
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                if (target == null)
                {
                    StaticContext.Log.Error("Applying metadata before the target has been assigned!");
                }

                data.BeforeUpdateFunctionality(target);
                data.UpdateFunctionality(target);
                data.AfterUpdateFunctionality(target);
            }
        }

        public void UpdateComponentSet<TSet, TSetMetadata>(
            ref TSetMetadata data,
            ref TSet target,
            GameObject parent,
            string setName)
            where TSet : ComponentSet<TSet, TSetMetadata>, new()
            where TSetMetadata : ComponentSetData<TSet, TSetMetadata>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                ComponentSetData<TSet, TSetMetadata>.UpdateComponentSet(
                    ref data,
                    ref target,
                    parent,
                    setName
                );
            }
        }

        // ReSharper disable once UnusedParameter.Global
        protected abstract void SubscribeResponsiveComponents(TFunctionality target);

        protected abstract void UpdateFunctionality(TFunctionality functionality);

        protected static string GetAssetName<T>()
        {
            return typeof(TFunctionality).Name + typeof(T).Name;
        }

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

        private void AfterUpdateFunctionality(TFunctionality target)
        {
            SubscribeResponsiveComponents(target);

            Updated.RaiseEvent();

            target.ApplyingMetadata = false;
        }

        private void BeforeUpdateFunctionality(TFunctionality functionality)
        {
            functionality.ApplyingMetadata = true;

            functionality.Changed.Event += () => UpdateFunctionality(
                this as TFunctionalityMetadata,
                functionality
            );
            Changed.Event += () => UpdateFunctionality(this as TFunctionalityMetadata, functionality);
        }

        #region IApplicationFunctionalityMetadata<TFunctionality> Members

        void IApplicationFunctionalityMetadata<TFunctionality>.UpdateFunctionality(
            TFunctionality functionality)
        {
            if (functionality.ApplyingMetadata)
            {
                return;
            }

            BeforeUpdateFunctionality(functionality);
            UpdateFunctionality(functionality);
            AfterUpdateFunctionality(functionality);
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_Apply =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateFunctionality));

        protected static readonly ProfilerMarker _PRF_SubscribeResponsiveComponents =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeResponsiveComponents));

        protected static readonly ProfilerMarker _PRF_UpdateFunctionality =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateFunctionality));

        private static readonly ProfilerMarker _PRF_UpdateComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSet));

        #endregion
    }
}
