#region

using System;
using Appalachia.Audio.Contextual.Context.Contexts;
using Appalachia.Audio.Core;
using Appalachia.Audio.Scriptables;
using Appalachia.Prototype.KOC.Character.Audio.Sounds;
using UnityEngine;

#endregion

namespace Appalachia.Prototype.KOC.Character.Audio.Execution
{
    [Serializable]
    public class CharacterFootstepAudioProcessor : CharacterAudioExecutionProcessor<FootstepSounds,
        AudioContext3, AudioContextParameters3>
    {
        

        [Tooltip("Amount of attenuation when walking on flat ground opposed to rocky terrain")]
        [Range(0, 1)]
        public float footstepElevationAttenuation = 0.5f;

        [Tooltip("Amount of attenuation when walking opposed to running")]
        [Range(0, 1)]
        public float footstepSpeedAttenuation = 0.5f;

        public override void Direct(
            CharacterAudioExecutionManagerBehaviour owner,
            out Patch patch,
            out AudioParameters.EnvelopeParams envelope,
            out Vector3 position,
            out float volume)
        {
            throw new NotImplementedException();
        }

        public override bool Update(
            CharacterAudioExecutionManagerBehaviour owner,
            out Patch patch,
            out AudioParameters.EnvelopeParams envelope,
            out Vector3 position,
            out float volume)
        {
            throw new NotImplementedException();

            /*var foley = GetFoleyAsset();

            if (foley && (foley.footsteps.Length > 0))
            {
                var asset = GetFootstepAsset(foley, position, physicalMaterial, speedScalar, VegetationType.None, landing);

                var elevationFactor = Vector3.Dot(normal, Vector3.up);
                var attackScalar = elevationFactor * elevationFactor * elevationFactor;
                var envelope = new Parameters.EnvelopeParams
                {
                    attack = Mathf.Lerp(
                        0f,
                        Mathf.Clamp01((landing ? 0.1f : 1f) * foley.footstepElevationAttenuation * (1f - speedScalar)),
                        attackScalar
                    )
                };
                
                volume = Mathf.Lerp(1f - foley.footstepSpeedAttenuation, 1f, speedScalar);

                Synthesizer.KeyOn(out _, asset, envelope, t, volume: volume);
            }*/
        }

        protected override void OnInitialize(CharacterAudioExecutionManagerBehaviour owner)
        {
        }

        /*public void PlayFootstep(
            Transform t,
            Vector3 position,
            Vector3 normal,
            PhysicMaterial physicalMaterial,
            float speedScalar,
            bool landing = false)
        {
        }
        private Patch GetFootstepAsset(
            FoleyAudioCollection foley,
            Vector3 position,
            PhysicMaterial physicalMaterial,
            float speedScalar,
            PrefabContentType type,
            bool landing = false)
        {
            ContextualAudio.Footstep footstep;
            
            int footstepIndex;
            var validFootstep = false;

            if (physicalMaterial != null)
            {
                if (_materialMap.TryGetValue(physicalMaterial, out footstepIndex))
                {
                    footstep = foley.footsteps[footstepIndex];
                    validFootstep = true;
                }
            }

            if (!validFootstep)
            {
                var terrainMetadata = TerrainMetadataManager.GetTerrainAt(position);
                footstepIndex = terrainMetadata.GetFoleyIndexAtPosition(position);
                footstep = foley.footsteps[footstepIndex];
                validFootstep = true;
            }

            Debug.Assert(validFootstep);

            var running = speedScalar >= 0.7f;
            var jogging = !running && (speedScalar >= 0.2f);

            var asset = type == PrefabContentType.None
                ? landing
                    ? footstep.landing
                    : running
                        ? footstep.running
                        : jogging
                            ? footstep.jogging
                            : footstep.walking
                : type == VegetationType.Undergrowth
                    ? running
                        ? footstep.runningUndergrowth
                        : jogging
                            ? footstep.joggingUndergrowth
                            : footstep.walkingUndergrowth
                    : null;

            return asset;
        }
        */

        /*private Patch GetFootstepAsset(
            FoleyAudioCollection foley,
            Vector3 position,
            PhysicMaterial physicalMaterial,
            float speedScalar,
            PrefabContentType type,
            bool landing = false)
        {
            ContextualAudio.Footstep footstep;
            
            int footstepIndex;
            var validFootstep = false;

            if (physicalMaterial != null)
            {
                if (_materialMap.TryGetValue(physicalMaterial, out footstepIndex))
                {
                    footstep = foley.footsteps[footstepIndex];
                    validFootstep = true;
                }
            }

            if (!validFootstep)
            {
                var terrainMetadata = TerrainMetadataManager.GetTerrainAt(position);
                footstepIndex = terrainMetadata.GetFoleyIndexAtPosition(position);
                footstep = foley.footsteps[footstepIndex];
                validFootstep = true;
            }

            Debug.Assert(validFootstep);

            var running = speedScalar >= 0.7f;
            var jogging = !running && (speedScalar >= 0.2f);

            var asset = type == PrefabContentType.None
                ? landing
                    ? footstep.landing
                    : running
                        ? footstep.running
                        : jogging
                            ? footstep.jogging
                            : footstep.walking
                : type == VegetationType.Undergrowth
                    ? running
                        ? footstep.runningUndergrowth
                        : jogging
                            ? footstep.joggingUndergrowth
                            : footstep.walkingUndergrowth
                    : null;

            return asset;
        }
        public void PlayFootstep(
            Transform t,
            Vector3 position,
            Vector3 normal,
            PhysicMaterial physicalMaterial,
            float speedScalar,
            bool landing = false)
        {
            var foley = GetFoleyAsset();

            if (foley && (foley.footsteps.Length > 0))
            {
                var asset = GetFootstepAsset(foley, position, physicalMaterial, speedScalar, VegetationType.None, landing);

                var elevationFactor = Vector3.Dot(normal, Vector3.up);
                var attackScalar = elevationFactor * elevationFactor * elevationFactor;
                var envelope = new AudioParameters.EnvelopeParams
                {
                    attack = Mathf.Lerp(
                        0f,
                        Mathf.Clamp01((landing ? 0.1f : 1f) * foley.footstepElevationAttenuation * (1f - speedScalar)),
                        attackScalar
                    )
                };
                var volume = Mathf.Lerp(1f - foley.footstepSpeedAttenuation, 1f, speedScalar);

                Synthesizer.KeyOn(out _, asset, envelope, t, volume: volume);
            }
        }*/
    }
}
