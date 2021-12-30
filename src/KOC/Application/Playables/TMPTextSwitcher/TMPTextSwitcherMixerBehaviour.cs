using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Appalachia.Prototype.KOC.Application.Playables.TMPTextSwitcher
{
    public class TMPTextSwitcherMixerBehaviour : AppalachiaPlayable<TMPTextSwitcherMixerBehaviour>
    {
        #region Fields and Autoproperties

        private Color m_DefaultColor;
        private float m_DefaultFontSize;
        private string m_DefaultText;

        private TextMeshProUGUI m_TrackBinding;
        private bool m_FirstFrameHappened;

        #endregion

        public override void OnPlayableDestroy(Playable playable)
        {
            if (m_TrackBinding != null)
            {
                m_TrackBinding.color = m_DefaultColor;
                m_TrackBinding.fontSize = m_DefaultFontSize;
                m_TrackBinding.text = m_DefaultText;
            }

            m_FirstFrameHappened = false;
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await AppaTask.CompletedTask;
        }

        protected override void OnPause(Playable playable, FrameData info)
        {
        }

        protected override void OnPlay(Playable playable, FrameData info)
        {
        }

        protected override void Update(Playable playable, FrameData info, object playerData)
        {
            m_TrackBinding = playerData as TextMeshProUGUI;

            if (m_TrackBinding == null)
            {
                return;
            }

            if (!m_FirstFrameHappened)
            {
                m_DefaultColor = m_TrackBinding.color;
                m_DefaultFontSize = m_TrackBinding.fontSize;
                m_DefaultText = m_TrackBinding.text;
                m_FirstFrameHappened = true;
            }

            var inputCount = playable.GetInputCount();

            var blendedColor = Color.clear;
            var blendedFontSize = 0f;
            var totalWeight = 0f;
            var greatestWeight = 0f;
            var currentInputs = 0;

            for (var i = 0; i < inputCount; i++)
            {
                var inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<TMPTextSwitcherBehaviour>)playable.GetInput(i);
                var input = inputPlayable.GetBehaviour();

                blendedColor += input.color * inputWeight;
                blendedFontSize += input.fontSize * inputWeight;
                totalWeight += inputWeight;

                if (inputWeight > greatestWeight)
                {
                    m_TrackBinding.text = input.text;
                    greatestWeight = inputWeight;
                }

                if (!Mathf.Approximately(inputWeight, 0f))
                {
                    currentInputs++;
                }
            }

            m_TrackBinding.color = blendedColor + (m_DefaultColor * (1f - totalWeight));
            m_TrackBinding.fontSize =
                Mathf.RoundToInt(blendedFontSize + (m_DefaultFontSize * (1f - totalWeight)));
            if ((currentInputs != 1) && ((1f - totalWeight) > greatestWeight))
            {
                m_TrackBinding.text = m_DefaultText;
            }
        }

        protected override void WhenDestroyed(Playable playable)
        {
        }

        protected override void WhenStarted(Playable playable)
        {
        }

        protected override void WhenStopped(Playable playable)
        {
        }
    }
}
