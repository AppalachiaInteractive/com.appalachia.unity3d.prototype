#region

using System;
using Appalachia.Audio.Contextual.Context;
using Appalachia.Audio.Contextual.Context.Contexts;
using Appalachia.Audio.Core;
using Appalachia.Audio.Scriptables;
using Appalachia.Prototype.KOC.Features.Character.Audio.Sounds;
using Appalachia.Prototype.KOC.Features.Character.States;
using Appalachia.Utility.Timing;
using UnityEngine;

#endregion

namespace Appalachia.Prototype.KOC.Features.Character.Audio.Execution
{
    [Serializable]
    public class CharacterBreathingAudioProcessor : CharacterAudioExecutionProcessor<HumanBreathingSounds,
        AudioContext3, AudioContextParameters3>
    {
        /// <inheritdoc />
        public override void Direct(
            CharacterAudioExecutionManagerBehaviour owner,
            out Patch patch,
            out AudioParameters.EnvelopeParams envelope,
            out Vector3 position,
            out float volume)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void OnJump(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
            var breathing = player.state.breathing;

            if (breathing.inhaling && (Mathf.Abs(breathing.time - breathing.period) >= 1f))
            {
                breathing.time = 0f;
            }
        }

        /// <inheritdoc />
        public override void OnLand(
            PlayerCharacter player,
            CharacterAudioExecutionManagerBehaviour audioManager)
        {
            var breathing = player.state.breathing;
            if (!breathing.inhaling && (Mathf.Abs(breathing.time - breathing.period) >= 0.667f))
            {
                breathing.time = 0f;
            }
        }

        /// <inheritdoc />
        public override bool Update(
            CharacterAudioExecutionManagerBehaviour owner,
            out Patch patch,
            out AudioParameters.EnvelopeParams envelope,
            out Vector3 position,
            out float volume)
        {
            var player = owner.player;
            var state = player.state;
            var positioning = state.positioning;
            var movement = state.movement;
            var breathing = state.breathing;
            var settings = player.settings;
            var breathingSettings = settings.breathing;

            var intensity = movement.jumping ? 1f : movement.speedScalar;

            var playerJumping = movement.jumping || movement.jumpStart || positioning.hasNoFeetPlanted;

            if (positioning.hasAnyFootPlanted && !movement.swimming)
            {
                var deltaTime = CoreClock.Instance.DeltaTime;

                if (playerJumping)
                {
                    deltaTime = 0f; // infinite time dilation when jumping to synchronize landing
                }

                breathing.nextIntensity += (intensity - breathingSettings.intensityDampening) * deltaTime;
                breathing.nextIntensity = Mathf.Clamp01(breathing.nextIntensity);
                breathing.nextPace = Mathf.Lerp(
                    breathing.nextPace,
                    breathing.nextIntensity * breathing.nextIntensity * breathing.nextIntensity,
                    deltaTime * breathingSettings.intensityTransference
                );

                breathing.period = breathingSettings.GetBreathingPeriod() -
                                   (breathing.nextIntensity *
                                    breathingSettings.breathingPeriodIntensityFactor);
                breathing.time -= deltaTime / breathing.period;

                if (breathing.time <= 0f)
                {
                    if (breathing.state == BreathDirection.Inhale)
                    {
                        breathing.state = BreathDirection.Exhale;
                        breathing.time = breathingSettings.GetExhalePeriod();
                    }
                    else
                    {
                        breathing.currentPace = breathing.nextPace;
                        breathing.currentIntensity = breathing.nextIntensity;

                        breathing.state = BreathDirection.Inhale;
                        breathing.time = breathingSettings.GetInhalePeriod();
                        breathing.time -= breathing.time *
                                          breathing.currentPace *
                                          breathingSettings.inhalePeriodPacingFactor;

                        breathing.style = breathing.currentPace > .5f
                            ? RespirationStyle.Mouth
                            : RespirationStyle.Nose;

                        if (breathing.style == RespirationStyle.Nose)
                        {
                            breathing.speed = breathing.currentIntensity >= 0.66f
                                ? RespirationSpeed.Fast
                                : breathing.currentIntensity >= .33f
                                    ? RespirationSpeed.Normal
                                    : RespirationSpeed.Slow;
                        }
                        else
                        {
                            breathing.speed = breathing.currentIntensity >= 0.5f
                                ? RespirationSpeed.Normal
                                : RespirationSpeed.Slow;
                        }
                    }

                    volume = breathingSettings.GetVolumeOverPace();
                    volume = Mathf.Clamp01(volume + (breathing.currentPace * volume));

                    var bestPatch = audio.GetBest(
                        Health_AudioContexts.Healthy,
                        breathing.speed.ToAudio(),
                        breathing.style.ToAudio(),
                        out var successful
                    );

                    patch = successful ? bestPatch % (breathing.state == BreathDirection.Inhale) : default;
                    envelope = default;
                    position = successful ? owner.player.parts.mouth.position : default;

                    return true;
                }
            }

            volume = 0f;
            patch = default;
            envelope = default;
            position = default;
            return false;
        }

        /// <inheritdoc />
        protected override void OnInitialize(CharacterAudioExecutionManagerBehaviour owner)
        {
        }
    }
}
