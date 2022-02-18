using Appalachia.Audio.Contextual.Context.Contexts;
using Appalachia.Audio.Contextual.Execution;
using Appalachia.Prototype.KOC.Features.Character.Audio.Sounds;
using Appalachia.Utility.Async;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Features.Character.Audio.Execution
{
    [DisallowMultipleComponent]
    public class CharacterAudioExecutionManagerBehaviour : AudioExecutionManagerSingletonBehaviour<
        CharacterAudioExecutionManagerBehaviour>
    {
        #region Fields and Autoproperties

        [SerializeField] public CharacterBreathingAudioProcessor breathing;
        [SerializeField] public CharacterFootstepAudioProcessor footsteps;
        [SerializeField] public PlayerCharacter player;

        #endregion

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                if (breathing == null)
                {
                    breathing = new CharacterBreathingAudioProcessor();
                }

                if (footsteps == null)
                {
                    footsteps = new CharacterFootstepAudioProcessor();
                }

                HandleExecution<CharacterBreathingAudioProcessor, HumanBreathingSounds, AudioContext3,
                    AudioContextParameters3>(this, breathing);
                HandleExecution<CharacterFootstepAudioProcessor, FootstepSounds, AudioContext3,
                    AudioContextParameters3>(this, footsteps);
            }
        }

        #endregion

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                player = FindObjectOfType<PlayerCharacter>();

                player.OnStep += OnStep;
                player.OnJump += OnJump;
                player.OnLand += OnLand;
                player.OnVocalize_Start += OnVocalize_Start;
                player.OnVocalize_End += OnVocalize_End;
                player.OnInWater_Start += OnInWater_Start;
                player.OnInWater_End += OnInWater_End;
                player.OnSwimming_Start += OnSwimming_Start;
                player.OnSwimming_End += OnSwimming_End;
                player.OnUnderWater_Start += OnUnderWater_Start;
                player.OnUnderWater_End += OnUnderWater_End;
                player.OnSleeping_Start += OnSleeping_Start;
                player.OnSleeping_End += OnSleeping_End;
                player.OnDie += OnDie;
            }
        }

        private void OnDie(PlayerCharacter pc)
        {
            breathing.OnDie(pc, this);
            footsteps.OnDie(pc, this);
        }

        private void OnInWater_End(PlayerCharacter pc)
        {
            breathing.OnInWater_End(pc, this);
            footsteps.OnInWater_End(pc, this);
        }

        private void OnInWater_Start(PlayerCharacter pc)
        {
            breathing.OnInWater_Start(pc, this);
            footsteps.OnInWater_Start(pc, this);
        }

        private void OnJump(PlayerCharacter pc)
        {
            breathing.OnJump(pc, this);
            footsteps.OnJump(pc, this);
        }

        private void OnLand(PlayerCharacter pc)
        {
            breathing.OnLand(pc, this);
            footsteps.OnLand(pc, this);
        }

        private void OnSleeping_End(PlayerCharacter pc)
        {
            breathing.OnSleeping_End(pc, this);
            footsteps.OnSleeping_End(pc, this);
        }

        private void OnSleeping_Start(PlayerCharacter pc)
        {
            breathing.OnSleeping_Start(pc, this);
            footsteps.OnSleeping_Start(pc, this);
        }

        /*
        
            [DisallowMultipleComponent]
    public class PlayerFoley : AppalachiaBehaviour<PlayerFoley>
    {
        public FoleyAudioCollection foley;

        private BreathingState _breathingState;

        private VegetationState _vegetationState;

        private Dictionary<PhysicMaterial, int> _materialMap;
        private bool _mapInitialized;

        public float breathingIntensity { get; set; }
        public BreathType breathType { get; set; }
        public bool playerJumping { get; set; }
        public Bounds playerBounds { get; set; }

        public bool intersectingVegetation => _vegetationState.intersecting;

        

        private FoleyAudioCollection GetFoleyAsset()
        {
            var overrideCount = PlayerFoleyZone.overrides.Count;
            while (overrideCount > 0)
            {
                var @override = PlayerFoleyZone.overrides[overrideCount - 1].foley;
                if (@override)
                {
                    return @override;
                }

                --overrideCount;
            }

            return foley;
        }

        

        public void PlayVegetation(Transform t, Vector3 position, float speedScalar)
        {
            var foley = GetFoleyAsset();

            if (foley && (foley.footsteps.Length > 0))
            {
                var asset = GetFootstepAsset(foley, position, null, speedScalar, _vegetationState.type);

                var volume = Mathf.Lerp(1f - foley.footstepSpeedAttenuation, 1f, speedScalar);

                Synthesizer.KeyOn(out _, asset, pos: _vegetationState.position, volume: volume);
            }
        }

        public void OnJump()
        {
            _breathingState.OnJump();
        }

        public void OnLand()
        {
            _breathingState.OnLand();
        }

        protected void LateUpdate()
        {
            var foley = GetFoleyAsset();

            if (!_mapInitialized && foley)
            {
                var terrainFoley = TerrainFoleyManager.current;

                if (terrainFoley)
                {
                    _materialMap = new Dictionary<PhysicMaterial, int>();

                    var list = new List<string>(foley.footsteps.Length);
                    for (int i = 0, n = foley.footsteps.Length; i < n; ++i)
                    {
                        var footstep = foley.footsteps[i];
                        list.Add(footstep.name);

                        if (footstep.physicalMaterial)
                        {
                            _materialMap.Add(footstep.physicalMaterial, i);
                        }
                    }

                    _foleyMap.Initialize(list.ToArray(), terrainFoley.splatMap);
                    _mapInitialized = true;
                }
            }

            if (!_breathingState.initialized && foley)
            {
                _breathingState.Initialize(foley);
            }

            if (_breathingState.initialized)
            {
                Patch patch;
                float volume;

                if (_breathingState.Update(foley, breathingIntensity, breathType, playerJumping, out patch, out volume))
                {
                    bool looping;
                    Synthesizer.KeyOn(out looping, patch, null, Vector3.zero, 0f, volume);
                }
            }

            _vegetationState.Update(playerBounds);
        }

        

        [Serializable]
        private struct VegetationState
        {
            public Vector3 position;

            //public VegetationType type;
            public bool intersecting;

            public void Update(Bounds bounds)
            {
                intersecting = TerrainMetadataManager.QueryVegetation(bounds, out position, out type);                
            }
        }
    }
} // Gameplay
*/

        private void OnStep(PlayerCharacter pc)
        {
            breathing.OnStep(pc, this);
            footsteps.OnStep(pc, this);
        }

        private void OnSwimming_End(PlayerCharacter pc)
        {
            breathing.OnSwimming_End(pc, this);
            footsteps.OnSwimming_End(pc, this);
        }

        private void OnSwimming_Start(PlayerCharacter pc)
        {
            breathing.OnSwimming_Start(pc, this);
            footsteps.OnSwimming_Start(pc, this);
        }

        private void OnUnderWater_End(PlayerCharacter pc)
        {
            breathing.OnUnderWater_End(pc, this);
            footsteps.OnUnderWater_End(pc, this);
        }

        private void OnUnderWater_Start(PlayerCharacter pc)
        {
            breathing.OnUnderWater_Start(pc, this);
            footsteps.OnUnderWater_Start(pc, this);
        }

        private void OnVocalize_End(PlayerCharacter pc)
        {
            breathing.OnVocalize_End(pc, this);
            footsteps.OnVocalize_End(pc, this);
        }

        private void OnVocalize_Start(PlayerCharacter pc)
        {
            breathing.OnVocalize_Start(pc, this);
            footsteps.OnVocalize_Start(pc, this);
        }
    }
}
