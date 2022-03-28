#if UNITY_EDITOR
#if UNITY_EDITOR
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Attributes;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Attributes;
using Appalachia.Core.ControlModel.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Functionality.Design.Controls.Template;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Areas
{
    [ExecuteAlways]
    [ExecutionOrder(ExecutionOrders.AreaManagers)]
    [InspectorIcon(Brand.AreaManager.Icon)]
    [CallStaticConstructorInEditor]
    public abstract partial class AreaManager<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("scaledTemplates")]
        [SerializeField]
        [FoldoutGroup(APPASTR.Components + "/" + APPASTR.Scaled_Templates)]
        protected TemplateControl templates;

        #endregion

        /// <inheritdoc />
        protected override string GetBackgroundColor()
        {
            return Brand.AreaManager.Banner;
        }

        /// <inheritdoc />
        protected override string GetFallbackTitle()
        {
            return Brand.AreaManager.Fallback;
        }

        /// <inheritdoc />
        protected override string GetTitle()
        {
            return Brand.AreaManager.Text;
        }

        /// <inheritdoc />
        protected override string GetTitleColor()
        {
            return Brand.AreaManager.Color;
        }

        protected void CreateAreaAsset<TC, TS>(ref TC assetReference, TS markAsModified, string additionalName = null)
            where TC : ScriptableObject
            where TS : ScriptableObject
        {
            using (_PRF_CreateAreaAsset.Auto())
            {
                if (assetReference == null)
                {
                    var assetName = GetSceneAssetName<TC>(additionalName);

                    assetReference = AppalachiaObjectFactory.LoadExistingOrCreateNewAsset<TC>(
                        assetName,
                        ownerType: typeof(ApplicationManager)
                    );

                    markAsModified.MarkAsModified();
                }
            }
        }

        private void InitializeEditor(Initializer initializer)
        {
            using (_PRF_InitializeEditor.Auto())
            {
                TemplateControl.Refresh(
                    ref templates,
                    RootCanvas.ScaledCanvas.gameObject,
                    nameof(templates)
                );

                areaMetadata.templates.Apply(templates);
            }
        }

        private void UpdateEditor()
        {
            using (_PRF_UpdateEditor.Auto())
            {
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_CreateAreaAsset =
            new ProfilerMarker(_PRF_PFX + nameof(CreateAreaAsset));

        protected static readonly ProfilerMarker _PRF_InitializeEditor =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeEditor));

        private static readonly ProfilerMarker _PRF_UpdateEditor = new ProfilerMarker(_PRF_PFX + nameof(UpdateEditor));

        #endregion
    }
}

#endif

#endif
