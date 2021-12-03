using System;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Attributes;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Attributes;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Application.Components;
using Appalachia.Prototype.KOC.Application.Components.Cursors;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Menus;
using Appalachia.Prototype.KOC.Application.Scenes;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    [ExecuteAlways]
    [Serializable]
    [InspectorIcon(Icons.Squirrel.Blue)]
    [ExecutionOrder(ExecutionOrders.AreaManagers)]
    public abstract class AreaManager<T, TM> : SingletonAppalachiaApplicationBehaviour<T>, IAreaManager
        where T : AreaManager<T, TM>
        where TM : AreaMetadata<T, TM>
    {
        #region Fields and Autoproperties

        [SerializeField] public TM metadata;

        protected ApplicationLifetimeComponents lifetimeComponents;

        [FormerlySerializedAs("_bootloadData")]
        [SerializeField]
        protected AreaSceneInformation _areaSceneInfo;

        [SerializeField, FoldoutGroup(APPASTR.Components)]
        protected UITemplateComponentSet template;

        [SerializeField, FoldoutGroup(APPASTR.Components)]
        protected UICanvasAreaComponentSet canvas;

        [SerializeField, FoldoutGroup(APPASTR.Components)]
        protected UIViewComponentSet view;

        [SerializeField, FoldoutGroup(APPASTR.Components)]
        protected UIMenuManager uiMenuManager;

        private IAreaManager _parent;

        #endregion

        public bool DoesApplicationManagerExist
        {
            get
            {
                var applicationManager = AreaRegistry.GetApplicationManager();

                return applicationManager == null;
            }
        }

        public IAreaManager Parent
        {
            get
            {
                if (HasParent)
                {
                    if (_parent != null)
                    {
                        return _parent;
                    }

                    _parent = AreaRegistry.GetManager(ParentArea);

                    return _parent;
                }

                return null;
            }
        }

        protected AreaSceneInformation areaSceneInfo => _areaSceneInfo;

        #region Event Functions

        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                base.Awake();

                AppaLog.Context.Area.Info(nameof(Awake));

                Initialize();
            }
        }

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
#if UNITY_EDITOR

                var isTemplateImageEnabled = template.image.enabled;
                var shouldAssignTemplateSprite = metadata.SelectedTemplate != template.image.sprite;
                var shouldDisableTemplate = !metadata.TemplateEnabled && template.image.enabled;
                var isCurrentFading = template.canvasFadeManager.IsFading;

                if (metadata.TemplateEnabled)
                {
                    var fadeRange = template.canvasFadeManager.fadeSettings.fadeRange;
                    fadeRange.y = metadata.TemplateAlpha;

                    if (isCurrentFading)
                    {
                        fadeRange.x = Math.Min(fadeRange.x, fadeRange.y);
                    }
                    else
                    {
                        fadeRange.x = fadeRange.y;
                    }

                    template.canvasFadeManager.fadeSettings.fadeRange = fadeRange;

                    if (shouldAssignTemplateSprite)
                    {
                        template.image.sprite = metadata.SelectedTemplate;
                    }

                    if (!isTemplateImageEnabled && !isCurrentFading)
                    {
                        fadeRange.x = 0f;
                        template.canvasFadeManager.fadeSettings.fadeRange = fadeRange;
                        template.image.enabled = true;
                        template.canvasFadeManager.OnFadeInCompleted += OnTemplateFadeInComplete;
                        template.canvasFadeManager.FadeIn();
                    }
                }
                else if (shouldDisableTemplate)
                {
                    var fadeRange = template.canvasFadeManager.fadeSettings.fadeRange;

                    if (!isCurrentFading)
                    {
                        fadeRange.x = 0f;
                        template.canvasFadeManager.fadeSettings.fadeRange = fadeRange;
                        template.canvasFadeManager.OnFadeOutCompleted += DisableTemplate;
                        template.canvasFadeManager.FadeOut();
                    }
                }
