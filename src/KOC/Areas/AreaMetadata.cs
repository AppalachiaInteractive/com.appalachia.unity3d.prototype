using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.UI.Functionality.Canvas.Controls.Root;
using Appalachia.UI.Functionality.Tooltips.Styling;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas
{
    [Serializable]
    public abstract partial class AreaMetadata<TManager, TMetadata> : SingletonAppalachiaObject<TMetadata>,
                                                                      IAreaMetadata
        where TManager : AreaManager<TManager, TMetadata>
        where TMetadata : AreaMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField]
        [FoldoutGroup(COMMON_FOLDOUT_GROUP_INNER + APPASTR.Input, Expanded = false)]
        [OnValueChanged(nameof(OnChanged))]
        public AreaMetadataConfigurations.AreaInputConfiguration input;

        [FoldoutGroup(COMMON_FOLDOUT_GROUP, Expanded = false)]
        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        public RootCanvasControlConfig rootCanvas;

        [SerializeField]
        [FoldoutGroup(COMMON_FOLDOUT_GROUP_INNER + APPASTR.Scene_Behaviour, Expanded = false)]
        [OnValueChanged(nameof(OnChanged))]
        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration sceneBehaviour;

        [SerializeField]
        [FoldoutGroup(COMMON_FOLDOUT_GROUP_INNER + APPASTR.Audio, Expanded = false)]
        [OnValueChanged(nameof(OnChanged))]
        public AreaMetadataConfigurations.AreaAudioConfiguration audio;

        [SerializeField]
        [FoldoutGroup(COMMON_FOLDOUT_GROUP, Expanded = false)]
        [OnValueChanged(nameof(OnChanged))]
        public TooltipStyleTypes tooltipStyle;
        
        [SerializeField]
        [FoldoutGroup(COMMON_FOLDOUT_GROUP, Expanded = false)]
        [OnValueChanged(nameof(OnChanged))]
        public TooltipStyleTypes fontStyle;

        #endregion

        [FoldoutGroup(COMMON_FOLDOUT_GROUP), PropertyOrder(-100)]
        [ShowInInspector, ReadOnly]
        public abstract AreaVersion Version { get; }

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

            RootCanvasControlConfig.Refresh(ref rootCanvas, this);

#if UNITY_EDITOR
            InitializeEditor(initializer);
#endif
        }

        /// <summary>
        ///     Returns an asset name which which concatenates the current area
        ///     with the <see cref="Type" />.<see cref="Type.Name" /> of the provided <see cref="T" />.
        /// </summary>
        /// <typeparam name="T">The type whose name should be the second half of the output name.</typeparam>
        /// <returns>The formatted name.</returns>
        /// <example>
        ///     If the area is "MyArea", and
        ///     <see cref="T" /> is "MySpecialComponent", the resulting output will be:
        ///     "MyAreaMySpecialComponent"
        /// </example>
        protected string GetAssetName<T>()
        {
            using (_PRF_GetAssetName.Auto())
            {
                return Area + typeof(T).Name;
            }
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

        private static readonly ProfilerMarker _PRF_GetAssetName = new ProfilerMarker(_PRF_PFX + nameof(GetAssetName));

        private static readonly ProfilerMarker _PRF_GetManager = new ProfilerMarker(_PRF_PFX + nameof(GetManager));

        #endregion
    }
}
