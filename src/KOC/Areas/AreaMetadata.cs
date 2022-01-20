using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.UI.Controls.Sets.Canvas;
using Appalachia.UI.Controls.Sets.RootCanvas;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Areas
{
    public abstract partial class AreaMetadata<TManager, TMetadata> : SingletonAppalachiaObject<TMetadata>,
                                                                      IAreaMetadata
        where TManager : AreaManager<TManager, TMetadata>
        where TMetadata : AreaMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Input, Expanded = false)]
        public AreaMetadataConfigurations.AreaInputConfiguration input;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas, Expanded = false)]
        public RootCanvasComponentSetStyle rootCanvas;

        [FormerlySerializedAs("scaledView")]
        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Scaled_View, Expanded = false)]
        public CanvasComponentSetStyle view;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Scene_Behaviour, Expanded = false)]
        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration sceneBehaviour;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Audio, Expanded = false)]
        public AreaMetadataConfigurations.AreaAudioConfiguration audio;

        #endregion

        [FoldoutGroup(FOLDOUT_GROUP_), PropertyOrder(-100)]
        [ShowInInspector, ReadOnly]
        public abstract AreaVersion Version { get; }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);
            using (_PRF_Initialize.Auto())
            {
                AreaRegistry.RegisterMetadata(this);

                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    return;
                }

                initializer.Reset(this, "2021-11-20a");
            }

            initializer.Do(
                this,
                APPASTR.Input,
                input.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        input.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Root_Canvas,
                rootCanvas == null,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        rootCanvas = RootCanvasComponentSetStyle.LoadOrCreateNew<RootCanvasComponentSetStyle>(
                            nameof(RootCanvasComponentSetStyle),
                            ownerType: typeof(ApplicationManager)
                        );
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Scaled_View,
                view == null,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        view = CanvasComponentSetStyle.LoadOrCreateNew<CanvasComponentSetStyle>(
                            $"Scaled{nameof(CanvasComponentSetStyle)}",
                            ownerType: typeof(ApplicationManager)
                        );
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Scene_Behaviour,
                sceneBehaviour.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        sceneBehaviour.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Audio,
                audio.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        audio.Initialize(Area);
                    }
                }
            );

#if UNITY_EDITOR
            InitializeEditor(initializer);
#endif
        }

        private IAreaManager GetManager()
        {
            using (_PRF_GetManager.Auto())
            {
                return AreaRegistry.GetManager(Area);
            }
        }

        #region IAreaMetadata Members

        public CanvasComponentSetStyle ScaledView => view;

        public AreaMetadataConfigurations.AreaInputConfiguration Input => input;

        public RootCanvasComponentSetStyle RootCanvas => rootCanvas;

        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration SceneBehaviour => sceneBehaviour;
        public AreaMetadataConfigurations.AreaAudioConfiguration Audio => audio;

        public abstract ApplicationArea Area { get; }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetManager =
            new ProfilerMarker(_PRF_PFX + nameof(GetManager));

        #endregion
    }
}
