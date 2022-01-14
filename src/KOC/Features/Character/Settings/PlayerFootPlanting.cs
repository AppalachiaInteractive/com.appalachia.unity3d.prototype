using System;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Features.Character.Settings
{
    [Serializable]
    public struct PlayerFootPlanting : IEquatable<PlayerFootPlanting>
    {
        #region Fields and Autoproperties

        [PropertyRange(0, 10f)]
        [Tooltip("[0, 10]")]
        public float runStepDistance;

        [PropertyRange(0, 10f)]
        [Tooltip("[0, 10]")]
        public float stopSpeedThreshold;

        [PropertyRange(0, 10f)]
        [Tooltip("[0, 10]")]
        public float walkStepDistance;

        public LayerMask floorLayers;

        #endregion

        #region IEquatable

        [DebuggerStepThrough]
        public bool Equals(PlayerFootPlanting other)
        {
            return floorLayers.Equals(other.floorLayers) &&
                   walkStepDistance.Equals(other.walkStepDistance) &&
                   runStepDistance.Equals(other.runStepDistance) &&
                   stopSpeedThreshold.Equals(other.stopSpeedThreshold);
        }

        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return obj is PlayerFootPlanting other && Equals(other);
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = floorLayers.GetHashCode();
                hashCode = (hashCode * 397) ^ walkStepDistance.GetHashCode();
                hashCode = (hashCode * 397) ^ runStepDistance.GetHashCode();
                hashCode = (hashCode * 397) ^ stopSpeedThreshold.GetHashCode();
                return hashCode;
            }
        }

        [DebuggerStepThrough]
        public static bool operator ==(PlayerFootPlanting left, PlayerFootPlanting right)
        {
            return left.Equals(right);
        }

        [DebuggerStepThrough]
        public static bool operator !=(PlayerFootPlanting left, PlayerFootPlanting right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
