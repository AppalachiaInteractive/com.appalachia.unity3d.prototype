using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Features.Character.States
{
    [Serializable]
    public struct PlayerParts : IEquatable<PlayerParts>
    {
        #region Fields and Autoproperties

        public Transform leftFoot;
        public Transform leftHand;
        public Transform mouth;
        public Transform rightFoot;
        public Transform rightHand;

        #endregion

        #region IEquatable

        [DebuggerStepThrough]
        public bool Equals(PlayerParts other)
        {
            return Equals(leftFoot,  other.leftFoot) &&
                   Equals(rightFoot, other.rightFoot) &&
                   Equals(leftHand,  other.leftHand) &&
                   Equals(rightHand, other.rightHand) &&
                   Equals(mouth,     other.mouth);
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return obj is PlayerParts other && Equals(other);
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = leftFoot != null ? leftFoot.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (rightFoot != null ? rightFoot.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (leftHand != null ? leftHand.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (rightHand != null ? rightHand.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (mouth != null ? mouth.GetHashCode() : 0);
                return hashCode;
            }
        }

        [DebuggerStepThrough]
        public static bool operator ==(PlayerParts left, PlayerParts right)
        {
            return left.Equals(right);
        }

        [DebuggerStepThrough]
        public static bool operator !=(PlayerParts left, PlayerParts right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
