using Appalachia.Core.Objects.Root;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Gameplay
{
    public class SpawnPoint : AppalachiaBehaviour<SpawnPoint>
    {
        #region Fields and Autoproperties

        public bool randomizePosition;

        [FormerlySerializedAs("camera")]
        public Camera playerCamera;

        public LayerMask snapLayers = (1 << 0) | (1 << 15);

        #endregion

        public void Spawn<T>(T agent, bool reset)
            where T : GameAgent<T>
        {
            if (reset)
            {
                agent.gameObject.SetActive(false);
            }

            SetPositionAndRotation(agent.transform);

            if (reset)
            {
                agent.gameObject.SetActive(true);
            }

            agent.OnSpawn(this, reset);
        }

        private bool SetPositionAndRotation(Transform agentTransform)
        {
            var offset = randomizePosition ? Random.insideUnitCircle * transform.localScale.y : Vector2.zero;

            if (SnapToFloor(agentTransform, offset))
            {
                agentTransform.localRotation = Quaternion.Euler(0f, transform.localEulerAngles.y, 0f);
                return true;
            }

            agentTransform.localRotation = transform.localRotation;
            return false;
        }

        private bool SnapToFloor(Transform agentTransform, Vector3 offset)
        {
            var position = transform.localPosition + new Vector3(offset.x, 2f, offset.y);
            RaycastHit hit;

            if (Physics.Raycast(position, Vector3.down, out hit, 4f, snapLayers))
            {
                agentTransform.localPosition = hit.point;
                return true;
            }

            Context.Log.Warn("SnapToFloor: Failed to to snap " + agentTransform + " at " + this, this);
            agentTransform.localPosition = position;
            return false;
        }
    }
} // Gameplay
