using System;
using System.Collections.Generic;
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Areas.Common.Features;
using Appalachia.Prototype.KOC.Areas.Common.Services;
using Appalachia.Prototype.KOC.Areas.Common.Widgets;
using Appalachia.Prototype.KOC.Behaviours;
using Appalachia.Prototype.KOC.Components.Cursors;
using Appalachia.Prototype.KOC.Components.UI;
using Appalachia.Prototype.KOC.Scenes;
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

namespace Appalachia.Prototype.KOC.Areas
{
    [Serializable]
    public abstract partial class
        AreaManager<TManager, TMetadata> : SingletonAppalachiaApplicationBehaviour<TManager>, IAreaManager
        where TManager : AreaManager<TManager, TMetadata>
        where TMetadata : AreaMetadata<TManager, TMetadata>
    {
        public delegate void ActivationHandler(ApplicationArea area, IAreaManager manager);

        public delegate void AreaInterfaceHiddenHandler(ApplicationArea area, IAreaManager manager);

        public delegate void AreaInterfaceShownHandler(ApplicationArea area, IAreaManager manager);

        public delegate void DeactivationHandler(ApplicationArea area, IAreaManager manager);

        public event ActivationHandler Activated;

        public event DeactivationHandler AreaInterfaceHidden;

        public event ActivationHandler AreaInterfaceShown;

        public event DeactivationHandler Deactivated;

        static AreaManager()
        {
            RegisterDependency<TMetadata>(i => _areaMetadata = i);
            RegisterDependency<MainAreaSceneInformationCollection>(
                i => _mainAreaSceneInformationCollection = i
            );
            RegisterDependency<ApplicationManager>(i => _applicationManager = i);
            RegisterDependency<CursorManager>(i => _cursorManager = i);

            _widgets = new List<IAreaWidget>();
            _features = new List<IAreaFeature>();
            _services = new List<IAreaService>();
        }

        #region Static Fields and Autoproperties

        private static ApplicationManager _applicationManager;

        private static CursorManager _cursorManager;

        [InlineProperty, HideLabel, ShowInInspector, Title("Features")]
        private static List<IAreaFeature> _features;

        [InlineProperty, HideLabel, ShowInInspector, Title("Services")]
        private static List<IAreaService> _services;

        [InlineProperty, HideLabel, ShowInInspector, Title("Widgets")]
        private static List<IAreaWidget> _widgets;

        private static MainAreaSceneInformationCollection _mainAreaSceneInformationCollection;

        [NonSerialized, ShowInInspector, InlineEditor]
        private static TMetadata _areaMetadata;

        #endregion

        #region Fields and Autoproperties

        [FormerlySerializedAs("_bootloadData")]
        [SerializeField]
        protected AreaSceneInformation _areaSceneInfo;

        [SerializeField, FoldoutGroup(APPASTR.Components)]
        protected TemplateComponentSet template;

        [SerializeField, FoldoutGroup(APPASTR.Components)]
        protected UICanvasComponentSet canvas;

        [FormerlySerializedAs("view")]
        [SerializeField, FoldoutGroup(APPASTR.Components)]
        protected UIViewComponentSet unscaledView;

        [SerializeField, FoldoutGroup(APPASTR.Components)]
        protected UIViewComponentSet scaledView;

        private IAreaManager _parent;

        [NonSerialized] private bool _isActivated;

        #endregion

        public static IReadOnlyList<IAreaFeature> Features => _features;
        public static IReadOnlyList<IAreaService> Services => _services;

        public static IReadOnlyList<IAreaWidget> Widgets => _widgets;

        protected static MainAreaSceneInformationCollection mainAreaSceneInformationCollection =>
            _mainAreaSceneInformationCollection;

        protected static TMetadata areaMetadata => _areaMetadata;

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

        public UICanvasComponentSet Canvas => canvas;
        public UIViewComponentSet ScaledView => scaledView;
        public UIViewComponentSet UnscaledView => unscaledView;

        protected AreaSceneInformation areaSceneInfo => _areaSceneInfo;

        #region Event Functions

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                if (!_isActivated)
                {
                    return;
                }
#if UNITY_EDITOR

                InitializeAreaTemplate();
#endif
            }
        }

        #endregion

        public void HideAreaInterface()
        {
            using (_PRF_HideAreaInterface.Auto())
            {
                canvas.fadeManager.EnsureFadeOut();

                OnHideAreaInterface();

                AreaInterfaceHidden?.Invoke(Area, this);
            }
        }

        public void ShowAreaInterface()
        {
            using (_PRF_ShowAreaInterface.Auto())
            {
                canvas.fadeManager.EnsureFadeIn();

                OnShowAreaInterface();

                AreaInterfaceShown?.Invoke(Area, this);
            }
        }

        public void ToggleAreaInterface()
        {
            using (_PRF_ToggleAreaInterface.Auto())
            {
                if (canvas.canvasGroup.IsHidden())
                {
                    if (canvas.fadeManager.IsFadingIn)
                    {
                        return;
                    }

                    ShowAreaInterface();
                }
                else
                {
                    if (canvas.fadeManager.IsFadingOut)
                    {
                        return;
                    }

                    HideAreaInterface();
                }
            }
        }

        protected abstract void OnActivation();
        protected abstract void OnDeactivation();
        protected abstract void ResetArea();
        protected abstract AppaTask SetFeaturesToInitialState();
        protected abstract AppaTask SetServicesToInitialState();

        protected abstract AppaTask SetWidgetsToInitialState();

        protected virtual void OnHideAreaInterface()
        {
            using (_PRF_OnHideAreaInterface.Auto())
            {
            }
        }

        protected virtual void OnShowAreaInterface()
        {
            using (_PRF_OnShowAreaInterface.Auto())
            {
            }
        }

        protected static void RegisterFeature<TDependency>(
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>,
            IRepositoryDependencyTracker<TDependency>, IAreaFeature
        {
            using (_PRF_RegisterFeature.Auto())
            {
                var wrapper = new SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler(
                    i =>
                    {
                        _features.Add(i);
                        handler(i);
                    }
                );

                _dependencyTracker.RegisterDependency(wrapper);
            }
        }

        protected static void RegisterService<TDependency>(
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>,
            IRepositoryDependencyTracker<TDependency>, IAreaService
        {
            using (_PRF_RegisterService.Auto())
            {
                var wrapper = new SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler(
                    i =>
                    {
                        _services.Add(i);
                        handler(i);
                    }
                );

                _dependencyTracker.RegisterDependency(wrapper);
            }
        }

        protected static void RegisterWidget<TDependency>(
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>,
            IRepositoryDependencyTracker<TDependency>, IAreaWidget
        {
            using (_PRF_RegisterWidget.Auto())
            {
                var wrapper = new SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler(
                    i =>
                    {
                        _widgets.Add(i);
                        handler(i);
                    }
                );

                _dependencyTracker.RegisterDependency(wrapper);
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                var type = GetType();
                var sceneName = type.Name.Replace("Manager", string.Empty);

                if (gameObject.scene.name != sceneName)
                {
                    Context.Log.Warn(ZString.Format("Scene name should be {0}.", sceneName));
                }

                gameObject.transform.SetToOrigin();

                AreaRegistry.RegisterManager(this);

                name = typeof(TManager).Name;

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

                using (_PRF_Initialize.Suspend())
                {
                    initializer.Do(
                        this,
                        nameof(MainAreaSceneInformationCollection),
                        _areaSceneInfo == null,
                        () =>
                        {
                            using (_PRF_Initialize.Auto())
                            {
                                _areaSceneInfo ??= _mainAreaSceneInformationCollection.Lookup.Items.Get(Area);
                            }
                        }
                    );
                }

                canvas.Configure(gameObject, fullObjectName);
                areaMetadata.Apply(canvas);

                unscaledView.Configure(canvas.GameObject, fullObjectName);
                areaMetadata.Apply(unscaledView);

                template.Configure(unscaledView.GameObject, fullObjectName);
                areaMetadata.Apply(template, gameObject, canvas.GameObject, unscaledView.GameObject);

                if (HasParent)
                {
                    var parent = AreaRegistry.GetManager(ParentArea);

                    if (parent != null)
                    {
                        canvas.graphController.enabled = false;
                    }
                }

                /*if (areaMetadata.menu.doesDrawMenu)
                {
                    GameObject uiMenuManagerGameObject = null;

                    view.GameObject.GetOrCreateChild(
                        ref uiMenuManagerGameObject,
                        ZString.Format("Menu - {0}", fullObjectName),
                        true
                    );

                    uiMenuManagerGameObject.GetOrCreateComponent(ref uiMenuManager);

#if UNITY_EDITOR
                    uiMenuManager.CreateMetadata(Area.ToString());
#endif
                }*/

                var applicationManagerObject = _applicationManager.gameObject;
                var applicationManagerInvalid = (applicationManagerObject == null) ||
                                                (applicationManagerObject.scene == default) ||
                                                !applicationManagerObject.scene.isLoaded;

                if (_applicationManager.RunningAsSubScene &&
                    (!_applicationManager.HasSubSceneManagerBeenIdentified || applicationManagerInvalid))
                {
                    _applicationManager.PrimarySubSceneArea = Area;

                    if (LifetimeComponentManager == null)
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

                await SetFeaturesToInitialState();
                await SetServicesToInitialState();
                await SetWidgetsToInitialState();
            }
        }

        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();

            using (_PRF_WhenDisabled.Auto())
            {
                if (_isActivated)
                {
                    Deactivate();
                }
            }
        }

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
            template.fadeManager.FadeOutCompleted -= DisableTemplate;
        }

        private void OnTemplateFadeInComplete()
        {
            var fadeRange = template.fadeManager.fadeSettings.fadeRange;
            fadeRange.x = areaMetadata.templates.templateAlpha;
            template.fadeManager.fadeSettings.fadeRange = fadeRange;
            template.fadeManager.FadeInCompleted -= OnTemplateFadeInComplete;
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

                    areaMetadata.cursor.Apply(_cursorManager);

                    areaMetadata.input.onEnableMapState.Apply(InputActions, this as TManager);

                    OnActivation();

                    Activated?.Invoke(Area, this);
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

                    throw;
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

                    areaMetadata.input.onDisableMapState.Apply(InputActions, this as TManager);

                    OnDeactivation();

                    Deactivated?.Invoke(Area, this);
                }
                catch (Exception ex)
                {
                    Context.Log.Error(nameof(Deactivate).GenericMethodException(this), this, ex);

                    throw;
                }
            }
        }

        public abstract ApplicationArea Area { get; }

        public abstract ApplicationArea ParentArea { get; }

        public bool HasParent => ParentArea != ApplicationArea.None;

        public string AreaName => Area.ToString();

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_HideAreaInterface =
            new ProfilerMarker(_PRF_PFX + nameof(HideAreaInterface));

        protected static readonly ProfilerMarker _PRF_OnHideAreaInterface =
            new ProfilerMarker(_PRF_PFX + nameof(OnHideAreaInterface));

        protected static readonly ProfilerMarker _PRF_OnShowAreaInterface =
            new ProfilerMarker(_PRF_PFX + nameof(OnShowAreaInterface));

        protected static readonly ProfilerMarker _PRF_SetFeaturesToInitialState =
            new ProfilerMarker(_PRF_PFX + nameof(SetFeaturesToInitialState));

        protected static readonly ProfilerMarker _PRF_SetServicesToInitialState =
            new ProfilerMarker(_PRF_PFX + nameof(SetServicesToInitialState));

        protected static readonly ProfilerMarker _PRF_SetWidgetsToInitialState =
            new ProfilerMarker(_PRF_PFX + nameof(SetWidgetsToInitialState));

        private static readonly ProfilerMarker _PRF_ShowAreaInterface =
            new ProfilerMarker(_PRF_PFX + nameof(ShowAreaInterface));

        private static readonly ProfilerMarker _PRF_ToggleAreaInterface =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleAreaInterface));

        private static readonly ProfilerMarker _PRF_RegisterService =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterService));

        private static readonly ProfilerMarker _PRF_RegisterWidget =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterWidget));

        private static readonly ProfilerMarker _PRF_RegisterFeature =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterFeature));

        private static readonly ProfilerMarker
            _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        private static readonly ProfilerMarker _PRF_OnActivation =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_OnDeactivation =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        #endregion
    }
}
