using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Behaviours;
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
    public abstract partial class AreaManager<TManager, TMetadata> :
        SingletonAppalachiaApplicationBehaviour<TManager>,
        IAreaManager
        where TManager : AreaManager<TManager, TMetadata>
        where TMetadata : AreaMetadata<TManager, TMetadata>
    {
        public delegate void AreaEventHandler(ApplicationArea area, IAreaManager manager);

        public event AreaEventHandler Activated;

        public event AreaEventHandler AreaInterfaceHidden;

        public event AreaEventHandler AreaInterfaceShown;

        public event AreaEventHandler Deactivated;

        static AreaManager()
        {
            RegisterDependency<TMetadata>(i => _areaMetadata = i);
            RegisterDependency<MainAreaSceneInformationCollection>(
                i => _mainAreaSceneInformationCollection = i
            );

            InitializeFunctionality();
        }

        #region Static Fields and Autoproperties

        private static ApplicationManager _applicationManager;

        private static MainAreaSceneInformationCollection _mainAreaSceneInformationCollection;
        private static string _areaObjectName;

        private static string _defaultName;

        [NonSerialized]
        [ShowInInspector]
        [InlineEditor]
        private static TMetadata _areaMetadata;

        #endregion

        #region Fields and Autoproperties

        [FormerlySerializedAs("_bootloadData")]
        [SerializeField]
        [FoldoutGroup(APPASTR.General)]
        protected AreaSceneInformation _areaSceneInfo;

        private IAreaManager _parent;

        [NonSerialized] private bool _isActivated;

        #endregion

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

        protected AreaSceneInformation areaSceneInfo => _areaSceneInfo;

        protected string areaObjectName
        {
            get
            {
                if (_areaObjectName == null)
                {
                    _areaObjectName = defaultName.Replace("Manager", string.Empty);
                }

                return _areaObjectName;
            }
        }

        protected string defaultName
        {
            get
            {
                if (_defaultName == null)
                {
                    _defaultName = typeof(TManager).Name;
                }

                return _defaultName;
            }
        }

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
                UpdateEditor();
#endif
            }
        }

        #endregion

        protected abstract void OnActivation();
        protected abstract void OnDeactivation();
        protected abstract void ResetArea();

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                var type = GetType();
                var correctSceneName = type.Name.Replace("Manager", string.Empty);

                if (gameObject.scene.name != correctSceneName)
                {
                    Context.Log.Warn(ZString.Format("Scene name should be {0}.", correctSceneName));
                }

                gameObject.transform.SetToOrigin();

                AreaRegistry.RegisterManager(this);

                name = defaultName;

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

                initializer.Do(
                    this,
                    nameof(MainAreaSceneInformationCollection),
                    _areaSceneInfo == null,
                    () => { _areaSceneInfo ??= _mainAreaSceneInformationCollection.Lookup.Items.Get(Area); }
                );

                areaMetadata.UpdateControl(
                    ref areaMetadata.rootCanvas,
                    ref rootCanvas,
                    gameObject,
                    areaObjectName
                );

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

#if UNITY_EDITOR
                    InitializeEditor(initializer, areaObjectName);
#endif
                }

                await InitializeFeatures();
            }
        }

        /// <inheritdoc />
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

                    //areaMetadata.cursor.Apply(_cursorManager);

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

        protected static readonly ProfilerMarker _PRF_Activate =
            new ProfilerMarker(_PRF_PFX + nameof(Activate));

        protected static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        protected static readonly ProfilerMarker _PRF_OnActivation =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        protected static readonly ProfilerMarker _PRF_OnDeactivation =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        protected static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        #endregion
    }
}
