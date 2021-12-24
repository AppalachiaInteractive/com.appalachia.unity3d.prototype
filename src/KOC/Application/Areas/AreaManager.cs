using System;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Attributes;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Application.Components;
using Appalachia.Prototype.KOC.Application.Components.Cursors;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Menus;
using Appalachia.Prototype.KOC.Application.Scenes;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    [ExecuteAlways]
    [Serializable]
    [ExecutionOrder(ExecutionOrders.AreaManagers)]
    [InspectorIcon(Brand.AreaManager.Icon)]
    [CallStaticConstructorInEditor]
    public abstract class AreaManager<T, TM> : SingletonAppalachiaApplicationBehaviour<T>, IAreaManager
        where T : AreaManager<T, TM>
        where TM : AreaMetadata<T, TM>
    {
        public delegate void ActivationHandler(ApplicationArea area, IAreaManager manager);

        public delegate void DeactivationHandler(ApplicationArea area, IAreaManager manager);

        // [CallStaticConstructorInEditor] should be added to the class (initsingletonattribute)
        static AreaManager()
        {
            RegisterDependency<TM>(i => _areaMetadata = i);
            RegisterDependency<MainAreaSceneInformationCollection>(
                i => _mainAreaSceneInformationCollection = i
            );
        }

        #region Static Fields and Autoproperties

        private static MainAreaSceneInformationCollection _mainAreaSceneInformationCollection;

        [NonSerialized, ShowInInspector, InlineEditor]
        private static TM _areaMetadata;

        #endregion

        #region Fields and Autoproperties

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

        [NonSerialized] private bool _isActivated;

        #endregion

        protected static TM areaMetadata => _areaMetadata;

        protected static MainAreaSceneInformationCollection mainAreaSceneInformationCollection =>
            _mainAreaSceneInformationCollection;

        public abstract AreaVersion Version { get; }

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

        public event ActivationHandler WhenActivated;

        public event DeactivationHandler WhenDeactivated;

        #region Event Functions

        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                base.Awake();

                var type = GetType();
                var sceneName = type.Name.Replace("Manager", string.Empty);

                if (gameObject.scene.name != sceneName)
                {
                    Context.Log.Warn(ZString.Format("Scene name should be {0}.", sceneName));
                }
            }
        }

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (!DependenciesAreReady)
                {
                    return;
                }

                if (!_isActivated)
                {
                    return;
                }

#if UNITY_EDITOR

                var isTemplateImageEnabled = template.image.enabled;
                var shouldAssignTemplateSprite =
                    areaMetadata.templates.selectedTemplate != template.image.sprite;
                var shouldDisableTemplate = !areaMetadata.templates.templateEnabled && template.image.enabled;
                var isCurrentFading = template.canvasFadeManager.IsFading;

                if (areaMetadata.templates.templateEnabled)
                {
                    var fadeRange = template.canvasFadeManager.fadeSettings.fadeRange;
                    fadeRange.y = areaMetadata.templates.templateAlpha;

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
                        template.image.sprite = areaMetadata.templates.selectedTemplate;
                    }

                    if (!isTemplateImageEnabled && !isCurrentFading)
                    {
                        fadeRange.x = 0f;
                        template.canvasFadeManager.fadeSettings.fadeRange = fadeRange;
                        template.image.enabled = true;
                        template.canvasFadeManager.FadeInCompleted += OnTemplateFadeInComplete;
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
                        template.canvasFadeManager.FadeOutCompleted += DisableTemplate;
                        template.canvasFadeManager.FadeOut();
                    }
                }
#endif
            }
        }

        protected override async AppaTask WhenDisabled()

        {
            using (_PRF_OnDisable.Auto())
            {
                await base.WhenDisabled();

                if (_isActivated)
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

                LifetimeComponentManager.instance.InitializeExternal();

                gameObject.transform.SetToOrigin();

                AreaRegistry.RegisterManager(this);

                name = typeof(T).Name;

                await initializer.Do(
                    this,
                    nameof(_areaMetadata),
                    _areaMetadata == null,
                    () => { _areaMetadata = SingletonAppalachiaObject<TM>.instance; }
                );

                if (areaMetadata.Area != Area)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Area misconfiguration between [{0}] and [{1}]",
                            name,
                            areaMetadata.name
                        )
                    );
                }

                var baseObjectName = name.Replace("Manager", string.Empty);
                var fullObjectName = ZString.Format(
                    "{0} - {1}",
                    baseObjectName,
                    areaMetadata.doozyView.viewName
                );

                await initializer.Do(
                    this,
                    nameof(MainAreaSceneInformationCollection),
                    _areaSceneInfo == null,
                    () =>
                    {
                        _areaSceneInfo ??= MainAreaSceneInformationCollection.instance.Lookup.Items.Get(Area);
                    }
                );

                canvas.Configure(gameObject, fullObjectName);
                areaMetadata.Apply(canvas);

                view.Configure(canvas.gameObject, fullObjectName);
                areaMetadata.Apply(view);

                template.Configure(view.gameObject, fullObjectName);
                areaMetadata.Apply(template);

                if (HasParent)
                {
                    var parent = AreaRegistry.GetManager(ParentArea);

                    if (parent != null)
                    {
                        canvas.graphController.enabled = false;
                    }
                }

                if (areaMetadata.menu.doesDrawMenu)
                {
                    GameObject uiMenuManagerGameObject = null;

                    view.gameObject.GetOrCreateChild(
                        ref uiMenuManagerGameObject,
                        ZString.Format("Menu - {0}", fullObjectName),
                        true
                    );

                    uiMenuManagerGameObject.GetOrCreateComponent(ref uiMenuManager);

#if UNITY_EDITOR
                    uiMenuManager.CreateMetadata(Area.ToString());
#endif
                }

                var applicationManager = ApplicationManager.instance;
                var applicationManagerObject = applicationManager.gameObject;
                var applicationManagerInvalid = (applicationManagerObject == null) ||
                                                (applicationManagerObject.scene == default) ||
                                                !applicationManagerObject.scene.isLoaded;

                if (applicationManager.RunningAsSubScene &&
                    (!applicationManager.HasSubSceneManagerBeenIdentified || applicationManagerInvalid))
                {
                    applicationManager.PrimarySubSceneArea = Area;

                    if (LifetimeComponentManager.instance == null)
                    {
                        Context.Log.Warn(
                            ZString.Format(
                                "Could not find {0} instance!",
                                nameof(LifetimeComponentManager).FormatNameForLogging()
                            )
                        );
                    }

                    Activate();
                }
            }
        }

