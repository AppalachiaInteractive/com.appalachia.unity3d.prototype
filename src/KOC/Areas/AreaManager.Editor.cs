#if UNITY_EDITOR
using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Attributes;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas
{
    [ExecuteAlways]
    [ExecutionOrder(ExecutionOrders.AreaManagers)]
    [InspectorIcon(Brand.AreaManager.Icon)]
    [CallStaticConstructorInEditor]
    public abstract partial class AreaManager<TManager, TMetadata>
    {
        protected override string GetBackgroundColor()
        {
            return Brand.AreaManager.Banner;
        }

        protected override string GetFallbackTitle()
        {
            return Brand.AreaManager.Fallback;
        }

        protected override string GetTitle()
        {
            return Brand.AreaManager.Text;
        }

        protected override string GetTitleColor()
        {
            return Brand.AreaManager.Color;
        }

        protected void CreateAreaAsset<TC, TS>(
            ref TC assetReference,
            TS markAsModified,
            string additionalName = null)
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

        private void InitializeAreaTemplate()
        {
            using (_PRF_InitializeAreaTemplate.Auto())
            {
                var isTemplateImageEnabled = template.image.enabled;
                var shouldAssignTemplateSprite =
                    areaMetadata.templates.selectedTemplate != template.image.sprite;
                var shouldDisableTemplate = !areaMetadata.templates.templateEnabled && template.image.enabled;
                var isCurrentFading = template.fadeManager.IsFading;

                if (areaMetadata.templates.templateEnabled)
                {
                    var fadeRange = template.fadeManager.fadeSettings.fadeRange;
                    fadeRange.y = areaMetadata.templates.templateAlpha;

                    if (isCurrentFading)
                    {
                        fadeRange.x = Math.Min(fadeRange.x, fadeRange.y);
                    }
                    else
                    {
                        fadeRange.x = fadeRange.y;
                    }

                    template.fadeManager.fadeSettings.fadeRange = fadeRange;

                    if (shouldAssignTemplateSprite)
                    {
                        template.image.sprite = areaMetadata.templates.selectedTemplate;
                    }

                    if (!isTemplateImageEnabled && !isCurrentFading)
                    {
                        fadeRange.x = 0f;
                        template.fadeManager.fadeSettings.fadeRange = fadeRange;
                        template.image.enabled = true;
                        template.fadeManager.FadeInCompleted += OnTemplateFadeInComplete;
                        template.fadeManager.FadeIn();
                    }
                }
                else if (shouldDisableTemplate)
                {
                    var fadeRange = template.fadeManager.fadeSettings.fadeRange;

                    if (!isCurrentFading)
                    {
                        fadeRange.x = 0f;
                        template.fadeManager.fadeSettings.fadeRange = fadeRange;
                        template.fadeManager.FadeOutCompleted += DisableTemplate;
                        template.fadeManager.FadeOut();
                    }
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_CreateAreaAsset =
            new ProfilerMarker(_PRF_PFX + nameof(CreateAreaAsset));

        private static readonly ProfilerMarker _PRF_InitializeAreaTemplate =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeAreaTemplate));

        #endregion
    }
}

#endif
