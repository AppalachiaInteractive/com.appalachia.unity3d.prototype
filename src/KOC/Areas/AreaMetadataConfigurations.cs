using System;
using Appalachia.Prototype.KOC.Input;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas
{
    [Serializable]
    public struct AreaMetadataConfigurations
    {
        #region Nested type: AreaAudioConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaAudioConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [SerializeField] public AudioMixerGroup mixerGroup;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaCanvasScalingConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaCanvasScalingConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [Range(0f, 1f)]
            [SerializeField]
            public float match;

            [Range(1f, 200f)]
            [SerializeField]
            public float referencePixelsPerUnit;

            [SerializeField] public CanvasScaler.ScaleMode uiScaleMode;

            [SerializeField] public CanvasScaler.ScreenMatchMode screenMatchMode;

            [SerializeField] public Vector2 referenceResolution;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
                using (_PRF_Initialize.Auto())
                {
                }
            }

            #endregion

            #region Profiling

            private const string _PRF_PFX = nameof(AreaCanvasScalingConfiguration) + ".";

            private static readonly ProfilerMarker _PRF_Initialize =
                new ProfilerMarker(_PRF_PFX + nameof(Initialize));

            #endregion
        }

        #endregion

        #region Nested type: AreaInputConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaInputConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [Title("On Disable")]
            [SerializeField]
            public KOCInputActions.MapEnableState onDisableMapState;

            [Title("On Enable")]
            [SerializeField]
            public KOCInputActions.MapEnableState onEnableMapState;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaSceneBehaviourConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaSceneBehaviourConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [ToggleLeft]
            [SerializeField]
            public bool setEntrySceneActive;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
            }

            #endregion
        }

        #endregion

        #region Nested type: IAreaConfiguration

        public interface IAreaConfiguration
        {
            bool ShouldForceReinitialize { get; }

            void Initialize(ApplicationArea area);
        }

        #endregion
    }
}