#if UNITY_EDITOR

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

#endif

        protected string GetChildObjectName(string prefix = null, string postfix = null)
        {
            var baseObjectName = name.Replace("Manager", string.Empty);

            var fullObjectName = ZString.Format("{0}", baseObjectName);

            if (prefix != null)
            {
                fullObjectName = ZString.Format("{0} - {1}", prefix, fullObjectName);
            }

            if (postfix != null)
            {
                fullObjectName += ZString.Format(" - {0}", postfix);
            }

            return fullObjectName;
        }

        protected string GetSceneAssetName<TC>(string middle = null)
            where TC : Object
        {
            if (middle == null)
            {
                return ZString.Format("{0}_{1}", typeof(TC).GetSimpleReadableName(), gameObject.scene.name);
            }

            return ZString.Format(
                "{0}_{1}_{2}",
                typeof(TC).GetSimpleReadableName(),
                middle,
                gameObject.scene.name
            );
        }

        private void DisableTemplate()
        {
            template.image.enabled = false;
            template.canvasFadeManager.FadeOutCompleted -= DisableTemplate;
        }

        private void OnTemplateFadeInComplete()
        {
            var fadeRange = template.canvasFadeManager.fadeSettings.fadeRange;
            fadeRange.x = areaMetadata.templates.templateAlpha;
            template.canvasFadeManager.fadeSettings.fadeRange = fadeRange;
            template.canvasFadeManager.FadeInCompleted -= OnTemplateFadeInComplete;
        }

        #region IAreaManager Members

        public void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                try
                {
                    if (!enabled)
                    {
                        enabled = true;
                    }

                    if (_isActivated)
                    {
                        return;
                    }

                    _isActivated = true;

                    Context.Log.Info(nameof(Activate), this);

                    areaMetadata.cursor.Apply(CursorManager.instance);

                    areaMetadata.input.onEnableMapState.Apply(InputActions, this);

                    OnActivation();

                    WhenActivated?.Invoke(Area, this);
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error activating area {0}: {1}",
                            Area.ToString().FormatNameForLogging(),
                            ex.Message
                        ),
                        this
                    );
                    Context.Log.Error(ex, this);
                }
            }
        }

        public void Deactivate()
        {
            using (_PRF_Deactivate.Auto())
            {
                try
                {
                    _isActivated = false;

                    Context.Log.Info(nameof(Deactivate), this);

                    areaMetadata.input.onDisableMapState.Apply(InputActions, null);

                    OnDeactivation();

                    WhenDeactivated?.Invoke(Area, this);
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error deactivating area {0}: {1}",
                            Area.ToString().FormatNameForLogging(),
                            ex.Message
                        ),
                        this
                    );
                    Context.Log.Error(ex, this);
                }
            }
        }

        public abstract ApplicationArea Area { get; }

        public abstract ApplicationArea ParentArea { get; }

        public bool HasParent => ParentArea != ApplicationArea.None;

        public string AreaName => Area.ToString();

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AreaManager<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_OnActivation =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_OnDeactivation =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        private static readonly ProfilerMarker _PRF_Start = new ProfilerMarker(_PRF_PFX + nameof(Start));

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

        private static readonly ProfilerMarker _PRF_CreateAreaAsset =
            new ProfilerMarker(_PRF_PFX + nameof(CreateAreaAsset));

        protected override string GetTitle()
        {
            return Brand.AreaManager.Text;
        }

        protected override string GetFallbackTitle()
        {
            return Brand.AreaManager.Fallback;
        }

        protected override string GetTitleColor()
        {
            return Brand.AreaManager.Color;
        }

        protected override string GetBackgroundColor()
        {
            return Brand.AreaManager.Banner;
        }
#endif
    }
}
