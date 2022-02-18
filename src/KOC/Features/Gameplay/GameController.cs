using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Behaviours;
using Appalachia.Utility.Async;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Features.Gameplay
{
    public sealed class GameController : AppalachiaApplicationBehaviour<GameController>
    {
        public delegate void AudioTransformsUpdater(Transform root, Transform eye);

        #region Fields and Autoproperties

        public AudioTransformsUpdater audioTransformsUpdater;
        public PlayerCamera playerCameraPrefab;

        public PlayerController playerPrefab;

        public SpawnPoint startPoint;
        public Camera defaultCamera { get; set; }

        public Object debugContext { get; set; }
        public PlayerCamera playerCamera { get; set; }

        public PlayerController playerController { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public SpawnPoint lastPlayerSpawnPoint { get; private set; }

        #endregion

        public static GameController FindGameController()
        {
            return GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        public void DespawnPlayer()
        {
            if (playerController)
            {
                Destroy(playerController.gameObject);
            }

            if (playerCamera)
            {
                Destroy(playerCamera.gameObject);
            }

            playerController = null;
            playerCamera = null;
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            defaultCamera = Camera.main;

            var cameraTransform = defaultCamera.transform;
            cameraTransform.localPosition = Vector3.zero;
            cameraTransform.localRotation = Quaternion.identity;
            cameraTransform.localScale = Vector3.one;

            //BOTDPlayerInput.SelectInputMapping(); 
        }

        /*public SpawnPoint[] GetAllSpawnPoints()
        {
            return (from a in GameAgent.GetAgents() where a is SpawnPoint select a as SpawnPoint).ToArray();
        }

        public SpawnPoint GetNextPlayerSpawnPoint()
        {
            var spawnPoints = GetAllSpawnPoints();
            Array.Sort(spawnPoints, (x, y) => string.Compare(x.agentIdentifier, y.agentIdentifier));
            var index = Array.IndexOf(spawnPoints, lastPlayerSpawnPoint);
            return spawnPoints[(index + 1) % spawnPoints.Length];
        }

        public SpawnPoint GetSpawnPoint(string name)
        {
            return SpawnPoint.Find(name);
        }

        public void SpawnPlayer(bool reset = true, bool nextFrame = true)
        {
            SpawnPlayer(startPoint, reset, nextFrame);
        }

        public void SpawnPlayer(SpawnPoint spawnPoint, bool reset = true, bool nextFrame = true)
        {
            StartCoroutine(SpawnPlayerCo(spawnPoint, reset, nextFrame));
        }

        private IEnumerator SpawnPlayerCo(SpawnPoint spawnPoint, bool reset, bool nextFrame)
        {
            if (nextFrame)
            {
                yield return null;
            }

            Profiler.BeginSample("SpawnPlayerCo");

            lastPlayerSpawnPoint = spawnPoint;

            if (!playerPrefab)
            {
                Context.Log.Error("Missing player prefab");
                yield break;
            }

            if (!playerController)
            {
                playerController = Object.Instantiate(playerPrefab);
            }

            if (!playerCamera)
            {
                playerCamera = Object.Instantiate(playerCameraPrefab);
            }

            using (var enumerator = GameAgent.GetEnumerator())
            {
                for (var i = enumerator; i.MoveNext();)
                {
                    if (i.Current)
                    {
                        i.Current.OnBeforeSpawnPlayer(reset);
                    }
                }
            }

            var cam = spawnPoint.playerCamera ? spawnPoint.playerCamera : defaultCamera;
            var cameraTransform = cam.transform;
            cameraTransform.parent = playerCamera.eyeTransform
                ? playerCamera.eyeTransform
                : playerCamera.transform;

            cameraTransform.localPosition = Vector3.zero;
            cameraTransform.localRotation = Quaternion.identity;
            cameraTransform.localScale = Vector3.one;

            playerCamera.playerCamera = cam;
            playerController.playerCamera = playerCamera;

            if (audioTransformsUpdater != null)
            {
                audioTransformsUpdater(playerController.transform, cam.transform);
            }

            spawnPoint.Spawn(playerController, reset);

            using (var enumerator = GameAgent.GetEnumerator())
            {
                for (var i = enumerator; i.MoveNext();)
                {
                    if (i.Current)
                    {
                        i.Current.OnAfterSpawnPlayer(spawnPoint, reset);
                    }
                }
            }

            Profiler.EndSample();
        }*/
    }
} // Gameplay
