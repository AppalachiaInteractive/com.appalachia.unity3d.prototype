using Appalachia.CI.Constants;
using Appalachia.Core.ControlModel.Controls;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.UI.Functionality.Canvas.Controls.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas
{
    public abstract partial class AreaMetadata<TManager, TMetadata> : SingletonAppalachiaObject<TMetadata>,
                                                                      IAreaMetadata
        where TManager : AreaManager<TManager, TMetadata>
        where TMetadata : AreaMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField]
        [FoldoutGroup(COMMON_FOLDOUT_GROUP_INNER + APPASTR.Input, Expanded = false)]
        public AreaMetadataConfigurations.AreaInputConfiguration input;

        [FoldoutGroup(COMMON_FOLDOUT_GROUP, Expanded = false)]
        [SerializeField]
        public RootCanvasControlConfig rootCanvas;

        [SerializeField]
        [FoldoutGroup(COMMON_FOLDOUT_GROUP_INNER + APPASTR.Scene_Behaviour, Expanded = false)]
        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration sceneBehaviour;

        [SerializeField]
        [FoldoutGroup(COMMON_FOLDOUT_GROUP_INNER + APPASTR.Audio, Expanded = false)]
        public AreaMetadataConfigurations.AreaAudioConfiguration audio;

        #endregion

        [FoldoutGroup(COMMON_FOLDOUT_GROUP), PropertyOrder(-100)]
        [ShowInInspector, ReadOnly]
        public abstract AreaVersion Version { get; }

        public void UpdateControl<TControl, TConfig>(
            ref TConfig config,
            ref TControl target,
            GameObject parent,
            string setName)
            where TControl : AppaControl<TControl, TConfig>, new()
            where TConfig: AppaControlConfig<TControl, TConfig>, new()
        {
            using (_PRF_UpdateControl.Auto())
            {
                AppaControlConfig<TControl, TConfig>.RefreshAndApply(
                    ref config,
                    ref target,
                    parent,
                    setName,
                    this
                );
            }
        }

        /// <inheritdoc />
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

        public AreaMetadataConfigurations.AreaInputConfiguration Input => input;

        public RootCanvasControlConfig RootCanvas => rootCanvas;

        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration SceneBehaviour => sceneBehaviour;
        public AreaMetadataConfigurations.AreaAudioConfiguration Audio => audio;

        public abstract ApplicationArea Area { get; }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetManager = new ProfilerMarker(_PRF_PFX + nameof(GetManager));

        private static readonly ProfilerMarker _PRF_UpdateControl =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateControl));

        #endregion
    }
}
