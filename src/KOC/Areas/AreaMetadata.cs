using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Sets;
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

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP_INNER + APPASTR.Input, Expanded = false)]
        public AreaMetadataConfigurations.AreaInputConfiguration input;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP, Expanded = false)]
        public RootCanvasComponentSetData rootCanvas;

        [FormerlySerializedAs("scaledView")]
        [SerializeField, FoldoutGroup(FOLDOUT_GROUP, Expanded = false)]
        public CanvasComponentSetData view;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP_INNER + APPASTR.Scene_Behaviour, Expanded = false)]
        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration sceneBehaviour;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP_INNER + APPASTR.Audio, Expanded = false)]
        public AreaMetadataConfigurations.AreaAudioConfiguration audio;

        #endregion

        [FoldoutGroup(FOLDOUT_GROUP), PropertyOrder(-100)]
        [ShowInInspector, ReadOnly]
        public abstract AreaVersion Version { get; }

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

        public CanvasComponentSetData View => view;

        public AreaMetadataConfigurations.AreaInputConfiguration Input => input;

        public RootCanvasComponentSetData RootCanvas => rootCanvas;

        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration SceneBehaviour => sceneBehaviour;
        public AreaMetadataConfigurations.AreaAudioConfiguration Audio => audio;

        public abstract ApplicationArea Area { get; }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetManager =
            new ProfilerMarker(_PRF_PFX + nameof(GetManager));

        private static readonly ProfilerMarker _PRF_UpdateComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSet));

        #endregion
    }
}