#endif
            }
        }

        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                base.OnEnable();

                AppaLog.Context.Area.Info(nameof(OnEnable));
                Initialize();

                if (!DoesApplicationManagerExist)
                {
                    Activate();
                }
            }
        }

        protected override void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                base.OnDisable();

                AppaLog.Context.Area.Info(nameof(OnDisable));

                if (!DoesApplicationManagerExist)
                {
                    Deactivate();
                }
            }
        }

        #endregion

        protected abstract void OnActivation();

        protected abstract void OnDeactivation();

        protected abstract void ResetArea();

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                AppaLog.Context.Area.Info(nameof(Initialize));

                AreaRegistry.RegisterManager(this);

                if (metadata == null)
                {
                    metadata = SingletonAppalachiaObject<TM>.instance;
                    this.MarkAsModified();
                }

                if (lifetimeComponents == null)
                {
                    lifetimeComponents = ApplicationLifetimeComponents.instance;
                    this.MarkAsModified();
                }

                if (_areaSceneInfo == null)
                {
                    _areaSceneInfo = AreaSceneInformationCollection.instance.GetByArea(Area);
                }

                name = typeof(T).Name;

                var baseObjectName = name.Replace("Manager", string.Empty);

                var fullObjectName = $"{baseObjectName} - {metadata.ViewName}";

                canvas.Configure(gameObject, fullObjectName);
                metadata.Apply(canvas);

                view.Configure(canvas.gameObject, fullObjectName);
                metadata.Apply(view);

                template.Configure(view.gameObject, fullObjectName);
                metadata.Apply(template);

                if (HasParent)
                {
                    var parent = AreaRegistry.GetManager(ParentArea);

                    if (parent != null)
                    {
                        canvas.graphController.enabled = false;
                    }
                }

                if (metadata.DoesDrawMenu)
                {
                    GameObject uiMenuManagerGameObject = null;

                    view.gameObject.CreateOrGetChild(
                        ref uiMenuManagerGameObject,
                        $"Menu - {fullObjectName}",
                        true
                    );

                    uiMenuManagerGameObject.CreateOrGetComponent(ref uiMenuManager);

                    uiMenuManager.CreateMetadata(Area.ToString());
                }
            }
        }

        protected string GetChildObjectName(string prefix = null, string postfix = null)
        {
            var baseObjectName = name.Replace("Manager", string.Empty);

            var fullObjectName = $"{baseObjectName}";

            if (prefix != null)
            {
                fullObjectName = $"{prefix} - {fullObjectName}";
            }

            if (postfix != null)
            {
                fullObjectName += $" - {postfix}";
            }

            return fullObjectName;
        }

        protected string GetSceneAssetName<TC>(string middle = null)
            where TC : Object
        {
            if (middle == null)
            {
                return $"{typeof(TC).GetSimpleReadableName()}_{gameObject.scene.name}";                
            }
            
            return $"{typeof(TC).GetSimpleReadableName()}_{middle}_{gameObject.scene.name}";
        }

        private void DisableTemplate()
        {
            template.image.enabled = false;
            template.canvasFadeManager.OnFadeOutCompleted -= DisableTemplate;
        }

        private void OnTemplateFadeInComplete()
        {
            var fadeRange = template.canvasFadeManager.fadeSettings.fadeRange;
            fadeRange.x = metadata.TemplateAlpha;
            template.canvasFadeManager.fadeSettings.fadeRange = fadeRange;
            template.canvasFadeManager.OnFadeInCompleted -= OnTemplateFadeInComplete;
        }

        #region IAreaManager Members

        public abstract ApplicationArea Area { get; }

        public abstract ApplicationArea ParentArea { get; }

        public bool HasParent => ParentArea != ApplicationArea.None;

        public string AreaName => Area.ToString();

        public void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                try
                {
                    AppaLog.Context.Area.Info(nameof(Activate));

                    if (metadata.HideCursor)
                    {
                        CursorManager.instance.HideCursor();
                    }
                    else if (metadata.HasCustomCursor)
                    {
                        CursorManager.instance.SetCursor(metadata.Cursor);
                    }

                    if (metadata.UpdateCursorLockState)
                    {
                        CursorManager.instance.SetLockState(metadata.CursorLockState);
                    }

                    metadata.OnEnableMapState.Apply(InputActions);

                    OnActivation();
                }
                catch (Exception ex)
                {
                    AppaLog.Context.Area.Error($"Error activating area {Area}: {ex.Message}");
                    AppaLog.Context.Area.Exception(ex);
                }
            }
        }

        public void Deactivate()
        {
            using (_PRF_Deactivate.Auto())
            {
                try
                {
                    AppaLog.Context.Area.Info(nameof(Deactivate));

                    metadata.OnDisableMapState.Apply(InputActions);

                    OnDeactivation();
                }
                catch (Exception ex)
                {
                    AppaLog.Context.Area.Error($"Error deactivating area {Area}: {ex.Message}");
                    AppaLog.Context.Area.Exception(ex);
                }
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AreaManager<T, TM>) + ".";

#if UNITY_EDITOR
        private static readonly ProfilerMarker _PRF_CreateAreaAsset =
            new ProfilerMarker(_PRF_PFX + nameof(CreateAreaAsset));
#endif

        private static readonly ProfilerMarker
            _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_OnDisable =
            new ProfilerMarker(_PRF_PFX + nameof(OnDisable));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        #endregion

#if UNITY_EDITOR

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

#endif
    }
}
