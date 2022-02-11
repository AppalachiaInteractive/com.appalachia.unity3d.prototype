using System;
using Appalachia.Prototype.KOC.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

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
