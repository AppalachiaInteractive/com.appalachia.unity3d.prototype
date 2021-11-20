using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Aspects.Criticality;
using Appalachia.Core.Behaviours;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Components;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Scenes;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    [ExecuteAlways]
    [Serializable]
    [InspectorIcon(Icons.Squirrel.Blue)]
    public abstract class AreaManager<T, TM> : SingletonAppalachiaBehaviour<T>, IAreaManager
        where T : AreaManager<T, TM>
        where TM : AreaMetadata<T, TM>
    {
        #region Fields and Autoproperties

        [SerializeField] public TM metadata;

        protected ApplicationLifetimeComponents lifetimeComponents;

        [SerializeField] protected SceneBootloadData _bootloadData;

        [SerializeField] protected UICanvasAreaComponentSet canvas;
        [SerializeField] protected UIViewComponentSet view;
        [SerializeField] protected CriticalReferenceHolder criticalReferences;

        #endregion

        protected SceneBootloadData bootloadData => _bootloadData;

        #region Event Functions

        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                base.OnEnable();
                
                AppaLog.Context.Area.Info(nameof(OnEnable));
                Initialize();
                Activate();
            }
        }

        protected override void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                base.OnDisable();
                
                AppaLog.Context.Area.Info(nameof(OnDisable));
                Deactivate();
            }
        }

        #endregion

        protected abstract void ResetArea();

        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                base.Awake();
                
                AppaLog.Context.Area.Info(nameof(Awake));

                Initialize();
            }
        }

        public override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();
                
                AppaLog.Context.Area.Info(nameof(Initialize));

                AreaManagerRegistry.Register(this);

                if (metadata == null)
                {
                    metadata = SingletonAppalachiaObject<TM>.instance;
                    SetDirty();
                }

                if (lifetimeComponents == null)
                {
                    lifetimeComponents = ApplicationLifetimeComponents.instance;
                    SetDirty();
                }

                if (_bootloadData == null)
                {
                    _bootloadData = AreaSceneBootloadDataCollection.instance.GetByArea(Area);
                }

                name = typeof(T).Name;

                var baseObjectName = name.Replace("Manager", string.Empty);

                var fullObjectName = $"{baseObjectName} - {metadata.viewName}";

                canvas.Configure(gameObject, fullObjectName);
                metadata.Apply(canvas);

                view.Configure(canvas.canvas.gameObject, fullObjectName);
                metadata.Apply(view);

                if (HasParent)
                {
                    var parent = AreaManagerRegistry.GetManager(ParentArea);

                    if (parent != null)
                    {
                        canvas.graphController.enabled = false;
                    }
                }

                gameObject.GetOrCreateComponent(ref criticalReferences);
            }
        }

        #region IAreaManager Members

        public abstract ApplicationArea Area { get; }

        public abstract ApplicationArea ParentArea { get; }

        public abstract bool HasParent { get; }

        public string AreaName => Area.ToString();

        public abstract void Activate();

        public abstract void Deactivate();

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AreaManager<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_OnDisable =
            new ProfilerMarker(_PRF_PFX + nameof(OnDisable));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
