using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Character;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Gameplay
{
    [SelectionBase]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]

//[RequireComponent(typeof(PlayerFoley))]
    public class PlayerController : GameAgent<PlayerController>
    {
        #region Fields and Autoproperties

        public CharacterController characterController { get; private set; }

        public PlayerCamera playerCamera { get; set; }
        public PlayerCharacter playerCharacter { get; private set; }

        #endregion

        #region Event Functions

        protected void Update()
        {
            var t = transform;

            //BOTDPlayerInput.Update(out var input);

            //if (input.move.y < 0f)
            //{
            //    input.look.y = -input.look.y;
            //}

            /*if (playerCharacter)
            {
                playerCharacter.Simulate(characterController, input);
            }*/
        }

        protected void LateUpdate()
        {
            using (_PRF_LateUpdate.Auto())
            {
                var firstPersonCamera = playerCamera as FirstPersonCamera;

                if (firstPersonCamera && playerCharacter)
                {
                    float pitch, yaw;
                    playerCharacter.GetLookPitchAndYaw(out pitch, out yaw);
                    firstPersonCamera.pitch = pitch;
                    firstPersonCamera.yaw = yaw;
                }

                var t = transform;
                var position = t.localPosition;
                var rotation = t.localEulerAngles;
                var deltaTime = Time.deltaTime;

                playerCamera.Simulate(position, rotation, deltaTime);
            }
        }

        #endregion

        public override void OnSpawn(SpawnPoint spawnPoint, bool reset)
        {
            using (_PRF_OnSpawn.Auto())
            {
                base.OnSpawn(spawnPoint, reset);

                var rotation = transform.localEulerAngles;
                var angles = spawnPoint.transform.localEulerAngles;

                if (playerCharacter)
                {
                    playerCharacter.OnSpawn(angles, reset, characterController);
                }

                playerCamera.OnSpawn(spawnPoint);

                var position = transform.localPosition;

                playerCamera.Warp(position, angles);

                for (var i = 0; i < 4; ++i)
                {
                    playerCamera.Simulate(position, rotation, 0.1f);
                }
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                characterController = GetComponent<CharacterController>();
                playerCharacter = GetComponent<PlayerCharacter>();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(PlayerController) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_OnSpawn = new ProfilerMarker(_PRF_PFX + nameof(OnSpawn));

        private static readonly ProfilerMarker _PRF_LateUpdate =
            new ProfilerMarker(_PRF_PFX + nameof(LateUpdate));

        #endregion
    }
} // Gameplay
