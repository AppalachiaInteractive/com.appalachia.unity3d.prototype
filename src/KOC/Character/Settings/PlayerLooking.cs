using System;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Character.Settings
{
    [Serializable]
    public struct PlayerLooking : IEquatable<PlayerLooking>
    {
        [PropertyRange(0.1f, 2f)]
        [Tooltip("[0.1, 2]")]
        public float lookSpeed;

        [PropertyRange(-360f, 360f)]
        [Tooltip("[-360, 360]")]
        public float pitchLimitMax;

        [PropertyRange(-360f, 360f)]
        [Tooltip("[-360, 360]")]
        public float pitchLimitMin;

        [PropertyRange(0.1f, 2f)]
        [Tooltip("[0.1, 2]")]
        public float runLookSpeed;

        #region IEquatable

        [DebuggerStepThrough] public bool Equals(PlayerLooking other)
        {
            return lookSpeed.Equals(other.lookSpeed) &&
                   runLookSpeed.Equals(other.runLookSpeed) &&
                   pitchLimitMin.Equals(other.pitchLimitMin) &&
                   pitchLimitMax.Equals(other.pitchLimitMax);
        }

        [DebuggerStepThrough] public override bool Equals(object obj)
        {
            return obj is PlayerLooking other && Equals(other);
        }

        [DebuggerStepThrough] public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = lookSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ runLookSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ pitchLimitMin.GetHashCode();
                hashCode = (hashCode * 397) ^ pitchLimitMax.GetHashCode();
                return hashCode;
            }
        }

        [DebuggerStepThrough] public static bool operator ==(PlayerLooking left, PlayerLooking right)
        {
            return left.Equals(right);
        }

        [DebuggerStepThrough] public static bool operator !=(PlayerLooking left, PlayerLooking right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
