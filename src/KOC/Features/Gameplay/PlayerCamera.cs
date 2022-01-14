using Appalachia.Core.Objects.Root;
using Appalachia.Rendering.PostProcessing.AutoFocus;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Features.Gameplay
{
    public abstract class PlayerCamera : AppalachiaBehaviour<PlayerCamera>
    {
        #region Fields and Autoproperties

        public Transform eyeTransform;

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public DepthOfFieldAutoFocus autoFocus { get; private set; }

        private Camera _playerCamera;

        #endregion

        public Camera playerCamera
        {
            get => _playerCamera;
            set => _playerCamera = value;

            //autoFocus = value.GetComponent<DepthOfFieldAutoFocus>();
        }

        // ReSharper disable once UnusedParameter.Global
        public virtual void OnSpawn(SpawnPoint spawnPoint)
        {
        }

        public virtual void Simulate(Vector3 playerPosition, Vector3 playerAngles, float deltaTime)
        {
        }

        public virtual void Warp(Vector3 position, Vector3 angles)
        {
        }
    }
} // Gameplay
